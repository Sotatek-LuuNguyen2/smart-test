﻿using Helper.Extension;
using Reporting.DailyStatic.DB;
using Reporting.DailyStatic.Enum;
using Reporting.DailyStatic.Model;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta1001.Service;
using Reporting.Statistics.Sta1002.Models;
using Reporting.Statistics.Sta1002.Service;
using Reporting.Statistics.Sta1010.Models;
using Reporting.Statistics.Sta1010.Service;
using Reporting.Statistics.Sta2001.Models;
using Reporting.Statistics.Sta2001.Service;
using Reporting.Statistics.Sta2002.Models;
using Reporting.Statistics.Sta2002.Service;
using Reporting.Statistics.Sta2003.Models;
using Reporting.Statistics.Sta2003.Service;
using Reporting.Statistics.Sta2010.Models;
using Reporting.Statistics.Sta2010.Service;
using Reporting.Statistics.Sta2011.Models;
using Reporting.Statistics.Sta2011.Service;
using Reporting.Statistics.Sta2021.Models;
using Reporting.Statistics.Sta2021.Service;

namespace Reporting.DailyStatic.Service;

public class StatisticService : IStatisticService
{
    private readonly IDailyStatisticCommandFinder _finder;
    private readonly ISta1002CoReportService _sta1002CoReportService;
    private readonly ISta1010CoReportService _sta1010CoReportService;
    private readonly ISta2001CoReportService _sta2001CoReportService;
    private readonly ISta2003CoReportService _sta2003CoReportService;
    private readonly ISta1001CoReportService _sta1001CoReportService;
    private readonly ISta2002CoReportService _sta2002CoReportService;
    private readonly ISta2010CoReportService _sta2010CoReportService;
    private readonly ISta2011CoReportService _sta2011CoReportService;
    private readonly ISta2021CoReportService _sta2021CoReportService;

    public StatisticService(IDailyStatisticCommandFinder finder, ISta1002CoReportService sta1002CoReportService, ISta1010CoReportService sta1010CoReportService, ISta2001CoReportService sta2001CoReportService, ISta2003CoReportService sta2003CoReportService, ISta1001CoReportService sta1001CoReportService, ISta2002CoReportService sta2002CoReportService, ISta2010CoReportService sta2010CoReportService, ISta2011CoReportService sta2011CoReportService, ISta2021CoReportService sta2021CoReportService)
    {
        _finder = finder;
        _sta1002CoReportService = sta1002CoReportService;
        _sta1010CoReportService = sta1010CoReportService;
        _sta2001CoReportService = sta2001CoReportService;
        _sta2003CoReportService = sta2003CoReportService;
        _sta1001CoReportService = sta1001CoReportService;
        _sta2002CoReportService = sta2002CoReportService;
        _sta2010CoReportService = sta2010CoReportService;
        _sta2011CoReportService = sta2011CoReportService;
        _sta2021CoReportService = sta2021CoReportService;
    }

    public CommonReportingRequestModel PrintExecute(int hpId, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo)
    {
        var configDaily = _finder.GetDailyConfigStatisticMenu(hpId, menuId);

        switch ((StatisticReportType)configDaily.ReportId)
        {
            case StatisticReportType.Sta1001:
                return PrintSta1001(hpId, configDaily, dateFrom, dateTo, timeFrom, timeTo);
            case StatisticReportType.Sta1002:
                return PrintSta1002(hpId, configDaily, dateFrom, dateTo, timeFrom, timeTo);
            case StatisticReportType.Sta1010:
                return PrintSta1010(hpId, configDaily, dateFrom, dateTo, timeFrom, timeTo);
            case StatisticReportType.Sta2001:
                return PrintSta2001(hpId, configDaily, monthFrom, monthTo);
            case StatisticReportType.Sta2002:
                return PrintSta2002(hpId, configDaily, monthFrom, monthTo);
            case StatisticReportType.Sta2003:
                return PrintSta2003(hpId, configDaily, monthFrom, monthTo);
            case StatisticReportType.Sta2010:
                return PrintSta2010(hpId, configDaily, monthFrom);
            case StatisticReportType.Sta2011:
                return PrintSta2011(hpId, configDaily, monthFrom);
            case StatisticReportType.Sta2021:
                return PrintSta2021(hpId, configDaily, monthFrom, monthTo);
        }
        return new();
    }

    #region Print Report
    private CommonReportingRequestModel PrintSta1001(int hpId, ConfigStatisticModel configDaily, int dateFrom, int dateTo, int timeFrom, int timeTo)
    {
        CoSta1001PrintConf printConf = CreateCoSta1001PrintConf(configDaily, dateFrom, dateTo, timeFrom, timeTo);
        return _sta1001CoReportService.GetSta1001ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta1002(int hpId, ConfigStatisticModel configDaily, int dateFrom, int dateTo, int timeFrom, int timeTo)
    {
        CoSta1002PrintConf printConf = CreateCoSta1002PrintConf(configDaily, dateFrom, dateTo, timeFrom, timeTo);
        return _sta1002CoReportService.GetSta1002ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta1010(int hpId, ConfigStatisticModel configDaily, int dateFrom, int dateTo, int timeFrom, int timeTo)
    {
        var printConf = CreateCoSta1010PrintConf(configDaily, dateFrom, dateTo, timeFrom, timeTo);
        return _sta1010CoReportService.GetSta1010ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta2001(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo)
    {
        var printConf = CreateCoSta2001PrintConf(configDaily, monthFrom, monthTo);
        return _sta2001CoReportService.GetSta2001ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta2002(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo)
    {
        var printConf = CreateCoSta2002PrintConf(configDaily, monthFrom, monthTo);
        return _sta2002CoReportService.GetSta2002ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta2003(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo)
    {
        var printConf = CreateCoSta2003PrintConf(configDaily, monthFrom, monthTo);
        return _sta2003CoReportService.GetSta2003ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta2010(int hpId, ConfigStatisticModel configDaily, int monthFrom)
    {
        var printConf = CreateCoSta2010PrintConf(configDaily, monthFrom);
        return _sta2010CoReportService.GetSta2010ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta2011(int hpId, ConfigStatisticModel configDaily, int monthFrom)
    {
        var printConf = CreateCoSta2011PrintConf(configDaily, monthFrom);
        return _sta2011CoReportService.GetSta2011ReportingData(printConf, hpId);
    }

    private CommonReportingRequestModel PrintSta2021(int hpId, ConfigStatisticModel configDaily, int monthFrom, int monthTo)
    {
        var printConf = CreateCoSta2021PrintConf(configDaily.ConfigStatistic2021, monthFrom, monthTo);
        return _sta2021CoReportService.GetSta2021ReportingData(printConf, hpId);

    }
    #endregion

    #region Create CoStatistic Print
    private CoSta1001PrintConf CreateCoSta1001PrintConf(ConfigStatisticModel configDaily,
                                                             int dateFrom,
                                                             int dateTo,
                                                             int timeFrom,
                                                             int timeTo)

    {
        CoSta1001PrintConf printConf = new CoSta1001PrintConf(configDaily.MenuId);
        printConf.StartNyukinDate = dateFrom;
        printConf.EndNyukinDate = dateTo;

        if (configDaily.TimeDailyFrom > 0 || configDaily.TimeDailyTo > 0)
        {
            printConf.StartNyukinTime = configDaily.TimeDailyFrom;
            printConf.EndNyukinTime = configDaily.TimeDailyTo;
        }
        else
        {
            printConf.StartNyukinTime = timeFrom;
            printConf.EndNyukinTime = timeTo;
        }

        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.PageBreak3 = configDaily.BreakPage3;
        printConf.SortOrder1 = configDaily.SortOrder1;
        printConf.SortOrder2 = configDaily.SortOrder2;
        printConf.SortOrder3 = configDaily.SortOrder3;
        printConf.SortOpt1 = configDaily.OrderBy1;
        printConf.SortOpt2 = configDaily.OrderBy2;
        printConf.SortOpt3 = configDaily.OrderBy3;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.IsExcludeUnpaid = configDaily.ExcludingUnpaid == 1;
        printConf.UketukeSbtIds = configDaily.UketukeKbnId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.PaymentMethodCds = configDaily.PaymentKbn.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        return printConf;
    }

    private CoSta1002PrintConf CreateCoSta1002PrintConf(ConfigStatisticModel configDaily,
                                                                 int dateFrom,
                                                                 int dateTo,
                                                                 int timeFrom,
                                                                 int timeTo)

    {
        CoSta1002PrintConf printConf = new CoSta1002PrintConf(configDaily.MenuId);
        printConf.StartNyukinDate = dateFrom;
        printConf.EndNyukinDate = dateTo;

        if (configDaily.TimeDailyFrom > 0 || configDaily.TimeDailyTo > 0)
        {
            printConf.StartNyukinTime = configDaily.TimeDailyFrom;
            printConf.EndNyukinTime = configDaily.TimeDailyTo;
        }
        else
        {
            printConf.StartNyukinTime = timeFrom;
            printConf.EndNyukinTime = timeTo;
        }

        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.PageBreak3 = configDaily.BreakPage3;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.IsExcludeUnpaid = configDaily.ExcludingUnpaid == 1;
        printConf.UketukeSbtIds = configDaily.UketukeKbnId
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        printConf.KaIds = configDaily.KaId
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        printConf.TantoIds = configDaily.UserId
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        printConf.PaymentMethodCds = configDaily.PaymentKbn
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        return printConf;
    }

    private CoSta1010PrintConf CreateCoSta1010PrintConf(ConfigStatisticModel configDaily, int dateFrom, int dateTo, long ptNumFrom, long ptNumTo)
    {
        CoSta1010PrintConf printConf = new CoSta1010PrintConf(configDaily.MenuId);
        printConf.StartSinDate = dateFrom;
        printConf.EndSinDate = dateTo;
        printConf.StartPtNum = ptNumFrom;
        printConf.EndPtNum = ptNumTo;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.SortOrder1 = configDaily.SortOrder1;
        printConf.SortOrder2 = configDaily.SortOrder2;
        printConf.SortOrder3 = configDaily.SortOrder3;
        printConf.SortOpt1 = configDaily.OrderBy1;
        printConf.SortOpt2 = configDaily.OrderBy2;
        printConf.SortOpt3 = configDaily.OrderBy3;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.KaIds = configDaily.KaId
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        printConf.TantoIds = configDaily.UserId
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.AsInteger())
            .ToList();
        printConf.IsNewSeikyu = configDaily.InvoiceKbn == 1;
        printConf.IsDiffSeikyu = printConf.IsNewSeikyu && configDaily.OnlyPatientInvoiceChange == 1;
        printConf.IncludeOutRangeNyukin = configDaily.IncludeOutRangeNyukin == 1;
        printConf.IncludeUnpaid = configDaily.UnPaidVisit == 1;
        printConf.MisyuKbns = configDaily.GetListMisyuKbn();
        return printConf;
    }

    private CoSta2001PrintConf CreateCoSta2001PrintConf(ConfigStatisticModel configDaily,
                                                                int monthFrom,
                                                                int monthTo)

    {
        CoSta2001PrintConf printConf = new CoSta2001PrintConf(configDaily.MenuId);
        printConf.StartNyukinYm = monthFrom;
        printConf.EndNyukinYm = monthTo;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.IsExcludeUnpaid = configDaily.ExcludingUnpaid == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        return printConf;
    }

    private CoSta2003PrintConf CreateCoSta2003PrintConf(ConfigStatisticModel configDaily,
                                                                 int monthFrom,
                                                                 int monthTo)

    {
        CoSta2003PrintConf printConf = new CoSta2003PrintConf(configDaily.MenuId);
        printConf.StartNyukinYm = monthFrom;
        printConf.EndNyukinYm = monthTo;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.SortOrder1 = configDaily.SortOrder1;
        printConf.SortOpt1 = configDaily.OrderBy1;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.IsExcludeUnpaid = configDaily.ExcludingUnpaid == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.HokenSbts = configDaily.InsuranceType.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.IsTensu = configDaily.MedicalTreatment;
        printConf.IsJihiFutan = configDaily.NonInsuranceAmount;
        return printConf;
    }

    private CoSta2002PrintConf CreateCoSta2002PrintConf(ConfigStatisticModel configDaily, int monthFrom, int monthTo)
    {
        CoSta2002PrintConf printConf = new CoSta2002PrintConf(configDaily.MenuId);
        printConf.StartNyukinYm = monthFrom;
        printConf.EndNyukinYm = monthTo;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.IsExcludeUnpaid = configDaily.ExcludingUnpaid == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();

        return printConf;
    }

    private CoSta2010PrintConf CreateCoSta2010PrintConf(ConfigStatisticModel configDaily, int monthFrom)
    {
        CoSta2010PrintConf printConf = new CoSta2010PrintConf(configDaily.MenuId);
        printConf.SeikyuYm = monthFrom;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.MainHokensyaNo = configDaily.Designated == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.HokenSbts = configDaily.InsuranceType.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.SeikyuTypes = configDaily.TargetReceipt.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();

        return printConf;
    }

    public static CoSta2011PrintConf CreateCoSta2011PrintConf(ConfigStatisticModel configDaily, int monthFrom)

    {
        CoSta2011PrintConf printConf = new CoSta2011PrintConf(configDaily.MenuId);
        printConf.SeikyuYm = monthFrom;
        printConf.FormFileName = configDaily.FormReport;
        printConf.ReportName = configDaily.ReportName;
        printConf.PageBreak1 = configDaily.BreakPage1;
        printConf.PageBreak2 = configDaily.BreakPage2;
        printConf.PageBreak3 = configDaily.BreakPage3;
        printConf.IsTester = configDaily.TestPatient == 1;
        printConf.KaIds = configDaily.KaId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.TantoIds = configDaily.UserId.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.SeikyuTypes = configDaily.TargetReceipt.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.AsInteger()).ToList();
        printConf.IsZaitaku = configDaily.HomePatient == 1;
        printConf.IsUchiwake = configDaily.ShowBreakdown == 1;
        printConf.ZaitakuItems = configDaily.ItemInput.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToList();

        return printConf;
    }

    private CoSta2021PrintConf CreateCoSta2021PrintConf(ConfigStatistic2021Model configStatistic, int monthFrom, int monthTo)
    {
        CoSta2021PrintConf printConf = new CoSta2021PrintConf(configStatistic.MenuId);
        printConf.FormFileName = configStatistic.FormReport;
        printConf.ReportName = configStatistic.ReportName;
        printConf.IsTester = configStatistic.TestPatient == 1;

        printConf.StartSinYm = monthFrom;
        printConf.EndSinYm = monthTo;

        printConf.DataKind = configStatistic.DataKind;
        printConf.PageBreak1 = configStatistic.PageBreak1;
        printConf.PageBreak2 = configStatistic.PageBreak2;
        printConf.SortOrder1 = configStatistic.SortOrder1;
        printConf.SortOpt1 = configStatistic.SortOpt1;
        printConf.SortOrder2 = configStatistic.SortOrder2;
        printConf.SortOpt2 = configStatistic.SortOpt2;
        printConf.SortOrder3 = configStatistic.SortOrder3;
        printConf.SortOpt3 = configStatistic.SortOpt3;
        printConf.KaIds = configStatistic.ListKaId;
        printConf.TantoIds = configStatistic.ListTantoId;
        printConf.HokenSbts = configStatistic.ListHokenSbt;
        printConf.SinIds = configStatistic.ListSinId;
        printConf.SinKouiKbns = configStatistic.ListSinKouiKbn;
        printConf.MadokuKbns = configStatistic.ListMadokuKbn;
        printConf.KouseisinKbns = configStatistic.ListKouseisinKbn;
        printConf.ItemCds = configStatistic.ListItemCd;
        printConf.SearchWord = configStatistic.SearchWord;
        printConf.SearchOpt = configStatistic.SearchOpt;
        printConf.ItemSearchOpt = configStatistic.ItemCdOpt;
        printConf.InoutKbns = configStatistic.ListInoutKbn;
        printConf.KohatuKbns = configStatistic.ListKohatuKbn;
        printConf.IsAdopteds = configStatistic.ListIsAdopted;

        return printConf;
    }
    #endregion
}
