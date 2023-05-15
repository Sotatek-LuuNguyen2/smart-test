﻿using Reporting.Mappers.Common;
using Reporting.Sokatu.KokhoSokatu.Service;
using Reporting.Structs;

namespace Reporting.ReceiptPrint.Service
{
    public class ReceiptPrintService : IReceiptPrintService
    {
        #region properties
        private bool _isNormal { get; set; }
        private bool _isDelay { get; set; }
        private bool _isHenrei { get; set; }
        private bool _isPaper { get; set; }
        private bool _isOnline { get; set; }

        #endregion

        private readonly IP28KokhoSokatuCoReportService _p28KokhoSokatuCoReportService;

        public ReceiptPrintService(IP28KokhoSokatuCoReportService p28KokhoSokatuCoReportService)
        {
            _p28KokhoSokatuCoReportService = p28KokhoSokatuCoReportService;
        }

        public CommonReportingRequestModel GetReceiptPrint(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int ptId, int seikyuYm, int sinYm, int hokenId)
        {
            var seikyuType = GetSeikyuType(dataKbn);

            if (prefNo == 28 && reportId == 102 && reportEdaNo == 0)
            {
                return _p28KokhoSokatuCoReportService.GetP28KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
            }

            return new();
        }

        private SeikyuType GetSeikyuType(int dataKbn)
        {
            int targetReceiptVal = (dataKbn >= 0 && dataKbn <= 2) ? (dataKbn + 1) : 0;
            switch (targetReceiptVal)
            {
                case 1:
                    _isNormal = true;
                    _isDelay = true;
                    _isHenrei = true;
                    _isPaper = true;
                    _isOnline = false;
                    break;
                case 2:
                    _isNormal = true;
                    _isDelay = true;
                    _isHenrei = false;
                    _isPaper = false;
                    _isOnline = false;
                    break;
                case 3:
                    _isNormal = false;
                    _isDelay = false;
                    _isHenrei = true;
                    _isPaper = true;
                    _isOnline = false;
                    break;
                default:
                    _isNormal = false;
                    _isDelay = false;
                    _isHenrei = false;
                    _isPaper = false;
                    _isOnline = false;
                    break;
            }

            return new SeikyuType(_isNormal, _isPaper, _isDelay, _isHenrei, _isOnline);
        }
    }
}
