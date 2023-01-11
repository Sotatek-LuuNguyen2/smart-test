﻿using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Repositories
{
    public class FlowSheetRepository : RepositoryBase, IFlowSheetRepository
    {
        private readonly int cmtKbn = 9;
        private readonly string sinDate = "sindate";
        private readonly string tagNo = "tagno";
        private readonly string fullLineOfKarte = "fulllineofkarte";
        private readonly string syosaisinKbn = "syosaisinkbn";
        private readonly string comment = "comment";

        public FlowSheetRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo, int startIndex, int count, string sort, ref long totalCount)
        {
            List<FlowSheetModel> result;

            var raiinInfsQueryable = NoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0);
            var raiinNos = raiinInfsQueryable.Select(r => r.RaiinNo);
            var karteInfsQueryable = NoTrackingDataContext.KarteInfs.Where(k => raiinNos.Contains(k.RaiinNo) && k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0 && (k.Text != null && !string.IsNullOrEmpty(k.Text.Trim()))).OrderBy(karte => karte.SinDate).ThenBy(karte => karte.KarteKbn); ;

            var tagsQueryable = NoTrackingDataContext.RaiinListTags.Where(tag => tag.HpId == hpId && tag.PtId == ptId);
            var commentsQueryable = NoTrackingDataContext.RaiinListCmts.Where(comment => comment.HpId == hpId && comment.PtId == ptId);

            var raiinKarteQuery = from raiinInf in raiinInfsQueryable.AsEnumerable<RaiinInf>()
                                  join karte in karteInfsQueryable on new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo, raiinInf.SinDate }
                                                equals new { karte.HpId, karte.PtId, karte.RaiinNo, karte.SinDate } into odrKarteLeft
                                  select new
                                  {
                                      RaiinList = raiinInf,
                                      Karte = odrKarteLeft.FirstOrDefault()
                                  };

            var query = from raiinInf in raiinKarteQuery
                        join tagInf in tagsQueryable on raiinInf.RaiinList.RaiinNo equals tagInf.RaiinNo into gjTag
                        from tagInf in gjTag.DefaultIfEmpty()
                        join commentInf in commentsQueryable on raiinInf.RaiinList.RaiinNo equals commentInf.RaiinNo into gjComment
                        from commentInf in gjComment.DefaultIfEmpty()
                        select new
                        {
                            raiinInf.RaiinList.RaiinNo,
                            raiinInf.RaiinList.SyosaisinKbn,
                            raiinInf.RaiinList.Status,
                            raiinInf.RaiinList.SinDate,
                            Text = raiinInf.Karte == null ? string.Empty : raiinInf.Karte.Text,
                            TagNo = tagInf == null ? 0 : tagInf.TagNo,
                            TagSeqNo = tagInf == null ? 0 : tagInf.SeqNo,
                            CommentContent = commentInf == null ? string.Empty : commentInf.Text,
                            CommentSeqNo = commentInf == null ? 0 : commentInf.SeqNo,
                            CommentKbn = commentInf == null ? 9 : commentInf.CmtKbn,
                            RaiinListInfs = (from raiinListInf in NoTrackingDataContext.RaiinListInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinInf.RaiinList.RaiinNo)
                                             join raiinListMst in NoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None)
                                             on raiinListInf.KbnCd equals raiinListMst.KbnCd
                                             select new RaiinListInfModel(raiinInf.RaiinList.RaiinNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName ?? string.Empty, raiinListMst.ColorCd ?? string.Empty)
                                            )
                            //.AsEnumerable<RaiinListInfModel>()
                        };

            var todayOdr = query.Select(r =>
                new FlowSheetModel(
                    r.SinDate,
                    r.TagNo,
                    r.Text,
                    r.RaiinNo,
                    r.SyosaisinKbn,
                    r.CommentContent,
                    r.Status,
                    false,
                    r.RaiinNo == raiinNo,
                    r.RaiinListInfs.ToList(),
                    ptId,
                    (r.RaiinNo == raiinNo && r.SinDate == sinDate && r.Status < 3)
                   )
            ).AsEnumerable<FlowSheetModel>();

            // Add NextOrder Information
            // Get next order information
            var rsvkrtOdrInfs = NoTrackingDataContext.RsvkrtOdrInfs.Where(r => r.HpId == hpId
                                                                                        && r.PtId == ptId
                                                                                        && r.IsDeleted == DeleteTypes.None);
            var rsvkrtMsts = NoTrackingDataContext.RsvkrtMsts.Where(r => r.HpId == hpId
                                                                                        && r.PtId == ptId
                                                                                        && r.IsDeleted == DeleteTypes.None
                                                                                        && r.RsvkrtKbn == 0);
            var nextOdrKarteInfs = NoTrackingDataContext.RsvkrtKarteInfs.Where(karte => karte.HpId == hpId
                                   && karte.PtId == ptId
                                   && karte.IsDeleted == 0
                                   && (karte.Text != null && !string.IsNullOrEmpty(karte.Text.Trim())))
                                   .OrderBy(karte => karte.RsvDate)
                                   .ThenBy(karte => karte.KarteKbn);

            var groupNextOdr = from rsvkrtOdrInf in rsvkrtOdrInfs.AsEnumerable<RsvkrtOdrInf>()
                               join rsvkrtMst in rsvkrtMsts on new { rsvkrtOdrInf.HpId, rsvkrtOdrInf.PtId, rsvkrtOdrInf.RsvkrtNo }
                                                equals new { rsvkrtMst.HpId, rsvkrtMst.PtId, rsvkrtMst.RsvkrtNo }
                               join karte in nextOdrKarteInfs on new { rsvkrtOdrInf.HpId, rsvkrtOdrInf.PtId, rsvkrtOdrInf.RsvkrtNo }
                                                equals new { karte.HpId, karte.PtId, karte.RsvkrtNo } into odrKarteLeft
                               group rsvkrtOdrInf by new { rsvkrtOdrInf.HpId, rsvkrtOdrInf.PtId, rsvkrtOdrInf.RsvDate, rsvkrtOdrInf.RsvkrtNo } into g
                               select new
                               {
                                   g.Key.HpId,
                                   g.Key.PtId,
                                   g.Key.RsvDate,
                                   g.Key.RsvkrtNo
                               };

            var queryNextOdr = from nextOdr in groupNextOdr
                               join karte in nextOdrKarteInfs on new { nextOdr.HpId, nextOdr.PtId, nextOdr.RsvkrtNo }
                                                equals new { karte.HpId, karte.PtId, karte.RsvkrtNo } into odrKarteLeft
                               join tagInf in tagsQueryable on nextOdr.RsvkrtNo equals tagInf.RaiinNo into gjTag
                               from tagInf in gjTag.DefaultIfEmpty()
                               select new
                               {
                                   NextOdr = nextOdr,
                                   TagInf = tagInf,
                                   Karte = odrKarteLeft.FirstOrDefault(),
                                   RaiinListInfs = (from raiinListInf in NoTrackingDataContext.RaiinListInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == nextOdr.RsvkrtNo)
                                                    join raiinListMst in NoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None)
                                                    on raiinListInf.KbnCd equals raiinListMst.KbnCd
                                                    select new RaiinListInfModel(nextOdr.RsvkrtNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName ?? string.Empty, raiinListMst.ColorCd ?? string.Empty)
                                            )
                                   //.AsEnumerable<RaiinListInfModel>()
                               };

            var nextOdrs = queryNextOdr.Select(
                    data => new FlowSheetModel(
                        data.NextOdr?.RsvDate ?? 0,
                        data.TagInf?.TagNo ?? 0,
                        data.Karte?.Text ?? string.Empty,
                        data.NextOdr?.RsvkrtNo ?? 0,
                        -1,
                        string.Empty,
                        0,
                        true,
                        false,
                        data.RaiinListInfs.ToList(),
                        data.NextOdr?.PtId ?? 0,
                        false
                    ));

            var todayNextOdrs = todayOdr.Union(nextOdrs).ToList();
            totalCount = todayNextOdrs.Count();

            FlowSheetModel? sinDateCurrent = null;
            if (!todayNextOdrs.Any(r => r.SinDate == sinDate && r.RaiinNo == raiinNo))
            {
                sinDateCurrent = new FlowSheetModel(
                        0,
                        0,
                        string.Empty,
                        0,
                        2,
                        string.Empty,
                        0,
                        false,
                        false,
                        new List<RaiinListInfModel>(),
                        0,
                        false
                    );
                totalCount = totalCount + 1;
            }

            if (string.IsNullOrEmpty(sort))
                result = todayNextOdrs.OrderByDescending(o => o.SinDate).Skip(startIndex).Take(count).ToList();
            else
            {
                todayNextOdrs = SortAll(sort, todayNextOdrs);
                result = todayNextOdrs.Skip(startIndex).Take(count).ToList();
            }

            if (sinDateCurrent != null && startIndex == 0)
            {
                result.Insert(0, sinDateCurrent);
            }

            return result;
        }

        public List<RaiinListMstModel> GetRaiinListMsts(int hpId)
        {
            var raiinListMst = NoTrackingDataContext.RaiinListMsts.Where(m => m.HpId == hpId && m.IsDeleted == DeleteTypes.None).ToList();
            var raiinListDetail = NoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None).ToList();
            var query = from mst in raiinListMst
                        select new
                        {
                            Mst = mst,
                            Detail = raiinListDetail.Where(c => c.HpId == mst.HpId && c.GrpId == mst.GrpId).ToList()
                        };
            var output = query.Select(
                data => new RaiinListMstModel(data.Mst.GrpId, data.Mst.GrpName ?? string.Empty, data.Mst.SortNo, data.Detail.Select(d => new RaiinListDetailModel(d.GrpId, d.KbnCd, d.SortNo, d.KbnName ?? string.Empty, d.ColorCd ?? String.Empty, d.IsDeleted)).ToList()));
            return output.ToList();
        }

        public List<HolidayModel> GetHolidayMst(int hpId, int holidayFrom, int holidayTo)
        {
            var holidayCollection = NoTrackingDataContext.HolidayMsts.Where(h => h.HpId == hpId && h.IsDeleted == DeleteTypes.None && holidayFrom <= h.SinDate && h.SinDate <= holidayTo);
            return holidayCollection.Select(h => new HolidayModel(h.SinDate, h.HolidayKbn, h.KyusinKbn, h.HolidayName ?? string.Empty)).ToList();
        }

        public void UpsertTag(List<FlowSheetModel> inputDatas, int hpId, int userId)
        {
            foreach (var inputData in inputDatas)
            {
                var raiinListTag = TrackingDataContext.RaiinListTags
                           .OrderByDescending(p => p.UpdateDate)
                           .FirstOrDefault(p => p.RaiinNo == inputData.RaiinNo);
                if (raiinListTag is null)
                {
                    TrackingDataContext.RaiinListTags.Add(new RaiinListTag
                    {
                        HpId = hpId,
                        PtId = inputData.PtId,
                        SinDate = inputData.SinDate,
                        RaiinNo = inputData.RaiinNo,
                        TagNo = inputData.TagNo,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        UpdateId = userId,
                        CreateId = userId
                    });
                }
                else
                {
                    raiinListTag.TagNo = inputData.TagNo;
                    raiinListTag.UpdateDate = DateTime.UtcNow;
                    raiinListTag.UpdateId = userId;
                }
            }
            TrackingDataContext.SaveChanges();
        }
        public void UpsertCmt(List<FlowSheetModel> inputDatas, int hpId, int userId)
        {
            foreach (var inputData in inputDatas)
            {
                var raiinListCmt = TrackingDataContext.RaiinListCmts
                               .OrderByDescending(p => p.UpdateDate)
                               .FirstOrDefault(p => p.RaiinNo == inputData.RaiinNo);

                if (raiinListCmt is null)
                {
                    TrackingDataContext.RaiinListCmts.Add(new RaiinListCmt
                    {
                        HpId = hpId,
                        PtId = inputData.PtId,
                        SinDate = inputData.SinDate,
                        RaiinNo = inputData.RaiinNo,
                        CmtKbn = cmtKbn,
                        Text = inputData.Comment,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        UpdateId = userId,
                        CreateId = userId
                    });
                }
                else
                {
                    raiinListCmt.Text = inputData.Comment;
                    raiinListCmt.UpdateDate = DateTime.UtcNow;
                    raiinListCmt.UpdateId = userId;
                }
            }
            TrackingDataContext.SaveChanges();
        }

        private List<FlowSheetModel> SortAll(string sort, List<FlowSheetModel> todayNextOdrs)
        {
            try
            {
                var childrenOfSort = sort.Trim().Split(",");
                var order = todayNextOdrs.OrderBy(o => o.PtId);
                foreach (var item in childrenOfSort)
                {
                    var elementDynamics = item.Trim().Split(" ");
                    var checkGroupId = int.TryParse(elementDynamics[0], out int groupId);

                    if (!checkGroupId)
                    {
                        order = SortStaticColumn(elementDynamics.FirstOrDefault() ?? string.Empty, elementDynamics?.Count() > 1 ? elementDynamics.LastOrDefault() ?? string.Empty : string.Empty, order);
                    }
                    else
                    {
                        if (elementDynamics.Length > 1)
                        {
                            if (elementDynamics[1].ToLower() == "desc")
                            {
                                order = order.ThenByDescending(o => o.RaiinListInfs.FirstOrDefault(r => r.GrpId == groupId)?.KbnName);
                            }
                            else
                            {
                                order = order.ThenBy(o => o.RaiinListInfs.FirstOrDefault(r => r.GrpId == groupId)?.KbnName);
                            }
                        }
                        else
                        {
                            order = order.ThenBy(o => o.RaiinListInfs.FirstOrDefault(r => r.GrpId == groupId)?.KbnName);
                        }
                    }
                }
                todayNextOdrs = order.ToList();
            }
            catch
            {
                todayNextOdrs = todayNextOdrs.OrderByDescending(o => o.SinDate).ToList();
            }

            return todayNextOdrs;
        }

        private IOrderedEnumerable<FlowSheetModel> SortStaticColumn(string fieldName, string sortType, IOrderedEnumerable<FlowSheetModel> order)
        {
            if (fieldName.ToLower().Equals(sinDate))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.SinDate);

                }
                else
                {
                    order = order.ThenByDescending(o => o.SinDate);
                }
            }
            if (fieldName.ToLower().Equals(tagNo))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.TagNo);

                }
                else
                {
                    order = order.ThenByDescending(o => o.TagNo);
                }
            }
            if (fieldName.ToLower().Equals(fullLineOfKarte))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.FullLineOfKarte);

                }
                else
                {
                    order = order.ThenByDescending(o => o.FullLineOfKarte);
                }
            }
            if (fieldName.ToLower().Equals(syosaisinKbn))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.SyosaisinKbn);

                }
                else
                {
                    order = order.ThenByDescending(o => o.SyosaisinKbn);
                }
            }
            if (fieldName.ToLower().Equals(comment))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.Comment);

                }
                else
                {
                    order = order.ThenByDescending(o => o.Comment);
                }
            }

            return order;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}