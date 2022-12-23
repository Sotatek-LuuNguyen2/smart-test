﻿using Domain.Constant;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Helper;
using Helper.Common;
using UseCase.PatientInfor.Save;

namespace Interactor.PatientInfor
{
    public class SavePatientInfoInteractor : ISavePatientInfoInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly ISystemConfRepository _systemConfRepository;

        public SavePatientInfoInteractor(IPatientInforRepository patientInforRepository, ISystemConfRepository systemConfRepository)
        {
            _patientInforRepository = patientInforRepository;
            _systemConfRepository = systemConfRepository;
        }

        public SavePatientInfoOutputData Handle(SavePatientInfoInputData inputData)
        {
            var validations = Validation(inputData);
            if (validations.Any())
            {
                string msgValidation = string.Empty;
                foreach (var item in validations)
                    msgValidation += string.IsNullOrEmpty(msgValidation) ? item : $",{item}";

                return new SavePatientInfoOutputData(msgValidation, SavePatientInfoStatus.Failed, 0);
            }
            try
            {
                (bool, long) result;
                if (inputData.Patient.PtId == 0)
                    result = _patientInforRepository.CreatePatientInfo(inputData.Patient, inputData.PtKyuseis, inputData.PtSanteis, inputData.Insurances, inputData.HokenInfs, inputData.HokenKohis, inputData.PtGrps, inputData.UserId);
                else
                    result = _patientInforRepository.UpdatePatientInfo(inputData.Patient, inputData.PtKyuseis, inputData.PtSanteis, inputData.Insurances, inputData.HokenInfs, inputData.HokenKohis, inputData.PtGrps, inputData.UserId);

                if (result.Item1)
                    return new SavePatientInfoOutputData(string.Empty, SavePatientInfoStatus.Successful, result.Item2);
                else
                    return new SavePatientInfoOutputData(string.Empty, SavePatientInfoStatus.Failed, 0);
            }
            catch
            {
                return new SavePatientInfoOutputData(string.Empty, SavePatientInfoStatus.Failed, 0);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
            }
        }

        private IEnumerable<SavePatientInfoValidationResult> Validation(SavePatientInfoInputData model)
        {
            var resultMessages = new List<SavePatientInfoValidationResult>();

            #region Patient Info

            if (model.Patient.HpId <= 0)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HpId`"), SavePatientInforValidationCode.InvalidHpId));

            if (string.IsNullOrEmpty(model.Patient.Name))
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), "`Patient.Name`"), SavePatientInforValidationCode.InvalidName));

            if (model.Patient.Name.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Name`"), SavePatientInforValidationCode.InvalidName));

            if (string.IsNullOrEmpty(model.Patient.KanaName))
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), "`Patient.KanaName`"), SavePatientInforValidationCode.InvalidKanaName));

            if (model.Patient.KanaName.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.KanaName`"), SavePatientInforValidationCode.InvalidKanaName));

            resultMessages.AddRange(IsValidKanjiName(model.Patient.KanaName, model.Patient.Name, model.Patient.HpId));

            if (model.Patient.Birthday == 0)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), "`Patient.Birthday`"), SavePatientInforValidationCode.InvalidBirthday));

            if (model.Patient.IsDead < 0 || model.Patient.IsDead > 1)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.IsDead`"), SavePatientInforValidationCode.InvalidIsDead));

            if (model.Patient.IsDead == 1 && model.Patient.DeathDate == 0)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), "`Patient.DeathDate`"), SavePatientInforValidationCode.InvalidDeathDate));

            if (model.Patient.HomePost != null && model.Patient.HomePost.Length > 7)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HomePost`"), SavePatientInforValidationCode.InvalidHomePost));

            if (model.Patient.HomeAddress1 != null && model.Patient.HomeAddress1.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HomeAddress1`"), SavePatientInforValidationCode.InvalidHomeAddress1));

            if (model.Patient.HomeAddress2 != null && model.Patient.HomeAddress2.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HomeAddress2`"), SavePatientInforValidationCode.InvalidHomeAddress2));

            if (model.Patient.Tel1 != null && model.Patient.Tel1.Length > 15)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Tel1`"), SavePatientInforValidationCode.InvalidTel1));

            if (model.Patient.Tel2 != null && model.Patient.Tel2.Length > 15)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Tel2`"), SavePatientInforValidationCode.InvalidTel2));

            if (model.Patient.Mail != null && model.Patient.Mail.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Mail`"), SavePatientInforValidationCode.InvalidMail));

            if (model.Patient.Setanusi != null && model.Patient.Setanusi.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Setanusi`"), SavePatientInforValidationCode.InvalidSetanusi));

            if (model.Patient.Zokugara != null && model.Patient.Zokugara.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Zokugara`"), SavePatientInforValidationCode.InvalidZokugara));

            if (model.Patient.Job != null && model.Patient.Job.Length > 40)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Job`"), SavePatientInforValidationCode.InvalidJob));

            if (model.Patient.RenrakuPost != null && model.Patient.RenrakuPost.Length > 7)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuPost`"), SavePatientInforValidationCode.InvalidRenrakuPost));

            if (model.Patient.RenrakuAddress1 != null && model.Patient.RenrakuAddress1.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuAddress1`"), SavePatientInforValidationCode.InvalidRenrakuAddress1));

            if (model.Patient.RenrakuAddress2 != null && model.Patient.RenrakuAddress2.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuAddress2`"), SavePatientInforValidationCode.InvalidRenrakuAddress2));

            if (model.Patient.RenrakuTel != null && model.Patient.RenrakuTel.Length > 15)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuTel`"), SavePatientInforValidationCode.InvalidRenrakuTel));

            if (model.Patient.RenrakuMemo != null && model.Patient.RenrakuMemo.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuMemo`"), SavePatientInforValidationCode.InvalidRenrakuMemo));

            if (model.Patient.OfficeName != null && model.Patient.OfficeName.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeName`"), SavePatientInforValidationCode.InvalidOfficeName));

            if (model.Patient.OfficePost != null && model.Patient.OfficePost.Length > 7)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficePost`"), SavePatientInforValidationCode.InvalidOfficePost));

            if (model.Patient.OfficeAddress1 != null && model.Patient.OfficeAddress1.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeAddress1`"), SavePatientInforValidationCode.InvalidOfficeAddress1));

            if (model.Patient.OfficeAddress2 != null && model.Patient.OfficeAddress2.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeAddress2`"), SavePatientInforValidationCode.InvalidOfficeAddress2));

            if (model.Patient.OfficeTel != null && model.Patient.OfficeTel.Length > 15)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeTel`"), SavePatientInforValidationCode.InvalidOfficeTel));

            if (model.Patient.OfficeMemo != null && model.Patient.OfficeMemo.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeMemo`"), SavePatientInforValidationCode.InvalidOfficeMemo));

            #endregion Patient Info

            #region Hoken

            var listHokenIdValid = model.HokenInfs.Where(x => x.IsDeleted == 0)
                                    .Select(x => x.HokenId).Where(x => x != 0).ToList();

            var listHokenKohiIdValid = model.HokenKohis.Where(x => x.IsDeleted == 0)
                                    .Select(x => x.HokenId).Where(x => x != 0).ToList();

            for (int i = 0; i < model.Insurances.Count; i++)
            {
                if (model.Insurances[i].HokenId != 0 && !listHokenIdValid.Contains(model.Insurances[i].HokenId))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`Insurances[{i}].HokenId`"), SavePatientInforValidationCode.ÍnuranceInvalidHokenId));

                if (model.Insurances[i].Kohi1Id != 0 && !listHokenKohiIdValid.Contains(model.Insurances[i].Kohi1Id))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`Insurances[{i}].Kohi1Id`"), SavePatientInforValidationCode.ÍnuranceInvalidKohi1Id));

                if (model.Insurances[i].Kohi2Id != 0 && !listHokenKohiIdValid.Contains(model.Insurances[i].Kohi2Id))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`Insurances[{i}].Kohi2Id`"), SavePatientInforValidationCode.ÍnuranceInvalidKohi2Id));

                if (model.Insurances[i].Kohi3Id != 0 && !listHokenKohiIdValid.Contains(model.Insurances[i].Kohi3Id))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`Insurances[{i}].Kohi3Id`"), SavePatientInforValidationCode.ÍnuranceInvalidKohi3Id));

                if (model.Insurances[i].Kohi4Id != 0 && !listHokenKohiIdValid.Contains(model.Insurances[i].Kohi4Id))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`Insurances[{i}].Kohi4Id`"), SavePatientInforValidationCode.ÍnuranceInvalidKohi4Id));
            }

            #endregion Hoken

            #region PtKytsei

            for (int i = 0; i < model.PtKyuseis.Count; i++)
            {
                if (model.PtKyuseis[i].SeqNo < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].SeqNo`"), SavePatientInforValidationCode.PtKyuseiInvalidSeqNo));

                if (string.IsNullOrEmpty(model.PtKyuseis[i].KanaName))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), $"`PtKyuseis[{i}].KanaName`"), SavePatientInforValidationCode.PtKyuseiInvalidKanaName));

                if (model.PtKyuseis[i].KanaName.Length > 100)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].KanaName`"), SavePatientInforValidationCode.PtKyuseiInvalidKanaName));

                if (string.IsNullOrEmpty(model.PtKyuseis[i].Name))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), $"`PtKyuseis[{i}].Name`"), SavePatientInforValidationCode.PtKyuseiInvalidName));

                if (model.PtKyuseis[i].Name.Length > 100)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].Name`"), SavePatientInforValidationCode.PtKyuseiInvalidName));

                if (model.PtKyuseis[i].EndDate < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].EndDate`"), SavePatientInforValidationCode.PtKyuseiInvalidEndDate));
            }

            #endregion PtKytsei

            #region PtSanteis

            for (int i = 0; i < model.PtSanteis.Count; i++)
            {
                if (model.PtSanteis[i].SeqNo < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].SeqNo`"), SavePatientInforValidationCode.PtSanteiInvalidSeqNo));

                if (model.PtSanteis[i].EdaNo < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].EdaNo`"), SavePatientInforValidationCode.PtSanteiInvalidEdaNo));

                if (model.PtSanteis[i].KbnVal < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].KbnVal`"), SavePatientInforValidationCode.PtSanteiInvalidKbnVal));;

                if (model.PtSanteis[i].StartDate < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].StartDate`"), SavePatientInforValidationCode.PtSanteiInvalidStartDate));

                if (model.PtSanteis[i].EndDate < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].EndDate`"), SavePatientInforValidationCode.PtSanteiInvalidEndDate));
            }

            #endregion PtSanteis

            #region PtGrps
            for (int i = 0; i < model.PtGrps.Count; i++)
            {
                if (model.PtGrps[i].GroupId < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtGrps[{i}].GroupId`"), SavePatientInforValidationCode.PtGrpInvalidGroupId));

                if (string.IsNullOrEmpty(model.PtGrps[i].GroupCode))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), $"`PtGrps[{i}].GroupCode`"), SavePatientInforValidationCode.PtGrpInvalidGroupCode));

                if (model.PtGrps[i].GroupCode.Length > 4)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtGrps[{i}].GroupCode`"), SavePatientInforValidationCode.PtGrpInvalidGroupCode));
            }
            #endregion PtGrps

            return resultMessages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kanaName">Kana Name</param>
        /// <param name="kanjiName">Name</param>
        /// <param name="hpId"></param>
        /// <returns></returns>
        private IEnumerable<SavePatientInfoValidationResult> IsValidKanjiName(string kanaName , string kanjiName, int hpId)
        {

            var resultMessages = new List<SavePatientInfoValidationResult>();
            SplitName(kanaName, out string firstNameKana, out string lastNameKana);
            SplitName(kanjiName, out string firstNameKanji, out string lastNameKanji);
            bool isValidateFullName = _systemConfRepository.GetSettingValue(1017, 0, hpId) == 0;

            string message = string.Empty;
            if (string.IsNullOrEmpty(firstNameKana))
            {
                message = string.Format(ErrorMessage.MessageType_mInp00010, new string[] { "カナ" });
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidKanaName));
            }

            if (string.IsNullOrEmpty(firstNameKanji))
            {
                message = string.Format(ErrorMessage.MessageType_mInp00010, new string[] { "氏名" });
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidName));
            }

            // validate full name if setting
            if (isValidateFullName)
            {
                if (string.IsNullOrEmpty(lastNameKana))
                {
                    message = string.Format(ErrorMessage.MessageType_mInp00010, new string[] { "カナ" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidKanaName));
                }

                if (string.IsNullOrEmpty(lastNameKanji))
                {
                    message = string.Format(ErrorMessage.MessageType_mInp00010, new string[] { "氏名" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidName));
                }
            }

            int FKanNmChkJIS = (int)_systemConfRepository.GetSettingValue(1003, 0, hpId);

            // 患者氏名チェック（受付）※JISコード
            if (FKanNmChkJIS > 0)
            {
                // 患者名_漢字 JisｺｰﾄﾞCheck
                string sBuf2 = string.Empty;
                string sBuf = CIUtil.Chk_JISKj(firstNameKanji, out sBuf2);
                if (!string.IsNullOrEmpty(sBuf))
                {
                    if (FKanNmChkJIS == 2)
                    {
                        message = string.Format(ErrorMessage.MessageType_mInp00140, new string[] { "漢字名", "'" + sBuf + "'" + " の文字" });
                        resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidName));
                    }
                }
                if (isValidateFullName)
                {
                    sBuf2 = string.Empty;
                    // 患者姓_漢字 JisｺｰﾄﾞCheck
                    sBuf = CIUtil.Chk_JISKj(lastNameKanji, out sBuf2);
                    if (!string.IsNullOrEmpty(sBuf))
                    {
                        if (FKanNmChkJIS == 2)
                        {
                            message = string.Format(ErrorMessage.MessageType_mInp00140, new string[] { "漢字姓", "'" + sBuf + "'" + " の文字" });
                            resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidName));
                        }
                    }
                }
            }

            if (firstNameKana.Length > 20)
            {
                message = string.Format(ErrorMessage.MessageType_mFree00030, new string[] { "患者名（カナ）は２０文字以下を入力してください。" });
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidKanaName));
            }

            if (firstNameKanji.Length > 30)
            {
                message = string.Format(ErrorMessage.MessageType_mFree00030, new string[] { "患者名は３０文字以下を入力してください。" });
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidName));
            }

            if (isValidateFullName)
            {
                if (lastNameKana.Length > 20)
                {
                    message = string.Format(ErrorMessage.MessageType_mFree00030, new string[] { "患者姓（カナ）は２０文字以下を入力してください。" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidKanaName));
                }

                if (lastNameKanji.Length > 30)
                {
                    message = string.Format(ErrorMessage.MessageType_mFree00030, new string[] { "患者姓は３０文字以下を入力してください。" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidName));
                }
            }
            return resultMessages;
        }

        private void SplitName(string name, out string firstName, out string lastName)
        {
            firstName = "";
            lastName = "";
            char[] arraySpace = { '　', ' ' };
            if (!string.IsNullOrEmpty(name))
            {
                if (!arraySpace.Any(u => name.Any(c => c == u)))
                {
                    firstName = name;
                    return;
                }
                for (int i = 0; i < arraySpace.Length; i++)
                {
                    var arrayName = name.Split(arraySpace[i]);
                    if (arrayName != null)
                    {
                        if (arrayName.Length < 2 && arrayName.Length > 0)
                        {
                            continue;
                        }
                        else if (arrayName.Length >= 2)
                        {
                            int index = name.IndexOf(arraySpace[i]);
                            lastName = name.Substring(0, index);
                            firstName = name.Substring(index + 1).Trim();
                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }
}