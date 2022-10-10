﻿using Helper.Common;

namespace Domain.Models.PatientInfor
{
    public class PtHokenCheckModel
    {
        public PtHokenCheckModel(int hokenGrp, int hokenId, int checkDateInt, int checkId, string checkCmt, long seqNo)
        {
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            CheckDateInt = checkDateInt;
            CheckId = checkId;
            CheckCmt = checkCmt;
            SeqNo = seqNo;
        }

        public int HokenGrp { get; private set; }
        public int HokenId { get; private set; }

        public int CheckDateInt
        {
            get; set;
        }

        public DateTime CheckDate
        {
            get
            {
                return DateTime.SpecifyKind(CIUtil.IntToDate(CheckDateInt), DateTimeKind.Utc);
            }
        }

        public int CheckId { get; private set; }
        public string CheckCmt { get; private set; }
        public long SeqNo { get; private set; }
    }
}