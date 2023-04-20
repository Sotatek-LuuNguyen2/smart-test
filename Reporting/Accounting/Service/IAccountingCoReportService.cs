﻿using Reporting.Accounting.Model;
using Reporting.Accounting.Model.Output;

namespace Reporting.Accounting.Service;

public interface IAccountingCoReportService
{
    AccountingOutputModel GetAccountingReportingData(
            int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId = 0,
            int miseisanKbn = 0, int saiKbn = 0, int misyuKbn = 0, int seikyuKbn = 1, int hokenKbn = 0,
            bool hokenSeikyu = false, bool jihiSeikyu = false, bool nyukinBase = false,
            int hakkoDay = 0, string memo = "", int printType = 0, string formFileName = "");

    AccountingOutputModel GetAccountingReportingData(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions,
            int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn,
            int hakkoDay, string memo, string formFileName);

    AccountingOutputModel GetAccountingReportingData(int hpId, List<CoAccountingParamModel> coAccountingParamModels);

}
