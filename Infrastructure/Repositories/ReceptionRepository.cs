using Domain.Common;
using Domain.Constant;
using Domain.Models.Reception;
using Infrastructure.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class ReceptionRepository : IReceptionRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public ReceptionRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public ReceptionModel? Get(long raiinNo)
        {
            var receptionEntity = _tenantDataContext.RaiinInfs.SingleOrDefault(r => r.RaiinNo == raiinNo);

            if (receptionEntity == null)
            {
                return null;
            }

            return new ReceptionModel
                (
                    receptionEntity.HpId,
                    receptionEntity.PtId,
                    receptionEntity.SinDate,
                    receptionEntity.RaiinNo,
                    receptionEntity.OyaRaiinNo,
                    receptionEntity.HokenPid,
                    receptionEntity.SanteiKbn,
                    receptionEntity.Status,
                    receptionEntity.IsYoyaku,
                    receptionEntity.YoyakuTime ?? string.Empty,
                    receptionEntity.YoyakuId,
                    receptionEntity.UketukeSbt,
                    receptionEntity.UketukeTime ?? string.Empty,
                    receptionEntity.UketukeId,
                    receptionEntity.UketukeNo,
                    receptionEntity.SinStartTime,
                    receptionEntity.SinEndTime ?? string.Empty,
                    receptionEntity.KaikeiTime ?? string.Empty,
                    receptionEntity.KaikeiId,
                    receptionEntity.KaId,
                    receptionEntity.TantoId,
                    receptionEntity.SyosaisinKbn,
                    receptionEntity.JikanKbn
                );
        }

        public List<ReceptionRowModel> GetList(int hpId, int sinDate)
        {
            return GetReceptionRowModels(hpId, sinDate);
        }

        private List<ReceptionRowModel> GetReceptionRowModels(int hpId, int sinDate)
        {
            // 1. Prepare all the necessary collections for the join operation
            // Raiin (Reception)
            var raiinInfs = _tenantDataContext.RaiinInfs.AsNoTracking().Where(x => x.IsDeleted == DeleteTypes.None);
            var raiinCmtInfs = _tenantDataContext.RaiinCmtInfs.AsNoTracking().Where(x => x.IsDelete == DeleteTypes.None);
            var raiinKbnInfs = _tenantDataContext.RaiinKbnInfs.AsNoTracking().Where(x => x.IsDelete == DeleteTypes.None);
            var raiinKbnDetails = _tenantDataContext.RaiinKbnDetails.AsNoTracking().Where(x => x.IsDeleted == DeleteTypes.None);
            // Pt (Patient)
            var ptInfs = _tenantDataContext.PtInfs.AsNoTracking().Where(x => x.IsDelete == DeleteTypes.None);
            var ptCmtInfs = _tenantDataContext.PtCmtInfs.AsNoTracking().Where(x => x.IsDeleted == DeleteTypes.None);
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.AsNoTracking().Where(x => x.IsDeleted == DeleteTypes.None);
            var ptKohis = _tenantDataContext.PtKohis.AsNoTracking().Where(x => x.IsDeleted == DeleteTypes.None);
            // Rsv (Reservation)
            var rsvInfs = _tenantDataContext.RsvInfs.AsNoTracking();
            var rsvFrameMsts = _tenantDataContext.RsvFrameMsts.AsNoTracking().Where(x => x.IsDeleted == DeleteTypes.None);
            // User (Doctor)
            var userMsts = _tenantDataContext.UserMsts.AsNoTracking().Where(x => x.IsDeleted == DeleteTypes.None);
            // Ka (Department)
            var kaMsts = _tenantDataContext.KaMsts.AsNoTracking().Where(x => x.IsDeleted == DeleteTypes.None);
            // Lock (Function lock)
            var lockInfs = _tenantDataContext.LockInfs.AsNoTracking().Where(x =>
                x.FunctionCd == FunctionCode.MedicalExaminationCode || x.FunctionCd == FunctionCode.TeamKarte);
            // Uketuke
            var uketukeSbtMsts = _tenantDataContext.UketukeSbtMsts.AsNoTracking().Where(x => x.IsDeleted == DeleteTypes.None);

            // 2. Perform the join operation
            var raiinQuery =
                from raiinInf in raiinInfs.Where(x => x.HpId == hpId && x.SinDate == sinDate)
                join ptInf in ptInfs on
                    new { raiinInf.HpId, raiinInf.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
                join raiinCmtInfComment in raiinCmtInfs.Where(x => x.CmtKbn == CmtKbns.Comment) on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo } equals
                    new { raiinCmtInfComment.HpId, raiinCmtInfComment.PtId, raiinCmtInfComment.SinDate, raiinCmtInfComment.RaiinNo } into relatedRaiinCmtInfComments
                from relatedRaiinCmtInfComment in relatedRaiinCmtInfComments.DefaultIfEmpty()
                join raiinCmtInfRemark in raiinCmtInfs.Where(x => x.CmtKbn == CmtKbns.Remark) on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo } equals
                    new { raiinCmtInfRemark.HpId, raiinCmtInfRemark.PtId, raiinCmtInfRemark.SinDate, raiinCmtInfRemark.RaiinNo } into relatedRaiinCmtInfRemarks
                from relatedRaiinCmtInfRemark in relatedRaiinCmtInfRemarks.DefaultIfEmpty()
                from ptCmtInf in ptCmtInfs.Where(x => x.PtId == ptInf.PtId).OrderByDescending(x => x.UpdateDate).Take(1).DefaultIfEmpty()
                join rsvInf in rsvInfs on raiinInf.RaiinNo equals rsvInf.RaiinNo into relatedRsvInfs
                from relatedRsvInf in relatedRsvInfs.DefaultIfEmpty()
                join rsvFrameMst in rsvFrameMsts on relatedRsvInf.RsvFrameId equals rsvFrameMst.RsvFrameId into relatedRsvFrameMsts
                from relatedRsvFrameMst in relatedRsvFrameMsts.DefaultIfEmpty()
                join lockInf in lockInfs on raiinInf.RaiinNo equals lockInf.RaiinNo into relatedLockInfs
                from relatedLockInf in relatedLockInfs.DefaultIfEmpty()
                join uketukeSbtMst in uketukeSbtMsts on raiinInf.UketukeSbt equals uketukeSbtMst.KbnId into relatedUketukeSbtMsts
                from relatedUketukeSbtMst in relatedUketukeSbtMsts.DefaultIfEmpty()
                join tanto in userMsts on
                    new { raiinInf.HpId, UserId = raiinInf.TantoId } equals
                    new { tanto.HpId, tanto.UserId } into relatedTantos
                from relatedTanto in relatedTantos.DefaultIfEmpty()
                join primaryDoctor in userMsts on
                    new { raiinInf.HpId, UserId = ptInf.PrimaryDoctor } equals
                    new { primaryDoctor.HpId, primaryDoctor.UserId } into relatedPrimaryDoctors
                from relatedPrimaryDoctor in relatedPrimaryDoctors.DefaultIfEmpty()
                join kaMst in kaMsts on
                    new { raiinInf.HpId, raiinInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into relatedKaMsts
                from relatedKaMst in relatedKaMsts.DefaultIfEmpty()
                join ptHokenPattern in ptHokenPatterns on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid } into relatedPtHokenPatterns
                from relatedPtHokenPattern in relatedPtHokenPatterns.DefaultIfEmpty()
                from ptKohi1 in ptKohis.Where(x => x.PtId == ptInf.PtId && x.HokenId == relatedPtHokenPattern.Kohi1Id).Take(1).DefaultIfEmpty()
                from ptKohi2 in ptKohis.Where(x => x.PtId == ptInf.PtId && x.HokenId == relatedPtHokenPattern.Kohi2Id).Take(1).DefaultIfEmpty()
                from ptKohi3 in ptKohis.Where(x => x.PtId == ptInf.PtId && x.HokenId == relatedPtHokenPattern.Kohi3Id).Take(1).DefaultIfEmpty()
                from ptKohi4 in ptKohis.Where(x => x.PtId == ptInf.PtId && x.HokenId == relatedPtHokenPattern.Kohi4Id).Take(1).DefaultIfEmpty()
                select new
                {
                    raiinInf,
                    ptInf,
                    ptCmtInf,
                    uketukeSbtName = relatedUketukeSbtMst.KbnName,
                    tantoName = relatedTanto.Sname,
                    primaryDoctorName = relatedPrimaryDoctor.Sname,
                    kaName = relatedKaMst.KaSname,
                    relatedRaiinCmtInfComment,
                    relatedRaiinCmtInfRemark,
                    relatedRsvFrameMst,
                    relatedLockInf,
                    relatedPtHokenPattern,
                    ptKohi1,
                    ptKohi2,
                    ptKohi3,
                    ptKohi4,
                    raiinKbnDetails = (
                        from inf in raiinKbnInfs
                        join detail in raiinKbnDetails on
                            new { inf.HpId, inf.GrpId, inf.KbnCd } equals
                            new { detail.HpId, GrpId = detail.GrpCd, detail.KbnCd }
                        where inf.HpId == hpId
                            && inf.PtId == raiinInf.PtId
                            && inf.SinDate == sinDate
                            && inf.RaiinNo == raiinInf.RaiinNo
                        select detail
                    ).ToList(),
                    parentRaiinNo = (
                        from r in raiinInfs
                        where r.HpId == hpId
                            && r.PtId == raiinInf.PtId
                            && r.SinDate == raiinInf.SinDate
                            && r.OyaRaiinNo == raiinInf.OyaRaiinNo
                            && r.RaiinNo != r.OyaRaiinNo
                        select r.OyaRaiinNo
                    ).FirstOrDefault(),
                    lastVisitDate = (
                        from x in raiinInfs
                        where x.HpId == hpId
                            && x.PtId == raiinInf.PtId
                            && x.SinDate < sinDate
                            && x.Status >= RaiinState.TempSave
                        orderby x.SinDate descending
                        select x.SinDate
                    ).FirstOrDefault()
                };

            var raiins = raiinQuery.ToList();
            var grpIds = _tenantDataContext.RaiinKbnMsts.Where(x => x.HpId == hpId && x.IsDeleted == DeleteTypes.None).Select(x => x.GrpCd).ToList();
            var models = raiins.Select(r => new ReceptionRowModel(
                r.raiinInf.RaiinNo,
                r.parentRaiinNo,
                r.raiinInf.UketukeNo,
                r.relatedLockInf is not null,
                r.raiinInf.Status,
                r.ptInf.PtNum,
                r.ptInf.KanaName,
                r.ptInf.Name,
                r.ptInf.Sex,
                r.ptInf.Birthday,
                r.raiinInf.YoyakuTime ?? string.Empty,
                r.relatedRsvFrameMst?.RsvFrameName ?? string.Empty,
                r.uketukeSbtName,
                r.raiinInf.UketukeTime ?? string.Empty,
                r.raiinInf.SinStartTime ?? string.Empty,
                r.raiinInf.SinEndTime ?? string.Empty,
                r.raiinInf.KaikeiTime ?? string.Empty,
                r.relatedRaiinCmtInfComment?.Text ?? string.Empty,
                r.ptCmtInf?.Text ?? string.Empty,
                r.tantoName,
                r.kaName,
                r.lastVisitDate,
                r.primaryDoctorName ?? string.Empty,
                r.relatedRaiinCmtInfRemark?.Text ?? string.Empty,
                r.raiinInf.ConfirmationState,
                r.raiinInf.ConfirmationResult ?? string.Empty,
                grpIds,
                dynamicCells: r.raiinKbnDetails.Select(d => new DynamicCell(d.GrpCd, d.KbnCd, d.KbnName, d.ColorCd ?? string.Empty)).ToList(),
                sinDate
            )).ToList();

            return models;
        }
    }
}
