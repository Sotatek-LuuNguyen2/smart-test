﻿namespace EmrCloudApi.Tenant.Requests.Insurance
{
    public class GetInsuranceByIdRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int HokenPid { get; set; }
    }
}
