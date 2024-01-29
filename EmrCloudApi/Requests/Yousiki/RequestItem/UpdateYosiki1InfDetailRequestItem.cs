﻿namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class UpdateYosiki1InfDetailRequestItem
    {
        public long PtId { get; set; }

        public string SinYmDisplay { get; set; } = string.Empty;

        public int DataType { get; set; }

        public int SeqNo { get; set; }

        public string CodeNo { get; set; } = string.Empty;

        public int RowNo { get; set; }

        public int Payload { get; set; }

        public string Value { get; set; } = string.Empty;
    }
}
