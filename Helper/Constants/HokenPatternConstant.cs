﻿using System.Collections.Immutable;

namespace Helper.Constants;

public static class HokenPatternConstant
{
    public static readonly ImmutableDictionary<string, string> PatternNameDic = new Dictionary<string, string>()
    {
        { "008000", "自費" },
        { "009000", "自費レセ" },

        { "011112", "社保単独・本人" },
        { "011114", "社保単独・６未" },
        { "011116", "社保単独・家族" },
        { "011118", "社保単独・高齢" },
        { "011110", "社保単独・高齢" },

        { "011122", "社保２併・本人" },
        { "011124", "社保２併・６未" },
        { "011126", "社保２併・家族" },
        { "011128", "社保２併・高齢" },
        { "011120", "社保２併・高齢" },

        { "011132", "社保３併・本人" },
        { "011134", "社保３併・６未" },
        { "011136", "社保３併・家族" },
        { "011138", "社保３併・高齢" },
        { "011130", "社保３併・高齢" },

        { "011142", "社保４併・本人" },
        { "011144", "社保４併・６未" },
        { "011146", "社保４併・家族" },
        { "011148", "社保４併・高齢" },
        { "011140", "社保４併・高齢" },

        { "011152", "社保５併・本人" },
        { "011154", "社保５併・６未" },
        { "011156", "社保５併・家族" },
        { "011158", "社保５併・高齢" },
        { "011150", "社保５併・高齢" },

        { "011210", "公費単独" },
        { "011220", "公費２併" },
        { "011230", "公費３併" },
        { "011240", "公費４併" },
        { "011250", "公費５併" },

        { "021112", "国保単独・本人" },
        { "021114", "国保単独・６未" },
        { "021116", "国保単独・家族" },
        { "021118", "国保単独・高齢" },
        { "021110", "国保単独・高齢" },

        { "021122", "国保２併・本人" },
        { "021124", "国保２併・６未" },
        { "021126", "国保２併・家族" },
        { "021128", "国保２併・高齢" },
        { "021120", "国保２併・高齢" },

        { "021132", "国保３併・本人" },
        { "021134", "国保３併・６未" },
        { "021136", "国保３併・家族" },
        { "021138", "国保３併・高齢" },
        { "021130", "国保３併・高齢" },

        { "021142", "国保４併・本人" },
        { "021144", "国保４併・６未" },
        { "021146", "国保４併・家族" },
        { "021148", "国保４併・高齢" },
        { "021140", "国保４併・高齢" },

        { "021152", "国保５併・本人" },
        { "021154", "国保５併・６未" },
        { "021156", "国保５併・家族" },
        { "021158", "国保５併・高齢" },
        { "021150", "国保５併・高齢" },

        { "021310", "後期単独" },
        { "021320", "後期２併" },
        { "021330", "後期３併" },
        { "021340", "後期４併" },
        { "021350", "後期５併" },

        { "021412", "退職単独・本人" },
        { "021414", "退職単独・６未" },
        { "021416", "退職単独・家族" },
        { "021418", "退職単独・高齢" },
        { "021410", "退職単独・高齢" },

        { "021422", "退職２併・本人" },
        { "021424", "退職２併・６未" },
        { "021426", "退職２併・家族" },
        { "021428", "退職２併・高齢" },
        { "021420", "退職２併・高齢" },

        { "021432", "退職３併・本人" },
        { "021434", "退職３併・６未" },
        { "021436", "退職３併・家族" },
        { "021438", "退職３併・高齢" },
        { "021430", "退職３併・高齢" },

        { "021442", "退職４併・本人" },
        { "021444", "退職４併・６未" },
        { "021446", "退職４併・家族" },
        { "021448", "退職４併・高齢" },
        { "021440", "退職４併・高齢" },

        { "021452", "退職５併・本人" },
        { "021454", "退職５併・６未" },
        { "021456", "退職５併・家族" },
        { "021458", "退職５併・高齢" },
        { "021450", "退職５併・高齢" },

        { "110000", "労災（短期給付）" },
        { "120000", "労災（傷病年金）" },
        { "130000", "労災（アフターケア）" },
        { "140000", "自賠責" },
    }.ToImmutableDictionary();
}
