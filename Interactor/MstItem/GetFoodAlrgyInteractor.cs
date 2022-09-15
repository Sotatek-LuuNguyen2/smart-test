﻿using Domain.Models.MstItem;
using UseCase.MstItem.GetFoodAlrgy;

namespace Interactor.MstItem
{
    public class GetFoodAlrgyInteractor : IGetFoodAlrgyInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetFoodAlrgyInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetFoodAlrgyOutputData Handle(GetFoodAlrgyInputData inputData)
        {
            var foodAlrgyMasterData = GetFoodAlrgyMasterData();
            return new GetFoodAlrgyOutputData(foodAlrgyMasterData, GetFoodAlrgyStatus.Successed);
        }
        private List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData()
        {
            return _mstItemRepository.GetFoodAlrgyMasterData(); 
        }
    }
}
