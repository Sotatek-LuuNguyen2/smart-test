﻿using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P42KokhoSokatuCoReportService : IP42KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 42;
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKokhoSokatuFinder _kokhoFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    #endregion

    #region Constructor and Init
    public P42KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p42KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    public CommonReportingRequestModel GetP42KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            this.currentPage = 1;
            var getData = GetData();
            hasNextPage = true;

            if (getData)
            {
                while (getData && hasNextPage)
                {
                    UpdateDrawForm();
                    currentPage++;
                }
            }

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new KokhoSokatuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData).GetData();
        }
        finally
        {
            _kokhoFinder.ReleaseResource();
        }
    }

    #region Private function
    private bool UpdateDrawForm()
    {
        bool _hasNextPage = false;

        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //医療機関コード
            SetFieldData("hpCode", hpInf.HpCd);
            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            SetFieldData("hpTel", hpInf.Tel);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            #region 合計
            //国保合計
            int totalCount = receInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList().Count;
            SetFieldData("kokhoTotalCount", totalCount.ToString());
            //後期合計
            totalCount = receInfs.Where(r => r.IsKoukiAll).ToList().Count;
            SetFieldData("koukiTotalCount", totalCount.ToString());
            //総合計
            totalCount = receInfs.ToList().Count;
            SetFieldData("totalCount", totalCount.ToString());
            //点数確認
            int kokhoTensu = receInfs.Where(r => r.Tensu >= 80000).ToList().Count;
            if (kokhoTensu >= 1)
            {
                SetVisibleFieldData("Circle1", true);
                SetVisibleFieldData("Circle2", false);
            }
            else
            {
                SetVisibleFieldData("Circle1", false);
                SetVisibleFieldData("Circle2", true);
            }
            #endregion

            #region Body
            const int maxRow = 2;

            //国保
            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = null;
                switch (rowNo)
                {
                    //case 0: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && (r.IsPrefIn || (r.IsPrefIn && r.IsKumiai))).ToList(); break;
                    case 0: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsPrefIn).ToList(); break;
                    case 1: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && !r.IsPrefIn).ToList(); break;
                }
                if (wrkReces == null) continue;

                //保険者数
                int kokhoSeikyuCount = wrkReces.GroupBy(r => r.HokensyaNo).Count();
                listDataPerPage.Add(new("kokhoSeikyuCount", 0, rowNo, kokhoSeikyuCount.ToString()));
            }

            //後期
            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = null;
                switch (rowNo)
                {
                    case 0: wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsPrefIn).ToList(); break;
                    case 1: wrkReces = receInfs.Where(r => r.IsKoukiAll && !r.IsPrefIn).ToList(); break;
                }
                if (wrkReces == null) continue;

                //保険者数
                int koukiSeikyuCount = wrkReces.GroupBy(r => r.HokensyaNo).Count();
                listDataPerPage.Add(new("koukiSeikyuCount", 0, rowNo, koukiSeikyuCount.ToString()));
            }
            #endregion
            _listTextData.Add(pageIndex, listDataPerPage);
            return 1;
        }
        #endregion

        #endregion

        if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
        {
            hasNextPage = _hasNextPage;
            return false;
        }
        hasNextPage = _hasNextPage;
        return true;
    }

    private bool GetData()
    {
        hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);

        return (receInfs?.Count ?? 0) > 0;
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

    private void SetVisibleFieldData(string field, bool value)
    {
        if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
        {
            _visibleFieldData.Add(field, value);
        }
    }
    #endregion
}
