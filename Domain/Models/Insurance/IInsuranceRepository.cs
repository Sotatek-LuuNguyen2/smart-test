﻿using Domain.Models.InsuranceInfor;

namespace Domain.Models.Insurance
{
    public interface IInsuranceRepository
    {
        IEnumerable<InsuranceModel> GetInsuranceListById(int hpId, long ptId, int sinDate);
        IEnumerable<InsuranceModel> GetListHokenPattern(int hpId, long ptId, bool allowDisplayDeleted);
        bool CheckHokenPIdList(List<int> hokenPIds);
    }
}
