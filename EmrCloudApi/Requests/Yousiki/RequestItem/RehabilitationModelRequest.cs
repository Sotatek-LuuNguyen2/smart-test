﻿namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class RehabilitationModelRequest
    {
        public List<OutpatientConsultationModelRequest> OutpatientConsultationList { get; set; } = new();

        public List<CommonForm1ModelRequest> ByomeiRehabilitationList { get; set; } = new();
    }
}
