﻿using CommonCheckers;
using Entity.Tenant;

namespace CommonChecker.Caches.Interface
{
    public interface IMasterDataCacheService
    {
        void InitCache(List<string> itemCodeList, int sinday, long ptId);

        TenMst? GetTenMst(string itemCode, int sinday);

        List<TenMst> GetTenMstList(List<string> itemCodeList, int sinday);

        List<M56ExIngrdtMain> GetM56ExIngrdtMainList(List<string> itemCodeList);

        List<M56ExEdIngredients> GetM56ExEdIngredientList(List<string> itemCodeList);

        List<M56ProdrugCd> GetM56ProdrugCdList(List<string> itemCodeList);

        List<M56ExAnalogue> GetM56ExAnalogueList(List<string> itemCodeList);

        List<M56YjDrugClass> GetM56YjDrugClassList(List<string> itemCodeList);

        List<M56DrugClass> GetM56DrugClassList(List<string> itemCodeList);

        List<KinkiMst> GetKinkiMstList(List<string> itemCodeList);

        PtInf GetPtInf();

        SystemConfig GetSystemConfig();
    }
}
