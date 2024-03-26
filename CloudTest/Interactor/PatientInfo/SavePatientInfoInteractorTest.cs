﻿using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Infrastructure.Repositories;
using Interactor.PatientInfor;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Text.Json;
using Reporting.CommonMasters.Constants;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Text;
using UseCase.PatientInfor.Save;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.Interactor.PatientInfo
{
    public class SavePatientInfoInteractorTest : BaseUT
    {
        [Test]
        public void TC_001_CloneByomei_PtByomeis_Count_Greater_Than_0()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            // Arrange
            var commonMedicalCheck = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6543",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-2109",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };


            mockPtDisease.Setup(x => x.GetPtByomeisByHokenId(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int input1, long input2, int input3) => new List<PtDiseaseModel>() { new PtDiseaseModel() });


            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            // Act

            var result = commonMedicalCheck.CloneByomei(inputData);

            // Assert
            Assert.That(inputData.ReactSave.ConfirmCloneByomei == false);
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel));
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel));
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && !p.IsAddNew));
            Assert.That(result == true);
        }

        [Test]
        public void TC_002_CloneByomei_Count_Greater_Is_0()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            // Arrange
            var commonMedicalCheck = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6543",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-2109",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };


            mockPtDisease.Setup(x => x.GetPtByomeisByHokenId(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int input1, long input2, int input3) => new List<PtDiseaseModel>() { new PtDiseaseModel() });


            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            // Act

            var result = commonMedicalCheck.CloneByomei(inputData);

            // Assert
            Assert.That(inputData.ReactSave.ConfirmCloneByomei == false);
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel));
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel));
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && !p.IsAddNew) == false);
            Assert.That(result == false);
        }

        /// <summary>
        ///HokenInfs not any p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel
        /// </summary>
        [Test]
        public void TC_003_CloneByomei_HokenInfs_Not_Any_newHokenInfs()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            // Arrange
            var commonMedicalCheck = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6543",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-2109",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };


            mockPtDisease.Setup(x => x.GetPtByomeisByHokenId(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int input1, long input2, int input3) => new List<PtDiseaseModel>() { new PtDiseaseModel() });


            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            // Act

            var result = commonMedicalCheck.CloneByomei(inputData);

            // Assert
            Assert.That(inputData.ReactSave.ConfirmCloneByomei == false);
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel) == false);
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && !p.IsAddNew) == true);
            Assert.That(result == false);
        }

        [Test]
        public void TC_004_CloneByomei_ReactSave_ConfirmCloneByomei_Is_True()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            // Arrange
            var commonMedicalCheck = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6543",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-2109",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };


            mockPtDisease.Setup(x => x.GetPtByomeisByHokenId(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int input1, long input2, int input3) => new List<PtDiseaseModel>() { new PtDiseaseModel() });

            var reactSave = new ReactSavePatientInfo()
            {

                ConfirmCloneByomei = true
            };

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), reactSave, new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            // Act

            var result = commonMedicalCheck.CloneByomei(inputData);

            // Assert
            Assert.That(inputData.ReactSave.ConfirmCloneByomei == true);
            Assert.That(result == false);
        }

        private static Stream GetSampleFileStream()
        {
            byte[] data = Encoding.UTF8.GetBytes("Sample file content.");
            return new MemoryStream(data);
        }

        /// <summary>
        /// model.ReactSave.ConfirmSamePatientInf = false
        /// samePatientInf count = 2
        /// </summary>
        [Test]
        public void TC_005_Validation_ConfirmSamePatientInf_SamePatientInf_GreaterThan_0()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            var resultFindSamePatientTest = new List<PatientInforModel>()
            {
               new PatientInforModel(
                        hpId: 123,
                        ptId: 987654,
                        referenceNo: 56789,
                        seqNo: 1,
                        ptNum: 1001,
                        kanaName: "あなたのテスト名前",
                        name: "Your Test Name",
                        sex: 1,
                        birthday: 19900101,
                        limitConsFlg: 0,
                        isDead: 0,
                        deathDate: 0,
                        homePost: "123-4567",
                        homeAddress1: "123 Test Street",
                        homeAddress2: "Apt. 45",
                        tel1: "123-555-7890",
                        tel2: "987-654-3210",
                        mail: "test@example.com",
                        setanusi: "Test Setanusi",
                        zokugara: "Test Zokugara",
                        job: "Software Engineer",
                        renrakuName: "Emergency Contact",
                        renrakuPost: "456 Emergency Street",
                        renrakuAddress1: "Emergency City",
                        renrakuAddress2: "Emergency State",
                        renrakuTel: "555-123-7890",
                        renrakuMemo: "Emergency Contact Memo",
                        officeName: "Test Hospital",
                        officePost: "789 Hospital Street",
                        officeAddress1: "Hospital City",
                        officeAddress2: "Hospital State",
                        officeTel: "789-456-1230",
                        officeMemo: "Hospital Memo",
                        isRyosyoDetail: 1,
                        primaryDoctor: 123,
                        isTester: 0,
                        mainHokenPid: 456,
                        memo: "Test Memo",
                        lastVisitDate: 20220101,
                        firstVisitDate: 20190101,
                        rainCount: 3,
                        comment: "Test Comment",
                        sinDate: 20220101,
                        isShowKyuSeiName: false
                    ),
               new PatientInforModel(
                        hpId: 123,
                        ptId: 987655,
                        referenceNo: 56789,
                        seqNo: 1,
                        ptNum: 1001,
                        kanaName: "あなたのテスト名前",
                        name: "Your Test Name",
                        sex: 1,
                        birthday: 19900101,
                        limitConsFlg: 0,
                        isDead: 0,
                        deathDate: 0,
                        homePost: "123-4567",
                        homeAddress1: "123 Test Street",
                        homeAddress2: "Apt. 45",
                        tel1: "123-555-7890",
                        tel2: "987-654-3210",
                        mail: "test@example.com",
                        setanusi: "Test Setanusi",
                        zokugara: "Test Zokugara",
                        job: "Software Engineer",
                        renrakuName: "Emergency Contact",
                        renrakuPost: "456 Emergency Street",
                        renrakuAddress1: "Emergency City",
                        renrakuAddress2: "Emergency State",
                        renrakuTel: "555-123-7890",
                        renrakuMemo: "Emergency Contact Memo",
                        officeName: "Test Hospital",
                        officePost: "789 Hospital Street",
                        officeAddress1: "Hospital City",
                        officeAddress2: "Hospital State",
                        officeTel: "789-456-1230",
                        officeMemo: "Hospital Memo",
                        isRyosyoDetail: 1,
                        primaryDoctor: 123,
                        isTester: 0,
                        mainHokenPid: 456,
                        memo: "Test Memo",
                        lastVisitDate: 20220101,
                        firstVisitDate: 20190101,
                        rainCount: 3,
                        comment: "Test Comment",
                        sinDate: 20220101,
                        isShowKyuSeiName: false
                    ),
            };
            mockPatientInfo.Setup(x => x.FindSamePatient(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, string input2, int input3, int input4) => resultFindSamePatientTest);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(1));
            Assert.That(resutl.First().Message, Is.EqualTo("同姓同名の患者が既に登録されています。\r\n登録しますか？\r\n患者番号：1001     \r\n患者番号：1001     "));
            Assert.That(resutl.First().Type, Is.EqualTo(3));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidSamePatient));
        }

        /// <summary>
        /// model.ReactSave.ConfirmSamePatientInf = false
        /// samePatientInf count = 1
        /// </summary>
        [Test]
        public void TC_006_Validation_ConfirmSamePatientInf_SamePatientInf_GreaterThan_0_Msg_NotIN_NewLine()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            var resultFindSamePatientTest = new List<PatientInforModel>()
            {
               new PatientInforModel(
                        hpId: 123,
                        ptId: 987654,
                        referenceNo: 56789,
                        seqNo: 1,
                        ptNum: 1001,
                        kanaName: "あなたのテスト名前",
                        name: "Your Test Name",
                        sex: 1,
                        birthday: 19900101,
                        limitConsFlg: 0,
                        isDead: 0,
                        deathDate: 0,
                        homePost: "123-4567",
                        homeAddress1: "123 Test Street",
                        homeAddress2: "Apt. 45",
                        tel1: "123-555-7890",
                        tel2: "987-654-3210",
                        mail: "test@example.com",
                        setanusi: "Test Setanusi",
                        zokugara: "Test Zokugara",
                        job: "Software Engineer",
                        renrakuName: "Emergency Contact",
                        renrakuPost: "456 Emergency Street",
                        renrakuAddress1: "Emergency City",
                        renrakuAddress2: "Emergency State",
                        renrakuTel: "555-123-7890",
                        renrakuMemo: "Emergency Contact Memo",
                        officeName: "Test Hospital",
                        officePost: "789 Hospital Street",
                        officeAddress1: "Hospital City",
                        officeAddress2: "Hospital State",
                        officeTel: "789-456-1230",
                        officeMemo: "Hospital Memo",
                        isRyosyoDetail: 1,
                        primaryDoctor: 123,
                        isTester: 0,
                        mainHokenPid: 456,
                        memo: "Test Memo",
                        lastVisitDate: 20220101,
                        firstVisitDate: 20190101,
                        rainCount: 3,
                        comment: "Test Comment",
                        sinDate: 20220101,
                        isShowKyuSeiName: false
                    ),
            };
            mockPatientInfo.Setup(x => x.FindSamePatient(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, string input2, int input3, int input4) => resultFindSamePatientTest);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(1));
            Assert.That(resutl.First().Message, Is.EqualTo("同姓同名の患者が既に登録されています。\r\n登録しますか？\r\n患者番号：1001     "));
            Assert.That(resutl.First().Type, Is.EqualTo(3));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidSamePatient));
        }

        /// <summary>
        /// model.ReactSave.ConfirmSamePatientInf = false
        /// FindSamePatient item.PtId != model.Patient.PtId count is 0
        /// samePatientInf count = 0
        /// </summary>
        [Test]
        public void TC_007_Validation_ConfirmSamePatientInf_SamePatientInf_Is_0()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            var resultFindSamePatientTest = new List<PatientInforModel>()
            {
               new PatientInforModel(
                        hpId: 123,
                        ptId: 123456,
                        referenceNo: 56789,
                        seqNo: 1,
                        ptNum: 1001,
                        kanaName: "あなたのテスト名前",
                        name: "Your Test Name",
                        sex: 1,
                        birthday: 19900101,
                        limitConsFlg: 0,
                        isDead: 0,
                        deathDate: 0,
                        homePost: "123-4567",
                        homeAddress1: "123 Test Street",
                        homeAddress2: "Apt. 45",
                        tel1: "123-555-7890",
                        tel2: "987-654-3210",
                        mail: "test@example.com",
                        setanusi: "Test Setanusi",
                        zokugara: "Test Zokugara",
                        job: "Software Engineer",
                        renrakuName: "Emergency Contact",
                        renrakuPost: "456 Emergency Street",
                        renrakuAddress1: "Emergency City",
                        renrakuAddress2: "Emergency State",
                        renrakuTel: "555-123-7890",
                        renrakuMemo: "Emergency Contact Memo",
                        officeName: "Test Hospital",
                        officePost: "789 Hospital Street",
                        officeAddress1: "Hospital City",
                        officeAddress2: "Hospital State",
                        officeTel: "789-456-1230",
                        officeMemo: "Hospital Memo",
                        isRyosyoDetail: 1,
                        primaryDoctor: 123,
                        isTester: 0,
                        mainHokenPid: 456,
                        memo: "Test Memo",
                        lastVisitDate: 20220101,
                        firstVisitDate: 20190101,
                        rainCount: 3,
                        comment: "Test Comment",
                        sinDate: 20220101,
                        isShowKyuSeiName: false
                    ),
            };
            mockPatientInfo.Setup(x => x.FindSamePatient(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, string input2, int input3, int input4) => resultFindSamePatientTest);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// model.Patient.PtId == 0 && 
        /// model.Patient.PtNum != 0 && 
        /// _systemConfRepository.GetSettingValue(1001, 0, model.Patient.HpId) == 1 && 
        /// !CIUtil.PtNumCheckDigits(model.Patient.PtNum
        /// </summary>
        [Test]
        public void TC_008_Validation_InvalidPtNumCheckDigits()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 0,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 1);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(1));
            Assert.That(resutl.First().Message, Is.EqualTo("患者番号が正しくありません。"));
            Assert.That(resutl.First().Type, Is.EqualTo(2));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidPtNumCheckDigits));
        }

        [Test]
        public void TC_009_Validation_InvalidHpId()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 0,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: -1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.HpId` property is invalid"));
            Assert.That(resutl1.First().Type, Is.EqualTo(2));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidHpId));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("`Patient.HpId` property is invalid"));
            Assert.That(resutl2.First().Type, Is.EqualTo(2));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidHpId));
        }

        [Test]
        public void TC_010_Validation_InvalidKanaName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Nam",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(2));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.KanaName` property is invalid"));
            Assert.That(resutl1.Last().Message, Is.EqualTo("患者名（カナ）は２０文字以下を入力してください。"));
            Assert.That(resutl1.First().Type, Is.EqualTo(2));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidKanaName));
            Assert.That(resutl1.Last().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidFirstNameKanaLength));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
        }

        [Test]
        public void TC_011_Validation_InvalidName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Nam",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(2));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Name` property is invalid"));
            Assert.That(resutl1.Last().Message, Is.EqualTo("患者名は３０文字以下を入力してください。"));
            Assert.That(resutl1.First().Type, Is.EqualTo(2));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidName));
            Assert.That(resutl1.Last().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidFirstNameKanjiLength));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("患者名は３０文字以下を入力してください。"));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidFirstNameKanjiLength));
        }

        [Test]
        public void TC_012_Validation_InvalidBirthday()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 0,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 0,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 0,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(0));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("生年月日を入力してください。"));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidBirthday));
        }

        [Test]
        public void TC_013_Validation_InvalidSex()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 0,
                                            birthday: 20001211,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 3,
                                            birthday: 20011111,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("性別を入力してください。"));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidSex));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("性別を入力してください。"));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidSex));
        }

        [Test]
        public void TC_014_Validation_InvalidIsDead()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: -1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 2,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.IsDead` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidIsDead));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("`Patient.IsDead` property is invalid"));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidIsDead));
        }

        [Test]
        public void TC_015_Validation_InvalidHomePost()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.HomePost` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidHomePost));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_016_Validation_InvalidHomeAddress1()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home11",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.HomeAddress1` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidHomeAddress1));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_017_Validation_InvalidHomeAddress2()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home11",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home1",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.HomeAddress2` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidHomeAddress2));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_018_Validation_InvalidTel1()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890-789",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890-78",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Tel1` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidTel1));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_019_Validation_InvalidTel2()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-789",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Tel2` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidTel2));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_020_Validation_InvalidMail()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Mail` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidMail));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_021_Validation_InvalidSetanusi()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @gmail.com",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @mail.com",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Setanusi` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidSetanusi));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_022_Validation_InvalidZokugara()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @gmail.com",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @mail.com",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Zokugara` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidZokugara));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_023_Validation_InvalidJob()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "Sample Job Sample Job Sample Job Sample41",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "Sample Job Sample Job Sample Job Sample2",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Job` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidJob));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_024_Validation_InvalidRenrakuPost()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6567",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.RenrakuPost` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidRenrakuPost));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_025_Validation_InvalidRenrakuAddress1()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample R1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample R",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.RenrakuAddress1` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidRenrakuAddress1));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_026_Validation_InvalidRenrakuAddress2()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample R1",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample R",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.RenrakuAddress2` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidRenrakuAddress2));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_027_Validation_InvalidRenrakuTel()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.RenrakuTel` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidRenrakuTel));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_028_Validation_InvalidRenrakuMemo()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo12",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo1",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.RenrakuMemo` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidRenrakuMemo));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_029_Validation_InvalidOfficeName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo12",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo1",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficeName` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficeName));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_030_Validation_InvalidOfficePost()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-2121",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficePost` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficePost));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_031_Validation_InvalidOfficeAddress1()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sampl",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Samp",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficeAddress1` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficeAddress1));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_032_Validation_InvalidOfficeAddress2()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sampl",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Samp",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficeAddress2` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficeAddress2));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_033_Validation_InvalidOfficeTel()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficeTel` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficeTel));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_034_Validation_InvalidOfficeMemo()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office Memo Sample Office Memo Sample Office Memo Sample Office Memo Sample Office Memo Sample",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office Memo Sample Office Memo Sample Office Memo Sample Office Memo Sample Office Memo Sampl",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficeMemo` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficeMemo));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_035_Validation_Hoken()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 77,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel
                (
                    hpId : 1,
                    ptId: 1,
                    ptBirthday: 19901212,
                    seqNo: 999,
                    hokenSbtCd: 45677,
                    hokenPid: 30,
                    hokenKbn: 1,
                    hokenMemo: "",
                    sinDate: 20240101,
                    startDate: 2030101,
                    endDate: 20240101,
                    hokenId: 40,
                    kohi1Id: 30,
                    kohi2Id: 20,
                    kohi3Id: 10,
                    kohi4Id : 50,
                    isAddNew: false,
                    isDeleted: 0,
                    hokenPatternSelected: true
                ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), insurances, hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);

            var resutl = resutl1.ToList();
            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(5));
            Assert.That(resutl[0].Message, Is.EqualTo("`Insurances[0].HokenId` property is invalid"));
            Assert.That(resutl[0].Code, Is.EqualTo(SavePatientInforValidationCode.ÍnuranceInvalidHokenId));
            Assert.That(resutl[1].Message, Is.EqualTo("`Insurances[0].Kohi1Id` property is invalid"));
            Assert.That(resutl[1].Code, Is.EqualTo(SavePatientInforValidationCode.ÍnuranceInvalidKohi1Id));
            Assert.That(resutl[2].Message, Is.EqualTo("`Insurances[0].Kohi2Id` property is invalid"));
            Assert.That(resutl[2].Code, Is.EqualTo(SavePatientInforValidationCode.ÍnuranceInvalidKohi2Id));
            Assert.That(resutl[3].Message, Is.EqualTo("`Insurances[0].Kohi3Id` property is invalid"));
            Assert.That(resutl[3].Code, Is.EqualTo(SavePatientInforValidationCode.ÍnuranceInvalidKohi3Id));
            Assert.That(resutl[4].Message, Is.EqualTo("`Insurances[0].Kohi4Id` property is invalid"));
            Assert.That(resutl[4].Code, Is.EqualTo(SavePatientInforValidationCode.ÍnuranceInvalidKohi4Id));
        }

        [Test]
        public void TC_036_Validation_PtKyuseiInvalidSeqNo()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 77,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel
                    (
                        hpId:  1,
                        ptId: 999,
                        seqNo: -1,
                        kanaName: "Sample KanaName",
                        name: "Sample Name",
                        endDate: 20241212
                    ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel1, ptKyuseis, new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(1));
            Assert.That(resutl.First().Message, Is.EqualTo("`PtKyuseis[0].SeqNo` property is invalid"));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.PtKyuseiInvalidSeqNo));
        }

        [Test]
        public void TC_037_Validation_PtKyuseiInvalidKanaName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 77,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel
                    (
                        hpId:  1,
                        ptId: 999,
                        seqNo: 0,
                        kanaName: "",
                        name: "Sample Name",
                        endDate: 20241212
                    ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel1, ptKyuseis, new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(1));
            Assert.That(resutl.First().Message, Is.EqualTo("`PtKyuseis[0].KanaName` property is required"));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.PtKyuseiInvalidKanaName));
        }

        [Test]
        public void TC_038_Validation_PtKyuseiInvalidName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 77,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel
                    (
                        hpId:  1,
                        ptId: 999,
                        seqNo: 0,
                        kanaName: "Sample KanaName",
                        name: "Sample KanaName Sample KanaName Sample KanaName Sample KanaName Sample KanaName Sample KanaName Sampl",
                        endDate: 20241212
                    ),

                new PtKyuseiModel
                    (
                        hpId:  1,
                        ptId: 999,
                        seqNo: 0,
                        kanaName: "Sample KanaName",
                        name: "",
                        endDate: 20241212
                    ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel1, ptKyuseis, new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(2));
            Assert.That(resutl.First().Message, Is.EqualTo("`PtKyuseis[0].Name` property is invalid"));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.PtKyuseiInvalidName));
            Assert.That(resutl.Last().Message, Is.EqualTo("`PtKyuseis[1].Name` property is required"));
            Assert.That(resutl.Last().Code, Is.EqualTo(SavePatientInforValidationCode.PtKyuseiInvalidName));
        }

        [Test]
        public void TC_039_SplitName_WithEmptyName_ShouldSetFirstAndLastNameToEmptyString()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            string name = "";
            string firstName, lastName;

            // Act
            savePatientInfo.SplitName(name, out firstName, out lastName);

            // Assert
            Assert.That(firstName, Is.EqualTo(""));
            Assert.That(lastName, Is.EqualTo(""));
        }

        [Test]
        public void TC_040_SplitName_WithSingleName_ShouldSetFirstNameToName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);
            string name = "John";
            string firstName, lastName;

            // Act
            savePatientInfo.SplitName(name, out firstName, out lastName);

            // Assert
            Assert.That(firstName, Is.EqualTo("John"));
            Assert.That(lastName, Is.EqualTo(""));
        }

        [Test]
        public void TC_041_SplitName_WithEmptyName_ShouldSplitCorrectly()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            // Arrange
            string name = "John Doe";
            string firstName, lastName;

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Act
            savePatientInfo.SplitName(name, out firstName, out lastName);

            // Assert
            Assert.That(firstName, Is.EqualTo("Doe"));
            Assert.That(lastName, Is.EqualTo("John"));
        }

        [Test]
        public void TC_041_SplitName_WithJapaneseEmptyName_ShouldSplitCorrectly()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);
            // Arrange
            string name = "山田 太郎";
            string firstName, lastName;

            // Act
            savePatientInfo.SplitName(name, out firstName, out lastName);

            // Assert
            Assert.That(firstName, Is.EqualTo("太郎"));
            Assert.That(lastName, Is.EqualTo("山田"));
        }

        [Test]
        public void TC_042_IsValidAgeCheckConfirm_ValidAgeCheck()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            int ageCheck = 18;
            int confirmDate = 20240114;
            int birthDay = 20000102;
            int sinDay = 20240114;

            // Act
            var result = savePatientInfo.IsValidAgeCheckConfirm(ageCheck, confirmDate, birthDay, sinDay);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TC_043_IsValidAgeCheckConfirm_InvalidAgeCheck()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            int ageCheck = 21;
            int confirmDate = 0;
            int birthDay = 20000102;
            int sinDay = 20220114;

            // Act
            var result = savePatientInfo.IsValidAgeCheckConfirm(ageCheck, confirmDate, birthDay, sinDay);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void TC_044_IsValidAgeCheckConfirm_BirthdayAfterSecondDay()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            int ageCheck = 18;
            int confirmDate = 20240114; // YYYYMMDD format
            int birthDay = 20000202;    // YYYYMMDD format (2nd day of February)
            int sinDay = 20240114;      // YYYYMMDD format

            // Act
            var result = savePatientInfo.IsValidAgeCheckConfirm(ageCheck, confirmDate, birthDay, sinDay);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TC_045_IsValidKanjiName_InvalidFirstNameKana_InvalidFirstNameKanji()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = string.Empty;
            var kanjiName = string.Empty;

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Message, Is.EqualTo("カナを入力してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidFirstNameKana));
            Assert.That(result.Last().Message, Is.EqualTo("氏名を入力してください。"));
            Assert.That(result.Last().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidFirstNameKanji));
        }

        [Test]
        public void TC_046_IsValidKanjiName_InvalidLastKanaName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "Ariga";
            var kanjiName = "救按 土";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("カナを入力してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidLastKanaName));
        }

        [Test]
        public void TC_047_IsValidKanjiName_InvalidLastKanjiName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "Ariga To";
            var kanjiName = "救按";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("氏名を入力してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidLastKanjiName));
        }

        [Test]
        public void TC_048_IsValidKanjiName_InvalidLastKanjiNameLength()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "Ariga To";
            var kanjiName = "KanjiNameKanjiNameKanjiNameKanj Sample";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("患者姓は３０文字以下を入力してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidLastKanjiNameLength));
        }

        [Test]
        public void TC_049_IsValidKanjiName_InvalidLastKanaNameLength()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "SampleKanaNameSampleS Sample";
            var kanjiName = "KanjiNameKanjiNameKanjiNameKan Sample";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("患者姓（カナ）は２０文字以下を入力してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidLastKanaNameLength));
        }

        [Test]
        public void TC_050_IsValidKanjiName_FKanNmChkJIS_SBUF_1()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "SampleS â!ă";
            var kanjiName = "Kanjin â!ă";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("漢字名に 'ă' の文字が入力されています。\n\r登録しますか？"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidJiscodeCheck));
        }

        [Test]
        public void TC_051_IsValidKanjiName_FKanNmChkJIS_SBUF_2()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "SampleS â!ă";
            var kanjiName = "Kanjin â!ă";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 2);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("漢字名に 'ă' の文字は入力できません。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidChineseCharacterName));
        }

        [Test]
        public void TC_052_IsValidKanjiName_FKanNmChkJIS_SBUF_FullName_1()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "â!ă 123";
            var kanjiName = "â!ă 123";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("漢字姓に 'ă' の文字が入力されています。\n\r登録しますか？"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidJiscodeCheck));
        }

        [Test]
        public void TC_053_IsValidKanjiName_FKanNmChkJIS_SBUF_FullName_2()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "â!ă 123";
            var kanjiName = "â!ă 123";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 2);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("漢字姓に 'ă' の文字は入力できません。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidChineseCharacterName));
        }


        [Test]
        public void TC_054_Handle_IsInValid()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);
            var input = new SavePatientInfoInputData();
            input.ReactSave.ConfirmSamePatientInf = true;
            // Act
            var result = savePatientInfo.Handle(input);

            // Assert
            Assert.IsTrue(result.Status == SavePatientInfoStatus.Failed);
        }


        [Test]
        public void TC_055_Handle_InsertPatientInf()
        {
            long ptId = 0;
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            try
            {
                ReceptionRepository receptionRepository = new ReceptionRepository(TenantProvider);
                PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receptionRepository);
                var mockConfiguration = new Mock<IConfiguration>();
                mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
                mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
                SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
                DiseaseRepository ptDiseaseRepository = new DiseaseRepository(TenantProvider);
                //Mock
                var mockAmazonS3 = new Mock<IAmazonS3Service>();

                var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, patientInforRepository, systemConfRepository, mockAmazonS3.Object, ptDiseaseRepository);
                var inputstr = @"{ ""Patient"":{ ""HpId"":1,""PtId"":0,""PtNum"":0,""KanaName"":""Luu Nguyen"",""Name"":""Luu Nguyen"",""Sex"":1,""Birthday"":20240101,""IsDead"":0,""DeathDate"":0,""Mail"":"""",""HomePost"":"""",""HomeAddress1"":"""",""HomeAddress2"":"""",""Tel1"":"""",""Tel2"":"""",""Setanusi"":"""",""Zokugara"":"""",""Job"":"""",""RenrakuName"":"""",""RenrakuPost"":"""",""RenrakuAddress1"":"""",""RenrakuAddress2"":"""",""RenrakuTel"":"""",""RenrakuMemo"":"""",""OfficeName"":"""",""OfficePost"":"""",""OfficeAddress1"":"""",""OfficeAddress2"":"""",""OfficeTel"":"""",""OfficeMemo"":"""",""IsRyosyoDetail"":0,""PrimaryDoctor"":0,""IsTester"":0,""MainHokenPid"":0,""ReferenceNo"":0,""LimitConsFlg"":0,""Memo"":""""},""PtKyuseis"":[],""PtSanteis"":[],""Insurances"":[],""PtGrps"":[],""HokenInfs"":[],""HokenKohis"":[],""MaxMoneys"":[],""ReactSave"":{ ""ConfirmHaveanExpiredHokenOnMain"":false,""ConfirmRegisteredInsuranceCombination"":false,""ConfirmAgeCheck"":false,""ConfirmInsuranceElderlyLaterNotYetCovered"":false,""ConfirmLaterInsuranceRegisteredPatientsElderInsurance"":false,""ConfirmInsuranceSameInsuranceNumber"":false,""ConfirmMultipleHokenSignedUpSameTime"":false,""ConfirmFundsWithSamePayerCode"":false,""ConfirmInvalidJiscodeCheck"":false,""ConfirmHokenPatternSelectedIsInfMainHokenPid"":false,""ConfirmSamePatientInf"":false,""ConfirmCloneByomei"":false},""InsuranceScans"":[],""HokenIdList"":[],""UserId"":2,""HpId"":1}
            ";

                var input = JsonSerializer.Deserialize<SavePatientInfoInputData>(inputstr);

                //// Act
                var result = savePatientInfo.Handle(input ?? new());

                //// Assert
                Assert.IsTrue(result.Status == SavePatientInfoStatus.Successful && result.PtID > 0);
                ptId = result.PtID;
            }
            finally
            {
                if (ptId > 0)
                {
                    var ptInf = tenantTracking.PtInfs.FirstOrDefault(pt => pt.PtId == ptId);
                    tenantTracking.PtInfs.Remove(ptInf ?? new());
                    tenantTracking.SaveChanges();
                }
            }
        }


        [Test]
        public void TC_056_Handle_UpdatePatientInf()
        {
            long ptId = 0;
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            try
            {
                ReceptionRepository receptionRepository = new ReceptionRepository(TenantProvider);
                PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receptionRepository);
                var mockConfiguration = new Mock<IConfiguration>();
                mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
                mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
                SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
                DiseaseRepository ptDiseaseRepository = new DiseaseRepository(TenantProvider);
                //Mock
                var mockAmazonS3 = new Mock<IAmazonS3Service>();

                var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, patientInforRepository, systemConfRepository, mockAmazonS3.Object, ptDiseaseRepository);
                var inputStrInsert = @"{ ""Patient"":{ ""HpId"":1,""PtId"":0,""PtNum"":0,""KanaName"":""Luu Nguyen"",""Name"":""Luu Nguyen"",""Sex"":1,""Birthday"":20240101,""IsDead"":0,""DeathDate"":0,""Mail"":"""",""HomePost"":"""",""HomeAddress1"":"""",""HomeAddress2"":"""",""Tel1"":"""",""Tel2"":"""",""Setanusi"":"""",""Zokugara"":"""",""Job"":"""",""RenrakuName"":"""",""RenrakuPost"":"""",""RenrakuAddress1"":"""",""RenrakuAddress2"":"""",""RenrakuTel"":"""",""RenrakuMemo"":"""",""OfficeName"":"""",""OfficePost"":"""",""OfficeAddress1"":"""",""OfficeAddress2"":"""",""OfficeTel"":"""",""OfficeMemo"":"""",""IsRyosyoDetail"":0,""PrimaryDoctor"":0,""IsTester"":0,""MainHokenPid"":0,""ReferenceNo"":0,""LimitConsFlg"":0,""Memo"":""""},""PtKyuseis"":[],""PtSanteis"":[],""Insurances"":[],""PtGrps"":[],""HokenInfs"":[],""HokenKohis"":[],""MaxMoneys"":[],""ReactSave"":{ ""ConfirmHaveanExpiredHokenOnMain"":false,""ConfirmRegisteredInsuranceCombination"":false,""ConfirmAgeCheck"":false,""ConfirmInsuranceElderlyLaterNotYetCovered"":false,""ConfirmLaterInsuranceRegisteredPatientsElderInsurance"":false,""ConfirmInsuranceSameInsuranceNumber"":false,""ConfirmMultipleHokenSignedUpSameTime"":false,""ConfirmFundsWithSamePayerCode"":false,""ConfirmInvalidJiscodeCheck"":false,""ConfirmHokenPatternSelectedIsInfMainHokenPid"":false,""ConfirmSamePatientInf"":false,""ConfirmCloneByomei"":false},""InsuranceScans"":[],""HokenIdList"":[],""UserId"":2,""HpId"":1}
            ";

                var input = JsonSerializer.Deserialize<SavePatientInfoInputData>(inputStrInsert);

                //// Act
                var resultInsert = savePatientInfo.Handle(input);
                ptId = resultInsert.PtID;

                var inputstrUpdate = @"{ ""Patient"":{ ""HpId"":1,""PtId"":" + resultInsert.PtID + @",""PtNum"":" + resultInsert.PatientInforModel.PtNum + @",""KanaName"":""Luu Nguyen 1"",""Name"":""Luu Nguyen 1"",""Sex"":1,""Birthday"":19930309,""IsDead"":0,""DeathDate"":0,""Mail"":"""",""HomePost"":"""",""HomeAddress1"":"""",""HomeAddress2"":"""",""Tel1"":"""",""Tel2"":"""",""Setanusi"":"""",""Zokugara"":"""",""Job"":"""",""RenrakuName"":"""",""RenrakuPost"":"""",""RenrakuAddress1"":"""",""RenrakuAddress2"":"""",""RenrakuTel"":"""",""RenrakuMemo"":"""",""OfficeName"":"""",""OfficePost"":"""",""OfficeAddress1"":"""",""OfficeAddress2"":"""",""OfficeTel"":"""",""OfficeMemo"":"""",""IsRyosyoDetail"":0,""PrimaryDoctor"":0,""IsTester"":0,""MainHokenPid"":0,""ReferenceNo"":0,""LimitConsFlg"":0,""Memo"":""""},""PtKyuseis"":[],""PtSanteis"":[],""Insurances"":[],""PtGrps"":[],""HokenInfs"":[],""HokenKohis"":[],""MaxMoneys"":[],""ReactSave"":{ ""ConfirmHaveanExpiredHokenOnMain"":false,""ConfirmRegisteredInsuranceCombination"":false,""ConfirmAgeCheck"":false,""ConfirmInsuranceElderlyLaterNotYetCovered"":false,""ConfirmLaterInsuranceRegisteredPatientsElderInsurance"":false,""ConfirmInsuranceSameInsuranceNumber"":false,""ConfirmMultipleHokenSignedUpSameTime"":false,""ConfirmFundsWithSamePayerCode"":false,""ConfirmInvalidJiscodeCheck"":false,""ConfirmHokenPatternSelectedIsInfMainHokenPid"":false,""ConfirmSamePatientInf"":false,""ConfirmCloneByomei"":false},""InsuranceScans"":[],""HokenIdList"":[],""UserId"":2,""HpId"":1}
            ";

                var inputUpdate = JsonSerializer.Deserialize<SavePatientInfoInputData>(inputstrUpdate);

                var resultUpdate = savePatientInfo.Handle(inputUpdate);
                //// Assert
                Assert.IsTrue(resultUpdate.Status == SavePatientInfoStatus.Successful && resultUpdate.PatientInforModel.KanaName == "Luu Nguyen 1" && resultUpdate.PatientInforModel.Name == "Luu Nguyen 1" && resultUpdate.PatientInforModel.Birthday == 19930309);
            }
            finally
            {
                if (ptId > 0)
                {
                    var ptInf = tenantTracking.PtInfs.FirstOrDefault(pt => pt.PtId == ptId);
                    tenantTracking.PtInfs.Remove(ptInf ?? new());
                    tenantTracking.SaveChanges();
                }
            }
        }

        [Test]
        public void TC_057_Handle_InsertOrUpdateFailed()
        {
            long ptId = 0;
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            try
            {
                ReceptionRepository receptionRepository = new ReceptionRepository(TenantProvider);
                PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receptionRepository);
                var mockConfiguration = new Mock<IConfiguration>();
                mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
                mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
                SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
                DiseaseRepository ptDiseaseRepository = new DiseaseRepository(TenantProvider);
                //Mock
                var mockAmazonS3 = new Mock<IAmazonS3Service>();

                var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, patientInforRepository, systemConfRepository, mockAmazonS3.Object, ptDiseaseRepository);

                //// Act

                var inputstrUpdate = @"{ ""Patient"":{ ""HpId"":1,""PtId"": 0,""PtNum"":1,""KanaName"":""Luu Nguyen"",""Name"":""Luu Nguyen"",""Sex"":1,""Birthday"":20240101,""IsDead"":0,""DeathDate"":0,""Mail"":"""",""HomePost"":"""",""HomeAddress1"":"""",""HomeAddress2"":"""",""Tel1"":"""",""Tel2"":"""",""Setanusi"":"""",""Zokugara"":"""",""Job"":"""",""RenrakuName"":"""",""RenrakuPost"":"""",""RenrakuAddress1"":"""",""RenrakuAddress2"":"""",""RenrakuTel"":"""",""RenrakuMemo"":"""",""OfficeName"":"""",""OfficePost"":"""",""OfficeAddress1"":"""",""OfficeAddress2"":"""",""OfficeTel"":"""",""OfficeMemo"":"""",""IsRyosyoDetail"":0,""PrimaryDoctor"":0,""IsTester"":0,""MainHokenPid"":0,""ReferenceNo"":0,""LimitConsFlg"":0,""Memo"":""""},""PtKyuseis"":[],""PtSanteis"":[],""Insurances"":[],""PtGrps"":[],""HokenInfs"":[],""HokenKohis"":[],""MaxMoneys"":[],""ReactSave"":{ ""ConfirmHaveanExpiredHokenOnMain"":false,""ConfirmRegisteredInsuranceCombination"":false,""ConfirmAgeCheck"":false,""ConfirmInsuranceElderlyLaterNotYetCovered"":false,""ConfirmLaterInsuranceRegisteredPatientsElderInsurance"":false,""ConfirmInsuranceSameInsuranceNumber"":false,""ConfirmMultipleHokenSignedUpSameTime"":false,""ConfirmFundsWithSamePayerCode"":false,""ConfirmInvalidJiscodeCheck"":false,""ConfirmHokenPatternSelectedIsInfMainHokenPid"":false,""ConfirmSamePatientInf"":false,""ConfirmCloneByomei"":false},""InsuranceScans"":[],""HokenIdList"":[],""UserId"":2,""HpId"":1}
            ";

                var inputUpdate = JsonSerializer.Deserialize<SavePatientInfoInputData>(inputstrUpdate);

                var resultUpdate = savePatientInfo.Handle(inputUpdate);
                ptId = resultUpdate.PtID;
                //// Assert
                Assert.IsTrue(resultUpdate.Status == SavePatientInfoStatus.Failed && resultUpdate.PatientInforModel.PtId == 0);
            }
            finally
            {
                if (ptId > 0)
                {
                    var ptInf = tenantTracking.PtInfs.FirstOrDefault(pt => pt.PtId == ptId);
                    tenantTracking.PtInfs.Remove(ptInf ?? new());
                    tenantTracking.SaveChanges();
                }
            }
        }


        [Test]
        public void TC_058_NeedCheckMainHoken_SelectedHokenPattern_Null()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new();
            List<HokenInfModel> hokenInfs = new();
            int ptInfMainHokenPid = 0;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(!result);
        }


        [Test]
        public void TC_059_NeedCheckMainHoken_Not_IsAddNew()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(false, true) };
            List<HokenInfModel> hokenInfs = new();
            int ptInfMainHokenPid = 0;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(!result);
        }


        [Test]
        public void TC_060_NeedCheckMainHoken_IsEmpty()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(true, false) };
            List<HokenInfModel> hokenInfs = new();
            int ptInfMainHokenPid = 0;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(!result);
        }


        [Test]
        public void TC_061_NeedCheckMainHoken_IsExpirated()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(true, true, new HokenInfModel(1, 20100101, 20130101), 0) };
            List<HokenInfModel> hokenInfs = new();
            int ptInfMainHokenPid = 0;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(!result);
        }

        [Test]
        public void TC_062_NeedCheckMainHoken_HokenMain_Equal_SelectedHoken()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(true, true, new HokenInfModel(1, 0, 99999999), 0) };
            List<HokenInfModel> hokenInfs = new();
            int ptInfMainHokenPid = 0;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(!result);
        }

        [Test]
        public void TC_063_NeedCheckMainHoken_Jihi()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(true, true, new HokenInfModel(1, 0, 99999999), 1) };
            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(0, 1, 0, string.Empty, 0, 0, 99999999, "108") };
            int ptInfMainHokenPid = 2;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(!result);
        }

        [Test]
        public void TC_064_NeedCheckMainHoken_MainHokenPattern_IsNull()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(true, true, new HokenInfModel(1, 0, 99999999), 0) };
            List<HokenInfModel> hokenInfs = new();
            int ptInfMainHokenPid = 2;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TC_065_NeedCheckMainHoken_Main_IsExpirated()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(true, true, new HokenInfModel(1, 0, 99999999), 1) };
            List<HokenInfModel> hokenInfs = new();
            int ptInfMainHokenPid = 1;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void TC_066_NeedCheckMainHoken_Main_HokenKbn()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(1, 1, 20230101, 1, 123, 1, 1, string.Empty, 20230101, 0, 99999999, 1, 0, 0, 0, 0, true, 0, true), new InsuranceModel(1, 1, 20230101, 1, 123, 2, 2, string.Empty, 20230101, 0, 99999999, 1, 0, 0, 0, 0, true, 0, false) };

            List<HokenInfModel> hokenInfs = new();
            int ptInfMainHokenPid = 2;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TC_067_NeedCheckMainHoken_Main_HokenKbn()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(1, 1, 20230101, 1, 123, 1, 1, string.Empty, 20230101, 0, 99999999, 1, 0, 0, 0, 0, true, 0, true), new InsuranceModel(1, 1, 20230101, 1, 123, 2, 1, string.Empty, 20230101, 0, 99999999, 0, 0, 0, 0, 0, true, 0, false) };

            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(1, 0, 1, 1, "0", 0, 99999999, 2024 / 03 / 11, new(), new()) };
            int ptInfMainHokenPid = 2;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void TC_068_NeedCheckMainHoken_Main_Last()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(1, 1, 20230101, 1, 123, 1, 1, string.Empty, 20230101, 0, 99999999, 1, 0, 0, 0, 0, true, 0, true), new InsuranceModel(1, 1, 20230101, 1, 123, 2, 1, string.Empty, 20230101, 0, 99999999, 1, 0, 0, 0, 0, true, 0, false) };

            List<HokenInfModel> hokenInfs = new();
            int ptInfMainHokenPid = 2;

            // Act
            var result = savePatientInfo.NeedCheckMainHoken(insurances, hokenInfs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(!result);
        }

        [Test]
        public void TC_069_IsValidAgeCheck_ValidPattern_Null()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel
                (
                    hpId : 1,
                    ptId: 1,
                    ptBirthday: 19901212,
                    seqNo: 999,
                    hokenSbtCd: 45677,
                    hokenPid: 30,
                    hokenKbn: 1,
                    hokenMemo: "",
                    sinDate: 20240101,
                    startDate: 2030101,
                    endDate: 20240101,
                    hokenId: 40,
                    kohi1Id: 30,
                    kohi2Id: 20,
                    kohi3Id: 10,
                    kohi4Id : 50,
                    isAddNew: false,
                    isDeleted: 0,
                    hokenPatternSelected: true
                ),
            };

            int birthDay = 20000101;
            int sinDay = 20240101;
            int hpId = 1;
            var reactFromUI = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1005, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            // Act
            var resultIEnum = savePatientInfo.IsValidAgeCheck(insurances, birthDay, sinDay, hpId, reactFromUI);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_070_IsValidAgeCheck_ValidPattern_Null()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var hokenInf = new HokenInfModel(
                                    hpId: 1,
                                    ptId: 99999,
                                    hokenId: 1,
                                    hokenKbn: 30,
                                    houbetu: string.Empty,
                                    startDate: 20230101,
                                    endDate: 20231212,
                                    sinDate: 20230505,
                                    hokenMst: new HokenMstModel(),
                                    confirmDateModels: new List<ConfirmDateModel>()
                               );

            // Arrange
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel
                (
                    hpId : 1,
                    ptId: 1,
                    ptBirthday: 19901212,
                    seqNo: 999,
                    hokenSbtCd: 45677,
                    hokenPid: 30,
                    hokenKbn: 1,
                    hokenMemo: "",
                    sinDate: 20240101,
                    startDate: 2030101,
                    endDate: 20240101,
                    hokenId: 40,
                    kohi1Id: 30,
                    kohi2Id: 20,
                    kohi3Id: 10,
                    kohi4Id : 50,
                    isAddNew: false,
                    isDeleted: 0,
                    hokenPatternSelected: true
                ),
            };

            int birthDay = 20000101;
            int sinDay = 20240101;
            int hpId = 1;
            var reactFromUI = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1005, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            mockSystemConf.Setup(x => x.GetSettingParams(1005, 0, hpId, string.Empty))
            .Returns((int input1, int input2, int input3, string input4) => "0");

            // Act
            var resultIEnum = savePatientInfo.IsValidAgeCheck(insurances, birthDay, sinDay, hpId, reactFromUI);

            var result = resultIEnum.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }


        [Test]
        public void TC_071_HasElderHoken_SinDay_Is_Less_Than_20080401()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var hokenInf = new HokenInfModel(
                                    hpId: 1,
                                    ptId: 99999,
                                    hokenId: 789012,
                                    hokenKbn: 30,
                                    houbetu: string.Empty,
                                    startDate: 20230101,
                                    endDate: 20230103,
                                    sinDate: 20230102,
                                    hokenMst: new HokenMstModel(),
                                    confirmDateModels: new List<ConfirmDateModel>()
                               );

            // Arrange
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel
                (
                    hpId : 1,
                    ptId: 1,
                    ptBirthday: 19901212,
                    seqNo: 999,
                    hokenSbtCd: 45677,
                    hokenPid: 30,
                    hokenKbn: 1,
                    hokenMemo: "",
                    sinDate: 20240101,
                    startDate: 2030101,
                    endDate: 20240101,
                    hokenId: 40,
                    kohi1Id: 30,
                    kohi2Id: 20,
                    kohi3Id: 10,
                    kohi4Id : 50,
                    isAddNew: false,
                    isDeleted: 0,
                    hokenPatternSelected: true
                ),
            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "39345678",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20080330,
                                  endDate: 20080401,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            int birthDay = 19480102;
            int sinDay = 20080331;
            int hpId = 1;
            var reactFromUI = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1005, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            mockSystemConf.Setup(x => x.GetSettingParams(1005, 0, hpId, string.Empty))
            .Returns((int input1, int input2, int input3, string input4) => "0");

            // Act
            var resultIEnum = savePatientInfo.HasElderHoken(insurances, hokenInfs, birthDay, sinDay, reactFromUI);

            var result = resultIEnum.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_072_HasElderHoken_WarningInsuranceElderlyLaterNotYetCovered()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var hokenInf = new HokenInfModel(
                                    hpId: 1,
                                    ptId: 99999,
                                    hokenId: 789012,
                                    hokenKbn: 30,
                                    houbetu: string.Empty,
                                    startDate: 20230101,
                                    endDate: 20230103,
                                    sinDate: 20230102,
                                    hokenMst: new HokenMstModel(),
                                    confirmDateModels: new List<ConfirmDateModel>()
                               );

            // Arrange
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel
                (
                    hpId : 1,
                    ptId: 1,
                    ptBirthday: 19901212,
                    seqNo: 999,
                    hokenSbtCd: 45677,
                    hokenPid: 30,
                    hokenKbn: 1,
                    hokenMemo: "",
                    sinDate: 20240101,
                    startDate: 2030101,
                    endDate: 20240101,
                    hokenId: 40,
                    kohi1Id: 30,
                    kohi2Id: 20,
                    kohi3Id: 10,
                    kohi4Id : 50,
                    isAddNew: false,
                    isDeleted: 0,
                    hokenPatternSelected: true
                ),
            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "39345678",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            int birthDay = 19480102;
            int sinDay = 20230102;
            int hpId = 1;
            var reactFromUI = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1005, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            mockSystemConf.Setup(x => x.GetSettingParams(1005, 0, hpId, string.Empty))
            .Returns((int input1, int input2, int input3, string input4) => "0");

            // Act
            var resultIEnum = savePatientInfo.HasElderHoken(insurances, hokenInfs, birthDay, sinDay, reactFromUI);

            var result = resultIEnum.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("後期高齢者保険が入力されていません。\r\n保険者証を確認してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.WarningInsuranceElderlyLaterNotYetCovered));
        }

        public void TC_073_HasElderHoken_WarningInsuranceElderlyLaterNotYetCovered()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var hokenInf = new HokenInfModel(
                                    hpId: 1,
                                    ptId: 99999,
                                    hokenId: 789012,
                                    hokenKbn: 30,
                                    houbetu: string.Empty,
                                    startDate: 20230101,
                                    endDate: 20230103,
                                    sinDate: 20230102,
                                    hokenMst: new HokenMstModel(),
                                    confirmDateModels: new List<ConfirmDateModel>()
                               );

            // Arrange
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel
                (
                    hpId : 1,
                    ptId: 1,
                    ptBirthday: 19901212,
                    seqNo: 999,
                    hokenSbtCd: 45677,
                    hokenPid: 30,
                    hokenKbn: 1,
                    hokenMemo: "",
                    sinDate: 20230102,
                    startDate: 2030101,
                    endDate: 20230103,
                    hokenId: 789012,
                    kohi1Id: 30,
                    kohi2Id: 20,
                    kohi3Id: 10,
                    kohi4Id : 50,
                    isAddNew: false,
                    isDeleted: 0,
                    hokenPatternSelected: true
                ),
            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "39345678",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20230101,
                                  endDate: 20230103,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20230102,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            int birthDay = 19590102;
            int sinDay = 20230102;
            int hpId = 1;
            var reactFromUI = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1005, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            mockSystemConf.Setup(x => x.GetSettingParams(1005, 0, hpId, string.Empty))
            .Returns((int input1, int input2, int input3, string input4) => "0");

            // Act
            var resultIEnum = savePatientInfo.HasElderHoken(insurances, hokenInfs, birthDay, sinDay, reactFromUI);

            var result = resultIEnum.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("後期高齢者保険の対象外の患者に、後期高齢者保険が登録されています。\r\n保険者証"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.WarningInsuranceElderlyLaterNotYetCovered));
        }

        [Test]
        public void TC_074_IsValidAgeCheck_ValidPattern_NotNull_Param_Equal_0()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var hokenInf = new HokenInfModel(
                                    hpId: 1,
                                    ptId: 99999,
                                    hokenId: 1,
                                    hokenKbn: 30,
                                    houbetu: string.Empty,
                                    startDate: 20230101,
                                    endDate: 20231212,
                                    sinDate: 20230505,
                                    hokenMst: new HokenMstModel(),
                                    confirmDateModels: new List<ConfirmDateModel>()
                               );

            // Arrange
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel
                (
                    hpId : 1,
                    ptId: 1,
                    ptBirthday: 19901212,
                    seqNo: 999,
                    hokenSbtCd: 45677,
                    hokenPid: 30,
                    hokenKbn: 1,
                    hokenMemo: "",
                    sinDate: 20240101,
                    startDate: 2023101,
                    endDate: 99999999,
                    hokenId: 40,
                    kohi1Id: 30,
                    kohi2Id: 20,
                    kohi3Id: 10,
                    kohi4Id : 50,
                    isAddNew: false,
                    isDeleted: 0,
                    hokenPatternSelected: true,
                    houbetu: "1"
                ),
            };


            int birthDay = 20000101;
            int sinDay = 20240101;
            int hpId = 1;
            var reactFromUI = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1005, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            mockSystemConf.Setup(x => x.GetSettingParams(1005, 0, hpId, string.Empty))
            .Returns((int input1, int input2, int input3, string input4) => "0");

            // Act
            var resultIEnum = savePatientInfo.IsValidAgeCheck(insurances, birthDay, sinDay, hpId, reactFromUI);

            var result = resultIEnum.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_075_IsValidAgeCheck_ValidPattern_NotNull_Param_Difference_0()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var hokenInf = new HokenInfModel(
                                    hpId: 1,
                                    ptId: 99999,
                                    hokenId: 1,
                                    hokenKbn: 30,
                                    houbetu: string.Empty,
                                    startDate: 20230101,
                                    endDate: 20231212,
                                    sinDate: 20230505,
                                    hokenMst: new HokenMstModel(),
                                    confirmDateModels: new List<ConfirmDateModel>()
                               );

            // Arrange
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel
                (
                    hpId : 1,
                    ptId: 1,
                    ptBirthday: 19901212,
                    seqNo: 999,
                    hokenSbtCd: 45677,
                    hokenPid: 30,
                    hokenKbn: 1,
                    hokenMemo: "",
                    sinDate: 20240101,
                    startDate: 2023101,
                    endDate: 99999999,
                    hokenId: 40,
                    kohi1Id: 30,
                    kohi2Id: 20,
                    kohi3Id: 10,
                    kohi4Id : 50,
                    isAddNew: false,
                    isDeleted: 0,
                    hokenPatternSelected: true,
                    houbetu: "1"
                ),
            };


            int birthDay = 20000101;
            int sinDay = 20240101;
            int hpId = 1;
            var reactFromUI = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1005, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            mockSystemConf.Setup(x => x.GetSettingParams(1005, 0, hpId, string.Empty))
            .Returns((int input1, int input2, int input3, string input4) => "1");

            // Act
            var resultIEnum = savePatientInfo.IsValidAgeCheck(insurances, birthDay, sinDay, hpId, reactFromUI);

            var result = resultIEnum.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.True(result.Any(r => r.Message == "1歳となりました。\r\n保険証を確認してください。"));
        }


        [Test]
        public void TC_076_IsValidAgeCheck_ValidPattern_NotNull_Age_More_70()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var hokenInf = new HokenInfModel(
                                    hpId: 1,
                                    ptId: 99999,
                                    hokenId: 1,
                                    hokenKbn: 30,
                                    houbetu: string.Empty,
                                    startDate: 20230101,
                                    endDate: 20231212,
                                    sinDate: 20230505,
                                    hokenMst: new HokenMstModel(),
                                    confirmDateModels: new List<ConfirmDateModel>()
                               );

            // Arrange
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel
                (
                    hpId : 1,
                    ptId: 1,
                    ptBirthday: 19901212,
                    seqNo: 999,
                    hokenSbtCd: 45677,
                    hokenPid: 30,
                    hokenKbn: 1,
                    hokenMemo: "",
                    sinDate: 20240101,
                    startDate: 2023101,
                    endDate: 99999999,
                    hokenId: 40,
                    kohi1Id: 30,
                    kohi2Id: 20,
                    kohi3Id: 10,
                    kohi4Id : 50,
                    isAddNew: false,
                    isDeleted: 0,
                    hokenPatternSelected: true,
                    houbetu: "1"
                ),
            };


            int birthDay = 19390101;
            int sinDay = 20240101;
            int hpId = 1;
            var reactFromUI = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1005, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            mockSystemConf.Setup(x => x.GetSettingParams(1005, 0, hpId, string.Empty))
            .Returns((int input1, int input2, int input3, string input4) => "1");

            // Act
            var resultIEnum = savePatientInfo.IsValidAgeCheck(insurances, birthDay, sinDay, hpId, reactFromUI);

            var result = resultIEnum.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.True(result.Any(r => r.Message == "1歳となりました。\r\n高齢受給者証を確認してください。"));
        }

        [Test]
        public void TC_077_IsValidMainHoken_PtInfMainHokenPid()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(true, true, new HokenInfModel(1, 0, 99999999), 1, 1) };
            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(0, 1, 0, string.Empty, 0, 0, 99999999, "105") };
            int ptInfMainHokenPid = 0;
            ReactSavePatientInfo rs = new ReactSavePatientInfo();
            // Act
            var result = savePatientInfo.IsValidMainHoken(insurances, hokenInfs, rs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(result.Any(r => r.Message == "'001. 自費'の保険組合せを主保険に設定しますか？"));
        }

        [Test]
        public void TC_078_IsValidMainHoken_PtInfMainHokenPid()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(true, true, new HokenInfModel(1, 1, 20100101), 1, 1) };
            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(0, 1, 0, string.Empty, 0, 0, 99999999, "105") };
            int ptInfMainHokenPid = 1;
            ReactSavePatientInfo rs = new ReactSavePatientInfo();
            // Act
            var result = savePatientInfo.IsValidMainHoken(insurances, hokenInfs, rs, ptInfMainHokenPid);

            // Assert
            Assert.IsTrue(result.Any(r => r.Message == "主保険に期限切れの保険が設定されています。主保険の設定を確認してください。"));
        }

        [Test]
        public void TC_079_IsValidDuplicateHoken_Duplicate_False()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(0, 1, 1, "0001", 0, 0, 99999999, "105"), new HokenInfModel(0, 1, 1, "0001", 0, 0, 99999999, "105") };
            // Act
            var result = savePatientInfo.IsValidDuplicateHoken(hokenInfs);

            // Assert
            Assert.IsTrue(!result);
        }

        [Test]
        public void TC_080_IsValidDuplicateHoken_Duplicate_True()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(0, 1, 1, "0001", 0, 0, 99999999, "105") };
            // Act
            var result = savePatientInfo.IsValidDuplicateHoken(hokenInfs);

            // Assert
            Assert.IsTrue(result);
        } 
        
        [Test]
        public void TC_081_IsValidPeriod_True()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(0, 1, 1, "0001", 0, 0, 99999999, "105") };
            // Act
            var result = savePatientInfo.IsValidPeriod(hokenInfs);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TC_082_IsValidPeriod_False()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(0, 1, 1, "0001", 0, 0, 99999999, "105"), new HokenInfModel(0, 1, 1, "0001", 0, 0, 99999999, "105") };
            // Act
            var result = savePatientInfo.IsValidPeriod(hokenInfs);

            // Assert
            Assert.IsTrue(!result);
        } 
        
        [Test]
        public void TC_083_IsValidDuplicateKohi_False()
        {
            //Mock
             var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<KohiInfModel> kohiInfModels = new List<KohiInfModel> { new KohiInfModel(0, 99999999, "abc") };
            // Act
            var result = savePatientInfo.IsValidDuplicateKohi(kohiInfModels);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TC_084_IsValidDuplicateKohi_False()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<KohiInfModel> kohiInfModels = new List<KohiInfModel> { new KohiInfModel(0, 99999999, "abc"), new KohiInfModel(0, 99999999, "abc")};
            // Act
            var result = savePatientInfo.IsValidDuplicateKohi(kohiInfModels);

            // Assert
            Assert.IsTrue(!result);
        }

        [Test]
        public void TC_085_IsValidHokenPatternAll_IsUpdateMode_True()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            //List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(1, 1, 19930101, 1, 0, 1, 1, 20230101, string.Empty, new HokenInfModel(1, 0, 99999999), new KohiInfModel(1), new KohiInfModel(2), new KohiInfModel(3), new KohiInfModel(4), 0, 0, 99999999, false)};
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(true, true, new(), 1, 1) };
            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(0, 1, 0, string.Empty, 0, 0, 99999999, "105") };
            List<KohiInfModel> kohiInfModels = new List<KohiInfModel> { new KohiInfModel(0, 99999999, "abc"), new KohiInfModel(0, 99999999, "abc") };
            int ptInfMainHokenPid = 0;
            ReactSavePatientInfo rs = new ReactSavePatientInfo();
            // Act
            var result = savePatientInfo.IsValidHokenPatternAll(insurances, hokenInfs, kohiInfModels, true, 19930903,20230101, 1, rs, ptInfMainHokenPid);

            // Assert
            Assert.That(result.Count() == 2);
        }

        [Test]
        public void TC_086_IsValidHokenPatternAll_PatternDuplicate()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(1, 1, 19930101, 1, 0, 1, 1, 20230101, string.Empty, new HokenInfModel(1, 0, 99999999), new KohiInfModel(1), new KohiInfModel(2), new KohiInfModel(3), new KohiInfModel(4), 0, 0, 99999999, true), new InsuranceModel(1, 1, 19930101, 1, 0, 1, 1, 20230101, string.Empty, new HokenInfModel(1, 0, 99999999), new KohiInfModel(1), new KohiInfModel(2), new KohiInfModel(3), new KohiInfModel(4), 0, 0, 99999999, true) };

            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(0, 1, 0, string.Empty, 0, 0, 99999999, "105") };
            List<KohiInfModel> kohiInfModels = new List<KohiInfModel> { new KohiInfModel(0, 99999999, "abc"), new KohiInfModel(0, 99999999, "abc") };
            int ptInfMainHokenPid = 0;
            ReactSavePatientInfo rs = new ReactSavePatientInfo();
            // Act
            var result = savePatientInfo.IsValidHokenPatternAll(insurances, hokenInfs, kohiInfModels, true, 19930903, 20230101, 1, rs, ptInfMainHokenPid);

            // Assert
            Assert.That(result.Count() == 2 && result.Any(r => r.Message == "同じ組合せの保険・公１・公２・公３・公４を持つ組合せが既に登録されています。\r\n登録しますか？"));
        }

        [Test]
        public void TC_087_IsValidHokenPatternAll_DuplicateHoken()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            List<InsuranceModel> insurances = new List<InsuranceModel> { new InsuranceModel(1, 1, 19930101, 1, 0, 1, 1, 20230101, string.Empty, new HokenInfModel(1, 0, 99999999), new KohiInfModel(1), new KohiInfModel(2), new KohiInfModel(3), new KohiInfModel(4), 0, 0, 99999999, true), new InsuranceModel(1, 1, 19930101, 1, 0, 1, 1, 20230101, string.Empty, new HokenInfModel(1, 0, 99999999), new KohiInfModel(1), new KohiInfModel(2), new KohiInfModel(3), new KohiInfModel(4), 0, 0, 99999999, true) };

            List<HokenInfModel> hokenInfs = new List<HokenInfModel> { new HokenInfModel(0, 1, 1, "001", 0, 0, 99999999, "105"), new HokenInfModel(0, 1, 1, "001", 0, 0, 99999999, "105") };
            List<KohiInfModel> kohiInfModels = new List<KohiInfModel> { new KohiInfModel(0, 99999999, "abc"), new KohiInfModel(0, 99999999, "abc") };
            int ptInfMainHokenPid = 0;
            ReactSavePatientInfo rs = new ReactSavePatientInfo();
            // Act
            var result = savePatientInfo.IsValidHokenPatternAll(insurances, hokenInfs, kohiInfModels, true, 19930903, 20230101, 1, rs, ptInfMainHokenPid);

            // Assert
            Assert.That(result.Count() == 4 && result.Any(r => r.Message == "同じ保険番号を持つ保険が既に登録されています。\r\n登録しますか？") && result.Any(r => r.Message == "同一期間内に複数保険が登録されています。\r\n有効期限を確認してください。"));
        }

    }
}
