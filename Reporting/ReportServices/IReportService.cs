﻿using Reporting.DrugInfo.Model;
using Reporting.Karte1.Mapper;
using Reporting.Mappers.Common;
using Reporting.OrderLabel.Model;
using Reporting.ReceiptList.Model;

namespace Reporting.ReportServices;

public interface IReportService
{
    CommonReportingRequestModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate);

    Karte1Mapper GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);

    DrugInfoData SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo);

    CommonReportingRequestModel GetByomeiReportingData(long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds);

    CommonReportingRequestModel GetSijisenReportingData(int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr);

    CommonReportingRequestModel GetOrderLabelReportingData(int mode, int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, List<RsvkrtOdrInfModel> rsvKrtOdrInfModels);

    CommonReportingRequestModel GetMedicalRecordWebIdReportingData(int hpId, long ptId, int sinDate);

    CommonReportingRequestModel GetReceiptCheckCoReportService(int hpId, List<long> ptIds, int seikyuYm);

    CommonReportingRequestModel GetReceiptListReportingData(int hpId, int seikyuYm, List<ReceiptInputModel> receiptListModels);
}
