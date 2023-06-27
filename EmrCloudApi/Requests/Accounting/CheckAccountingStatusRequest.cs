﻿using UseCase.Accounting.CheckAccountingStatus;

namespace EmrCloudApi.Requests.Accounting
{
    public class CheckAccountingStatusRequest
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public long RaiinNo { get; set; }
        public int DebitBalance { get; set; }
        public int SumAdjust { get; set; }
        public int Credit { get; set; }
        public int Wari { get; set; }
        public bool IsDisCharge { get; set; }
        public bool IsSaveAccounting { get; set; }
        public List<SyunoSeikyuDto> SyunoSeikyuDtos { get; set; } = new();
        public List<SyunoSeikyuDto> AllSyunoSeikyuDtos { get; set; } = new();
    }
}
