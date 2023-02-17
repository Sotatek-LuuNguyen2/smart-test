﻿using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Receipt;
using EmrCloudApi.Requests.Receipt;
using EmrCloudApi.Requests.Receipt.RequestItem;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Receipt;
using UseCase.Receipt.GetListSyobyoKeika;
using UseCase.Receipt.GetListSyoukiInf;
using UseCase.Receipt.GetReceCmt;
using UseCase.Receipt.GetReceHenReason;
using UseCase.Receipt.GetReceiCheckList;
using UseCase.Receipt.ReceiptListAdvancedSearch;
using UseCase.Receipt.SaveListReceCmt;
using UseCase.Receipt.SaveListSyobyoKeika;
using UseCase.Receipt.SaveListSyoukiInf;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class ReceiptController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    public ReceiptController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpPost(ApiPath.GetList)]
    public ActionResult<Response<ReceiptListAdvancedSearchResponse>> GetList([FromBody] ReceiptListAdvancedSearchRequest request)
    {
        var input = ConvertToReceiptListAdvancedSearchInputData(HpId, request);
        var output = _bus.Handle(input);

        var presenter = new ReceiptListAdvancedSearchPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<ReceiptListAdvancedSearchResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetReceCmtList)]
    public ActionResult<Response<GetReceCmtListResponse>> GetListReceCmt([FromQuery] GetReceCmtListRequest request)
    {
        var input = new GetReceCmtListInputData(HpId, request.SinYm, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetReceCmtListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceCmtListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveReceCmtList)]
    public ActionResult<Response<SaveReceCmtListResponse>> SaveListReceCmt([FromBody] SaveReceCmtListRequest request)
    {
        var listReceCmtItem = request.ReceCmtList.Select(item => ConvertToReceCmtItem(item)).ToList();
        var input = new SaveReceCmtListInputData(HpId, UserId, request.PtId, request.SinYm, request.HokenId, listReceCmtItem);
        var output = _bus.Handle(input);

        var presenter = new SaveReceCmtListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveReceCmtListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSyoukiInfList)]
    public ActionResult<Response<GetSyoukiInfListResponse>> GetListSyoukiInf([FromQuery] GetSyoukiInfListRequest request)
    {
        var input = new GetSyoukiInfListInputData(HpId, request.SinYm, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetSyoukiInfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSyoukiInfListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSyobyoKeikaList)]
    public ActionResult<Response<GetSyobyoKeikaListResponse>> GetListSyobyoKeika([FromQuery] GetSyobyoKeikaListRequest request)
    {
        var input = new GetSyobyoKeikaListInputData(HpId, request.SinYm, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetSyobyoKeikaListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSyobyoKeikaListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveSyoukiInfList)]
    public ActionResult<Response<SaveSyoukiInfListResponse>> SaveListSyoukiInf([FromBody] SaveSyoukiInfListRequest request)
    {
        var listReceSyoukiInf = request.SyoukiInfList.Select(item => ConvertToSyoukiInfItem(item)).ToList();
        var input = new SaveSyoukiInfListInputData(HpId, UserId, request.PtId, request.SinYm, request.HokenId, listReceSyoukiInf);
        var output = _bus.Handle(input);

        var presenter = new SaveSyoukiInfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveSyoukiInfListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveSyobyoKeikaList)]
    public ActionResult<Response<SaveSyobyoKeikaListResponse>> SaveListSyobyoKeika([FromBody] SaveSyobyoKeikaListRequest request)
    {
        var listReceSyoukiInf = request.SyobyoKeikaList.Select(item => ConvertToSyobyoKeikaItem(item)).ToList();
        var input = new SaveSyobyoKeikaListInputData(HpId, UserId, request.PtId, request.SinYm, request.HokenId, listReceSyoukiInf);
        var output = _bus.Handle(input);

        var presenter = new SaveSyobyoKeikaListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveSyobyoKeikaListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetReceHenReason)]
    public ActionResult<Response<GetReceHenReasonResponse>> GetReceHenReason([FromQuery] GetReceHenReasonRequest request)
    {
        var input = new GetReceHenReasonInputData(HpId, request.SeikyuYm, request.SinDate, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetReceHenReasonPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceHenReasonResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetReceiCheckList)]
    public ActionResult<Response<GetReceiCheckListResponse>> GetReceiCheckList([FromQuery] GetReceiCheckListRequest request)
    {
        var input = new GetReceiCheckListInputData(HpId, request.SinYm, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetReceiCheckListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceiCheckListResponse>>(presenter.Result);
    }

    #region Private function
    private ReceiptListAdvancedSearchInputData ConvertToReceiptListAdvancedSearchInputData(int hpId, ReceiptListAdvancedSearchRequest request)
    {
        var itemList = request.ItemList.Select(item => new ItemSearchInputItem(
                                                                                    item.ItemCd,
                                                                                    item.InputName,
                                                                                    item.RangeSeach,
                                                                                    item.Amount,
                                                                                    item.OrderStatus,
                                                                                    item.IsComment
                                                                               )).ToList();

        var byomeiCdList = request.ByomeiCdList.Select(item => new SearchByoMstInputItem(
                                                                                            item.ByomeiCd,
                                                                                            item.InputName,
                                                                                            item.IsComment
                                                                                        )).ToList();

        return new ReceiptListAdvancedSearchInputData(
                hpId,
                request.SeikyuYm,
                request.Tokki,
                request.IsAdvanceSearch,
                request.HokenSbts,
                request.IsAll,
                request.IsNoSetting,
                request.IsSystemSave,
                request.IsSave1,
                request.IsSave2,
                request.IsSave3,
                request.IsTempSave,
                request.IsDone,
                request.ReceSbtCenter,
                request.ReceSbtRight,
                request.HokenHoubetu,
                request.Kohi1Houbetu,
                request.Kohi2Houbetu,
                request.Kohi3Houbetu,
                request.Kohi4Houbetu,
                request.IsIncludeSingle,
                request.HokensyaNoFrom,
                request.HokensyaNoTo,
                request.HokensyaNoFromLong,
                request.HokensyaNoToLong,
                request.PtId,
                request.PtIdFrom,
                request.PtIdTo,
                request.PtSearchOption,
                request.TensuFrom,
                request.TensuTo,
                request.LastRaiinDateFrom,
                request.LastRaiinDateTo,
                request.BirthDayFrom,
                request.BirthDayTo,
                itemList,
                request.ItemQuery,
                request.IsOnlySuspectedDisease,
                request.ByomeiQuery,
                byomeiCdList,
                request.IsFutanIncludeSingle,
                request.FutansyaNoFromLong,
                request.FutansyaNoToLong,
                request.KaId,
                request.DoctorId,
                request.Name,
                request.IsTestPatientSearch,
                request.IsNotDisplayPrinted,
                request.GroupSearchModels,
                request.SeikyuKbnAll,
                request.SeikyuKbnDenshi,
                request.SeikyuKbnPaper
            );
    }

    private ReceCmtItem ConvertToReceCmtItem(SaveReceCmtRequestItem requestItem)
    {
        return new ReceCmtItem(
                                    requestItem.Id,
                                    requestItem.SeqNo,
                                    requestItem.CmtKbn,
                                    requestItem.CmtSbt,
                                    requestItem.Cmt,
                                    requestItem.CmtData,
                                    requestItem.ItemCd,
                                    requestItem.IsDeleted
                               );
    }

    private SyoukiInfItem ConvertToSyoukiInfItem(SaveSyoukiInfRequestItem requestItem)
    {
        return new SyoukiInfItem(
                                    requestItem.SeqNo,
                                    requestItem.SortNo,
                                    requestItem.SyoukiKbn,
                                    requestItem.SyoukiKbnStartYm,
                                    requestItem.Syouki,
                                    requestItem.IsDeleted
                               );
    }

    private SyobyoKeikaItem ConvertToSyobyoKeikaItem(SaveSyobyoKeikaRequestItem item)
    {
        return new SyobyoKeikaItem(
                                      item.SinDay,
                                      item.SeqNo,
                                      item.Keika,
                                      item.IsDeleted
                                  );
    }
    #endregion
}
