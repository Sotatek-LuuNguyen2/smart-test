﻿namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StrokeHistoryModelRequest
    {
        /*public UpdateYosiki1InfDetailRequestItem Type { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem OnsetDate { get; set; } = new();*/
        public long PtId { get; set; }

        public int SinYm { get; set; }

        public int DataType { get; set; }

        public int SeqNo { get; set; }

        public string CodeNo { get; set; } = string.Empty;

        public int RowNo { get; set; }

        public int Payload { get; set; }

        public string Value { get; set; } = string.Empty;

        public int IsDeleted { get; set; }
    }
}
