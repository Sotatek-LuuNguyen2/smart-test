﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceMst
{
    public class InsuranceMstModel
    {
        public InsuranceMstModel()
        {
            ListTokkiMstModel = new List<TokkiMstModel>();
            HokenKogakuKbnDict = new Dictionary<int, string>();
            KantokuMstData = new List<KantokuMstModel>();
            ByomeiMstAftercareData = new List<ByomeiMstAftercareModel>();
            RoudouMst = new List<RoudouMstModel>();
            HokenMstAlLData = new List<HokenMstModel>();
        }

        public InsuranceMstModel(List<TokkiMstModel> listTokkiMstModel, Dictionary<int, string> hokenKogakuKbnDict, List<KantokuMstModel> kantokuMstData, List<ByomeiMstAftercareModel> byomeiMstAftercareData, List<RoudouMstModel> roudouMst, List<HokenMstModel> hokenMstAlLData)
        {
            ListTokkiMstModel = listTokkiMstModel;
            HokenKogakuKbnDict = hokenKogakuKbnDict;
            KantokuMstData = kantokuMstData;
            ByomeiMstAftercareData = byomeiMstAftercareData;
            RoudouMst = roudouMst;
            HokenMstAlLData = hokenMstAlLData;
        }

        public List<TokkiMstModel> ListTokkiMstModel { get; private set; }

        public Dictionary<int, string> HokenKogakuKbnDict { get; private set; }

        public List<KantokuMstModel> KantokuMstData { get; private set; }

        public List<ByomeiMstAftercareModel> ByomeiMstAftercareData { get; private set; }

        public List<RoudouMstModel> RoudouMst { get; private set; }

        public List<HokenMstModel> HokenMstAlLData { get; private set; }
    }
}
