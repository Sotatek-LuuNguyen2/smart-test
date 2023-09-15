﻿using Domain.Models.ListSetGenerationMst;

namespace EmrCloudApi.Responses.ListSetGenerationMst
{
    public class GetListSetGenerationMstResponse
    {
        public GetListSetGenerationMstResponse(List<ListSetGenerationMstModel> data)
        {
            Data = data;
        }

        public List<ListSetGenerationMstModel> Data { get; private set; } = new List<ListSetGenerationMstModel>();
    }
}
