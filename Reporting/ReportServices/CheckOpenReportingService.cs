﻿using Reporting.Accounting.DB;
using Reporting.Accounting.Model;
using Reporting.Accounting.Service;
using Reporting.CommonMasters.Enums;
using Reporting.OrderLabel.Service;

namespace Reporting.ReportServices;

public class CheckOpenReportingService : ICheckOpenReportingService
{
    private readonly ICoAccountingFinder _coAccountingFinder;
    private readonly IAccountingCoReportService _accountingCoReportService;
    private readonly IReportService _reportService;

    public CheckOpenReportingService(ICoAccountingFinder coAccountingFinder, IAccountingCoReportService accountingCoReportService, IReportService reportService)
    {
        _coAccountingFinder = coAccountingFinder;
        _accountingCoReportService = accountingCoReportService;
        _reportService = reportService;
    }

    public bool CheckOpenAccountingForm(int hpId, long ptId, int printTypeInput, List<long> raiinNoList, List<long> raiinNoPayList, bool isCalculateProcess = false)
    {
        return _accountingCoReportService.CheckOpenReportingForm(hpId, ptId, printTypeInput, raiinNoList, raiinNoPayList, isCalculateProcess);
    }

    public bool CheckExistTemplate(string templateName, int printType)
    {
        return _accountingCoReportService.CheckExistTemplate(templateName, printType);
    }

    public bool CheckOpenAccountingForm(int hpId, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> multiAccountDueListModels, bool isPrintMonth, bool ryoshusho, bool meisai)
    {
        List<CoAccountingParamModel> requestAccountting = new();

        List<CoAccountDueListModel> nyukinModels = _coAccountingFinder.GetAccountDueList(hpId, ptId);
        List<int> months = new();
        foreach (var model in multiAccountDueListModels)
        {
            var selectedAccountDueListModel = model;
            var accountDueListModels = nyukinModels.FindAll(p => p.SinDate / 100 == model.SinDate / 100);
            if (isPrintMonth)
            {
                if (!months.Contains(model.SinDate / 100))
                {
                    var printItem = _reportService.PrintWithoutThread(ryoshusho, meisai, mode, ptId, accountDueListModels, selectedAccountDueListModel, isPrintMonth, model.SinDate, model.OyaRaiinNo, accountDueListModels);
                    requestAccountting.AddRange(printItem);
                    months.Add(model.SinDate / 100);
                }
            }
            else
            {
                var printItem = _reportService.PrintWithoutThread(ryoshusho, meisai, mode, ptId, accountDueListModels, selectedAccountDueListModel, isPrintMonth, model.SinDate, model.OyaRaiinNo);
                requestAccountting.AddRange(printItem);
            }
        }
        return _accountingCoReportService.CheckOpenReportingForm(hpId, requestAccountting);
    }
}
