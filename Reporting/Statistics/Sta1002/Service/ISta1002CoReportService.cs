﻿using Reporting.Mappers.Common;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta1002.Models;

namespace Reporting.Statistics.Sta1002.Service;

public interface ISta1002CoReportService
{
    CommonReportingRequestModel GetSta1002ReportingData(CoSta1002PrintConf printConf, int hpId);

    List<string> ExportCsv(CoSta1002PrintConf printConf, int hpId);
}
