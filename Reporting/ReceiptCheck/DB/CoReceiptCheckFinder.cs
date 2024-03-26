﻿using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.ReceiptCheck.Model;

namespace Reporting.ReceiptCheck.DB;

public class CoReceiptCheckFinder : RepositoryBase, ICoReceiptCheckFinder
{
    public CoReceiptCheckFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public List<CoReceiptCheckModel> GetCoReceiptChecks(int hpId, List<long> ptIds, int sinYm)
    {
        var listReceCheckErr = NoTrackingDataContext.ReceCheckErrs.Where(x => x.HpId == hpId);

        var listPtInf = NoTrackingDataContext.PtInfs.Where(x => x.HpId == hpId && x.IsTester == 0);
        var listReceInf = NoTrackingDataContext.ReceInfs.Where(x => x.HpId == hpId);
        var listReceSeikyu = NoTrackingDataContext.ReceSeikyus.Where(receSeikyu => receSeikyu.HpId == hpId && receSeikyu.SeikyuYm == sinYm);

        var lisResultReceSeikyu = from receiptErr in listReceCheckErr
                                  join receInf in listReceInf on new { receiptErr.HpId, receiptErr.PtId, receiptErr.HokenId, receiptErr.SinYm }
                                                    equals new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                  join ptInf in listPtInf on new { HpId = receiptErr.HpId, PtId = receiptErr.PtId }
                                                         equals new { ptInf.HpId, ptInf.PtId }
                                  where receiptErr.HpId == hpId &&
                                        receInf.SeikyuYm == sinYm &&
                                       (receiptErr.SinYm == sinYm ||
                                        listReceSeikyu.Any(receSeikyu => receiptErr.HpId == receSeikyu.HpId
                                                                      && receiptErr.PtId == receSeikyu.PtId
                                                                      && receiptErr.HokenId == receSeikyu.HokenId
                                                                      && receiptErr.SinYm == receSeikyu.SinYm
                                                                      && receInf.SeikyuYm == receSeikyu.SeikyuYm)) &&
                                       (ptIds.Count <= 0 || ptIds.Contains(receiptErr.PtId))
                                  select new
                                  {
                                      PtInf = ptInf,
                                      ReceInf = receInf,
                                      ReceCheckErr = receiptErr
                                  };

        var result = lisResultReceSeikyu.AsEnumerable()
                                        .Select(x => new CoReceiptCheckModel(x.PtInf, x.ReceInf, x.ReceCheckErr))
                                        .OrderBy(x => x.SinYm)
                                        .ThenBy(x => x.PtId)
                                        .ToList();

        return result ?? new();
    }
}
