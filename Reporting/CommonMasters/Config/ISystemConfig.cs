﻿using Domain.Common;

namespace Reporting.CommonMasters.Config;

public interface ISystemConfig : IRepositoryBase
{
    int OrderLabelKaPrint();
    int OrderLabelSyosaiPrint();
    int OrderLabelCreateNamePrint();
    int OrderLabelHeaderPrint();
    int OrderLabelKensaDsp();
    int OrderLabelYoyakuDateDsp();
    int OrderLabelSanteiGaiDsp();
    string JyusinHyoRaiinKbn();
    int SijisenRpName();
    int JyusinHyoRpName();
    int SijisenAlrgy();
    int JyusinHyoAlrgy();
    int SijisenPtCmt();
    int JyusinHyoPtCmt();
    int SijisenKensaYokiZairyo();
    int JyusinHyoKensaYokiZairyo();
    string WebIdQrCode();
    string MedicalInstitutionCode();
    string WebIdUrlForPc();
    int SyohosenQRVersion();
    int SyohosenChiikiHoukatu();
    int SyohosenRinjiKisai();
    int SyohosenTani();
    int SyohosenHikae();
    int SyohosenFutanRate();
    string SyohosenRefillZero();
    int SyohosenRefillStrikeLine();
    int SyohosenQRKbn();
    int RosaiReceden();
    string RosaiRecedenTerm();
    int AccountingDetailIncludeComment();
    int AccountingDetailIncludeOutDrug();
    int AccountingUseBackwardFields();
    int AccountingTeikeibunPrint();
    string AccountingTeikeibun1();
    string AccountingTeikeibun2();
    string AccountingTeikeibun3();
    int AccountingFormType();
    int AccountingDetailFormType();
    int AccountingMonthFormType();
    int AccountingDetailMonthFormType();
    int PrintReceiptPay0Yen();
    int PrintDetailPay0Yen();
    string PlanetHostName();
    string PlanetDatabase();
    string PlanetUserName();
    string PlanetPassword();
    int PlanetType();
    int YakutaiTaniDsp();
    int YakutaiOnceAmount();
    string YakutaiFukuyojiIppokaItemCd();
    int YakutaiPrintUnit();
    int YakutaiPaperSize();
    int YakutaiNaifukuPaperSmallMinValue();
    int YakutaiNaifukuPaperNormalMinValue();
    int YakutaiNaifukuPaperBigMinValue();
    int YakutaiTonpukuPaperSmallMinValue();
    int YakutaiTonpukuPaperNormalMinValue();
    int YakutaiTonpukuPaperBigMinValue();
    int YakutaiGaiyoPaperSmallMinValue();
    int YakutaiGaiyoPaperNormalMinValue();
    int YakutaiGaiyoPaperBigMinValue();
    string YakutaiNaifukuPaperSmallPrinter();
    string YakutaiNaifukuPaperNormalPrinter();
    string YakutaiNaifukuPaperBigPrinter();
    string YakutaiTonpukuPaperSmallPrinter();
    string YakutaiTonpukuPaperNormalPrinter();
    string YakutaiTonpukuPaperBigPrinter();
    string YakutaiGaiyoPaperSmallPrinter();
    string YakutaiGaiyoPaperNormalPrinter();
    string YakutaiGaiyoPaperBigPrinter();
    int HikariDiskIsTotalCnt();
    int PrintReceipt();
    int PrintDetail();
    int P13WelfareGreenSeikyuType();
    int P13WelfareBlueSeikyuType();
    int OdrKensaIraiKaCode();
    int SyohosenKouiDivide();
    /// <summary>
    /// 電子処方箋
    /// VAL in (0, 1)以外はライセンスなし
    /// </summary>
    int ElectronicPrescriptionLicense();
    int JibaiJunkyo();
}
