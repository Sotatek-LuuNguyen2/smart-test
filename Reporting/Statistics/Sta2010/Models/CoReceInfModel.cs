﻿using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Statistics.Sta2010.Models
{
    public class CoReceInfModel
    {
        public ReceInf ReceInf { get; private set; }
        public PtHokenInf PtHokenInf { get; private set; }
        public PtKohi PtKohi1 { get; set; }
        public PtKohi PtKohi2 { get; set; }
        public PtKohi PtKohi3 { get; set; }
        public PtKohi PtKohi4 { get; set; }
        public KaMst KaMst { get; private set; }
        public UserMst TantoMst { get; private set; }

        //true: 政令指定都市及び、広域連合を代表番号にまとめる
        private readonly bool changeMainHokensyaNo = false;
        //都道府県番号
        private readonly int prefNo;

        public CoReceInfModel(ReceInf receInf, PtHokenInf ptHokenInf,
            PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4,
            bool mainHokensyaNo, int prefNo, KaMst kaMst, UserMst tantoMst)
        {
            ReceInf = receInf;
            PtHokenInf = ptHokenInf;
            PtKohi1 = ptKohi1;
            PtKohi2 = ptKohi2;
            PtKohi3 = ptKohi3;
            PtKohi4 = ptKohi4;
            changeMainHokensyaNo = mainHokensyaNo;
            this.prefNo = prefNo;
            KaMst = kaMst;
            TantoMst = tantoMst;
        }

        /// <summary>
        /// 請求年月
        /// </summary>
        public int SeikyuYm
        {
            get => ReceInf.SeikyuYm;
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get => ReceInf.PtId;
        }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm
        {
            get => ReceInf.SinYm;
        }

        /// <summary>
        /// 主保険保険ID
        /// </summary>
        public int HokenId
        {
            get => ReceInf.HokenId;
        }

        /// <summary>
        /// 公費１保険ID
        /// </summary>
        public int Kohi1Id
        {
            get => ReceInf.Kohi1Id;
        }

        /// <summary>
        /// 公費２保険ID
        /// </summary>
        public int Kohi2Id
        {
            get => ReceInf.Kohi2Id;
        }

        /// <summary>
        /// 公費３保険ID
        /// </summary>
        public int Kohi3Id
        {
            get => ReceInf.Kohi3Id;
        }

        /// <summary>
        /// 公費４保険ID
        /// </summary>
        public int Kohi4Id
        {
            get => ReceInf.Kohi4Id;
        }


        #region レセプト種別
        /// <summary>
        /// 11x2: 本人
        /// </summary>
        public bool IsNrMine
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "1" && ReceInf.ReceSbt.Substring(3, 1) == "2";
        }

        /// <summary>
        /// 11x4: 未就学者
        /// </summary>
        public bool IsNrPreSchool
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "1" && ReceInf.ReceSbt.Substring(3, 1) == "4";
        }

        /// <summary>
        /// 11x6: 家族
        /// </summary>
        public bool IsNrFamily
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "1" && ReceInf.ReceSbt.Substring(3, 1) == "6";
        }

        /// <summary>
        /// 11x8: 高齢一般・低所
        /// </summary>
        public bool IsNrElderIppan
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "1" && ReceInf.ReceSbt.Substring(3, 1) == "8";
        }

        /// <summary>
        /// 11x0: 高齢上位
        /// </summary>
        public bool IsNrElderUpper
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "1" && ReceInf.ReceSbt.Substring(3, 1) == "0";
        }

        /// <summary>
        /// 11xx: 一般すべて
        /// </summary>
        public bool IsNrAll
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "1";
        }

        /// <summary>
        /// 12x2: 公費
        /// </summary>
        public bool IsKohiOnly
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "2" && ReceInf.ReceSbt.Substring(3, 1) == "2";
        }

        /// <summary>
        /// 13x8: 後期一般・低所
        /// </summary>
        public bool IsKoukiIppan
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "3" && ReceInf.ReceSbt.Substring(3, 1) == "8";
        }

        /// <summary>
        /// 13x0: 後期上位
        /// </summary>
        public bool IsKoukiUpper
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "3" && ReceInf.ReceSbt.Substring(3, 1) == "0";
        }

        /// <summary>
        /// 13xx: 後期すべて
        /// </summary>
        public bool IsKoukiAll
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "3";
        }

        /// <summary>
        /// 14x2: 退職本人
        /// </summary>
        public bool IsRetMine
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "4" && ReceInf.ReceSbt.Substring(3, 1) == "2";
        }

        /// <summary>
        /// 14x4: 退職未就学者
        /// </summary>
        public bool IsRetPreSchool
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "4" && ReceInf.ReceSbt.Substring(3, 1) == "4";
        }

        /// <summary>
        /// 14x6: 退職家族
        /// </summary>
        public bool IsRetFamily
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "4" && ReceInf.ReceSbt.Substring(3, 1) == "6";
        }

        /// <summary>
        /// 14x8: 退職高齢一般・低所
        /// </summary>
        public bool IsRetElderIppan
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "4" && ReceInf.ReceSbt.Substring(3, 1) == "8";
        }

        /// <summary>
        /// 14x0: 退職高齢上位
        /// </summary>
        public bool IsRetElderUpper
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "4" && ReceInf.ReceSbt.Substring(3, 1) == "0";
        }

        /// <summary>
        /// 14xx: 退職すべて
        /// </summary>
        public bool IsRetAll
        {
            get => ReceInf.ReceSbt?.Substring(1, 1) == "4";
        }

        /// <summary>
        /// true: 併用レセ
        /// </summary>
        public bool IsHeiyo
        {
            get => ReceInf.ReceSbt?.Substring(2, 1) != "1";
        }
        #endregion


        /// <summary>
        /// 指定した法別番号の公費を持っているかどうか
        /// </summary>
        /// <param name="houbetu"></param>
        /// <returns></returns>
        public bool IsKohi(string houbetu)
        {
            return
                (ReceInf.Kohi1Houbetu == houbetu && ReceInf.Kohi1ReceKisai == 1) ||
                (ReceInf.Kohi2Houbetu == houbetu && ReceInf.Kohi2ReceKisai == 1) ||
                (ReceInf.Kohi3Houbetu == houbetu && ReceInf.Kohi3ReceKisai == 1) ||
                (ReceInf.Kohi4Houbetu == houbetu && ReceInf.Kohi4ReceKisai == 1);
        }

        /// <summary>
        /// 他県公費の有無
        /// </summary>
        public bool IsPrefOutKohi
        {
            get
            {
                string myPrefNo = string.Format("{0:D2}", prefNo);
                string kohi1PrefNo = PtKohi1?.FutansyaNo?.AsString().Substring(2, 2) ?? string.Empty;
                string kohi2PrefNo = PtKohi2?.FutansyaNo?.AsString().Substring(2, 2) ?? string.Empty;
                string kohi3PrefNo = PtKohi3?.FutansyaNo?.AsString().Substring(2, 2) ?? string.Empty;
                string kohi4PrefNo = PtKohi4?.FutansyaNo?.AsString().Substring(2, 2) ?? string.Empty;

                return
                    (ReceInf.Kohi1ReceKisai == 1 && kohi1PrefNo != myPrefNo) ||
                    (ReceInf.Kohi2ReceKisai == 1 && kohi2PrefNo != myPrefNo) ||
                    (ReceInf.Kohi3ReceKisai == 1 && kohi3PrefNo != myPrefNo) ||
                    (ReceInf.Kohi4ReceKisai == 1 && kohi4PrefNo != myPrefNo);
            }
        }

        /// <summary>
        /// マル長
        /// </summary>
        public bool IsChoki
        {
            get => TokkiContains("02") || TokkiContains("16");

        }

        public bool TokkiContains(string tokkiCd)
        {
            return
                CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 1, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 3, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 5, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 7, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 9, 2) == tokkiCd;
        }

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        public string Kohi1Houbetu
        {
            get => ReceInf.Kohi1Houbetu ?? string.Empty;
        }

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        public string Kohi2Houbetu
        {
            get => ReceInf.Kohi2Houbetu ?? string.Empty;
        }

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        public string Kohi3Houbetu
        {
            get => ReceInf.Kohi3Houbetu ?? string.Empty;
        }

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        public string Kohi4Houbetu
        {
            get => ReceInf.Kohi4Houbetu ?? string.Empty;
        }

        /// <summary>
        /// 公１レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public bool Kohi1ReceKisai
        {
            get => ReceInf.Kohi1ReceKisai == 1;
        }

        /// <summary>
        /// 公２レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public bool Kohi2ReceKisai
        {
            get => ReceInf.Kohi2ReceKisai == 1;
        }

        /// <summary>
        /// 公３レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public bool Kohi3ReceKisai
        {
            get => ReceInf.Kohi3ReceKisai == 1;
        }

        /// <summary>
        /// 公４レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public bool Kohi4ReceKisai
        {
            get => ReceInf.Kohi4ReceKisai == 1;
        }

        public int KohiReceTensu(string houbetu)
        {
            int retTensu = 0;
            if (ReceInf.Kohi1Houbetu == houbetu && ReceInf.Kohi1ReceKisai == 1) retTensu += ReceInf.Kohi1ReceTensu ?? 0;
            if (ReceInf.Kohi2Houbetu == houbetu && ReceInf.Kohi2ReceKisai == 1) retTensu += ReceInf.Kohi2ReceTensu ?? 0;
            if (ReceInf.Kohi3Houbetu == houbetu && ReceInf.Kohi3ReceKisai == 1) retTensu += ReceInf.Kohi3ReceTensu ?? 0;
            if (ReceInf.Kohi4Houbetu == houbetu && ReceInf.Kohi4ReceKisai == 1) retTensu += ReceInf.Kohi4ReceTensu ?? 0;

            return retTensu;
        }

        public int KohiReceTensu()
        {
            int retTensu = 0;
            if (ReceInf.Kohi1ReceKisai == 1) retTensu += ReceInf.Kohi1ReceTensu ?? 0;
            if (ReceInf.Kohi2ReceKisai == 1) retTensu += ReceInf.Kohi2ReceTensu ?? 0;
            if (ReceInf.Kohi3ReceKisai == 1) retTensu += ReceInf.Kohi3ReceTensu ?? 0;
            if (ReceInf.Kohi4ReceKisai == 1) retTensu += ReceInf.Kohi4ReceTensu ?? 0;

            return retTensu;
        }

        public int KohiReceFutan(string houbetu)
        {
            int retFutan = 0;
            if (ReceInf.Kohi1Houbetu == houbetu && ReceInf.Kohi1ReceKisai == 1) retFutan += ReceInf.Kohi1ReceFutan ?? 0;
            if (ReceInf.Kohi2Houbetu == houbetu && ReceInf.Kohi2ReceKisai == 1) retFutan += ReceInf.Kohi2ReceFutan ?? 0;
            if (ReceInf.Kohi3Houbetu == houbetu && ReceInf.Kohi3ReceKisai == 1) retFutan += ReceInf.Kohi3ReceFutan ?? 0;
            if (ReceInf.Kohi4Houbetu == houbetu && ReceInf.Kohi4ReceKisai == 1) retFutan += ReceInf.Kohi4ReceFutan ?? 0;

            return retFutan;
        }

        public int KohiReceFutan()
        {
            int retFutan = 0;
            if (ReceInf.Kohi1ReceKisai == 1) retFutan += ReceInf.Kohi1ReceFutan ?? 0;
            if (ReceInf.Kohi2ReceKisai == 1) retFutan += ReceInf.Kohi2ReceFutan ?? 0;
            if (ReceInf.Kohi3ReceKisai == 1) retFutan += ReceInf.Kohi3ReceFutan ?? 0;
            if (ReceInf.Kohi4ReceKisai == 1) retFutan += ReceInf.Kohi4ReceFutan ?? 0;

            return retFutan;
        }

        public int KohiReceNissu(string houbetu)
        {
            int retNissu = 0;
            if (ReceInf.Kohi1Houbetu == houbetu && ReceInf.Kohi1ReceKisai == 1) retNissu += ReceInf.Kohi1Nissu ?? 0;
            if (ReceInf.Kohi2Houbetu == houbetu && ReceInf.Kohi2ReceKisai == 1) retNissu += ReceInf.Kohi2Nissu ?? 0;
            if (ReceInf.Kohi3Houbetu == houbetu && ReceInf.Kohi3ReceKisai == 1) retNissu += ReceInf.Kohi3Nissu ?? 0;
            if (ReceInf.Kohi4Houbetu == houbetu && ReceInf.Kohi4ReceKisai == 1) retNissu += ReceInf.Kohi4Nissu ?? 0;

            return retNissu;
        }

        public int KohiReceNissu()
        {
            int retNissu = 0;
            if (ReceInf.Kohi1ReceKisai == 1) retNissu += ReceInf.Kohi1Nissu ?? 0;
            if (ReceInf.Kohi2ReceKisai == 1) retNissu += ReceInf.Kohi2Nissu ?? 0;
            if (ReceInf.Kohi3ReceKisai == 1) retNissu += ReceInf.Kohi3Nissu ?? 0;
            if (ReceInf.Kohi4ReceKisai == 1) retNissu += ReceInf.Kohi4Nissu ?? 0;

            return retNissu;
        }

        /// <summary>
        /// 法別番号
        /// 
        /// </summary>        
        public string Houbetu
        {
            get => ReceInf.Houbetu ?? string.Empty;
        }

        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get
            {
                string hokensyaNo = ReceInf.HokensyaNo?.AsString() ?? string.Empty;
                return
                    ReceInf.HokenKbn == Helper.Constants.HokenKbn.Syaho ? hokensyaNo : mainHokensyaNo;
            }
        }

        /// <summary>
        /// 代表保険者番号
        /// </summary>
        private string mainHokensyaNo
        {
            get
            {
                string hokensyaNo = ReceInf.HokensyaNo?.AsString() ?? string.Empty;

                //広域連合
                if (hokensyaNo.Length == 8)
                {
                    switch (hokensyaNo.Substring(0, 4))
                    {
                        case "3901": return "39010004";  //北海道
                        case "3902": return "39020003";  //青森県
                        case "3903": return "39030002";  //岩手県
                        case "3904": return "39040001";  //宮城県
                        case "3905": return "39050000";  //秋田県
                        case "3906": return "39060009";  //山形県
                        case "3907": return "39070008";  //福島県
                        case "3908": return "39080007";  //茨城県
                        case "3909": return "39090006";  //栃木県
                        case "3910": return "39100003";  //群馬県
                        case "3911": return "39110002";  //埼玉県
                        case "3912": return "39120001";  //千葉県
                        case "3913": return "39130000";  //東京都
                        case "3914": return "39140009";  //神奈川県
                        case "3915": return "39150008";  //新潟県
                        case "3916": return "39160007";  //富山県
                        case "3917": return "39170006";  //石川県
                        case "3918": return "39180005";  //福井県
                        case "3919": return "39190004";  //山梨県
                        case "3920": return "39200001";  //長野県
                        case "3921": return "39210000";  //岐阜県
                        case "3922": return "39220009";  //静岡県
                        case "3923": return "39230008";  //愛知県
                        case "3924": return "39240007";  //三重県
                        case "3925": return "39250006";  //滋賀県
                        case "3926": return "39260005";  //京都府
                        case "3927": return "39270004";  //大阪府
                        case "3928": return "39280003";  //兵庫県
                        case "3929": return "39290002";  //奈良県
                        case "3930": return "39300009";  //和歌山県
                        case "3931": return "39310008";  //鳥取県
                        case "3932": return "39320007";  //島根県
                        case "3933": return "39330006";  //岡山県
                        case "3934": return "39340005";  //広島県
                        case "3935": return "39350004";  //山口県
                        case "3936": return "39360003";  //徳島県
                        case "3937": return "39370002";  //香川県
                        case "3938": return "39380001";  //愛媛県
                        case "3939": return "39390000";  //高知県
                        case "3940": return "39400007";  //福岡県
                        case "3941": return "39410006";  //佐賀県
                        case "3942": return "39420005";  //長崎県
                        case "3943": return "39430004";  //熊本県
                        case "3944": return "39440003";  //大分県
                        case "3945": return "39450002";  //宮崎県
                        case "3946": return "39460001";  //鹿児島県
                        case "3947": return "39470000";  //沖縄県
                    }
                }

                //政令指定都市
                if (changeMainHokensyaNo)
                {
                    switch (hokensyaNo.Substring(hokensyaNo.Length - 6, 3))
                    {
                        case "014": return "014001";  //札幌市
                        case "044": return "044008";  //仙台市
                        case "114": return "114009";  //さいたま市
                        case "124": return "124008";  //千葉市
                        case "144": return "144006";  //横浜市
                        case "145": return "145003";  //川崎市
                        case "234": return "234005";  //名古屋市
                        case "264": return "264002";  //京都市
                        case "274": return "274001";  //大阪市
                        case "284": return "284000";  //神戸市
                        case "344": return "344002";  //広島市
                        case "404": return "404004";  //北九州市
                        case "405": return "405001";  //福岡市
                        case "224": return "224006";  //静岡市
                        case "275": return "275008";  //堺市
                        case "154": return "154005";  //新潟市
                        case "225": return "225003";  //浜松市
                    }

                    switch (hokensyaNo.Substring(hokensyaNo.Length - 6, 5))
                    {
                        case "27002": return "275008";  //堺市
                    }
                }

                return hokensyaNo.Substring(hokensyaNo.Length - 6, 6);
            }
        }

        /// <summary>
        /// 県内保険者
        /// </summary>
        public bool IsPrefIn
        {
            get
            {
                List<string> prefIn = new List<string>();
                prefIn.Add(string.Format("{0:D2}", prefNo));
                prefIn.Add(string.Format("{0:D2}", prefNo + 50));

                return prefIn.Contains(ReceInf.HokensyaNo?.Substring(ReceInf.HokensyaNo.Length - 6, 2) ?? string.Empty);
            }
        }

        /// <summary>
        /// 件数（2併の場合は1レセで2件とカウント）
        /// </summary>
        public int ReceCnt
        {
            get => ReceInf.ReceSbt?.Substring(2, 1).AsInteger() ?? 0;
        }

        /// <summary>
        /// 公費件数（2併の場合は1レセで2件とカウント）
        /// </summary>
        public int KohiCnt
        {
            get
            {
                if (ReceInf.ReceSbt?.Substring(1, 1) != "2")
                {
                    return ReceInf.ReceSbt?.Substring(2, 1).AsInteger() - 1 ?? 0;
                }
                else
                {
                    return ReceInf.ReceSbt?.Substring(2, 1).AsInteger() ?? 0;
                }
            }
        }

        /// <summary>
        /// 点数
        /// </summary>
        public int Tensu
        {
            get => ReceInf.Tensu;
        }

        /// <summary>
        /// 保険実日数
        /// 
        /// </summary>
        public int HokenNissu
        {
            get => ReceInf.HokenNissu ?? 0;
        }

        /// <summary>
        /// 保険レセ負担額
        /// 
        /// </summary>
        public int HokenReceFutan
        {
            get => ReceInf.HokenReceFutan ?? 0;
        }

        /// <summary>
        /// 公１レセ負担額
        /// 
        /// </summary>
        public int Kohi1ReceFutan
        {
            get => ReceInf.Kohi1ReceKisai == 1 ? (ReceInf.Kohi1ReceFutan ?? 0) : 0;
        }

        /// <summary>
        /// 公２レセ負担額
        /// 
        /// </summary>
        public int Kohi2ReceFutan
        {
            get => ReceInf.Kohi2ReceKisai == 1 ? (ReceInf.Kohi2ReceFutan ?? 0) : 0;
        }

        /// <summary>
        /// 公３レセ負担額
        /// 
        /// </summary>
        public int Kohi3ReceFutan
        {
            get => ReceInf.Kohi3ReceKisai == 1 ? (ReceInf.Kohi3ReceFutan ?? 0) : 0;
        }

        /// <summary>
        /// 公４レセ負担額
        /// 
        /// </summary>
        public int Kohi4ReceFutan
        {
            get => ReceInf.Kohi4ReceKisai == 1 ? (ReceInf.Kohi4ReceFutan ?? 0) : 0;
        }

        /// <summary>
        /// 患者負担額
        /// </summary>
        public int PtFutan
        {
            get => ReceInf.PtFutan;
        }

        /// <summary>
        /// 振込予定額
        /// </summary>
        public int Furikomi
        {
            get => ReceInf.TotalIryohi - ReceInf.PtFutan;
        }

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        public int HokenRate
        {
            get => ReceInf.HokenRate;
        }

        /// <summary>
        /// 国保減免区分
        ///     1:減額 2:免除 3:支払猶予 4:自立支援減免
        /// </summary>
        public int GenmenKbn
        {
            get => ReceInf.GenmenKbn;
        }

        /// <summary>
        /// 在医総フラグ
        ///     1: 在医総管又は在医総
        /// </summary>
        public int IsZaiiso
        {
            get => ReceInf.IsZaiiso;
        }

        /// <summary>
        /// 職務上区分
        ///     1:職務上
        ///     2:下船後３月以内
        ///     3:通勤災害
        /// </summary>
        public int SyokumuKbn
        {
            get => PtHokenInf.SyokumuKbn;
        }

        /// <summary>
        /// 診療科ID
        /// </summary>
        public int KaId
        {
            get => ReceInf.KaId;
        }

        /// <summary>
        /// 診療科略称
        /// </summary>
        public string KaSname
        {
            get => KaMst?.KaSname ?? "";
        }

        /// <summary>
        /// 担当医ID
        /// </summary>
        public int TantoId
        {
            get => ReceInf.TantoId;
        }

        /// <summary>
        /// 担当医略称
        /// </summary>
        public string TantoSname
        {
            get => TantoMst?.Sname ?? "";
        }

        /// <summary>
        /// 保険区分
        ///     0:自費
        ///     1:社保          
        ///     2:国保          
        ///     11:労災(短期給付)          
        ///     12:労災(傷病年金)          
        ///     13:アフターケア          
        ///     14:自賠責          
        /// </summary>
        public int HokenKbn
        {
            get => ReceInf.HokenKbn;
        }
    }
}
