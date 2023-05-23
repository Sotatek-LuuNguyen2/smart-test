﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Responses.Holiday;
using EmrCloudApi.Requests.Holiday;
using UseCase.Holiday.SaveHoliday;
using EmrCloudApi.Presenters.Holiday;
using Domain.Models.FlowSheet;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class HolidayController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public HolidayController(UseCaseBus bus, IUserService userService) : base(userService) => _bus = bus;

        [HttpPost(ApiPath.SaveHolidayMst)]
        public ActionResult<Response<SaveHolidayMstResponse>> SaveHolidayMst([FromBody] SaveHolidayMstRequest request)
        {
            var input = new SaveHolidayMstInputData(new HolidayModel(HpId, 
                                                                    request.Holiday.SeqNo, 
                                                                    request.Holiday.SinDate, 
                                                                    request.Holiday.HolidayKbn,
                                                                    request.Holiday.KyusinKbn,
                                                                    request.Holiday.HolidayName), UserId);
            var output = _bus.Handle(input);
            var presenter = new SaveHolidayMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveHolidayMstResponse>>(presenter.Result);
        }
    }
}
