﻿using Domain.Common;

namespace Domain.Models.ColumnSetting;

public interface IColumnSettingRepository : IRepositoryBase
{
    List<ColumnSettingModel> GetList(int hpId, int userId, string tableName);
    Dictionary<string, List<ColumnSettingModel>> GetList(int hpId, int userId, List<string> tableNameList);
    bool SaveList(List<ColumnSettingModel> settingModels);
}
