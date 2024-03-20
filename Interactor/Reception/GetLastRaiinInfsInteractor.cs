﻿using Domain.Models.Reception;
using UseCase.Reception.GetLastRaiinInfs;

namespace Interactor.Reception;

public class GetLastRaiinInfsInteractor : IGetLastRaiinInfsInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public GetLastRaiinInfsInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public GetLastRaiinInfsOutputData Handle(GetLastRaiinInfsInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetLastRaiinInfsOutputData(GetLastRaiinInfsStatus.InvalidHpId);
            }
            else if (inputData.PtId <= 0)
            {
                return new GetLastRaiinInfsOutputData(GetLastRaiinInfsStatus.InvalidPtId);
            }

            var result = new List<ReceptionModel>();
            if (inputData.IsLastVisit)
            {
                var lastVisit = _receptionRepository.GetLastVisit(inputData.HpId, inputData.PtId, inputData.SinDate, true);
                if (lastVisit.HpId != 0 && lastVisit.PtId != 0 && lastVisit.RaiinNo != 0 && lastVisit.SinDate != 0)
                {
                    result.Add(new ReceptionModel(lastVisit.HpId,
                                           lastVisit.PtId,
                                           lastVisit.SinDate,
                                           lastVisit.RaiinNo,
                                           lastVisit.OyaRaiinNo,
                                           lastVisit.HokenPid,
                                           lastVisit.SanteiKbn,
                                           lastVisit.Status,
                                           lastVisit.IsYoyaku,
                                           lastVisit.YoyakuTime ?? String.Empty,
                                           lastVisit.YoyakuId,
                                           lastVisit.UketukeSbt,
                                           lastVisit.UketukeTime ?? String.Empty,
                                           lastVisit.UketukeId,
                                           lastVisit.UketukeNo,
                                           lastVisit.SinStartTime ?? string.Empty,
                                           lastVisit.SinEndTime ?? String.Empty,
                                           lastVisit.KaikeiTime ?? String.Empty,
                                           lastVisit.KaikeiId,
                                           lastVisit.KaId,
                                           lastVisit.TantoId,
                                           lastVisit.SyosaisinKbn,
                                           lastVisit.JikanKbn,
                                           string.Empty));
                }
               
            }
            else
            {
                result = _receptionRepository.GetLastRaiinInfs(inputData.HpId, inputData.PtId, inputData.SinDate, true);
            }
            return new GetLastRaiinInfsOutputData(GetLastRaiinInfsStatus.Successed, result);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}
