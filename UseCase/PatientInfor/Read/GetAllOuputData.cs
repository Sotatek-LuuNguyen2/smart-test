﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.Read
{
    public class GetAllOuputData: IOutputData
    {
        public string Keyword { get; set; }

        public List<PatientInfor> listData { get; set; }

        public GetAllOuputData(string keyword)
        {
            Keyword = keyword;
        }

    }
}
