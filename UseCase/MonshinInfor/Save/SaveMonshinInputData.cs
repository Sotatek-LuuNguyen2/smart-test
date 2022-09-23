﻿using Domain.Models.MonshinInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MonshinInfor.Insert;

namespace UseCase.MonshinInfor.Save
{
    public class SaveMonshinInputData : IInputData<SaveMonshinOutputData>
    {
        public SaveMonshinInputData(List<MonshinInforModel> monshinInfors)
        {
            MonshinInfors = monshinInfors;
        }

        public List<MonshinInforModel> MonshinInfors { get; private set; }
    }
}