﻿using Domain.Models.Receipt;

namespace UseCase.Receipt;

public class ReceCmtItem
{
    public ReceCmtItem(ReceCmtModel model)
    {
        Id = model.Id;
        PtId = model.PtId;
        SeqNo = model.SeqNo;
        SinYm = model.SinYm;
        HokenId = model.HokenId;
        CmtKbn = model.CmtKbn;
        CmtSbt = model.CmtSbt;
        Cmt = model.Cmt;
        CmtData = model.CmtData;
        ItemCd = model.ItemCd;
        IsDeleted = false;
    }

    public ReceCmtItem(long id, long ptId, int seqNo, int sinYm, int hokenId, int cmtKbn, int cmtSbt, string cmt, string cmtData, string itemCd, bool isDeleted)
    {
        Id = id;
        PtId = ptId;
        SeqNo = seqNo;
        SinYm = sinYm;
        HokenId = hokenId;
        CmtKbn = cmtKbn;
        CmtSbt = cmtSbt;
        Cmt = cmt;
        CmtData = cmtData;
        ItemCd = itemCd;
        IsDeleted = isDeleted;
    }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public int SeqNo { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int CmtKbn { get; private set; }

    public int CmtSbt { get; private set; }

    public string Cmt { get; private set; }

    public string CmtData { get; private set; }

    public string ItemCd { get; private set; }

    public bool IsDeleted { get; private set; }
}
