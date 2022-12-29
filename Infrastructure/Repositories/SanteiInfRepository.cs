﻿using Domain.Constant;
using Domain.Models.Santei;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SanteiInfRepository : RepositoryBase, ISanteiInfRepository
{
    public SanteiInfRepository(ITenantProvider tenantProvider) : base(tenantProvider) { }

    public List<SanteiInfModel> GetListSanteiInfModel(int hpId, long ptId, int sinDate)
    {
        // Santei Inf
        var santeiInfQuery = NoTrackingDataContext.SanteiInfs.Where(item => item.HpId == hpId
                                                                && (item.PtId == ptId || item.PtId == 0));

        // Item Cd List from SanteiInf
        var itemCdList = santeiInfQuery.Select(item => item.ItemCd).Distinct().ToList();

        var tenMsts = NoTrackingDataContext.TenMsts.Where(e => e.HpId == hpId
                                                            && itemCdList.Contains(e.ItemCd)
                                                            && e.StartDate <= sinDate
                                                            && e.EndDate >= sinDate)
                                                   .Select(item => new { item.ItemCd, item.Name });

        // Query Santei inf code
        var kensaTenMst = NoTrackingDataContext.TenMsts.Where(e => e.HpId == hpId
                                                                && e.StartDate <= sinDate
                                                                && e.EndDate >= sinDate)
                                                       .Select(item => new { item.SanteiItemCd, item.ItemCd });

        var tenMstList = from santeiInf in santeiInfQuery
                         join tenMst in kensaTenMst on santeiInf.ItemCd
                                                   equals tenMst.SanteiItemCd into tenMstLeft
                         from tenMst in tenMstLeft.DefaultIfEmpty()
                         select new
                         {
                             SanteCd = santeiInf.ItemCd,
                             ItemCd = tenMst.ItemCd ?? santeiInf.ItemCd
                         };

        // Santei inf detail query
        var santeiInfDetailQuery = NoTrackingDataContext.SanteiInfDetails.Where(item => item.HpId == hpId
                                                                                     && item.PtId == ptId
                                                                                     && item.IsDeleted == 0
                                                                                     && item.EndDate >= sinDate)
                                                                         .Select(item => new { item.ItemCd, item.KisanDate });

        // Get 起算日. Get min KISAN_DATE (Check YUUKOU_DATE >= SINDATE)
        var kisanDateQuery = from kisanDate in santeiInfDetailQuery
                             group kisanDate by kisanDate.ItemCd into g
                             select new
                             {
                                 ItemCd = g.Key,
                                 KisanDate = g.Select(item => item.KisanDate).Max()
                             };

        // Get Last order 前回日
        var odrInfDetailQuery = NoTrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                                        && item.PtId == ptId
                                                                                        && item.SinDate < sinDate)
                                                                                        .Select(item => new
                                                                                        {
                                                                                            item.HpId,
                                                                                            item.SinDate,
                                                                                            item.PtId,
                                                                                            item.RaiinNo,
                                                                                            item.RpEdaNo,
                                                                                            item.RpNo,
                                                                                            item.ItemCd,
                                                                                            item.Suryo
                                                                                        });

        var odrInfQuery = NoTrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                                && item.PtId == ptId
                                                                                && item.SinDate < sinDate
                                                                                && item.IsDeleted == 0)
                                                                                .Select(item => new
                                                                                {
                                                                                    item.HpId,
                                                                                    item.SinDate,
                                                                                    item.PtId,
                                                                                    item.RaiinNo,
                                                                                    item.RpEdaNo,
                                                                                    item.RpNo
                                                                                });


        var odrInfLastOdrQuery = from odrInfDetail in odrInfDetailQuery
                                 join odrInf in odrInfQuery on new { odrInfDetail.HpId, odrInfDetail.SinDate, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpEdaNo, odrInfDetail.RpNo }
                                                            equals new { odrInf.HpId, odrInf.SinDate, odrInf.PtId, odrInf.RaiinNo, odrInf.RpEdaNo, odrInf.RpNo }
                                 join tenMst in tenMstList on new { odrInfDetail.ItemCd }
                                                           equals new { tenMst.ItemCd }
                                 group odrInfDetail by tenMst.SanteCd into g
                                 select new
                                 {
                                     ItemCd = g.Key,
                                     SinDate = g.Max(x => x.SinDate)
                                 };

        Dictionary<string, int> dicLastOrderDate = new();
        foreach (var lastOdr in odrInfLastOdrQuery.ToList())
        {
            dicLastOrderDate.Add(lastOdr.ItemCd, lastOdr.SinDate);
        }

        // Count and sum 回数 and 数量
        var odrInfDetailCountQuery = from odrInfDetail in odrInfDetailQuery
                                     join odrInf in odrInfQuery on new { odrInfDetail.HpId, odrInfDetail.SinDate, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpEdaNo, odrInfDetail.RpNo }
                                                                equals new { odrInf.HpId, odrInf.SinDate, odrInf.PtId, odrInf.RaiinNo, odrInf.RpEdaNo, odrInf.RpNo }
                                     join tenMst in tenMstList on new { odrInfDetail.ItemCd }
                                                                equals new { tenMst.ItemCd }
                                     join kisan in kisanDateQuery on new { odrInfDetail.ItemCd }
                                                                equals new { kisan.ItemCd } into kisanLeft
                                     from kisan in kisanLeft.DefaultIfEmpty()
                                     where kisan.KisanDate == 0 || odrInfDetail.SinDate >= kisan.KisanDate
                                     group odrInfDetail by tenMst.SanteCd into g
                                     select new
                                     {
                                         ItemCd = g.Key,
                                         Count = g.Count(),
                                         Sum = g.Sum(o => o.Suryo)
                                     };
        var odrInfDetailCountList = odrInfDetailCountQuery.Select(item => new SanteiInfModel(
                                                                                                item.ItemCd,
                                                                                                item.Count,
                                                                                                item.Sum,
                                                                                                0,
                                                                                                0
                                                            )).ToList();

        // Current Month Count query
        int currentMonthStDate = 100 * (sinDate / 100) + 1;
        int currentMonthEndDate = currentMonthStDate + 30;
        var currentMonthOdrInfDetailQuery = from odrInfDetail in odrInfDetailQuery
                                            join odrInf in odrInfQuery on new { odrInfDetail.HpId, odrInfDetail.SinDate, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpEdaNo, odrInfDetail.RpNo }
                                                                       equals new { odrInf.HpId, odrInf.SinDate, odrInf.PtId, odrInf.RaiinNo, odrInf.RpEdaNo, odrInf.RpNo }
                                            join tenMst in tenMstList on new { odrInfDetail.ItemCd }
                                                                       equals new { tenMst.ItemCd }
                                            where odrInfDetail.SinDate <= currentMonthEndDate
                                               && odrInfDetail.SinDate >= currentMonthStDate
                                            group odrInfDetail by tenMst.SanteCd into g
                                            select new
                                            {
                                                ItemCd = g.Key,
                                                Count = g.Count(),
                                                Sum = g.Sum(o => o.Suryo)
                                            };
        var currentMonthOdrInfDetailList = currentMonthOdrInfDetailQuery.Select(item => new SanteiInfModel(
                                                                                                             item.ItemCd,
                                                                                                             0,
                                                                                                             0,
                                                                                                             item.Count,
                                                                                                             item.Sum
                                                                        )).ToList();

        // If have 起算日 then have 回数 and 数量 
        // Main query
        var query = from santeiInf in santeiInfQuery
                    join tenMst in tenMsts on new { santeiInf.ItemCd }
                                               equals new { tenMst.ItemCd }
                    select new
                    {
                        SanteiInf = santeiInf,
                        TenMst = tenMst
                    };

        var result = query.OrderBy(item => item.SanteiInf.PtId)
                    .ThenBy(item => item.SanteiInf.SeqNo)
                    .ToList();

        return result.Select(item => ConvertToSanteiInfModel(
                                                    item.SanteiInf,
                                                    item.TenMst.Name,
                                                    dicLastOrderDate,
                                                    odrInfDetailCountList,
                                                    currentMonthOdrInfDetailList))
                     .ToList();
    }

    public List<SanteiInfModel> GetOnlyListSanteiInf(int hpId, long ptId)
    {
        var listSanteiInfs = NoTrackingDataContext.SanteiInfs.Where(item => item.HpId == hpId
                                                                          && (item.PtId == ptId || item.PtId == 0))
                                                              .ToList();
        return listSanteiInfs.Select(item => ConvertToSanteiInfModel(item))
                             .ToList();
    }

    public List<SanteiInfDetailModel> GetListSanteiInfDetailModel(int hpId, long ptId)
    {
        return NoTrackingDataContext.SanteiInfDetails.Where(item => item.HpId == hpId
                                                                 && item.IsDeleted == 0
                                                                 && item.PtId == ptId)
                                                     .Select(item => new SanteiInfDetailModel(
                                                                                            item.Id,
                                                                                            item.PtId,
                                                                                            item.ItemCd ?? string.Empty,
                                                                                            item.EndDate,
                                                                                            item.KisanSbt,
                                                                                            item.KisanDate,
                                                                                            item.Byomei ?? string.Empty,
                                                                                            item.HosokuComment ?? string.Empty,
                                                                                            item.Comment ?? string.Empty
                                                     )).ToList();
    }

    public List<string> GetListSanteiByomeis(int hpId, long ptId, int sinDate, int hokenPid)
    {
        return NoTrackingDataContext.PtByomeis.Where(item => item.HpId == hpId
                                                                     && item.PtId == ptId
                                                                     && item.IsDeleted != 1
                                                                     && (item.HokenPid == hokenPid || item.HokenPid == 0)
                                                                     && item.IsNodspKarte == 0
                                                                     && (item.TenkiKbn <= TenkiKbnConst.Continued || (item.StartDate <= (sinDate / 100 * 100 + 31) && item.TenkiDate >= (sinDate / 100 * 100 + 1)))
                                                                ).OrderBy(p => p.TenkiKbn)
                                                                 .ThenByDescending(x => x.IsImportant)
                                                                 .ThenBy(p => p.SortNo)
                                                                 .ThenByDescending(p => p.StartDate)
                                                                 .ThenByDescending(p => p.TenkiDate)
                                                                 .ThenBy(p => p.Id)
                                                                 .Select(item => item.Byomei ?? string.Empty)
                                                                 .ToList();

    }

    public bool SaveSantei(int hpId, int userId, List<SanteiInfModel> listSanteiInfModels, List<SanteiInfDetailModel> listSanteiInfDetailModels)
    {
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        return executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    if (SaveListSanteiInf(hpId, userId, listSanteiInfModels) && SaveListSanteiInfDetail(hpId, userId, listSanteiInfDetailModels))
                    {
                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    transaction.Rollback();
                    return false;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            });
    }

    public bool CheckExistItemCd(int hpId, List<string> listItemCds)
    {
        var tenMsts = NoTrackingDataContext.TenMsts.Where(item =>
                                                                    item.HpId == hpId
                                                                    && item.IsDeleted == 0
                                                                    && listItemCds.Contains(item.ItemCd)
                                                              ).Select(item => item.ItemCd)
                                                               .ToList();
        if (listItemCds.Any() && tenMsts.Any())
        {
            foreach (var item in listItemCds)
            {
                if (!tenMsts.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    #region private function
    private SanteiInfModel ConvertToSanteiInfModel(SanteiInf santeiInf, string itemName, Dictionary<string, int> dicLastOrderDate, List<SanteiInfModel> odrInfDetailCountList, List<SanteiInfModel> currentMonthOdrInfDetailList)
    {
        int lastOdrDate = 0;
        int santeiItemCount = 0;
        double santeiItemSum = 0;
        int currentMonthSanteiItemCount = 0;
        double currentMonthSanteiItemSum = 0;

        // Get last order sindate
        foreach (var item in dicLastOrderDate)
        {
            if (item.Key == santeiInf.ItemCd)
            {
                lastOdrDate = item.Value;
                break;
            }
        }

        // Count and sum item
        var odrDetail = odrInfDetailCountList.FirstOrDefault(item => item.ItemCd == santeiInf.ItemCd);
        if (odrDetail != null)
        {
            santeiItemCount = odrDetail.SanteiItemCount;
            santeiItemSum = odrDetail.SanteiItemSum;
        }

        // Count and sum item by month
        var currentMonthOdrDetail = currentMonthOdrInfDetailList.FirstOrDefault(item => item.ItemCd == santeiInf.ItemCd);
        if (currentMonthOdrDetail != null)
        {
            currentMonthSanteiItemCount = currentMonthOdrDetail.CurrentMonthSanteiItemCount;
            currentMonthSanteiItemSum = currentMonthOdrDetail.CurrentMonthSanteiItemSum;
        }

        return new SanteiInfModel(
                                     santeiInf.Id,
                                     santeiInf.PtId,
                                     santeiInf.ItemCd ?? string.Empty,
                                     santeiInf.SeqNo,
                                     santeiInf.AlertDays,
                                     santeiInf.AlertTerm,
                                     itemName,
                                     lastOdrDate,
                                     santeiItemCount,
                                     santeiItemSum,
                                     currentMonthSanteiItemCount,
                                     currentMonthSanteiItemSum
                                 );
    }

    private SanteiInfModel ConvertToSanteiInfModel(SanteiInf santeiInf)
    {
        return new SanteiInfModel(
                                     santeiInf.Id,
                                     santeiInf.PtId,
                                     santeiInf.ItemCd ?? string.Empty,
                                     santeiInf.AlertDays,
                                     santeiInf.AlertTerm
                                 );
    }

    private Dictionary<long, int> GetLastSanteiInfSeqNoDic(int hpId, List<long> listPtIds)
    {
        Dictionary<long, int> result = new();
        var listSanteiInfs = NoTrackingDataContext.SanteiInfs.Where(item => item.HpId == hpId && listPtIds.Contains(item.PtId)).Distinct().ToList();
        if (listSanteiInfs.Any())
        {
            foreach (var ptId in listPtIds.Distinct())
            {
                var listSanteiInfByPtId = listSanteiInfs.Where(item => item.PtId == ptId).ToList();
                int maxSeqNo = 0;
                if (listSanteiInfByPtId.Any())
                {
                    maxSeqNo = listSanteiInfByPtId.Max(item => item.SeqNo);
                }
                result.Add(ptId, maxSeqNo);
            }
        }
        return result;
    }

    private bool SaveListSanteiInf(int hpId, int userId, List<SanteiInfModel> listSanteiInfModels)
    {
        var listIdSanteiInf = listSanteiInfModels.Where(item => item.Id > 0).Select(item => item.Id).ToList();
        var listSanteiInfDb = TrackingDataContext.SanteiInfs.Where(item => listIdSanteiInf.Contains(item.Id)).ToList();
        var lastSeqNoDic = GetLastSanteiInfSeqNoDic(hpId, listSanteiInfModels.Select(item => item.PtId).ToList());
        foreach (var model in listSanteiInfModels)
        {
            if (model.Id <= 0 && !model.IsDeleted)
            {
                int lastSeqNo = lastSeqNoDic.FirstOrDefault(item => item.Key == model.PtId).Value;
                TrackingDataContext.SanteiInfs.Add(ConvertToNewSanteiInfEntity(hpId, userId, lastSeqNo, model));
            }
            else if (model.Id > 0)
            {
                var santeiInf = listSanteiInfDb.FirstOrDefault(item => item.Id == model.Id);
                if (santeiInf != null)
                {
                    if (model.IsDeleted)
                    {
                        var listSantaiInfDetailDeletes = NoTrackingDataContext.SanteiInfDetails.Where(item =>
                                                                                                            item.HpId == hpId
                                                                                                         && item.IsDeleted == 0
                                                                                                         && item.PtId == santeiInf.PtId)
                                                                                               .Select(item => new SanteiInfDetailModel(
                                                                                                                                    item.Id,
                                                                                                                                    item.PtId,
                                                                                                                                    item.ItemCd ?? string.Empty,
                                                                                                                                    item.EndDate,
                                                                                                                                    item.KisanSbt,
                                                                                                                                    item.KisanDate,
                                                                                                                                    item.Byomei ?? string.Empty,
                                                                                                                                    item.HosokuComment ?? string.Empty,
                                                                                                                                    item.Comment ?? string.Empty,
                                                                                                                                    true
                                                                                               )).ToList();
                        TrackingDataContext.SanteiInfs.Remove(santeiInf);
                        SaveListSanteiInfDetail(hpId, userId, listSantaiInfDetailDeletes);
                    }
                    else
                    {
                        santeiInf.AlertTerm = model.AlertTerm;
                        santeiInf.AlertDays = model.AlertDays;
                        santeiInf.UpdateDate = DateTime.UtcNow;
                        santeiInf.UpdateId = userId;
                    }
                }
            }
        }
        return true;
    }

    private bool SaveListSanteiInfDetail(int hpId, int userId, List<SanteiInfDetailModel> listSanteiInfDetailModels)
    {
        var listIdSanteiInf = listSanteiInfDetailModels.Where(item => item.Id > 0).Select(item => item.Id).ToList();
        var listSanteiInfDb = TrackingDataContext.SanteiInfDetails.Where(item => listIdSanteiInf.Contains(item.Id)).ToList();
        foreach (var model in listSanteiInfDetailModels)
        {
            if (model.Id <= 0 && !model.IsDeleted)
            {
                TrackingDataContext.SanteiInfDetails.Add(ConvertToNewSanteiInfDetailEntity(hpId, userId, model));
            }
            else if (model.Id > 0)
            {
                var santeiInfDetail = listSanteiInfDb.FirstOrDefault(item => item.Id == model.Id);
                if (santeiInfDetail != null)
                {
                    santeiInfDetail.UpdateDate = DateTime.UtcNow;
                    santeiInfDetail.UpdateId = userId;
                    if (model.IsDeleted)
                    {
                        santeiInfDetail.IsDeleted = 1;
                    }
                    else
                    {
                        santeiInfDetail.KisanSbt = model.KisanSbt;
                        santeiInfDetail.KisanDate = model.KisanDate;
                        santeiInfDetail.Byomei = model.Byomei;
                        santeiInfDetail.HosokuComment = model.HosokuComment;
                        santeiInfDetail.EndDate = model.EndDate;
                        santeiInfDetail.Comment = model.Comment;
                    }
                }
            }
        }
        return true;
    }

    private SanteiInf ConvertToNewSanteiInfEntity(int hpId, int userId, int lastSeqNo, SanteiInfModel model)
    {
        return new SanteiInf()
        {
            Id = 0,
            PtId = model.PtId,
            HpId = hpId,
            ItemCd = model.ItemCd,
            SeqNo = lastSeqNo + 1,
            AlertDays = model.AlertDays,
            AlertTerm = model.AlertTerm,
            CreateDate = DateTime.UtcNow,
            CreateId = userId,
            UpdateDate = DateTime.UtcNow,
            UpdateId = userId
        };
    }

    private SanteiInfDetail ConvertToNewSanteiInfDetailEntity(int hpId, int userId, SanteiInfDetailModel model)
    {
        return new SanteiInfDetail()
        {
            Id = 0,
            PtId = model.PtId,
            HpId = hpId,
            ItemCd = model.ItemCd,
            EndDate = model.EndDate,
            KisanSbt = model.KisanSbt,
            KisanDate = model.KisanDate,
            Byomei = model.Byomei,
            HosokuComment = model.HosokuComment,
            Comment = model.Comment,
            IsDeleted = 0,
            CreateDate = DateTime.UtcNow,
            CreateId = userId,
            UpdateDate = DateTime.UtcNow,
            UpdateId = userId
        };
    }
    #endregion
}
