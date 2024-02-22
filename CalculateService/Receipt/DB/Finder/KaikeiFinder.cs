﻿using CalculateService.Constants;
using CalculateService.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using CalculateService.Ika.Models;
using Helper.Common;
using Domain.Constant;
using CalculateService.Interface;
using CalculateService.Futan.DB;

namespace CalculateService.Receipt.DB.Finder
{
    class KaikeiFinder
    {
        /// <summary>
        /// Do not hardcode hpId
        /// </summary>
        //private int HpId = Hardcode.HospitalID;
        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public KaikeiFinder(TenantDataContext tenantDataContext, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }
        public List<Models.KaikeiDetailModel> FindKaikeiDetail(int hpId, long ptId, int sinYm, int hokenId)
        {
            var kaikeiDtl = _tenantDataContext.KaikeiDetails.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate >= sinYm * 100 + 1 &&
                p.SinDate <= sinYm * 100 + 31 &&
                p.HokenId == hokenId)
                .OrderBy(p => p.SinDate)
                .ThenBy(p => p.OyaRaiinNo)
                .ThenBy(p => p.RaiinNo)
                .ToList();

            List<Models.KaikeiDetailModel> results = new List<Models.KaikeiDetailModel>();

            kaikeiDtl.ForEach(p => results.Add(new Models.KaikeiDetailModel(p)));

            return results;
        }
    }
}
