﻿using Domain.Models.Insurance;

namespace EmrCloudApi.Tenant.Requests.Insurance
{
    public class ValidInsuranceOtherRequest
    {
        public ValidInsuranceOtherRequest(ValidInsuranceOtherModel validModel)
        {
            ValidModel = validModel;
        }

        public ValidInsuranceOtherModel ValidModel { get; private set; }
    }
}
