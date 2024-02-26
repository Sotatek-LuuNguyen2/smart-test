﻿using Domain.Models.InsuranceMst;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Insurance;
using EmrCloudApi.Presenters.InsuranceMst;
using EmrCloudApi.Requests.Insurance;
using EmrCloudApi.Requests.InsuranceMst;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Insurance;
using EmrCloudApi.Responses.InsuranceMst;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Insurance.FindHokenInfByPtId;
using UseCase.InsuranceMst.DeleteHokenMaster;
using UseCase.InsuranceMst.GetHokenMasterReadOnly;
using UseCase.InsuranceMst.GetInfoCloneInsuranceMst;
using UseCase.InsuranceMst.GetMasterDetails;
using UseCase.InsuranceMst.GetSelectMaintenance;
using UseCase.InsuranceMst.SaveHokenMaster;
using UseCase.InsuranceMst.SaveOrdInsuranceMst;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceMstController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;
        public InsuranceMstController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) => _bus = bus;

        [HttpGet(ApiPath.GetList + "InsuranceMstDetail")]
        public ActionResult<Response<GetInsuranceMasterDetailResponse>> GetList([FromQuery] GetInsuranceMasterDetailRequest request)
        {
            var input = new GetInsuranceMasterDetailInputData(HpId, request.FHokenNo, request.FHokenSbtKbn, request.IsJitan, request.IsTaken);
            var output = _bus.Handle(input);
            var presenter = new GetInsuranceMasterDetailPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.Get + "SelectMaintenance")]
        public ActionResult<Response<GetSelectMaintenanceResponse>> GetSelectMaintenance([FromQuery] GetSelectMaintenanceRequest request)
        {
            var input = new GetSelectMaintenanceInputData(HpId, request.HokenNo, request.HokenEdaNo, request.PrefNo, request.StartDate);
            var output = _bus.Handle(input);
            var presenter = new GetSelectMaintenancePresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.Delete + "HokenMaster")]
        public ActionResult<Response<DeleteHokenMasterResponse>> DeleteHokenMaster([FromBody] DeleteHokenMasterRequest request)
        {
            var input = new DeleteHokenMasterInputData(HpId, request.PrefNo, request.HokenNo, request.HokenEdaNo, request.StartDate);
            var output = _bus.Handle(input);
            var presenter = new DeleteHokenMasterPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.Get + "InfoCloneInsuranceMst")]
        public ActionResult<Response<GetInfoCloneInsuranceMstResponse>> GetInfoCloneInsuranceMst([FromQuery] GetInfoCloneInsuranceMstRequest request)
        {
            var input = new GetInfoCloneInsuranceMstInputData(HpId, request.HokenNo, request.PrefNo, request.StartDate);
            var output = _bus.Handle(input);
            var presenter = new GetInfoCloneInsuranceMstPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.Save + "InsuranceMst")]
        public ActionResult<Response<SaveHokenMasterResponse>> SaveInsuranceMst([FromBody] SaveHokenMasterRequest request)
        {
            var input = new SaveHokenMasterInputData(HpId, UserId, new HokenMstModel(request.Insurance.FutanKbn,
                                                          request.Insurance.FutanRate,
                                                          request.Insurance.StartDate,
                                                          request.Insurance.EndDate,
                                                          request.Insurance.HokenNo,
                                                          request.Insurance.HokenEdaNo,
                                                          request.Insurance.HokenSName ?? string.Empty,
                                                          request.Insurance.Houbetu ?? string.Empty,
                                                          request.Insurance.HokenSbtKbn,
                                                          request.Insurance.CheckDigit,
                                                          request.Insurance.AgeStart,
                                                          request.Insurance.AgeEnd,
                                                          request.Insurance.IsFutansyaNoCheck,
                                                          request.Insurance.IsJyukyusyaNoCheck,
                                                          request.Insurance.JyuKyuCheckDigit,
                                                          request.Insurance.IsTokusyuNoCheck,
                                                          request.Insurance.HokenName ?? string.Empty,
                                                          request.Insurance.HokenNameCd ?? string.Empty,
                                                          request.Insurance.HokenKohiKbn,
                                                          request.Insurance.IsOtherPrefValid,
                                                          request.Insurance.ReceKisai,
                                                          request.Insurance.IsLimitList,
                                                          request.Insurance.IsLimitListSum,
                                                          request.Insurance.EnTen,
                                                          request.Insurance.KaiLimitFutan,
                                                          request.Insurance.DayLimitFutan,
                                                          request.Insurance.MonthLimitFutan,
                                                          request.Insurance.MonthLimitCount,
                                                          request.Insurance.LimitKbn,
                                                          request.Insurance.CountKbn,
                                                          request.Insurance.FutanYusen,
                                                          request.Insurance.CalcSpKbn,
                                                          request.Insurance.MonthSpLimit,
                                                          request.Insurance.KogakuTekiyo,
                                                          request.Insurance.KogakuTotalKbn,
                                                          request.Insurance.KogakuHairyoKbn,
                                                          request.Insurance.ReceSeikyuKbn,
                                                          request.Insurance.ReceKisaiKokho,
                                                          request.Insurance.ReceKisai2,
                                                          request.Insurance.ReceTenKisai,
                                                          request.Insurance.ReceFutanRound,
                                                          request.Insurance.ReceZeroKisai,
                                                          request.Insurance.ReceSpKbn,
                                                          string.Empty,
                                                          request.Insurance.PrefNo,
                                                          request.Insurance.SortNo,
                                                          request.Insurance.SeikyuYm,
                                                          request.Insurance.ReceFutanHide,
                                                          request.Insurance.ReceFutanKbn,
                                                          request.Insurance.KogakuTotalAll,
                                                          request.Insurance.IsAdded,
                                                          request.Insurance.DayLimitCount,
                                                          request.Insurance.ExcepHokenSyas));
            var output = _bus.Handle(input);
            var presenter = new SaveHokenMasterPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        /// <summary>
        /// Only pass IsModifed = true
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(ApiPath.Save + "OrderInsuranceMstList")]
        public ActionResult<Response<SaveOrdInsuranceMstResponse>> SaveOrderInsuranceMstList([FromBody] SaveOrdInsuranceMstRequest request)
        {
            var input = new SaveOrdInsuranceMstInputData(request.Insurances.Select(x => new HokenMstModel(x.HokenNo, x.HokenEdaNo, x.StartDate, x.PrefNo, x.Sort)).ToList(),
                                                            HpId,
                                                            UserId);
            var output = _bus.Handle(input);
            var presenter = new SaveOrdInsuranceMstPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.GetHokenMasterReadOnly)]
        public ActionResult<Response<GetInfoCloneInsuranceMstResponse>> GetHokenMasterReadOnly([FromQuery] GetHokenMasterReadOnlyRequest request)
        {
            var input = new GetHokenMasterReadOnlyInputData(HpId, request.HokenNo, request.HokenEdaNo, request.PrefNo, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetHokenMasterReadOnlyPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.FindHokenInfByPtId)]
        public ActionResult<Response<FindHokenInfByPtIdResponse>> FindHokenInfByPtId([FromQuery] FindHokenInfByPtIdRequest request)
        {
            var input = new FindHokenInfByPtIdInputData(HpId, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new FindHokenInfByPtIdPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<FindHokenInfByPtIdResponse>>(presenter.Result);
        }
    }
}
