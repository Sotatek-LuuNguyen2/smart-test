﻿namespace UseCase.OrdInfs.Upsert
{
    public class OdrInfItem
    {
        public OdrInfItem(int hpId, long raiinNo, long rpNo, long rpEdaNo, long ptId, int sinDate, int hokenPid, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, long id)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            PtId = ptId;
            SinDate = sinDate;
            HokenPid = hokenPid;
            OdrKouiKbn = odrKouiKbn;
            RpName = rpName;
            InoutKbn = inoutKbn;
            SikyuKbn = sikyuKbn;
            SyohoSbt = syohoSbt;
            SanteiKbn = santeiKbn;
            TosekiKbn = tosekiKbn;
            DaysCnt = daysCnt;
            SortNo = sortNo;
            Id = id;
        }

        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public long RpNo { get; private set; }
        public long RpEdaNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int HokenPid { get; private set; }
        public int OdrKouiKbn { get; private set; }
        public string RpName { get; private set; }
        public int InoutKbn { get; private set; }
        public int SikyuKbn { get; private set; }
        public int SyohoSbt { get; private set; }
        public int SanteiKbn { get; private set; }
        public int TosekiKbn { get; private set; }
        public int DaysCnt { get; private set; }
        public int SortNo { get; private set; }
        public long Id { get; private set; }
        public List<OdrInfDetailItem> OdrDetails { get; private set; }
    }
}
