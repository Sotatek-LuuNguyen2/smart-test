﻿using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Schema;
using EmrCloudApi.Requests.Schema;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Schema;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Schema.GetListImageTemplates;
using UseCase.Schema.GetListInsuranceScan;
using UseCase.Schema.SaveListFileTodayOrder;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SchemaController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;
        public SchemaController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetListImageTemplatesResponse>> GetList()
        {
            var input = new GetListImageTemplatesInputData();
            var output = _bus.Handle(input);

            var presenter = new GetListImageTemplatesPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListImageTemplatesResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UploadListFileKarte)]
        public ActionResult<Response<SaveListFileResponse>> MultiUpload([FromQuery] SaveListFileRequest request, [FromForm] List<IFormFile> files)
        {
            List<FileItem> listFiles = new();
            foreach (var file in files)
            {
                if (file.Length > 30000000)
                {
                    return Ok(new Response<SaveListFileResponse>()
                    {
                        Message = "Invalid file size!",
                        Status = (int)SaveListFileTodayOrderStatus.InvalidSizeFile
                    });
                }
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    bool isSchema = false;
                    if (file.FileName.EndsWith(".schema"))
                    {
                        fileName = file.FileName.Replace(".schema", "." + file.ContentType.Replace("image/", string.Empty));
                        isSchema = true;
                    }
                    var streamImage = new MemoryStream();
                    file.CopyTo(streamImage);
                    listFiles.Add(new FileItem(fileName, isSchema, streamImage));
                }
            }
            var input = new SaveListFileTodayOrderInputData(HpId, UserId, request.PtId, request.SetCd, request.TypeUpload, listFiles);
            var output = _bus.Handle(input);
            var presenter = new SaveListFilePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveListFileResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListInsuranceScan)]
        public ActionResult<Response<GetListInsuranceScanResponse>> GetListInsuranceScan([FromQuery] long ptId)
        {
            var input = new GetListInsuranceScanInputData(HpId, ptId);
            var output = _bus.Handle(input);
            var presenter = new GetListInsuranceScanPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListInsuranceScanResponse>>(presenter.Result);
        }
    }
}
