﻿namespace EmrCloudApi.Requests.ExportPDF;

public class OrderLabelExportRequest
{
    //public int Mode { get; set; }

    public int HpId { get; set; }

    public long PtId { get; set; }

    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public List<(int from, int to)> OdrKouiKbns { get; set; } = new();
}
