﻿namespace EmrCalculateApi.Requests
{
    public class CalculateRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int SeikyuUp { get; set; }

        public string Prefix { get; set; } = string.Empty;
    }

    public class ReceCalculateRequest
    {
        public List<long> PtIds { get; set; } = new();

        public int SeikyuYm { get; set; }

        public string HostName { get; set; } = string.Empty;

        public string UniqueKey { get; set; } = string.Empty;
    }
}
