﻿using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Types;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class DosageChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        public double CurrentHeight { get; set; }

        public double CurrentWeight { get; set; }

        public bool TermLimitCheckingOnly { get; set; }

        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            throw new NotImplementedException();
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            bool isMinCheck = SystemConfig.Instance.DosageMinCheckSetting;
            double ratioSetting = SystemConfig.Instance.DosageRatioSetting;
            List<DosageResultModel> resultList = new List<DosageResultModel>();
            List<TOdrInf> errorOrderList = new List<TOdrInf>();
            foreach (var checkingOrder in unitCheckerForOrderListResult.CheckingOrderList)
            {
                if (checkingOrder.OdrKouiKbn == 21 && !SystemConfig.Instance.DosageDrinkingDrugSetting ||
                checkingOrder.OdrKouiKbn == 22 && !SystemConfig.Instance.DosageDrugAsOrderSetting ||
                checkingOrder.OdrKouiKbn == 23 ||
                checkingOrder.OdrKouiKbn == 28 ||
                !new List<int>() { 21, 22, 23, 28 }.Contains(checkingOrder.OdrKouiKbn) && !SystemConfig.Instance.DosageOtherDrugSetting)
                {
                    continue;
                }

                double usageQuantity = 0;
                var usageItem = checkingOrder.(d => d.IsStandardUsage);
                if (usageItem != null)
                {
                    usageQuantity = usageItem.Suryo;
                }
                // Get listItemCode
                List<DrugInfo> itemList = checkingOrder.OdrInfDetailModelsIgnoreEmpty
                    .Where(i => i.DrugKbn > 0)
                    .Select(i => new DrugInfo()
                    {
                        ItemCD = i.ItemCd,
                        ItemName = i.ItemName,
                        Suryo = i.Suryo,
                        UnitName = i.UnitName,
                        TermVal = i.TermVal,
                        SinKouiKbn = checkingOrder.OdrKouiKbn,
                        UsageQuantity = usageQuantity
                    })
                    .ToList();

                if (itemList.Count == 0)
                {
                    continue;
                }

                List<DosageResultModel> checkedResult = Finder.CheckDosage(HpID, PtID, Sinday, itemList, isMinCheck, ratioSetting, CurrentHeight, CurrentWeight);

                if (TermLimitCheckingOnly)
                {
                    checkedResult = checkedResult.Where(r => r.LabelChecking == DosageLabelChecking.TermLimit).ToList();
                }

                if (checkedResult.Count > 0)
                {
                    errorOrderList.Add(checkingOrder);
                    resultList.AddRange(checkedResult);
                }
            }

            if (resultList.Count > 0)
            {
                unitCheckerForOrderListResult.ErrorInfo = resultList;
                unitCheckerForOrderListResult.ErrorOrderList = errorOrderList;
            }

            return unitCheckerForOrderListResult;
        }
    }
}
