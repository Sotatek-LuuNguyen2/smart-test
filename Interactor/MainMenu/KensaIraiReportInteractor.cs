﻿using Domain.Models.KensaIrai;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Entity.Tenant;
using Helper.Common;
using Helper.Redis;
using Microsoft.Extensions.Configuration;
using Reporting.Kensalrai.DB;
using Reporting.Kensalrai.Service;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;
using UseCase.MainMenu.KensaIraiReport;

namespace Interactor.MainMenu;

public class KensaIraiReportInteractor : IKensaIraiReportInputPort
{
    private static HttpClient _httpClient = new HttpClient();
    private readonly IKensaIraiRepository _kensaIraiRepository;
    private readonly IKensaIraiCoReportService _kensaIraiCoReportService;
    private readonly IConfiguration _configuration;
    private readonly ISystemConfRepository _systemConfigRepository;
    private readonly IDatabase _cache;
    private readonly ICoKensaIraiFinder _coKensaIraiFinder;
    private readonly IReceptionRepository _receptionRepository;

    public KensaIraiReportInteractor(IKensaIraiRepository kensaIraiRepository, IKensaIraiCoReportService kensaIraiCoReportService, IConfiguration configuration, ISystemConfRepository systemConfigRepository, ICoKensaIraiFinder coKensaIraiFinder, IReceptionRepository receptionRepository)
    {
        _kensaIraiRepository = kensaIraiRepository;
        _kensaIraiCoReportService = kensaIraiCoReportService;
        _configuration = configuration;
        _coKensaIraiFinder = coKensaIraiFinder;
        _systemConfigRepository = systemConfigRepository;
        _receptionRepository = receptionRepository;
        GetRedis();
        _cache = RedisConnectorHelper.Connection.GetDatabase();
    }

    private void GetRedis()
    {
        string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
        if (RedisConnectorHelper.RedisHost != connection)
        {
            RedisConnectorHelper.RedisHost = connection;
        }
    }

    public KensaIraiReportOutputData Handle(KensaIraiReportInputData inputData)
    {
        try
        {
            var kensaIraiList = AsKensaIraiReportModel(inputData.HpId, inputData.KensaIraiList);
            var odrKensaIraiKaCode = _systemConfigRepository.GetSettingValue(100019, 8, inputData.HpId);
            var odrKensaIraiFileType = _systemConfigRepository.GetSettingValue(100019, 7, inputData.HpId);
            List<string> data;
            if (odrKensaIraiFileType == 3)
            {
                data = GetIraiFileData(inputData.CenterCd, kensaIraiList, 0, odrKensaIraiKaCode, odrKensaIraiFileType);
            }
            else
            {
                data = GetIraiFileDataStandard(inputData.CenterCd, kensaIraiList, 0, odrKensaIraiKaCode, odrKensaIraiFileType);
            }

            var datFileByte = data.SelectMany(item => Encoding.UTF8.GetBytes(item + Environment.NewLine)).ToArray();
            string datFile = Convert.ToBase64String(datFileByte);

            var outputPrint = _kensaIraiCoReportService.GetKensalraiData(inputData.HpId, inputData.SystemDate, inputData.FromDate, inputData.ToDate, inputData.CenterCd, kensaIraiList);
            var waitResult = ReturnPDF(outputPrint);
            waitResult.Wait();
            var pdfFileByte = waitResult.Result;
            string pdfFile = Convert.ToBase64String(pdfFileByte);
            var guid = Guid.NewGuid();
            string key = guid.ToString();
            string finalKey = "KensaIraiPdfReport_" + key;
            _cache.StringSet(finalKey, pdfFile);
            _cache.KeyExpire(finalKey, new TimeSpan(8, 0, 30));
            var kensaIraiLogModel = new KensaIraiLogModel(
                                        inputData.SystemDate,
                                        inputData.CenterCd,
                                        string.Empty,
                                        inputData.FromDate,
                                        inputData.ToDate,
                                        ListToString(data),
                                        pdfFileByte,
                                        CIUtil.GetJapanDateTimeNow());
            if (_kensaIraiRepository.SaveKensaIraiLog(inputData.HpId, inputData.UserId, kensaIraiLogModel))
            {
                return new KensaIraiReportOutputData(key, datFile, KensaIraiReportStatus.Successed);
            }
            return new KensaIraiReportOutputData(key, datFile, KensaIraiReportStatus.Failed);
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
            _receptionRepository.ReleaseResource();
        }
    }

    #region private function
    private List<Reporting.Kensalrai.Model.KensaIraiModel> AsKensaIraiReportModel(int hpId, List<KensaIraiModel> source)
    {
        List<Reporting.Kensalrai.Model.KensaIraiModel> kensaIraiList = new();
        foreach (var kensaIrai in source)
        {
            var weightHeight = _coKensaIraiFinder.GetHeightWeight(hpId, kensaIrai.PtId, kensaIrai.SinDate);

            var raiinInf = _receptionRepository.GetRaiinInf(hpId, kensaIrai.PtId, kensaIrai.SinDate, kensaIrai.RaiinNo);
            kensaIraiList.Add(new Reporting.Kensalrai.Model.KensaIraiModel(
                kensaIrai.SinDate,
                kensaIrai.RaiinNo,
                kensaIrai.IraiCd,
                kensaIrai.PtId,
                kensaIrai.PtNum,
                kensaIrai.Name,
                kensaIrai.KanaName,
                kensaIrai.Sex,
                kensaIrai.Birthday,
                kensaIrai.TosekiKbn,
                kensaIrai.SikyuKbn,
                kensaIrai.KaId,
                weightHeight.weight,
                weightHeight.height,
                raiinInf?.TantoName ?? string.Empty,
                raiinInf?.TantoKanaName ?? string.Empty,
                raiinInf?.KaSName ?? string.Empty,
            AsKensaIraiDetailReportModel(kensaIrai.KensaIraiDetails))) ;
        }
        return kensaIraiList;
    }

    private List<Reporting.Kensalrai.Model.KensaIraiDetailModel> AsKensaIraiDetailReportModel(List<KensaIraiDetailModel> source)
    {
        List<Reporting.Kensalrai.Model.KensaIraiDetailModel> kensaIraiDetails = new();
        foreach (var kensaDetail in source)
        {
            KensaMst kensaMst = new();
            kensaMst.KensaItemCd = kensaDetail.KensaItemCd;
            kensaMst.CenterItemCd1 = kensaDetail.CenterItemCd;
            kensaMst.KensaKana = kensaDetail.KensaKana;
            kensaMst.KensaName = kensaDetail.KensaName;
            kensaMst.ContainerCd = kensaDetail.ContainerCd;
            kensaIraiDetails.Add(new Reporting.Kensalrai.Model.KensaIraiDetailModel(true, kensaDetail.RpNo, kensaDetail.RpEdaNo, kensaDetail.RowNo, kensaDetail.SeqNo, kensaMst));
        }
        return kensaIraiDetails;
    }

    private async Task<byte[]> ReturnPDF(object data)
    {
        StringContent jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

        string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;

        string functionName = "common-reporting";

        using (HttpResponseMessage response = await _httpClient.PostAsync($"{basePath}{functionName}", jsonContent))
        {
            response.EnsureSuccessStatusCode();
            using (var streamingData = (MemoryStream)response.Content.ReadAsStream())
            {
                var byteData = streamingData.ToArray();
                return byteData;
            }
        }
    }

    private string ListToString(List<string> data)
    {
        StringBuilder line = new();
        foreach (var item in data)
        {
            line.AppendLine(item);
        }
        return line.ToString();
    }
    #endregion

    #region OdrKensaIraiFileType in (0,1,2)
    private List<string> GetIraiFileDataStandard(string CenterCd, List<Reporting.Kensalrai.Model.KensaIraiModel> kensaIrais, int fileType, double odrKensaIraiKaCode, double odrKensaIraiFileType)
    {
        const int RightJustification = 0;
        const int LeftJustification = 1;

        #region local method
        // 文字列を指定の長さに調整する（文字カット、スペース埋め）
        string adjStr(string str, int length, int justification = LeftJustification)
        {
            //string result = CIUtil.Copy(str, 1, length);

            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }

            string result = str;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding enc = Encoding.GetEncoding("Shift_JIS");
            Byte[] b = enc.GetBytes(str);

            if (enc.GetByteCount(str) > length)
            {
                //バイト数で文字カット
                result = enc.GetString(b, 0, length);

                if (enc.GetByteCount(str) >= length + 1)
                {
                    //区切りが全角文字の途中の場合、１文字多くなってしまうのを防ぐ
                    string result2 = enc.GetString(b, 0, length + 1);
                    if (result.Length == result2.Length)
                    {
                        result = result.Remove(result.Length - 1);
                    }
                }
            }

            //埋める文字数　=　最大バイト数　-　(対象文字列のバイト数 - 対象文字列の文字列数)
            int width = length - (enc.GetByteCount(result) - result.Length);
            if (justification == RightJustification)
            {
                // 右寄せ（左にスペース）
                result = result.PadLeft(width);
            }
            else
            {
                // 左寄せ（右にスペース）
                result = result.PadRight(width);
            }
            return result;
        }

        // 身長体重を出力用文字列に変換する
        string getHeightWeight(double val)
        {
            string retStr = "";
            if (val == 0)
            {
                retStr = "00";
            }
            else
            {
                val = Math.Round(val, 1, MidpointRounding.AwayFromZero) * 10;
                retStr = val.ToString();
            }
            return adjStr(retStr, 4, RightJustification);
        }
        #endregion

        List<string> results = new List<string>();

        foreach (var kensaIrai in kensaIrais)
        {
            #region O1レコード
            string o1 = "";

            // レコード区分   2桁
            o1 += "O1";
            // センターコード  6桁
            o1 += adjStr(CenterCd, 6);
            // 依頼コード    20桁
            if (odrKensaIraiFileType == 2)
            {
                o1 += adjStr((kensaIrai.SinDate % 1000000).ToString(), 20);
            }
            else
            {
                o1 += adjStr(kensaIrai.IraiCd.ToString(), 20);
            }

            // 科コード     15桁
            if (odrKensaIraiKaCode == 1)
            {
                string kacdname = kensaIrai.KaCodeName;
                kacdname = CIUtil.CiCopyStrWidth(kacdname, 1, 15, 1);
                o1 += kacdname;

            }
            else if (odrKensaIraiKaCode == 2)
            {
                string kacdname = "";

                if (kensaIrai.KaId > 0)
                {
                    kacdname = kensaIrai.KaId.ToString();
                }

                kacdname = kacdname.PadLeft(15, ' ');
                o1 += kacdname;
            }
            else
            {
                // 通常は未使用
                o1 += new string(' ', 15);
            }
            // 病棟コード    15桁 ※未使用
            o1 += new string(' ', 15);
            // 入院外来区分   1桁  ※2固定
            o1 += "2";
            // 提出医      10桁 ※未使用
            if (fileType == 1)
            {
                // 加古川
                o1 += CIUtil.CiCopyStrWidth(kensaIrai.TantoKanaName, 1, 10, 1);
            }
            else
            {
                if (odrKensaIraiFileType == 0)
                {
                    o1 += new string(' ', 10);
                }
                else if (odrKensaIraiFileType == 1 || odrKensaIraiFileType == 2)
                {
                    o1 += CIUtil.CiCopyStrWidth(kensaIrai.TantoKanaName, 1, 10, 1);
                }
            }
            // 被検者ＩＤ    15桁
            o1 += adjStr(kensaIrai.PtNum.ToString(), 15);
            // カルテＮＯ    15  ※未使用
            o1 += new string(' ', 15);
            // 被検者名     20桁
            o1 += adjStr(kensaIrai.KanaName, 20);
            // 性別       1桁  ※ M/F
            if (fileType == 1)
            {
                // 加古川
                o1 += adjStr(kensaIrai.GetSexStr("1", "2"), 1);
            }
            else
            {
                if (odrKensaIraiFileType == 0)
                {
                    o1 += adjStr(kensaIrai.GetSexStr("M", "F"), 1);
                }
                else
                {
                    o1 += adjStr(kensaIrai.GetSexStr("1", "2"), 1);
                }
            }
            // 年齢区分     1桁  ※Y固定
            o1 += "Y";
            // 年齢       3桁
            o1 += adjStr(kensaIrai.Age.ToString(), 3, RightJustification);
            // 生年月日区分   1桁  ※スペース固定
            o1 += " ";
            // 年月日      6桁  ※YYMMDD（西暦）
            o1 += (kensaIrai.Birthday % 1000000).ToString().PadLeft(6, '0');
            // 採取日      6桁  ※YYMMDD（西暦）
            o1 += (kensaIrai.SinDate % 1000000).ToString().PadLeft(6, '0');
            // 採取時間     4桁  ※未使用
            o1 += new string(' ', 4);
            // 項目数      3桁  
            o1 += adjStr(kensaIrai.DetailCount.ToString(), 3, RightJustification);
            // 身長       4桁（前3桁整数部、後1桁小数点部）
            o1 += getHeightWeight(kensaIrai.Height);
            // 体重       4桁（前3桁整数部、後1桁小数点部）
            o1 += getHeightWeight(kensaIrai.Weight);
            // 尿量       4桁  ※未使用
            o1 += adjStr("0", 4, RightJustification);
            // 尿量単位     2桁  ※未使用
            o1 += new string(' ', 2);
            // 妊娠週数     2桁  ※未使用
            o1 += new string(' ', 2);
            // 透析前後     1桁  ※0はスペースで出力
            o1 += adjStr(CIUtil.ToStringIgnoreZero(kensaIrai.TosekiKbn), 1);
            // 至急報告     1桁
            if (fileType == 1)
            {
                // 加古川
                if (kensaIrai.SikyuKbn == 0)
                {
                    o1 += adjStr(" ", 1);
                }
                else
                {
                    o1 += adjStr(kensaIrai.SikyuKbn.ToString(), 1);
                }
            }
            else
            {
                if (kensaIrai.SikyuKbn == 0)
                {
                    o1 += adjStr(" ", 1);
                }
                else
                {
                    o1 += adjStr(kensaIrai.SikyuKbn.ToString(), 1);
                }
            }

            // 依頼コメント内容     20桁 ※未使用
            o1 += new string(' ', 20);
            // 空白       74桁 ※未使用
            o1 += new string(' ', 74);

            results.Add(o1);
            #endregion

            string o2 = "";
            int dtlCount = 0;
            foreach (var kensaDtl in kensaIrai.Details)
            {
                dtlCount++;
                if (dtlCount > 9)
                {
                    o2 += new string(' ', 3);
                    dtlCount = 1;
                }

                if (dtlCount == 1)
                {
                    if (!string.IsNullOrEmpty(o2))
                    {
                        results.Add(o2);
                        o2 = "";
                    }

                    // レコード区分   2桁
                    o2 += "O2";
                    // センターコード  6桁
                    o2 += adjStr(CenterCd, 6);
                    // 依頼コード    20桁
                    o2 += adjStr(kensaIrai.IraiCd.ToString(), 20);

                }

                if (fileType == 1)
                {
                    // 加古川
                    o2 += adjStr(kensaDtl.CenterItemCd, 25);
                }
                else
                {

                    // 電子カルテ検査項目コード     15桁
                    o2 += adjStr(kensaDtl.KensaItemCd, 15);
                    // センター項目検査コード      10桁
                    o2 += adjStr(kensaDtl.CenterItemCd, 10);
                }
            }

            if (dtlCount <= 9)
            {
                // 不足分をスペース埋め
                //o2 += new string(' ', 256 - o2.Length);
                // 不足バイト数をスペース埋め
                o2 = adjStr(o2, 256);
                results.Add(o2);
            }

        }

        return results;
    }
    #endregion

    #region OdrKensaIraiFileType = 3

    public List<string> GetIraiFileData(string CenterCd, List<Reporting.Kensalrai.Model.KensaIraiModel> kensaIrais, int fileType, double odrKensaIraiKaCode, double odrKensaIraiFileType)
    {
        const int RightJustification = 0;
        const int LeftJustification = 1;

        #region local method
        // 文字列を指定の長さに調整する（文字カット、スペース埋め）
        string adjStr(string str, int length, int justification = LeftJustification)
        {
            //string result = CIUtil.Copy(str, 1, length);

            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }

            string result = str;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding enc = Encoding.GetEncoding("Shift_JIS");
            Byte[] b = enc.GetBytes(str);

            if (enc.GetByteCount(str) > length)
            {
                //バイト数で文字カット
                result = enc.GetString(b, 0, length);

                if (enc.GetByteCount(str) >= length + 1)
                {
                    //区切りが全角文字の途中の場合、１文字多くなってしまうのを防ぐ
                    string result2 = enc.GetString(b, 0, length + 1);
                    if (result.Length == result2.Length)
                    {
                        result = result.Remove(result.Length - 1);
                    }
                }
            }

            //埋める文字数　=　最大バイト数　-　(対象文字列のバイト数 - 対象文字列の文字列数)
            int width = length - (enc.GetByteCount(result) - result.Length);
            if (justification == RightJustification)
            {
                // 右寄せ（左にスペース）
                result = result.PadLeft(width);
            }
            else
            {
                // 左寄せ（右にスペース）
                result = result.PadRight(width);
            }
            return result;
        }

        // 身長体重を出力用文字列に変換する
        string getHeightWeight(double val)
        {
            string retStr = "";
            if (val == 0)
            {
                retStr = "00";
            }
            else
            {
                val = Math.Round(val, 1, MidpointRounding.AwayFromZero) * 10;
                retStr = val.ToString();
            }
            return adjStr(retStr, 4, RightJustification);
        }
        #endregion

        List<string> results = new List<string>();

        foreach (var kensaIrai in kensaIrais)
        {
            #region O1レコード
            string o1 = "";

            // センターコード
            o1 += CenterCd + ",";
            // 依頼者ＫＥＹ
            o1 += kensaIrai.IraiCd.ToString() + ",";

            //予備１ 予備２
            o1 += "," + ",";

            //科コード・科名
            o1 += kensaIrai.KaSName.ToString() + ",";

            //病棟コード病棟名
            o1 += ",";

            // 入院外来区分   1桁  ※2固定
            o1 += "1" + ",";
            // 提出医      10桁 ※未使用

            o1 += CIUtil.CiCopyStrWidth(string.IsNullOrEmpty(kensaIrai.DrName) ? kensaIrai.TantoName : kensaIrai.DrName, 1, 10, 1) + ",";

            // 被検者ＩＤ    
            o1 += kensaIrai.PtNum.ToString() + ",";
            // カルテＮＯ   
            o1 += kensaIrai.PtNum.ToString() + ",";
            //カルテＮＯ区分
            o1 += ",";
            //被検者名カナ
            o1 += kensaIrai.KanaName + ",";
            //被験者名漢字
            o1 += kensaIrai.Name + ",";
            // 性別      
            o1 += kensaIrai.GetSexStr("1", "2") + ",";

            // 年齢区分     1桁  ※Y固定
            o1 += ",";
            // 年齢       3桁
            o1 += kensaIrai.Age.ToString() + ",";
            // 生年月日区分   1桁  ※スペース固定
            o1 += ",";
            // 生年月日
            o1 += (kensaIrai.Birthday % 1000000).ToString().PadLeft(6, '0') + ",";
            // 採取日      6桁  ※YYMMDD（西暦）
            o1 += (kensaIrai.SinDate % 1000000).ToString().PadLeft(6, '0') + ",";
            // 採取時間     4桁  ※未使用
            o1 += (kensaIrai.UpdateTime.PadLeft(6, '0')) + ",";
            //保険情報
            o1 += ",,,,,,,";

            if (kensaIrai.SikyuKbn == 0)
            {
                o1 += string.Empty + ",";
            }
            else
            {
                o1 += kensaIrai.SikyuKbn.ToString() + ",";
            }

            o1 += ",,,,,,,,,,,,";

            int dtlCount = 0;
            foreach (var kensaDtl in kensaIrai.Details)
            {
                dtlCount++;
                if (dtlCount > 50)
                {
                    o1 += new string(' ', 3);
                    dtlCount = 1;
                }
                o1 += ",";
                o1 += kensaDtl.KensaItemCd;
                o1 += "=";
                o1 += kensaDtl.CenterItemCd;
            }
            results.Add(o1);
            #endregion
        }

        return results;
    }
    #endregion
}
