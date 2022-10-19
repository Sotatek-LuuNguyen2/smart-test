﻿using CommonCheckers.OrderRealtimeChecker.Enums;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class UnitCheckerResult<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        public int Sinday;

        public long PtId;

        public ActionResultType ActionType = ActionResultType.OK;

        public TOdrInf NewData { get; set; } = null;

        public bool IsError = false;

        public object ErrorInfo = null;

        public RealtimeCheckerType CheckerType { get; private set; }

        public TOdrInf CheckingData { get; private set; }

        public List<string> AdditionData { get; set; } = new List<string>();

        public UnitCheckerResult(RealtimeCheckerType checkerType, TOdrInf checkingData, int sinday, long ptId)
        {
            CheckerType = checkerType;
            CheckingData = checkingData;
            Sinday = sinday;
            PtId = ptId;
        }
    }
}
