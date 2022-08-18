﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InputItem
{
    public interface IInputItemRepository
    {
        public IEnumerable<InputItemModel> SearchDataInputItem(string keyword, int kouiKbn, int sinDate, int startIndex, int pageCount, int genericOrSameItem, string yjCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired);
    }
}
