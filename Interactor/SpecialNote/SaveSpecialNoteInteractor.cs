﻿using Domain.Models.SpecialNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Helper.Common;
using UseCase.SpecialNote.Save;

namespace Interactor.SpecialNote
{
    public class SaveSpecialNoteInteractor : ISaveSpecialNoteInputPort
    {
        private readonly ISpecialNoteRepository _specialNoteRepository;

        public SaveSpecialNoteInteractor(ISpecialNoteRepository specialNoteRepository)
        {
            _specialNoteRepository = specialNoteRepository;
        }

        public SaveSpecialNoteOutputData Handle(SaveSpecialNoteInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.InvalidPtId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.InvalidSinDate);
                }
                var result = _specialNoteRepository.SaveSpecialNote(inputData.HpId, inputData.PtId, inputData.SinDate, new SummaryInfModel(inputData.SummaryTab.Id, inputData.SummaryTab.HpId, inputData.SummaryTab.PtId, inputData.SummaryTab.SeqNo, inputData.SummaryTab.Text, inputData.SummaryTab.Rtext, CIUtil.GetJapanDateTimeNow(), CIUtil.GetJapanDateTimeNow()), inputData.ImportantNoteTab, new PatientInfoModel(inputData.PatientInfoTab.PregnancyItems.Select(p => new PtPregnancyModel(
                        p.Id,
                        p.HpId,
                        p.PtId,
                        p.SeqNo,
                        p.StartDate,
                        p.EndDate,
                        p.PeriodDate,
                        p.PeriodDueDate,
                        p.OvulationDate,
                        p.OvulationDueDate,
                        p.IsDeleted,
                        CIUtil.GetJapanDateTimeNow(),
                        inputData.UserId,
                        string.Empty,
                        p.SinDate
                    )
                    ).ToList(), inputData.PatientInfoTab.PtCmtInfItems, inputData.PatientInfoTab.SeikatureInfItems, new List<PhysicalInfoModel> { new PhysicalInfoModel(inputData.PatientInfoTab.KensaInfDetailItems.Select(k => new KensaInfDetailModel(k.HpId, k.PtId, k.IraiCd, k.SeqNo, k.IraiDate, k.RaiinNo, k.KensaItemCd, k.ResultVal, k.ResultType, k.AbnormalKbn, k.IsDeleted, k.CmtCd1, k.CmtCd2, DateTime.MinValue, string.Empty, string.Empty, 0)).ToList()) }), inputData.UserId);

                if (!result) return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.Failed);

                return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.Successed);
            }
            finally
            {
                _specialNoteRepository.ReleaseResource();
            }
        }
    }
}
