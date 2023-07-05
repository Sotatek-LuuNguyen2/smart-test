﻿using Domain.Common;

namespace Domain.Models.Lock
{
    public interface ILockRepository : IRepositoryBase
    {
        bool ExistLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo);

        bool AddLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId, string token, string tabKey);

        LockModel CheckOpenSpecialNote(int hpId, string functionCd, long ptId);

        List<LockModel> GetLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        List<long> RemoveLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId, string tabKey);

        List<long> RemoveAllLock(int hpId, int userId);

        List<long> RemoveAllLock(int hpId, int userId, long ptId, int sinDate, string functionCd, string tabKey);

        bool ExtendTtl(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        List<LockModel> GetLockInfo(int hpId, long ptId, List<string> lisFunctionCd_B, int sinDate_B, long raiinNo);

        List<LockModel> GetVisitingLockStatus(int hpId, int userId, long ptId, string functionCode);

        string GetFunctionNameLock(string functionCode);

        List<ResponseLockModel> GetResponseLockModel(int hpId, long ptId, int sinDate);

        bool CheckLockOpenAccounting(int hpId, long ptId, long raiinNo, int userId);
    }
}
