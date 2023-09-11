﻿using Domain.Models.MedicalExamination;
using UseCase.UpdateKensaMst;
using static Helper.Constants.TenMstConst;

namespace Interactor.MedicalExamination
{
    public class UpdateKensaMstInteractor : IUpdateKensaMstInputPort
    {
        private readonly IMedicalExaminationRepository _medicalExaminationRepository;

        public UpdateKensaMstInteractor(IMedicalExaminationRepository medicalExaminationRepository)
        {
            _medicalExaminationRepository = medicalExaminationRepository;
        }

        public UpdateKensaMstOutputData Handle(UpdateKensaMstInputData inputData)
        {
            try
            {
                foreach (var data in inputData.TenMsts)
                {
                    var status = data.Validation();
                    if (status != ValidationStatus.Valid)
                    {
                        return new UpdateKensaMstOutputData(ConvertStatusTenMst(status));
                    }
                }
                _medicalExaminationRepository.UpdateKensaMst(inputData.HpId, inputData.UserId, inputData.KensaMsts, inputData.TenMsts);
                return new UpdateKensaMstOutputData(UpdateKensaMstStatus.Success);
            }
            finally
            {
                _medicalExaminationRepository.ReleaseResource();
            }
        }
        private static UpdateKensaMstStatus ConvertStatusTenMst(ValidationStatus status)
        {
            if (status == ValidationStatus.InvalidMasterSbt)
                return UpdateKensaMstStatus.InvalidMasterSbt;
            if (status == ValidationStatus.InvalidItemCd)
                return UpdateKensaMstStatus.InvalidItemCd;
            if (status == ValidationStatus.InvalidMinAge)
                return UpdateKensaMstStatus.InvalidMinAge;
            if (status == ValidationStatus.InvalidMaxAge)
                return UpdateKensaMstStatus.InvalidMaxAge;
            if (status == ValidationStatus.InvalidCdKbn)
                return UpdateKensaMstStatus.InvalidCdKbn;
            if (status == ValidationStatus.InvalidKokuji1)
                return UpdateKensaMstStatus.InvalidKokuji1;
            if (status == ValidationStatus.InvalidKokuji2)
                return UpdateKensaMstStatus.InvalidKokuji2;

            return UpdateKensaMstStatus.Success;
        }
    }
}
