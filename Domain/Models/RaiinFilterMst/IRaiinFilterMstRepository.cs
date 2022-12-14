﻿namespace Domain.Models.RaiinFilterMst;

public interface IRaiinFilterMstRepository
{
    List<RaiinFilterMstModel> GetList();

    int GetTantoId(long ptId, int sinDate, long raiinNo);

    int GetLastTimeDate(int hpId, long ptId, int sinDate);

    void SaveList(List<RaiinFilterMstModel> mstModels, int hpId, int userId);
}
