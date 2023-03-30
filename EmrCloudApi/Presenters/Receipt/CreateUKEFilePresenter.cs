﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.CreateUKEFile;

namespace EmrCloudApi.Presenters.Receipt
{
    public class CreateUKEFilePresenter : ICreateUKEFileOutpuport
    {
        public Response<CreateUKEFileResponse> Result { get; private set; } = new Response<CreateUKEFileResponse>();

        public void Complete(CreateUKEFileOutputData outputData)
        {
            Result.Data = new CreateUKEFileResponse(outputData.Status, outputData.Message);
            Result.Status = (int)outputData.Status;
            Result.Message = outputData.Message ?? GetMessage(outputData.Status);
        }

        private string GetMessage(CreateUKEFileStatus status) => status switch
        {
            CreateUKEFileStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            CreateUKEFileStatus.InvaliSeikyuYm => ResponseMessage.InvalidPtId,
            _ => string.Empty
        };
    }
}
