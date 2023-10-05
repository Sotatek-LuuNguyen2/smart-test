﻿namespace Domain.Models.Lock
{
    public class LockCalcStatusModel
    {
        public LockCalcStatusModel(long calcId, long ptId, long ptNum, int sinDate, DateTime createDate, string createMachine)
        {
            CalcId = calcId;
            PtId = ptId;
            PtNum = ptNum;
            SinDate = sinDate;
            CreateDate = createDate;
            CreateMachine = createMachine;
        }

        public long CalcId { get; private set; }

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public int SinDate { get; private set; }

        public DateTime CreateDate { get; private set; }

        public DateTime LockDate
        {
            get
            {
                return CreateDate;
            }
        }

        public string CreateMachine { get; set; }

        public string Machine
        {
            get
            {
                return CreateMachine?.ToUpper() ?? string.Empty;
            }
        }
    }
}
