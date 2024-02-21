﻿using CalculateService.Futan.Models;

namespace CalculateService.Interface
{
    public interface IFutancalcViewModel
    {
        void FutanCalculation(
            int hpId,
            long ptId,
            int sinDate,
            List<SinKouiCountModel> sinKouiCounts,
            List<SinKouiModel> sinKouis,
            List<SinKouiDetailModel> sinKouiDetails,
            List<SinRpInfModel> sinRpInfs,
            int seikyuUp);

        List<KaikeiInfModel> TrialFutanCalculation(
            int hpId,
            long ptId,
            int sinDate,
            long raiinNo,
            List<SinKouiCountModel> sinKouiCounts,
            List<SinKouiModel> sinKouis,
            List<SinKouiDetailModel> sinKouiDetails,
            List<SinRpInfModel> sinRpInfs,
            List<RaiinInfModel> raiinInfs
        );

        void DetailCalculate(bool raiinAdjust);

        void ReleaseResource();
    }
}
