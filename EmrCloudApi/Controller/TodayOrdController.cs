﻿using Domain.Models.MstItem;
using EmrCloudApi.Constants;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.InsuranceList;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.Insurance;
using EmrCloudApi.Requests.MedicalExamination;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.InsuranceList;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Insurance.GetComboList;
using UseCase.Insurance.GetDefaultSelectPattern;
using UseCase.MedicalExamination.AddAutoItem;
using UseCase.MedicalExamination.AutoCheckOrder;
using UseCase.MedicalExamination.ChangeAfterAutoCheckOrder;
using UseCase.MedicalExamination.CheckedExpired;
using UseCase.MedicalExamination.CheckedItemName;
using UseCase.MedicalExamination.ConvertFromHistoryTodayOrder;
using UseCase.MedicalExamination.ConvertItem;
using UseCase.MedicalExamination.ConvertNextOrderToTodayOdr;
using UseCase.MedicalExamination.GetAddedAutoItem;
using UseCase.MedicalExamination.GetValidGairaiRiha;
using UseCase.MedicalExamination.GetValidJihiYobo;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.ValidationTodayOrd;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class TodayOrdController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IWebSocketService _webSocketService;
        public TodayOrdController(UseCaseBus bus, IWebSocketService webSocketService, IUserService userService) : base(userService)
        {
            _bus = bus;
            _webSocketService = webSocketService;
        }

        [HttpPost(ApiPath.Upsert)]
        public async Task<ActionResult<Response<UpsertTodayOdrResponse>>> Upsert([FromBody] UpsertTodayOdrRequest request)
        {
            var input = new UpsertTodayOrdInputData(request.SyosaiKbn, request.JikanKbn, request.HokenPid, request.SanteiKbn, request.TantoId, request.KaId, request.UketukeTime, request.SinStartTime, request.SinEndTime, request.Status, request.OdrInfs.Select(
                    o => new OdrInfItemInputData(
                            HpId,
                            o.RaiinNo,
                            o.RpNo,
                            o.RpEdaNo,
                            o.PtId,
                            o.SinDate,
                            o.HokenPid,
                            o.OdrKouiKbn,
                            o.RpName,
                            o.InoutKbn,
                            o.SikyuKbn,
                            o.SyohoSbt,
                            o.SanteiKbn,
                            o.TosekiKbn,
                            o.DaysCnt,
                            o.SortNo,
                            o.Id,
                            o.OdrDetails.Select(
                                    od => new OdrInfDetailItemInputData(
                                            HpId,
                                            od.RaiinNo,
                                            od.RpNo,
                                            od.RpEdaNo,
                                            od.RowNo,
                                            od.PtId,
                                            od.SinDate,
                                            od.SinKouiKbn,
                                            od.ItemCd,
                                            od.ItemName,
                                            od.Suryo,
                                            od.UnitName,
                                            od.UnitSbt,
                                            od.TermVal,
                                            od.KohatuKbn,
                                            od.SyohoKbn,
                                            od.SyohoLimitKbn,
                                            od.DrugKbn,
                                            od.YohoKbn,
                                            od.Kokuji1,
                                            od.Kokuji2,
                                            od.IsNodspRece,
                                            od.IpnCd,
                                            od.IpnName,
                                            od.JissiKbn,
                                            od.JissiDate,
                                            od.JissiId,
                                            od.JissiMachine,
                                            od.ReqCd,
                                            od.Bunkatu,
                                            od.CmtName,
                                            od.CmtOpt,
                                            od.FontColor,
                                            od.CommentNewline
                                        )
                                ).ToList(),
                            o.IsDeleted
                        )
                ).ToList(),
                new KarteItemInputData(
                    HpId,
                    request.KarteItem.RaiinNo,
                    request.KarteItem.PtId,
                    request.KarteItem.SinDate,
                    request.KarteItem.Text,
                    request.KarteItem.IsDeleted,
                    request.KarteItem.RichText),
                UserId,
                new FileItemInputItem(request.FileItem.IsUpdateFile, request.FileItem.ListFileItems)
            );
            var output = _bus.Handle(input);

            if (output.Status == UpsertTodayOrdStatus.Successed)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.MedicalChanged,
                    new CommonMessage { PtId = output.PtId, SinDate = output.SinDate, RaiinNo = output.RaiinNo });
            }

            var presenter = new UpsertTodayOdrPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertTodayOdrResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Validate)]
        public ActionResult<Response<ValidationTodayOrdResponse>> Validate([FromBody] ValidationTodayOrdRequest request)
        {
            var input = new ValidationTodayOrdInputData(
                request.SyosaiKbn,
                request.JikanKbn,
                request.HokenPid,
                request.SanteiKbn,
                request.TantoId,
                request.KaId,
                request.UketukeTime,
                request.SinStartTime,
                request.SinEndTime,
                request.OdrInfs.Select(o =>
                    new ValidationOdrInfItem(
                        HpId,
                        o.RaiinNo,
                        o.RpNo,
                        o.RpEdaNo,
                        o.PtId,
                        o.SinDate,
                        o.HokenPid,
                        o.OdrKouiKbn,
                        o.RpName,
                        o.InoutKbn,
                        o.SikyuKbn,
                        o.SyohoSbt,
                        o.SanteiKbn,
                        o.TosekiKbn,
                        o.DaysCnt,
                        o.SortNo,
                        o.IsDeleted,
                        o.Id,
                        o.OdrDetails.Select(od => new ValidationOdrInfDetailItem(
                            HpId,
                            od.RaiinNo,
                            od.RpNo,
                            od.RpEdaNo,
                            od.RowNo,
                            od.PtId,
                            od.SinDate,
                            od.SinKouiKbn,
                            od.ItemCd,
                            od.ItemName,
                            od.Suryo,
                            od.UnitName,
                            od.UnitSbt,
                            od.TermVal,
                            od.KohatuKbn,
                            od.SyohoKbn,
                            od.SyohoLimitKbn,
                            od.DrugKbn,
                            od.YohoKbn,
                            od.Kokuji1,
                            od.Kokuji2,
                            od.IsNodspRece,
                            od.IpnCd,
                            od.IpnName,
                            od.JissiKbn,
                            od.JissiDate,
                            od.JissiId,
                            od.JissiMachine,
                            od.ReqCd,
                            od.Bunkatu,
                            od.CmtName,
                            od.CmtOpt,
                            od.FontColor,
                            od.CommentNewline
                        )).ToList()
                    )
               ).ToList(),
                new ValidationKarteItem(
                    HpId,
                    request.Karte.RaiinNo,
                    request.Karte.PtId,
                    request.Karte.SinDate,
                    request.Karte.Text,
                    request.Karte.IsDeleted,
                    request.Karte.RichText
                )
               );
            var output = _bus.Handle(input);

            var presenter = new ValidationTodayOrdPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidationTodayOrdResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDefaultSelectPattern)]
        public ActionResult<Response<GetDefaultSelectPatternResponse>> GetDefaultSelectPattern([FromQuery] GetDefaultSelectPatternRequest request)
        {
            var input = new GetDefaultSelectPatternInputData(
                HpId,
                request.PtId,
                request.SinDate,
                request.HistoryPid,
                request.SelectedHokenPid);

            var output = _bus.Handle(input);

            var presenter = new GetDefaultSelectPatternPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDefaultSelectPatternResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetInsuranceComboList)]
        public ActionResult<Response<GetInsuranceComboListResponse>> GetInsuranceComboList([FromQuery] GetInsuranceComboListRequest request)
        {
            var input = new GetInsuranceComboListInputData(HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetInsuranceComboListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceComboListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetAddedAutoItem)]
        public ActionResult<Response<GetAddedAutoItemResponse>> GetAddedAutoItem([FromBody] GetAddedAutoItemRequest request)
        {
            var input = new GetAddedAutoItemInputData(HpId, request.PtId, request.SinDate, request.OrderInfItems, request.CurrentOrderInfs);
            var output = _bus.Handle(input);
            var presenter = new GetAddedAutoItemPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAddedAutoItemResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.AddAutoItem)]
        public ActionResult<Response<AddAutoItemResponse>> AddAutoItem([FromBody] AddAutoItemRequest request)
        {
            var input = new AddAutoItemInputData(HpId, UserId, request.SinDate, request.AddedOrderInfs, request.OrderInfItems);
            var output = _bus.Handle(input);
            var presenter = new AddAutoItemPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<AddAutoItemResponse>>(presenter.Result);
        }


        [HttpPost(ApiPath.GetInfCheckedItemName)]
        public ActionResult<Response<CheckedItemNameResponse>> GetInfCheckedItemName([FromBody] CheckedItemNameRequest request)
        {
            var input = new CheckedItemNameInputData(request.OdrInfs);
            var output = _bus.Handle(input);
            var presenter = new CheckedItemNamePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckedItemNameResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetValidGairaiRiha)]
        public ActionResult<Response<GetValidGairaiRihaResponse>> GetValidGairaiRiha([FromBody] GetValidGairaiRihaRequest request)
        {
            var input = new GetValidGairaiRihaInputData(HpId, request.PtId, request.RaiinNo, request.SinDate, request.SyosaiKbn, request.AllOdrInfItems.Select(a => new Tuple<string, string>(a.ItemCd, a.ItemName)).ToList());
            var output = _bus.Handle(input);

            var presenter = new GetValidGairaiRihaPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetValidGairaiRihaResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetValidJihiYobo)]
        public ActionResult<Response<GetValidJihiYoboResponse>> GetValidJihiYobo([FromBody] GetValidJihiYoboRequest request)
        {
            var input = new GetValidJihiYoboInputData(HpId, request.SinDate, request.SyosaiKbn, request.ItemCds);
            var output = _bus.Handle(input);

            var presenter = new GetValidJihiYoboPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetValidJihiYoboResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ConvertNextOrderToTodayOrder)]
        public ActionResult<Response<ConvertNextOrderToTodayOrderResponse>> ConvertNextOrderToTodayOrder([FromBody] ConvertNextOrderToTodayOrderRequest request)
        {
            var input = new ConvertNextOrderToTodayOrdInputData(HpId, request.SinDate, request.RaiinNo, UserId, request.PtId, request.rsvKrtOrderInfItems);
            var output = _bus.Handle(input);

            var presenter = new ConvertNextOrderToTodayOrderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ConvertNextOrderToTodayOrderResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CheckedExpired)]
        public ActionResult<Response<CheckedExpiredResponse>> CheckedExpired([FromBody] CheckedExpiredRequest request)
        {
            var input = new CheckedExpiredInputData(HpId, request.SinDate, request.CheckedExpiredItems);
            var output = _bus.Handle(input);

            var presenter = new CheckedExpiredPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckedExpiredResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ConvertItem)]
        public ActionResult<Response<ConvertItemResponse>> ConvertItem([FromBody] ConvertItemRequest request)
        {

            var input = new ConvertItemInputData(HpId, UserId, request.RaiinNo, request.PtId, request.SinDate, request.OdrInfItems, ConvertExpiredItem(request.ExpiredItems));
            var output = _bus.Handle(input);

            var presenter = new ConvertItemPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ConvertItemResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.AutoCheckOrder)]
        public ActionResult<Response<AutoCheckOrderResponse>> AutoCheckOrder([FromBody] AutoCheckOrderRequest request)
        {
            var input = new AutoCheckOrderInputData(HpId, request.SinDate, request.PtId, request.OdrInfs);
            var output = _bus.Handle(input);

            var presenter = new AutoCheckOrderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<AutoCheckOrderResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ChangeAfterAutoCheckOrder)]
        public ActionResult<Response<ChangeAfterAutoCheckOrderResponse>> ChangeAfterAutoCheckOrder([FromBody] ChangeAfterAutoCheckOrderRequest request)
        {
            var input = new ChangeAfterAutoCheckOrderInputData(HpId, request.SinDate, UserId, request.RaiinNo, request.PtId, request.OdrInfs, request.TargetItems);
            var output = _bus.Handle(input);

            var presenter = new ChangeAfterAutoCheckOrderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ChangeAfterAutoCheckOrderResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ConvertFromHistoryToTodayOrder)]
        public ActionResult<Response<ConvertFromHistoryToTodayOrderResponse>> ConvertFromHistoryToTodayOrder([FromBody] ConvertFromHistoryToTodayOrderRequest request)
        {
            var input = new ConvertFromHistoryTodayOrderInputData(HpId, request.SinDate, request.RaiinNo, request.SanteiKbn, UserId, request.PtId, request.HistoryOdrInfModels);
            var output = _bus.Handle(input);

            var presenter = new ConvertFromHistoryToTodayOrderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ConvertFromHistoryToTodayOrderResponse>>(presenter.Result);
        }

        private Dictionary<string, List<TenItemModel>> ConvertExpiredItem(Dictionary<string, List<TenItemDto>> request)
        {
            Dictionary<string, List<TenItemModel>> result = new();
            foreach (var keyValuePair in request)
            {
                List<TenItemModel> tenItemDtos = new();
                foreach (var item in keyValuePair.Value)
                {
                    if (!string.IsNullOrEmpty(item.ItemCd))
                    {
                        var tenItemDto = new TenItemModel(
                                item.HpId,
                                item.ItemCd,
                                item.RousaiKbn,
                                item.KanaName1,
                                item.Name,
                                item.KohatuKbn,
                                item.MadokuKbn,
                                item.KouseisinKbn,
                                item.OdrUnitName,
                                item.EndDate,
                                item.DrugKbn,
                                item.MasterSbt,
                                item.BuiKbn,
                                item.IsAdopted,
                                item.Ten,
                                item.TenId,
                                item.KensaMstCenterItemCd1,
                                item.KensaMstCenterItemCd2,
                                item.CmtCol1,
                                item.IpnNameCd,
                                item.SinKouiKbn,
                                item.YjCd,
                                item.CnvUnitName,
                                item.StartDate,
                                item.YohoKbn,
                                item.CmtColKeta1,
                                item.CmtColKeta2,
                                item.CmtColKeta3,
                                item.CmtColKeta4,
                                item.CmtCol2,
                                item.CmtCol3,
                                item.CmtCol4,
                                item.IpnCD,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                item.OdrTermVal,
                                item.CnvTermVal,
                                item.DefaultValue,
                                string.Empty,
                                string.Empty,
                                item.ModeStatus
                            );
                        tenItemDtos.Add(tenItemDto);
                    }
                }
                result.Add(keyValuePair.Key, tenItemDtos);
            }

            return result;
        }
    }
}
