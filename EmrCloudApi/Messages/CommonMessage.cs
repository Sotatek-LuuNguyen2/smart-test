﻿using Helper.Constants;

namespace EmrCloudApi.Messages;

public class CommonMessage
{
    public int SinDate { get; set; }
    public long RaiinNo { get; set; } = CommonConstants.InvalidId;
    public long PtId { get; set; } = CommonConstants.InvalidId;
}

public class LockMessage
{
    public int SinDate { get; set; }
    public long RaiinNo { get; set; } = CommonConstants.InvalidId;
    public long PtId { get; set; } = CommonConstants.InvalidId;
    public byte Type { get; set; }
    public string FunctionCod { get; set; } = String.Empty;
}

public class SuperSetMessage
{
    public List<int> SetCds { get; set; } = new();
}
