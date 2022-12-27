﻿namespace Domain.Models.RaiinFilterMst;

public class RaiinFilterMstModel
{
    public RaiinFilterMstModel(int filterId, int sortNo, string filterName,
        int selectKbn, string shortcut, List<RaiinFilterSortModel> columnSortInfos)
    {
        FilterId = filterId;
        SortNo = sortNo;
        FilterName = filterName;
        SelectKbn = selectKbn;
        Shortcut = shortcut;
        ColumnSortInfos = columnSortInfos;
    }

    public RaiinFilterMstModel(long ptId, int sinDate, int uketukeNo, int status, string kaName, string sName)
    {
        PtId = ptId;
        SinDate = sinDate;
        UketukeNo = uketukeNo;
        Status = status;
        KaName = kaName;
        SName = sName;
    }

    public long PtId { get; private set; }
    public int SinDate { get; private set; }
    public int UketukeNo { get; private set; }
    public int Status { get; private set; }
    public string KaName { get; private set; }
    public string SName { get; private set; }

    public int FilterId { get; private set; }
    public int SortNo { get; private set; }
    public string FilterName { get; private set; }

    /// <summary>
    ///     0: 状態に応じた画面を開く
    ///     1: 属性編集
    ///     2: カルテ作成
    ///     3: 窓口精算
    /// </summary>
    public int SelectKbn { get; private set; }
    public string Shortcut { get; private set; }
    public List<RaiinFilterSortModel> ColumnSortInfos { get; private set; }
}
