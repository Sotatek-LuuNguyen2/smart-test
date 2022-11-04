﻿using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class DeleteStickyNoteInputData : IInputData<DeleteStickyNoteOutputData>
    {
        public DeleteStickyNoteInputData(int hpId, int ptId, int seqNo, int userId)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            UserId = userId;
        }

        public int HpId { get; private set; }
        public int PtId { get; private set; }
        public int SeqNo { get; private set; }
        public int UserId { get; private set; }
    }
}
