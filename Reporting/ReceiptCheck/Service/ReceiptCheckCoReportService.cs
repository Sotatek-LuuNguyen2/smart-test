﻿using Helper.Common;
using Helper.Extension;
using Infrastructure.Interfaces;
using Reporting.Mappers.Common;
using Reporting.ReceiptCheck.DB;
using Reporting.ReceiptCheck.Mapper;
using Reporting.ReceiptCheck.Model;

namespace Reporting.ReceiptCheck.Service;

public class ReceiptCheckCoReportService : IReceiptCheckCoReportService
{
    private const int MAX_LENG_MESSAGE = 90;

    private readonly ITenantProvider _tenantProvider;
    private List<CoReceiptCheckModel> _coModels;
    private CoReceiptCheckModel _coModel;
    private string _messageOld;
    private readonly char[] _arrCharNotEnd = new char[] { '(', '"', '\'', '{', '[', '’', '′', '“', '「', '【', '［', '『', '（', '’', '″', '‘', '`', '‘' };
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private bool _hasNextPage;

    public ReceiptCheckCoReportService(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
        _tableFieldData = new();
        _singleFieldData = new();
        _messageOld = string.Empty;
        _coModel = new();
        _coModels = new();
    }

    public CommonReportingRequestModel GetReceiptCheckCoReportingData(int hpId, List<long> ptIds, int seikyuYm)
    {
        using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
        {
            var finder = new CoReceiptCheckFinder(_tenantProvider);

            // データ取得
            _coModels = finder.GetCoReceiptChecks(hpId, ptIds, seikyuYm);
            // レセプト印刷
            while (_hasNextPage)
            {
                UpdateDrawForm(seikyuYm);
            }

            return new CoReceiptCheckMapper(_singleFieldData, _tableFieldData).GetData();
        }
    }

    private void UpdateDrawForm(int seikyuYm)
    {
        string tempSinYm = seikyuYm.AsString().Insert(4, "年");
        string sYmd = CIUtil.SDateToShowSWDate(CIUtil.StrToIntDef(CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd"), 0), fmtDate: 1) +
                      "（" + CIUtil.JapanDayOfWeek(CIUtil.GetJapanDateTimeNow()) + "）" +
                       CIUtil.GetJapanDateTimeNow().ToString("HH:mm") + " 作成";

        string sTgtYm = tempSinYm + "月度";

        setFieldData("DT_TgtYm", sTgtYm);
        setFieldData("DT_Date", sYmd);

        int linePinted = 0;
        while (linePinted < 45)
        {
            string fieldMessage = "DT_ErrMsg" + (linePinted + 1);
            if (!string.IsNullOrEmpty(_messageOld))
            {
                string message = CIUtil.CiCopyStrWidth(_messageOld, 1, MAX_LENG_MESSAGE);
                setFieldData(fieldMessage, message);
                _messageOld = _messageOld.Remove(0, message.Length);
            }
            else
            {
                var coModelItem = _coModels.FirstOrDefault();
                if (coModelItem == null)
                {
                    _hasNextPage = false;
                    return;
                }

                if (_coModel == null ||
                    coModelItem.SinYm != _coModel.SinYm ||
                    coModelItem.PtId != _coModel.PtId ||
                    coModelItem.HokenId != _coModel.HokenId)
                {
                    if (linePinted > 43)
                    {
                        _hasNextPage = true;
                    }

                    string sinYm = coModelItem?.SinYm.AsString().Insert(4, "年") + "月";
                    var data = new Dictionary<string, CellModel>
                    {
                        { "DT_Ym", new CellModel(sinYm) },
                        { "DT_KanID", new CellModel((coModelItem?.PtNum ?? 0).ToString()) },
                        { "DT_KanNM", new CellModel(coModelItem?.PtName ?? string.Empty) },
                        { "DT_HoInf", new CellModel(coModelItem?.HokenName ?? string.Empty) }
                    };
                    _tableFieldData.Add(data);
                    _coModel = coModelItem!;
                }

                var messagetemp = coModelItem?.ErrorMessage ?? string.Empty;
                string message = CIUtil.CiCopyStrWidth(messagetemp, 1, MAX_LENG_MESSAGE);
                if (_arrCharNotEnd.Contains(message.LastOrDefault()))
                {
                    message = message.Remove(message.Length - 1, 1);
                }

                setFieldData(fieldMessage, message);
                _messageOld = messagetemp.Remove(0, message.Length);

                _coModels.RemoveAt(0);
            }

            linePinted++;
        }
        _hasNextPage = !string.IsNullOrEmpty(_messageOld) || _coModels.Count > 0;
    }


    private void setFieldData(string field, string value)
    {
        if (string.IsNullOrEmpty(field) || !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }
}
