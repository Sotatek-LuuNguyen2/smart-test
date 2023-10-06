﻿using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.GrowthCurve.Model;

namespace Reporting.GrowthCurve.DB;

public class SpecialNoteFinder : RepositoryBase, ISpecialNoteFinder
{
    public SpecialNoteFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<GcStdInfModel> GetStdPoint(int hpId)
    {
        var list = NoTrackingDataContext.GcStdMsts.Where(item => item.HpId == hpId)
                                                  .Select(item => new GcStdInfModel(item))
                                                  .ToList();
        return list;
    }

    public List<KensaInfDetail> GetKensaInf(long ptId, int fromDate, int toDate, string itemCD)
    {
        var kensaInfDetailCollection = NoTrackingDataContext.KensaInfDetails.Where(item => item.PtId == ptId && (item.KensaItemCd ?? string.Empty).StartsWith("V") && item.IsDeleted == 0);

        if (itemCD != "")
        {
            kensaInfDetailCollection = kensaInfDetailCollection.Where(item => item.KensaItemCd == itemCD);
        }

        if (fromDate >= 0)
        {
            kensaInfDetailCollection = kensaInfDetailCollection.Where(item => item.IraiDate >= fromDate);
        }

        if (toDate >= 0)
        {
            kensaInfDetailCollection = kensaInfDetailCollection.Where(item => item.IraiDate <= toDate);
        }
        var kensaInfDetailList = kensaInfDetailCollection.ToList();
        return kensaInfDetailList.GroupBy(item => new { item.KensaItemCd, item.IraiDate })
                                 .Select(item => item.OrderByDescending(x => x.SeqNo).FirstOrDefault())
                                 .ToList();
    }
}
