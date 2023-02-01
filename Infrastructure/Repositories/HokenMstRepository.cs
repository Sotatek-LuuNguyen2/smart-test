﻿using Domain.Models.HokenMst;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class HokenMstRepository : RepositoryBase, IHokenMstRepository
    {
        public HokenMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public HokenMasterModel GetHokenMaster(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int sinDate)
        {
            var hokenMaster = NoTrackingDataContext.HokenMsts.FirstOrDefault(u => u.HpId == hpId &&
                                                                 u.HokenNo == hokenNo &&
                                                                 u.HokenEdaNo == hokenEdaNo &&
                                                                 (u.PrefNo == prefNo
                                                                 || u.IsOtherPrefValid == 1) &&
                                                                 u.StartDate <= sinDate &&
                                                                 u.EndDate >= sinDate);

            if (hokenMaster == null)
            {
                hokenMaster = NoTrackingDataContext.HokenMsts.FirstOrDefault(u => u.HpId == hpId &&
                                                                                u.HokenNo == hokenNo &&
                                                                                u.HokenEdaNo == hokenEdaNo &&
                                                                                (u.PrefNo == prefNo
                                                                                || u.IsOtherPrefValid == 1));
            }

            var result = new HokenMasterModel(hpId,
                                        hokenMaster?.HokenNo ?? 0,
                                        hokenMaster?.HokenEdaNo ?? 0,
                                        hokenMaster?.StartDate ?? 0,
                                        hokenMaster?.EndDate ?? 0,
                                        hokenMaster?.PrefNo ?? 0,
                                        hokenMaster?.Houbetu ?? string.Empty,
                                        hokenMaster?.HokenName ?? string.Empty,
                                        hokenMaster?.HokenNameCd ?? string.Empty,
                                        hokenMaster?.HokenSname ?? string.Empty,
                                        hokenMaster?.HokenSbtKbn ?? 0,
                                        hokenMaster?.HokenKohiKbn ?? 0,
                                        hokenMaster?.IsLimitList ?? 0,
                                        hokenMaster?.IsLimitListSum ?? 0,
                                        hokenMaster?.CheckDigit ?? 0,
                                        hokenMaster?.JyukyuCheckDigit ?? 0,
                                        hokenMaster?.IsFutansyaNoCheck ?? 0,
                                        hokenMaster?.IsJyukyusyaNoCheck ?? 0,
                                        hokenMaster?.IsTokusyuNoCheck ?? 0,
                                        hokenMaster?.AgeStart ?? 0,
                                        hokenMaster?.AgeEnd ?? 0,
                                        hokenMaster?.IsOtherPrefValid ?? 0,
                                        hokenMaster?.EnTen ?? 0,
                                        hokenMaster?.FutanKbn ?? 0,
                                        hokenMaster?.FutanRate ?? 0,
                                        hokenMaster?.KaiLimitFutan ?? 0,
                                        hokenMaster?.DayLimitFutan ?? 0,
                                        hokenMaster?.DayLimitCount ?? 0,
                                        hokenMaster?.MonthLimitFutan ?? 0,
                                        hokenMaster?.MonthLimitCount ?? 0,
                                        hokenMaster?.LimitKbn ?? 0,
                                        hokenMaster?.CountKbn ?? 0,
                                        hokenMaster?.CalcSpKbn ?? 0,
                                        hokenMaster?.MonthSpLimit ?? 0,
                                        hokenMaster?.KogakuTekiyo ?? 0,
                                        hokenMaster?.KogakuTotalKbn ?? 0,
                                        hokenMaster?.FutanYusen ?? 0,
                                        hokenMaster?.ReceSeikyuKbn ?? 0,
                                        hokenMaster?.ReceKisai ?? 0,
                                        hokenMaster?.ReceKisai2 ?? 0,
                                        hokenMaster?.ReceTenKisai ?? 0,
                                        hokenMaster?.ReceFutanHide ?? 0,
                                        hokenMaster?.ReceFutanRound ?? 0,
                                        hokenMaster?.ReceZeroKisai ?? 0,
                                        hokenMaster?.ReceSpKbn ?? 0,
                                        string.Empty);

            string? roudou = NoTrackingDataContext.RoudouMsts.FirstOrDefault(u => u.RoudouCd == result.PrefNo.ToString())?.RoudouName;
            if (!string.IsNullOrEmpty(roudou))
                result.Roudou = "(" + roudou + ")";

            return result;
        }
        public List<HokenMasterModel> CheckExistHokenEdaNo(int hokenNo, int hpId)
        {
            var existHokenEdaNo = NoTrackingDataContext.HokenMsts
               .Where(x => x.HpId == hpId && x.HokenNo == hokenNo)
               .Select(x => new HokenMasterModel(
                       x.HokenNo,
                       x.HokenEdaNo
               )).ToList();
            return existHokenEdaNo;
        }

        public List<PtHokenPatternModel> FindPtHokenPatternList(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var oyaRaiinNo = NoTrackingDataContext.RaiinInfs.FirstOrDefault(item =>
                item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate &&
                item.RaiinNo == raiinNo && item.IsDeleted == 0);

            if (oyaRaiinNo == null || oyaRaiinNo.Status <= 3)
                return new List<PtHokenPatternModel>();

            var listRaiinInf = NoTrackingDataContext.RaiinInfs.Where(item =>
                item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate &&
                item.OyaRaiinNo == oyaRaiinNo.OyaRaiinNo && item.Status > 3 && item.IsDeleted == 0);
            if (!listRaiinInf.Any())
                return new List<PtHokenPatternModel>();

            var listHokenPattern = NoTrackingDataContext.PtHokenPatterns
                                                        .Where(pattern => pattern.HpId == hpId
                                                         && pattern.PtId == ptId
                                                         && pattern.IsDeleted == 0
                                                         && listRaiinInf.Select(item => item.HokenPid).ToList().Contains(pattern.HokenPid))
                                                        .Select(x => new PtHokenPatternModel(
                                                                x.HpId,
                                                                x.PtId,
                                                                x.HokenPid,
                                                                x.HokenKbn,
                                                                x.HokenSbtCd,
                                                                x.HokenId,
                                                                x.Kohi1Id,
                                                                x.Kohi2Id,
                                                                x.Kohi3Id,
                                                                x.Kohi4Id,
                                                                x.HokenMemo ?? string.Empty,
                                                                x.StartDate,
                                                                x.EndDate
                                                            )).ToList();

            return listHokenPattern;
        }


        public void ReleaseResource()
        {
            DisposeDataContext();
        }

    }
}
