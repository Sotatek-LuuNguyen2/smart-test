﻿using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetailData.ShowKanjaMuke
{
    public class ShowKanjaMukeInputData : IInputData<ShowKanjaMukeOutputData>
    {
        public ShowKanjaMukeInputData(string itemCd, int selectedIndexOfMenuLevel, int level, string drugName, string yJCode)
        {
            ItemCd = itemCd;
            SelectedIndexOfMenuLevel = selectedIndexOfMenuLevel;
            Level = level;
            DrugName = drugName;
            YJCode = yJCode;
        }

        public string ItemCd { get; private set; }

        public int SelectedIndexOfMenuLevel { get; private set; }

        public int Level { get; private set; }

        public string DrugName { get; private set; }

        public string YJCode { get; private set; }
    }
}
