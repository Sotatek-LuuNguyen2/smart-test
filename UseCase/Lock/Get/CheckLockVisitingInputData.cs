﻿using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Get
{
    public class CheckLockVisitingInputData : IInputData<CheckLockVisitingOutputData>
    {
        public CheckLockVisitingInputData(int hpId, long ptId, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
    }
}
