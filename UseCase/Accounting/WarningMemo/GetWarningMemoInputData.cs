﻿using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.WarningMemo
{
    public class GetWarningMemoInputData : IInputData<GetWarningMemoOutputData>
    {
        public GetWarningMemoInputData(int hpId, long ptId, int sinDate, long raiinNo)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }
    }
}
