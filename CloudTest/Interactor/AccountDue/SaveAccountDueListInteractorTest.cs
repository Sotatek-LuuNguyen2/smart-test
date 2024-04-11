﻿using Domain.Models.AccountDue;
using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.User;
using EventProcessor.Interfaces;
using Infrastructure.Logger;
using Interactor.AccountDue;
using Microsoft.AspNetCore.Http;
using Moq;
using UseCase.AccountDue.SaveAccountDueList;

namespace CloudUnitTest.Interactor.AccountDue
{
    public class SaveAccountDueListInteractorTest : BaseUT
    {
        #region ValidateInputDatas
        [Test] 
        public void TC_001_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidHpId()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(999, 99999);
            string kaikeiTime = "";

            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>();

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId))
            .Returns((int hpId) => false);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidHpId);
        }

        [Test]
        public void TC_002_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidUserId()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(999, 99999);
            string kaikeiTime = "";

            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>();

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => false);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidUserId);
        }

        [Test]
        public void TC_003_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidPtId()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(999, 99999);
            string kaikeiTime = "";

            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>();

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => false);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidPtId);
        }

        [Test]
        public void TC_004_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidSindate()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(999, 99999);
            string kaikeiTime = "";
            int nyukinKbn = -1, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 0, nyukinDate = 0, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0; 
            long raiinNo = 0, seqNo = 0;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidSindate);
        }

        [Test]
        public void TC_005_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidNyukinKbn()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = -1, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 0, nyukinDate = 0, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 0, seqNo = 0;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidNyukinKbn);
        }

        [Test]
        public void TC_006_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidRaiinNo()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = 0, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 0, nyukinDate = 0, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 0, seqNo = 0;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidRaiinNo);
        }

        [Test]
        public void TC_007_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidSortNo()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = 0, sortNo = -1, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 0, nyukinDate = 0, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 1, seqNo = 0;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidSortNo);
        }

        [Test]
        public void TC_008_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidPaymentMethodCd()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = 0, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = -1, nyukinDate = 0, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 1, seqNo = 0;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidPaymentMethodCd);
        }

        [Test]
        public void TC_009_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidNyukinDate()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = 0, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 1, nyukinDate = -1, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 1, seqNo = 0;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidNyukinDate);
        }

        [Test]
        public void TC_010_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_NyukinCmtMaxLength100()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = 0, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 1, nyukinDate = 1, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 1, seqNo = 0;
            string longString = "OpenAI is amazing! " , seikyuDetail = "";
            string nyukinCmt = string.Concat(Enumerable.Repeat(longString, 10));
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.NyukinCmtMaxLength100);
        }

        [Test]
        public void TC_011_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidSeikyuTensu()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = 0, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 1, nyukinDate = 1, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = -1, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 1, seqNo = 0;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidSeikyuTensu);
        }
        
        [Test]
        public void TC_012_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidSeqNo()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = 0, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 1, nyukinDate = 1, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 1, seqNo = -1;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidSeqNo);
        }

        [Test]
        public void TC_013_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidNyukinKbn()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = 0, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 1, nyukinDate = 1, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 1, seqNo = 0;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete),
                new SyunoNyukinInputItem(nyukinKbn + 1, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidNyukinKbn);
        }

        [Test]
        public void TC_014_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidSeikyuGaku()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = 0, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 1, nyukinDate = 1, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 1, seqNo = 0;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete),
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku + 1, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidSeikyuGaku);
        }

        [Test]
        public void TC_015_SaveAccountDueListInteractor_Handle_ValidateInputDatas_Return_InvalidSeikyuAdjustFutan()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIEventProcessorService = new Mock<IEventProcessorService>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockILoggingHandler = new Mock<ILoggingHandler>();

            var saveAccountDueListInteractor = new SaveAccountDueListInteractor(mockIAccountDueRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                                mockIPatientInforRepository.Object, mockIEventProcessorService.Object, mockIReceptionRepository.Object, TenantProvider);

            Random random = new Random();

            int hpId = random.Next(999, 99999); ;
            int userId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(10000000, 99999999);
            string kaikeiTime = "";
            int nyukinKbn = 0, sortNo = 0, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 1, nyukinDate = 1, uketukeSbt = 0, seikyuGaku = 0, seikyuTensu = 0, raiinInfStatus = 0, seikyuAdjustFutan = 0, seikyuSinDate = 0;
            long raiinNo = 1, seqNo = 0;
            string nyukinCmt = "", seikyuDetail = "";
            bool isUpdated = false, isDelete = false;
            List<SyunoNyukinInputItem> syunoNyukinInputItems = new List<SyunoNyukinInputItem>()
            {
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan, seikyuSinDate, isDelete),
                new SyunoNyukinInputItem(nyukinKbn, raiinNo, sortNo, adjustFutan, nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, seikyuGaku, seikyuTensu, seikyuDetail, isUpdated, seqNo, raiinInfStatus, seikyuAdjustFutan + 1, seikyuSinDate, isDelete)
            };

            mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId)).Returns((int hpId) => true);
            mockIUserRepository.Setup(finder => finder.CheckExistedUserId(hpId, userId)).Returns((int hpId, int userId) => true);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new List<long> { ptId })).Returns((int hpId, List<long> ptId) => true);
            var inputData = new SaveAccountDueListInputData(hpId, userId, ptId, sinDate, kaikeiTime, syunoNyukinInputItems);
            var result = saveAccountDueListInteractor.Handle(inputData);

            Assert.That(result.Status == SaveAccountDueListStatus.InvalidSeikyuAdjustFutan);
        }
        #endregion
    }
}
