﻿using EmrCalculateApi.Interface;
using EmrCalculateApi.Requests;
using EmrCalculateApi.Responses;
using Helper.Messaging.Data;
using Helper.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using EmrCalculateApi.Realtime;
using EmrCalculateApi.Constants;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;

namespace EmrCalculateApi.Controllers
{
#pragma warning disable CS8625
    [Route("api/[controller]")]
    [ApiController]
    public class ReceFutanController : ControllerBase
    {
        private readonly IReceFutanViewModel _receFutanCalculate;
        private readonly IWebSocketService _webSocketService;
        private CancellationToken? _cancellationToken;

        public ReceFutanController(IReceFutanViewModel receFutanCalculate, IWebSocketService webSocketService)
        {
            _receFutanCalculate = receFutanCalculate;
            _webSocketService = webSocketService;
        }

        [HttpPost("ReceFutanCalculateMain")]
        public ActionResult ReceFutanCalculateMain([FromBody] ReceCalculateRequest calculateRequest, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            try
            {
                Messenger.Instance.Register<RecalculationStatus>(this, UpdateRecalculationStatus);
                Messenger.Instance.Register<StopCalcStatus>(this, StopCalculation);

                _receFutanCalculate.ReceFutanCalculateMain(
                                   calculateRequest.PtIds,
                                   calculateRequest.SeikyuYm,
                                   calculateRequest.UniqueKey);
            }
            catch
            {
                var sendMessager = _webSocketService.SendMessageAsync(FunctionCodes.RunCalculate, "Error");
                sendMessager.Wait();
            }
            finally
            {
                Messenger.Instance.Deregister<RecalculationStatus>(this, UpdateRecalculationStatus);
                Messenger.Instance.Deregister<StopCalcStatus>(this, StopCalculation);
                HttpContext.Response.Body.Close();
            }
            return Ok();
        }

        [HttpPost("GetListReceInf")]
        public ActionResult<GetListReceInfResponse> GetListReceInf([FromBody] GetListReceInfRequest request)
        {
            var response = _receFutanCalculate.KaikeiTotalCalculate(request.PtId, request.SinYm);

            return new ActionResult<GetListReceInfResponse>(new GetListReceInfResponse(response));
        }

        private void StopCalculation(StopCalcStatus stopCalcStatus)
        {
            if (!_cancellationToken.HasValue)
            {
                stopCalcStatus.CallFailCallback(false);
            }
            else
            {
                stopCalcStatus.CallSuccessCallback(_cancellationToken!.Value.IsCancellationRequested);
            }
        }

        private void UpdateRecalculationStatus(RecalculationStatus status)
        {
            var objectJson = JsonSerializer.Serialize(status);
            var result = _webSocketService.SendMessageAsync(FunctionCodes.RunCalculate, objectJson);
            result.Wait();
        }
    }
}
