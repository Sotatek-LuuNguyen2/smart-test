﻿using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP40WelfareSeikyuCoReportService
    {
        CommonReportingRequestModel GetP40WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
