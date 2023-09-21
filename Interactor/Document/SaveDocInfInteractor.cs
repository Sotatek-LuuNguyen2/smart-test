﻿using Domain.Models.Document;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using UseCase.Document.SaveDocInf;

namespace Interactor.Document;

public class SaveDocInfInteractor : ISaveDocInfInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IReceptionRepository _receptionRepository;

    public SaveDocInfInteractor(IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IPatientInforRepository patientInforRepository, IReceptionRepository receptionRepository)
    {
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _patientInforRepository = patientInforRepository;
        _receptionRepository = receptionRepository;
    }

    public SaveDocInfOutputData Handle(SaveDocInfInputData inputData)
    {
        try
        {
            bool overwriteFile = false;
            string path = string.Empty;
            string fileName = string.Empty;
            var resultValidate = ValidateInputData(inputData);
            if (resultValidate != SaveDocInfStatus.ValidateSuccess)
            {
                return new SaveDocInfOutputData(resultValidate);
            }
            Console.WriteLine("StartConsoleWriteLineDocument");
            // upload file to S3
            var memoryStream = inputData.StreamImage.ToMemoryStreamAsync().Result;
            if (memoryStream.Length == 0 && inputData.FileId <= 0)
            {
                return new SaveDocInfOutputData(SaveDocInfStatus.InvalidFileInput);
            }
            else if (memoryStream.Length > 0)
            {
                Console.WriteLine("memoryStream.Length: " + memoryStream.Length);
                var ptNum = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, 0, 0)?.PtNum ?? 0;
                var listFolderPath = new List<string>(){
                                                          CommonConstants.Store,
                                                          CommonConstants.Files
                                                       };
                path = _amazonS3Service.GetFolderUploadToPtNum(listFolderPath, ptNum);
                fileName = _amazonS3Service.GetUniqueFileNameKey(inputData.FileName.Trim());
                var response = _amazonS3Service.UploadObjectAsync(path, fileName, memoryStream);
                response.Wait();

                if (response.Result.Length > 0)
                {
                    inputData.SetFileName(fileName);
                    overwriteFile = true;
                }
                Console.WriteLine("path: " + path);
                Console.WriteLine("fileName: " + fileName);
                Console.WriteLine("response.Result: " + response.Result);
            }
            if (_documentRepository.SaveDocInf(inputData.UserId, ConvertToDocInfModel(inputData), overwriteFile))
            {
                return new SaveDocInfOutputData(SaveDocInfStatus.Successed);
            }
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(fileName))
            {
                _amazonS3Service.DeleteObjectAsync(path + fileName);
            }
            return new SaveDocInfOutputData(SaveDocInfStatus.Failed);
        }
        finally
        {
            _documentRepository.ReleaseResource();
            _receptionRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }

    private DocInfModel ConvertToDocInfModel(SaveDocInfInputData inputData)
    {
        return new DocInfModel(inputData.HpId,
                               inputData.FileId,
                               inputData.PtId,
                               inputData.GetDate,
                               inputData.CategoryCd,
                               string.Empty,
                               inputData.FileName,
                               inputData.DisplayFileName,
                               CIUtil.GetJapanDateTimeNow());
    }

    private SaveDocInfStatus ValidateInputData(SaveDocInfInputData inputData)
    {
        if (inputData.GetDate.ToString().Length != 8)
        {
            return SaveDocInfStatus.InvalidGetDate;
        }
        else if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.CategoryCd))
        {
            return SaveDocInfStatus.InvalidCategoryCd;
        }
        else if (inputData.DisplayFileName.Length == 0)
        {
            return SaveDocInfStatus.InvalidDisplayFileName;
        }
        if (inputData.FileId > 0)
        {
            var docInfDetail = _documentRepository.GetDocInfDetail(inputData.HpId, inputData.FileId);
            if (docInfDetail != null)
            {
                return SaveDocInfStatus.ValidateSuccess;
            }
        }
        else
        {
            if (!_patientInforRepository.CheckExistIdList(new List<long> { inputData.PtId }))
            {
                return SaveDocInfStatus.InvalidPtId;
            }
        }
        return SaveDocInfStatus.ValidateSuccess;
    }
}
