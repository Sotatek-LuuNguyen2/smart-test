﻿using Domain.Common;
using Domain.Enum;
using Domain.Models.FlowSheet;
using Domain.Models.OrdInf;
using Domain.Models.TodayOdr;
using Helper.Enum;

namespace Domain.Models.MstItem
{
    public interface IMstItemRepository : IRepositoryBase
    {
        List<DosageDrugModel> GetDosages(List<string> yjCds);

        List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData();

        (List<OtcItemModel>, int) SearchOTCModels(string searchValue, int pageIndex, int pageSize);

        (List<SearchSupplementModel>, int) GetListSupplement(string searchValue, int pageIndex, int pageSize);

        (List<TenItemModel> tenItemModels, int totalCount) SearchTenMst(string keyword, int kouiKbn, int sinDate, int pageIndex, int pageCount, int genericOrSameItem, string yjCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired, string itemCodeStartWith, bool isMasterSearch, bool isSearch831SuffixOnly, bool isSearchSanteiItem, byte searchFollowUsage, List<int> KouiKbns, string masterSBT);

        TenItemModel GetTenMst(int hpId, int sinDate, string itemCd);

        bool UpdateAdoptedItemAndItemConfig(int valueAdopted, string itemCdInputItem, int startDateInputItem, int hpId, int userId);

        List<ByomeiMstModel> DiseaseSearch(bool isPrefix, bool isByomei, bool isSuffix, bool isMisaiyou, string keyword, int sindate, int pageIndex, int pageSize);

        List<ByomeiMstModel> DiseaseSearch(List<string> keyCodes);

        bool UpdateAdoptedByomei(int hpId, string byomeiCd, int userId);

        List<TenItemModel> GetCheckTenItemModels(int hpId, int sinDate, List<string> itemCds);

        bool CheckItemCd(string itemCd);

        (int, List<PostCodeMstModel>) PostCodeMstModels(int hpId, string postCode1, string postCode2, string address, int pageIndex, int pageSize);

        TenItemModel FindTenMst(int hpId, string itemCd, int sinDate);

        List<TenItemModel> FindTenMst(int hpId, List<string> itemCds, int minSinDate, int maxSinDate);

        List<TenItemModel> GetTenMstList(int hpId, List<string> itemCds);

        List<ItemCmtModel> GetCmtCheckMsts(int hpId, int userId, List<string> itemCds);

        List<ItemGrpMstModel> FindItemGrpMst(int hpId, int sinDate, int grpSbt, List<long> itemGrpCds);

        List<ItemGrpMstModel> FindItemGrpMst(int hpId, int minSinDate, int maxSinDate, int grpSbt, List<long> itemGrpCds);

        List<TenItemModel> GetAdoptedItems(List<string> itemCds, int sinDate, int hpId);

        bool UpdateAdoptedItems(int valueAdopted, List<string> itemCds, int sinDate, int hpId, int userId);

        List<ItemCommentSuggestionModel> GetSelectiveComment(int hpId, List<string> listItemCd, int sinDate, List<int> isInvalidList, bool isRecalculation = false);

        List<string> GetCheckItemCds(List<string> itemCds);

        List<Tuple<string, string>> GetCheckIpnCds(List<string> ipnCds);

        List<string> GetListSanteiByomeis(int hpId, long ptId, int sinDate, int hokenPid);

        //Key of Dictionary is itemCd
        Dictionary<string, (int sinkouiKbn, string itemName, List<TenItemModel>)> GetConversionItem(List<(string itemCd, int sinKouiKbn, string itemName)> expiredItems, int sinDate, int hpId);

        bool ExceConversionItem(int hpId, int userId, Dictionary<string, List<TenItemModel>> values);

        List<TenItemModel> FindTenMst(int hpId, List<string> itemCds);

        List<HolidayModel> FindHolidayMstList(int hpId, int fromDate, int toDate);

        List<KensaCenterMstModel> GetListKensaCenterMst(int hpId);

        List<TenMstOriginModel> GetGroupTenMst(string itemCd);

        string GetMaxItemCdByTypeForAdd(string startWithstr);

        int GetMinJihiSbtMst(int hpId);

        bool SaveKensaCenterMst(int userId, List<KensaCenterMstModel> kensaCenterMstModels);

        bool IsTenMstItemCdUsed(int hpId, string itemCd);

        bool SaveDeleteOrRecoverTenMstOrigin(DeleteOrRecoverTenMstMode mode, string itemCd, int userId, List<TenMstOriginModel> tenMstModifieds);

        List<CmtKbnMstModel> GetListCmtKbnMstModelByItemCd(int hpId, string itemCd);

        TenMstOriginModel GetTenMstOriginModel(int hpId,string itemCd, int sinDate);

        string GetTenMstName(int hpId, string santeiItemCd);

        List<M10DayLimitModel> GetM10DayLimitModels(string yjCdItem);

        List<IpnMinYakkaMstModel> GetIpnMinYakkaMstModels(int hpId, string IpnNameCd);

        List<DrugDayLimitModel> GetDrugDayLimitModels(int hpId, string ItemCd);

        DosageMstModel GetDosageMstModel(int hpId, string ItemCd);

        IpnNameMstModel GetIpnNameMstModel(int hpId, string ipnNameCd, int sinDate);

        string GetYohoInfMstPrefixByItemCd(string itemCd);

        List<DrugInfModel> GetDrugInfByItemCd(int hpId, string itemCd);

        PiImageModel GetImagePiByItemCd(int hpId, string itemCd, int imageType);

        List<TeikyoByomeiModel> GetTeikyoByomeiModel(int hpId, string itemCd, bool isFromCheckingView = false);

        TekiouByomeiMstExcludedModel GetTekiouByomeiMstExcludedModelByItemCd(int hpId, string itemCd);

        List<DensiSanteiKaisuModel> GetDensiSanteiKaisuByItemCd(int hpId, string itemCd);

        List<DensiHaihanModel> GetDensiHaihans(int hpId, string itemCd, int haihanKbn);

        List<DensiHoukatuModel> GetListDensiHoukatuByItemCd(int hpId, string itemCd, int sinDate);

        List<DensiHoukatuGrpModel> GetListDensiHoukatuGrpByItemCd(int hpId, string itemCd, int sinDate);

        List<DensiHoukatuModel> GetListDensiHoukatuMaster(int hpId, List<string> listGrpNo);

        List<CombinedContraindicationModel> GetContraindicationModelList(int sinDate, string itemCd);

        bool SaveTenMstOriginSetData(IEnumerable<CategoryItemEnums> tabActs, string itemCd, List<TenMstOriginModel> tenMstGrigins, SetDataTenMstOriginModel setDataTen, int userId, int hpId);

        List<TenMstMaintenanceModel> GetTenMstListByItemType(int hpId, ItemTypeEnums itemType, string startWithstr, int sinDate);
    }
}
