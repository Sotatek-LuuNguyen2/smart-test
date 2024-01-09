﻿
using Domain.Models.SuperSetDetail;

namespace UseCase.SuperSetDetail.GetSuperSetDetail
{
    public class SetByomeiItem
    {
        public SetByomeiItem(long id, bool isSyobyoKbn, int sikkanKbn, int nanByoCd, string mainByomei, string fullByomei, bool isSuspected, bool isDspRece, bool isDspKarte, string byomeiCmt, string byomeiCd, List<PrefixSuffixModel> prefixSuffixList, string icd10, string icd102013, string icd1012013, string icd1022013)
        {
            Id = id;
            IsSyobyoKbn = isSyobyoKbn;
            SikkanKbn = sikkanKbn;
            NanByoCd = nanByoCd;
            MainByomei = mainByomei;
            FullByomei = fullByomei;
            IsSuspected = isSuspected;
            IsDspRece = isDspRece;
            IsDspKarte = isDspKarte;
            ByomeiCmt = byomeiCmt;
            ByomeiCd = byomeiCd;
            PrefixSuffixList = prefixSuffixList;
            Icd10 = icd10;
            Icd1012013 = icd1012013;
            Icd102013 = icd102013;
            Icd1022013 = icd1022013;
        }

        public long Id { get; private set; }

        public bool IsSyobyoKbn { get; private set; }

        public int SikkanKbn { get; private set; }

        public int NanByoCd { get; private set; }

        public string MainByomei { get; private set; }

        public string FullByomei { get; private set; }

        public bool IsSuspected { get; private set; }

        public bool IsDspRece { get; private set; }

        public bool IsDspKarte { get; private set; }

        public string ByomeiCmt { get; private set; }

        public string ByomeiCd { get; private set; }

        public string Icd10 { get; private set; }

        public string Icd102013 { get; private set; }

        public string Icd1012013 { get; private set; }

        public string Icd1022013 { get; private set; }

        public List<PrefixSuffixModel> PrefixSuffixList { get; private set; }
    }
}
