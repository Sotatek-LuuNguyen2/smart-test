﻿namespace EmrCloudApi.Tenant.Requests.SetMst
{
    public class SaveImageSuperSetDetailRequest
    {
        public int SetCd { get; set; }
        public int Position { get; set; }
        public string OldImage { get; set; } = string.Empty;
    }
}
