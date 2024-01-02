﻿using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta2020.Models
{
    public class CoSta2020PrintData
    {
        public CoSta2020PrintData(RowType rowType = RowType.Data)
        {
            RowType = rowType;
        }

        /// <summary>
        /// 行タイプ
        /// </summary>
        public RowType RowType { get; set; }

        /// <summary>
        /// 合計行のキャプション
        /// </summary>
        public string TotalCaption { get; set; }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm { get; set; }

        /// <summary>
        /// 診療年月 (yyyy/MM)
        /// </summary>
        public string SinYmFmt
        {
            get => CIUtil.SMonthToShowSMonth(SinYm);
        }

        /// <summary>
        /// 診療科ID
        /// </summary>
        public string KaId { get; set; }

        /// <summary>
        /// 診療科略称
        /// </summary>
        public string KaSname { get; set; }

        /// <summary>
        /// 担当医ID
        /// </summary>
        public string TantoId { get; set; }

        /// <summary>
        /// 担当医略称
        /// </summary>
        public string TantoSname { get; set; }

        /// <summary>
        /// 診療識別
        /// </summary>
        public string SinId { get; set; }

        /// <summary>
        /// 診療行為区分
        /// </summary>
        public string SinKouiKbn { get; set; }

        /// <summary>
        /// 診療行為区分名称
        /// </summary>
        public string SinKouiKbnName
        {
            get
            {
                switch (SinKouiKbn)
                {
                    case "11": return "初診";
                    case "12": return "再診";
                    case "13": return "医学管理";
                    case "14": return "在宅";
                    case "20": return "投薬";
                    case "21": return "内用薬";
                    case "23": return "外用薬";
                    case "2x": return "他薬";
                    case "25": return "処方料";
                    case "26": return "麻毒加算";
                    case "27": return "調基";
                    case "28": return "自己注射";
                    case "30": return "注射薬";
                    case "31": return "皮下筋";
                    case "32": return "静脈内";
                    case "33": return "点滴";
                    case "34": return "他注";
                    case "40": return "処置";
                    case "50": return "手術";
                    case "54": return "麻酔";
                    case "60": return "検査";
                    case "61": return "検体検査";
                    case "62": return "生体検査";
                    case "64": return "病理診断";
                    case "70": return "画像診断";
                    case "77": return "フィルム";
                    case "80": return "その他";
                    case "81": return "リハビリ";
                    case "82": return "精神";
                    case "83": return "処方箋料";
                    case "84": return "放射線";
                    case "96": return "保険外";
                    case "99": return "コメント";
                    case "T": return "特材";
                }
                return "";
            }
        }

        /// <summary>
        /// 診療行為コード
        /// </summary>
        public string ItemCd { get; set; }

        /// <summary>
        /// 診療行為名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 単価
        /// </summary>
        public string Ten { get; set; }

        /// <summary>
        /// 単価(単位)
        /// </summary>
        public string TenUnit { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Suryo { get; set; }

        /// <summary>
        /// 単位名称
        /// </summary>
        //public string UnitName { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public string Money { get; set; }

        /// <summary>
        /// 全体比率
        /// </summary>
        public string Rate { get; set; }

        /// <summary>
        /// グループ比率
        /// </summary>
        public string GrpRate { get; set; }

        /// <summary>
        /// 麻毒区分
        /// </summary>
        public int MadokuKbn { get; set; }

        /// <summary>
        /// 麻毒区分略称
        /// </summary>
        public string MadokuKbnSname
        {
            get
            {
                switch (MadokuKbn)
                {
                    case 1: return "麻";
                    case 2: return "毒";
                    case 3: return "覚";
                    case 5: return "向";
                }
                return "";
            }
        }

        /// <summary>
        /// 麻毒区分名称
        /// </summary>
        public string MadokuKbnName
        {
            get
            {
                switch (MadokuKbn)
                {
                    case 1: return "麻薬";
                    case 2: return "毒薬";
                    case 3: return "覚せい剤原料";
                    case 5: return "向精神薬";
                }
                return "";
            }
        }

        /// <summary>
        /// 向精神薬区分
        /// </summary>
        public int KouseisinKbn { get; set; }

        /// <summary>
        /// 向精神薬区分略称
        /// </summary>
        public string KouseisinKbnSname
        {
            get
            {
                switch (KouseisinKbn)
                {
                    case 1: return "不";
                    case 2: return "睡";
                    case 3: return "う";
                    case 4: return "精";
                }
                return "";
            }
        }

        /// <summary>
        /// 向精神薬区分名称
        /// </summary>
        public string KouseisinKbnName
        {
            get
            {
                switch (KouseisinKbn)
                {
                    case 1: return "抗不安薬";
                    case 2: return "睡眠薬";
                    case 3: return "抗うつ薬";
                    case 4: return "抗精神病薬";
                }
                return "";
            }
        }

        /// <summary>
        /// 課税区分
        /// </summary>
        public int KazeiKbn { get; set; }

        /// <summary>
        /// 課税区分名称
        /// </summary>
        public string KazeiKbnName
        {
            get
            {
                switch (KazeiKbn)
                {
                    case 0: return "";  //"非課税";
                    case 1: return "外税";
                    case 2: return "外税(減)";
                    case 3: return "内税";
                    case 4: return "内税(減)";
                }
                return "";
            }
        }

        /// <summary>
        /// 来院数
        /// </summary>
        public string RaiinCount { get; set; }

        /// <summary>
        /// 実人数
        /// </summary>
        public string PtCount { get; set; }
    }
}
