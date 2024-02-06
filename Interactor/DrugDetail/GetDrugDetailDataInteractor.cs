﻿using Domain.Models.DrugDetail;
using UseCase.DrugDetailData.Get;

namespace Interactor.DrugDetailData
{
    public class GetDrugDetailDataInteractor : IGetDrugDetailDataInputPort
    {
        private readonly IDrugDetailRepository _drugInforRepository;
        public GetDrugDetailDataInteractor(IDrugDetailRepository drugInforRepository)
        {
            _drugInforRepository = drugInforRepository;
        }

        public GetDrugDetailDataOutputData Handle(GetDrugDetailDataInputData inputData)
        {
            try
            {
                if (string.IsNullOrEmpty(inputData.ItemCd))
                {
                    return new GetDrugDetailDataOutputData(new DrugDetailModel(), GetDrugDetailDataStatus.InvalidItemCd);
                }

                if (string.IsNullOrEmpty(inputData.YJCode))
                {
                    return new GetDrugDetailDataOutputData(new DrugDetailModel(), GetDrugDetailDataStatus.InvalidYJCode);
                }

                var data = _drugInforRepository.GetDataDrugSeletedTree(inputData.HpId, inputData.SelectedIndexOfMenuLevel, inputData.Level, inputData.DrugName, inputData.ItemCd, inputData.YJCode);

                return new GetDrugDetailDataOutputData(data, GetDrugDetailDataStatus.Successed);
            }
            finally
            {
                _drugInforRepository.ReleaseResource();
            }
        }
    }
}
