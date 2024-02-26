﻿using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.DrugDetail;
using EmrCloudApi.Presenters.DrugDetailData;
using EmrCloudApi.Presenters.DrugInfor;
using EmrCloudApi.Presenters.ListSetMst;
using EmrCloudApi.Presenters.UsageTreeSet;
using EmrCloudApi.Presenters.YohoSetMst;
using EmrCloudApi.Requests.DrugDetail;
using EmrCloudApi.Requests.DrugInfor;
using EmrCloudApi.Requests.ListSetMst;
using EmrCloudApi.Requests.UsageTreeSet;
using EmrCloudApi.Requests.YohoSetMst;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.DrugDetail;
using EmrCloudApi.Responses.DrugInfor;
using EmrCloudApi.Responses.NewFolder;
using EmrCloudApi.Responses.UsageTreeSetResponse;
using EmrCloudApi.Responses.YohoSetMst;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.DrugDetail;
using UseCase.DrugDetailData.Get;
using UseCase.DrugDetailData.ShowKanjaMuke;
using UseCase.DrugDetailData.ShowMdbByomei;
using UseCase.DrugDetailData.ShowProductInf;
using UseCase.DrugInfor.Get;
using UseCase.DrugInfor.GetDataPrintDrugInfo;
using UseCase.ListSetMst.UpdateListSetMst;
using UseCase.UsageTreeSet.GetTree;
using UseCase.YohoSetMst.GetByItemCd;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class InputItemController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;

        public InputItemController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }

        [HttpGet("GetDrugInf")]
        public ActionResult<Response<GetDrugInforResponse>> GetDrugInformation([FromQuery] GetDrugInforRequest request)
        {
            var input = new GetDrugInforInputData(HpId, request.SinDate, request.ItemCd);
            var output = _bus.Handle(input);

            var presenter = new GetDrugInforPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugInforResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDrugMenuTree)]
        public ActionResult<Response<GetDrugDetailResponse>> GetDrugMenuTree([FromQuery] GetDrugDetailRequest request)
        {
            var input = new GetDrugDetailInputData(HpId, request.SinDate, request.ItemCd);
            var output = _bus.Handle(input);

            var presenter = new GetDrugDetailPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugDetailResponse>>(presenter.Result);
        }

        [HttpGet("GetListUsageTreeSet")]
        public ActionResult<Response<GetUsageTreeSetListResponse>> GetUsageTree([FromQuery] GetUsageTreeSetListRequest request)
        {
            var input = new GetUsageTreeSetInputData(HpId, request.SinDate, request.KouiKbn);
            var output = _bus.Handle(input);
            var present = new GetUsageTreeSetListPresenter();
            present.Complete(output);
            return new ActionResult<Response<GetUsageTreeSetListResponse>>(present.Result);
        }

        [HttpGet(ApiPath.DrugDataSelectedTree)]
        public ActionResult<Response<GetDrugDetailDataResponse>> DrugDataSelectedTree([FromQuery] GetDrugDetailDataRequest request)
        {
            var input = new GetDrugDetailDataInputData(HpId, request.SelectedIndexOfMenuLevel, request.Level, request.DrugName, request.ItemCd, request.YJCode);
            var output = _bus.Handle(input);

            var presenter = new GetDrugDetailDataPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugDetailDataResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetYohoSetMstByItemCd)]
        public ActionResult<Response<GetYohoSetMstByItemCdResponse>> GetYohoSetMstByItemCd([FromQuery] GetYohoSetMstByItemCdRequest request)
        {
            var input = new GetYohoMstByItemCdInputData(HpId, UserId, request.ItemCd, request.StartDate, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetYohoMstByItemCdPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetYohoSetMstByItemCdResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.ShowProductInf)]
        public ActionResult<Response<ShowDrugDetailHtmlResponse>> ShowProductInf([FromQuery] ShowProductInfRequest request)
        {
            var input = new ShowProductInfInputData(HpId, request.SinDate, request.ItemCd, request.Level, request.DrugName, request.YJCode);
            var output = _bus.Handle(input);

            var presenter = new ShowProductInfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ShowDrugDetailHtmlResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.ShowKanjaMuke)]
        public ActionResult<Response<ShowDrugDetailHtmlResponse>> ShowKanjaMuke([FromQuery] ShowKanjaMukeRequest request)
        {
            var input = new ShowKanjaMukeInputData(HpId, request.ItemCd, request.Level, request.DrugName, request.YJCode);
            var output = _bus.Handle(input);

            var presenter = new ShowKanjaMukePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ShowDrugDetailHtmlResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.ShowMdbByomei)]
        public ActionResult<Response<ShowDrugDetailHtmlResponse>> ShowMdbByomei([FromQuery] ShowMdbByomeiRequest request)
        {
            var input = new ShowMdbByomeiInputData(HpId, request.ItemCd, request.Level, request.DrugName, request.YJCode);
            var output = _bus.Handle(input);

            var presenter = new ShowMdbByomeiPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ShowDrugDetailHtmlResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDataPrintDrugInfo)]
        public ActionResult<Response<GetDataPrintDrugInfoResponse>> GetDataPrintDrugInfo([FromQuery] GetDataPrintDrugInfoRequest request)
        {
            var input = new GetDataPrintDrugInfoInputData(request.HpId, request.SinDate, request.ItemCd, request.Level, string.Empty, request.YJCode, request.Type);
            var output = _bus.Handle(input);

            var presenter = new GetDataPrintDrugInfoPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDataPrintDrugInfoResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateListSetMst)]
        public ActionResult<Response<UpdateListSetMstResponse>> UpdateListSetMst([FromBody] UpdateListSetMstRequest request)
        {
            var input = new UpdateListSetMstInputData(UserId, HpId, request.ListSetMsts);
            var output = _bus.Handle(input);
            var presenter = new UpdateListSetMstPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
