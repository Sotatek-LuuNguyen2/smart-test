﻿using Reporting.Accounting.Model;
using Reporting.Accounting.Model.Output;
using Reporting.Accounting.Service;
using Reporting.AccountingCard.Service;
using Reporting.Byomei.Service;
using Reporting.CommonMasters.Enums;
using Reporting.DailyStatic.Service;
using Reporting.DrugInfo.Model;
using Reporting.DrugInfo.Service;
using Reporting.DrugNoteSeal.Service;
using Reporting.Karte1.Mapper;
using Reporting.Karte1.Service;
using Reporting.Kensalrai.Service;
using Reporting.Mappers.Common;
using Reporting.MedicalRecordWebId.Service;
using Reporting.Memo.Service;
using Reporting.NameLabel.Service;
using Reporting.OrderLabel.Model;
using Reporting.OrderLabel.Service;
using Reporting.OutDrug.Model.Output;
using Reporting.OutDrug.Service;
using Reporting.PatientManagement.Service;
using Reporting.Receipt.Service;
using Reporting.ReceiptCheck.Service;
using Reporting.ReceiptList.Model;
using Reporting.ReceiptList.Service;
using Reporting.ReceiptPrint.Service;
using Reporting.ReceTarget.Service;
using Reporting.Sijisen.Service;
using Reporting.SyojyoSyoki.Service;
using Reporting.Yakutai.Service;

namespace Reporting.ReportServices;

public class ReportService : IReportService
{
    private readonly IOrderLabelCoReportService _orderLabelCoReportService;
    private readonly IDrugInfoCoReportService _drugInfoCoReportService;
    private readonly ISijisenReportService _sijisenReportService;
    private readonly IByomeiService _byomeiService;
    private readonly IKarte1Service _karte1Service;
    private readonly INameLabelService _nameLabelService;
    private readonly IMedicalRecordWebIdReportService _medicalRecordWebIdReportService;
    private readonly IReceiptCheckCoReportService _receiptCheckCoReportService;
    private readonly IReceiptListCoReportService _receiptListCoReportService;
    private readonly IOutDrugCoReportService _outDrugCoReportService;
    private readonly IAccountingCoReportService _accountingCoReportService;
    private readonly IStatisticService _statisticService;
    private readonly IReceiptCoReportService _receiptCoReportService;
    private readonly IPatientManagementService _patientManagementService;
    private readonly ISyojyoSyokiCoReportService _syojyoSyokiCoReportService;
    private readonly IKensaIraiCoReportService _kensaIraiCoReportService;
    private readonly IReceiptPrintService _receiptPrintService;
    private readonly IMemoMsgCoReportService _memoMsgCoReportService;
    private readonly IReceTargetCoReportService _receTargetCoReportService;
    private readonly IDrugNoteSealCoReportService _drugNoteSealCoReportService;
    private readonly IYakutaiCoReportService _yakutaiCoReportService;
    private readonly IAccountingCardCoReportService _accountingCardCoReportService;

    public ReportService(IOrderLabelCoReportService orderLabelCoReportService, IDrugInfoCoReportService drugInfoCoReportService, ISijisenReportService sijisenReportService, IByomeiService byomeiService, IKarte1Service karte1Service, INameLabelService nameLabelService, IMedicalRecordWebIdReportService medicalRecordWebIdReportService, IReceiptCheckCoReportService receiptCheckCoReportService, IReceiptListCoReportService receiptListCoReportService, IOutDrugCoReportService outDrugCoReportService, IAccountingCoReportService accountingCoReportService, IStatisticService statisticService, IReceiptCoReportService receiptCoReportService, IPatientManagementService patientManagementService, ISyojyoSyokiCoReportService syojyoSyokiCoReportService, IKensaIraiCoReportService kensaIraiCoReportService, IReceiptPrintService receiptPrintService, IMemoMsgCoReportService memoMsgCoReportService, IReceTargetCoReportService receTargetCoReportService, IDrugNoteSealCoReportService drugNoteSealCoReportService, IYakutaiCoReportService yakutaiCoReportService, IAccountingCardCoReportService accountingCardCoReportService)
    {
        _orderLabelCoReportService = orderLabelCoReportService;
        _drugInfoCoReportService = drugInfoCoReportService;
        _sijisenReportService = sijisenReportService;
        _byomeiService = byomeiService;
        _karte1Service = karte1Service;
        _nameLabelService = nameLabelService;
        _medicalRecordWebIdReportService = medicalRecordWebIdReportService;
        _receiptCheckCoReportService = receiptCheckCoReportService;
        _receiptListCoReportService = receiptListCoReportService;
        _outDrugCoReportService = outDrugCoReportService;
        _accountingCoReportService = accountingCoReportService;
        _statisticService = statisticService;
        _receiptCoReportService = receiptCoReportService;
        _patientManagementService = patientManagementService;
        _syojyoSyokiCoReportService = syojyoSyokiCoReportService;
        _kensaIraiCoReportService = kensaIraiCoReportService;
        _receiptPrintService = receiptPrintService;
        _memoMsgCoReportService = memoMsgCoReportService;
        _receTargetCoReportService = receTargetCoReportService;
        _drugNoteSealCoReportService = drugNoteSealCoReportService;
        _yakutaiCoReportService = yakutaiCoReportService;
        _accountingCardCoReportService = accountingCardCoReportService;
    }



    //Byomei
    public CommonReportingRequestModel GetByomeiReportingData(long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds)
    {
        return _byomeiService.GetByomeiReportingData(ptId, fromDay, toDay, tenkiIn, hokenIds);
    }

    //Karte1
    public Karte1Mapper GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
    {
        return _karte1Service.GetKarte1ReportingData(hpId, ptId, sinDate, hokenPid, tenkiByomei, syuByomei);
    }

    //NameLabel
    public CommonReportingRequestModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate)
    {
        return _nameLabelService.GetNameLabelReportingData(ptId, kanjiName, sinDate);
    }

    //Sijisen
    public CommonReportingRequestModel GetSijisenReportingData(int hpId, int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr)
    {
        return _sijisenReportService.GetSijisenReportingData(hpId, formType, ptId, sinDate, raiinNo, odrKouiKbns, printNoOdr);
    }

    //OrderLabel
    public CommonReportingRequestModel GetOrderLabelReportingData(int mode, int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, List<RsvkrtOdrInfModel> rsvKrtOdrInfModels)
    {
        return _orderLabelCoReportService.GetOrderLabelReportingData(mode, hpId, ptId, sinDate, raiinNo, odrKouiKbns, rsvKrtOdrInfModels);
    }

    //OrderInfo
    public DrugInfoData SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _drugInfoCoReportService.SetOrderInfo(hpId, ptId, sinDate, raiinNo);
    }

    //MedicalRecordWebId
    public CommonReportingRequestModel GetMedicalRecordWebIdReportingData(int hpId, long ptId, int sinDate)
    {
        return _medicalRecordWebIdReportService.GetMedicalRecordWebIdReportingData(hpId, ptId, sinDate);
    }

    //ReceiptCheckCoReport
    public CommonReportingRequestModel GetReceiptCheckCoReportService(int hpId, List<long> ptIds, int seikyuYm)
    {
        return _receiptCheckCoReportService.GetReceiptCheckCoReportingData(hpId, ptIds, seikyuYm);
    }

    //ReceiptListCoReport
    public CommonReportingRequestModel GetReceiptListReportingData(int hpId, int seikyuYm, List<ReceiptInputModel> receiptListModels)
    {
        return _receiptListCoReportService.GetReceiptListReportingData(hpId, seikyuYm, receiptListModels);
    }

    public CommonReportingRequestModel GetSyojyoSyokiReportingData(int hpId, long ptId, int seikyuYm, int hokenId)
    {
        return _syojyoSyokiCoReportService.GetSyojyoSyokiReportingData(hpId, ptId, seikyuYm, hokenId);
    }

    /// <summary>
    /// OutDrug
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="sinDate"></param>
    /// <param name="raiinNo"></param>
    /// <returns></returns>
    public CoOutDrugReportingOutputData GetOutDrugReportingData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _outDrugCoReportService.GetOutDrugReportingData(hpId, ptId, sinDate, raiinNo);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="raiinNos"></param>
    /// <param name="hokenId"></param>
    /// <param name="miseisanKbn"></param>
    /// <param name="saiKbn"></param>
    /// <param name="misyuKbn"></param>
    /// <param name="seikyuKbn"></param>
    /// <param name="hokenKbn"></param>
    /// <param name="hokenSeikyu"></param>
    /// <param name="jihiSeikyu"></param>
    /// <param name="nyukinBase"></param>
    /// <param name="hakkoDay"></param>
    /// <param name="memo"></param>
    /// <param name="printType"></param>
    /// <param name="formFileName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public AccountingResponse GetAccountingReportingData(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId = 0, int miseisanKbn = 0, int saiKbn = 0, int misyuKbn = 0, int seikyuKbn = 1, int hokenKbn = 0, bool hokenSeikyu = false, bool jihiSeikyu = false, bool nyukinBase = false, int hakkoDay = 0, string memo = "", int printType = 0, string formFileName = "")
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, ptId, startDate, endDate, raiinNos, hokenId, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hokenSeikyu, jihiSeikyu, nyukinBase, hakkoDay, memo, printType, formFileName);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="ptConditions"></param>
    /// <param name="grpConditions"></param>
    /// <param name="sort"></param>
    /// <param name="miseisanKbn"></param>
    /// <param name="saiKbn"></param>
    /// <param name="misyuKbn"></param>
    /// <param name="seikyuKbn"></param>
    /// <param name="hokenKbn"></param>
    /// <param name="hakkoDay"></param>
    /// <param name="memo"></param>
    /// <param name="formFileName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public AccountingResponse GetAccountingReportingData(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions, int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, int hakkoDay, string memo, string formFileName)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, startDate, endDate, ptConditions, grpConditions, sort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hakkoDay, memo, formFileName);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="coAccountingParamModels"></param>
    /// <returns></returns>
    public AccountingResponse GetAccountingReportingData(int hpId, List<CoAccountingParamModel> coAccountingParamModels)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, coAccountingParamModels);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="printTypeInput"></param>
    /// <param name="raiinNoList"></param>
    /// <param name="raiinNoPayList"></param>
    /// <param name="isCalculateProcess"></param>
    /// <returns></returns>
    public AccountingResponse GetAccountingReportingData(int hpId, long ptId, int printTypeInput, List<long> raiinNoList, List<long> raiinNoPayList, bool isCalculateProcess)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, ptId, printTypeInput, raiinNoList, raiinNoPayList, isCalculateProcess);
    }

    public CommonReportingRequestModel GetStatisticReportingData(int hpId, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo, CoFileType? coFileType = null, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0)
    {
        return _statisticService.PrintExecute(hpId, menuId, monthFrom, monthTo, dateFrom, dateTo, timeFrom, timeTo, coFileType, isPutTotalRow, tenkiDateFrom, tenkiDateTo, enableRangeFrom, enableRangeTo, ptNumFrom, ptNumTo);
    }

    public CommonReportingRequestModel GetPatientManagement(int hpId, int menuId)
    {
        return _patientManagementService.PrintData(hpId, menuId);
    }

    //Receipt Preview
    public CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId)
    {
        return _receiptCoReportService.GetReceiptData(hpId, ptId, sinYm, hokenId);
    }

    public CommonReportingRequestModel GetKensalraiData(int hpId, int systemDate, int fromDate, int toDate, string centerCd)
    {
        return _kensaIraiCoReportService.GetKensalraiData(hpId, systemDate, fromDate, toDate, centerCd);
    }

    public CommonReportingRequestModel GetReceiptPrint(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int ptId, int seikyuYm, int sinYm, int hokenId)
    {
        return _receiptPrintService.GetReceiptPrint(hpId, prefNo, reportId, reportEdaNo, dataKbn, ptId, seikyuYm, sinYm, hokenId);
    }

    public CommonReportingRequestModel GetMemoMsgReportingData(string reportName, string title, List<string> listMessage)
    {
        return _memoMsgCoReportService.GetMemoMsgReportingData(reportName, title, listMessage);
    }

    public CommonReportingRequestModel GetReceTargetPrint(int hpId, int seikyuYm)
    {
        return _receTargetCoReportService.GetReceTargetPrintData(hpId, seikyuYm);
    }

    public CommonReportingRequestModel GetDrugNoteSealPrintData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _drugNoteSealCoReportService.GetDrugNoteSealPrintData(hpId, ptId, sinDate, raiinNo);
    }

    public CommonReportingRequestModel GetYakutaiReportingData(int hpId, long ptId, int sinDate, int raiinNo)
    {
        return _yakutaiCoReportService.GetYakutaiReportingData(hpId, ptId, sinDate, raiinNo);
    }

    public CommonReportingRequestModel GetAccountingCardReportingData(int hpId, long ptId, int sinYm, int hokenId, bool includeOutDrug)
    {
        return _accountingCardCoReportService.GetAccountingCardReportingData(hpId, ptId, sinYm, hokenId, includeOutDrug);
    }
}
