﻿namespace EmrCloudApi.Responses.MstItem
{
    public class GetDrugActionResponse
    {
        public GetDrugActionResponse(string drugInfo)
        {
            DrugInf = drugInfo;
        }

        public string DrugInfo { get; private set; }
    }
}
