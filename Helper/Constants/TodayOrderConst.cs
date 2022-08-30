﻿namespace Helper.Constants
{
    public class TodayOrderConst
    {
        public enum TodayOrdValidationStatus
        {
            InvalidSpecialItem = 1,
            InvalidSpecialStadardUsage,
            InvalidOdrKouiKbn,
            InvalidSpecialSuppUsage,
            InvalidHasUsageButNotDrug,
            InvalidHasUsageButNotInjectionOrDrug,
            InvalidHasDrugButNotUsage,
            InvalidHasInjectionButNotUsage,
            InvalidHasNotBothInjectionAndUsageOf28,
            InvalidStandardUsageOfDrugOrInjection,
            InvalidSuppUsageOfDrugOrInjection,
            InvalidBunkatu,
            InvalidUsageWhenBuntakuNull,
            InvalidSumBunkatuDifferentSuryo,
            InvalidQuantityUnit,
            InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull,
            InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu,
            InvalidPrice,
            InvalidSuryoOfReffill,
            InvalidCmt840,
            InvalidCmt842,
            InvalidCmt842CmtOptMoreThan38,
            InvalidCmt830CmtOpt,
            InvalidCmt830CmtOptMoreThan38,
            InvalidCmt831,
            InvalidCmt850Date,
            InvalidCmt850OtherDate,
            InvalidCmt851,
            InvalidCmt852,
            InvalidCmt853,
            InvalidCmt880,
            DuplicateTodayOrd,
            InvalidKohatuKbn,
            InvalidDrugKbn,
            Valid
        };

        public static Dictionary<string, int> OdrKouiKbns { get; } = new Dictionary<string, int>()
        {
            {"未設定", 0 },
            {"初再診", 10},
            {"初診料", 11},
            {"再診料", 12},
            {"医学管理", 13},
            {"在宅", 14},
            {"投薬", 20},
            {"内服", 21},
            {"頓服", 22},
            {"外用", 23},
            {"調剤料", 24},
            {"処方料", 25},
            {"麻毒加算", 26},
            {"調剤技術基本料", 27},
            {"自己注射", 28},
            {"注射", 30},
            {"皮下筋肉注射", 31},
            {"静脈注射", 32},
            {"点滴注射", 33},
            {"その他注射", 34},
            {"処置", 40},
            {"手術", 50},
            {"輸血", 52},
            {"麻酔", 54},
            {"検査", 60},
            {"検体検査", 61},
            {"生体検査", 62},
            {"病理診断", 64},
            {"画像診断", 70},
            {"フィルム", 77},
            {"その他", 80},
            {"リハビリ", 81},
            {"精神", 82},
            {"処方箋料", 83},
            {"放射線", 84},
            {"入院料", 90},
            {"特定入院料", 92},
            {"老人一部負担金", 93},
            {"", 95},
            {"保険外医療", 96},
            {"食事療養・生活療養・標準負担", 97},
            {"コメント", 99},
            {"コメント（処方箋)", 100},
            {"コメント（処方箋備考)", 101},
        };

        public static Dictionary<string, int> KohatuKbns { get; } = new Dictionary<string, int>()
        {
            {"後発医薬品のない先発医薬品", 0 },
            {"先発医薬品がある後発医薬品である", 1},
            {"後発医薬品がある先発医薬品である", 2},
            {"先発医薬品のない後発医薬品である", 7},
        };

        public static Dictionary<string, int> DrugKbns { get; } = new Dictionary<string, int>()
        {
            {"薬剤以外", 0 },
            {"内用薬", 1},
            {"その他", 3},
            {"注射薬", 4},
            {"外用薬", 6},
            {"歯科用薬剤", 8},
        };
    }
}
