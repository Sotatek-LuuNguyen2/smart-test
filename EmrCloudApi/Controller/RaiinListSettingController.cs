﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Responses.RaiinListSetting;
using UseCase.RaiinListSetting.GetDocCategory;
using EmrCloudApi.Presenters.RaiinListSetting;
using UseCase.RaiinListSetting.GetFilingcategory;
using UseCase.RaiinListSetting.GetRaiiinListSetting;
using UseCase.RaiinListSetting.SaveRaiinListSetting;
using EmrCloudApi.Requests.RaiinListSetting;
using Helper.Mapping;
using Domain.Models.RaiinListMst;
using Domain.Models.RaiinListSetting;
using Helper.Extension;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class RaiinListSettingController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public RaiinListSettingController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList + "DocCategoryRaiin")]
        public ActionResult<Response<GetDocCategoryRaiinResponse>> GetDocCategoryRaiin()
        {
            var input = new GetDocCategoryRaiinInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetDocCategoryRaiinPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetDocCategoryRaiinResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList + "GetFilingcategory")]
        public ActionResult<Response<GetFilingcategoryResponse>> GetFilingcategory()
        {
            var input = new GetFilingcategoryInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetFilingcategoryPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetFilingcategoryResponse>>(presenter.Result);
        }

        [AllowAnonymous]
        [HttpGet(ApiPath.GetList + "RaiinListSetting")]
        public ActionResult<Response<GetRaiiinListSettingResponse>> GetRaiinListSetting()
        {
            var input = new GetRaiiinListSettingInputData(1);
            var output = _bus.Handle(input);
            var presenter = new GetRaiiinListSettingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetRaiiinListSettingResponse>>(presenter.Result);
        }

        [AllowAnonymous]
        [HttpPost(ApiPath.Save + "RaiinListSetting")]
        public ActionResult<Response<SaveRaiinListSettingResponse>> SaveRaiinListSetting([FromBody] SaveRaiinListSettingRequest request)
        {
            var inputModel = request.RaiinListSettings
                            .Select(x => new RaiinListMstModel(x.GrpId,
                                                              x.GrpName,
                                                              x.SortNo,
                                                              x.IsDeleted,
                                                              x.RaiinListDetailsList.Select(d => new RaiinListDetailModel(d.GrpId,
                                                                                                                          d.KbnCd,
                                                                                                                          d.SortNo,
                                                                                                                          d.KbnName,
                                                                                                                          d.ColorCd,
                                                                                                                          d.IsDeleted,
                                                                                                                          d.RaiinListDoc.Select(doc => new RaiinListDocModel(doc.HpId,
                                                                                                                                                                             doc.GrpId,
                                                                                                                                                                             doc.KbnCd,
                                                                                                                                                                             doc.SeqNo,
                                                                                                                                                                             doc.CategoryCd,
                                                                                                                                                                             doc.CategoryName,
                                                                                                                                                                             doc.IsDeleted,
                                                                                                                                                                             doc.IsModify)).ToList(),
                                                                                                                          d.RaiinListItem.Select(item => new RaiinListItemModel(item.HpId,
                                                                                                                                                                                item.GrpId,
                                                                                                                                                                                item.KbnCd,
                                                                                                                                                                                item.ItemCd,
                                                                                                                                                                                item.SeqNo,
                                                                                                                                                                                item.InputName,
                                                                                                                                                                                item.IsExclude,
                                                                                                                                                                                item.IsAddNew,
                                                                                                                                                                                item.IsDeleted,
                                                                                                                                                                                item.IsModify)).ToList(),
                                                                                                                          d.RaiinListFile.Select(file => new RaiinListFileModel(file.HpId,
                                                                                                                                                                                file.GrpId,
                                                                                                                                                                                file.KbnCd,
                                                                                                                                                                                file.CategoryCd,
                                                                                                                                                                                file.CategoryName,
                                                                                                                                                                                file.SeqNo,
                                                                                                                                                                                file.IsDeleted,
                                                                                                                                                                                file.IsModify)).ToList(),
                                                                                                                          new KouiKbnCollectionModel(Mapper.Map(d.KouCollection.IKanModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.ZaitakuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.NaifukuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.TonpukuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.GaiyoModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.HikaKinchuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.JochuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.TentekiModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.TachuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.JikochuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.ShochiModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.ShujutsuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.MasuiModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.KentaiModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.SeitaiModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.SonohokaModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.GazoModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.RihaModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.SeishinModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.HoshaModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.ByoriModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.JihiModel, new RaiinListKouiModel())))).ToList()

                                                              )).ToList();

            var input = new SaveRaiinListSettingInputData(1, inputModel ,2);
            var output = _bus.Handle(input);
            var presenter = new SaveRaiinListSettingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveRaiinListSettingResponse>>(presenter.Result);
        }
    }
}
