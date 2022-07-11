﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RSVKRT_ODR_INF_DETAIL")]
    public class RsvkrtOdrInfDetail : EmrCloneable<RsvkrtOdrInfDetail>
    {
        private int _hpId;
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }


        private long _ptId;
        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        private long _rsvkrtNo;
        /// <summary>
        /// 予約カルテ番号
        /// 
        /// </summary>
        //[Key]
        [Column("RSVKRT_NO", Order = 3)]
        public long RsvkrtNo { get; set; }

        private long _rpNo;
        /// <summary>
        /// 剤番号
        /// ODR_INF.RP_NO
        /// </summary>
        //[Key]
        [Column("RP_NO", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public long RpNo { get; set; }

        private long _rpEdaNo;
        /// <summary>
        /// 剤枝番
        /// 
        /// </summary>
        //[Key]
        [Column("RP_EDA_NO", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public long RpEdaNo { get; set; }

        private int _rowNo;
        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        //[Key]
        [Column("ROW_NO", Order = 6)]
        [CustomAttribute.DefaultValue(1)]
        public int RowNo { get; set; }

        private int _rsvDate;
        /// <summary>
        /// 予約日
        /// yyyymmdd
        /// </summary>
        [Column("RSV_DATE")]
        public int RsvDate { get; set; }

        private int _sinKouiKbn;
        /// <summary>
        /// 診療行為区分
        /// TEN_MST.SIN_KOUI_KBN
        /// </summary>
        [Column("SIN_KOUI_KBN")]
        public int SinKouiKbn { get; set; }

        private string _itemCd;
        /// <summary>
        /// 項目コード
        /// TEN_MST.ITEM_CD
        /// </summary>
        [Column("ITEM_CD")]
        [MaxLength(10)]
        public string ItemCd { get; set; }

        private string _itemName;
        /// <summary>
        /// 項目名称
        /// 
        /// </summary>
        [Column("ITEM_NAME")]
        [MaxLength(240)]
        public string ItemName { get; set; }

        private double _suryo;
        /// <summary>
        /// 数量
        /// 
        /// </summary>
        [Column("SURYO")]
        [CustomAttribute.DefaultValue(0)]
        public double Suryo { get; set; }

        private string _unitName;
        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        [Column("UNIT_NAME")]
        [MaxLength(24)]
        public string UnitName { get; set; }

        private int _unitSbt;
        /// <summary>
        /// 単位種別
        /// "0: TEN_MST.単位
        /// 1: TEN_MST.数量換算単位"
        /// </summary>
        [Column("UNIT_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int UnitSbt { get; set; }

        private double _termVal;
        /// <summary>
        /// 単位換算値
        /// "UNIT_SBT=0 -> TEN_MST.ODR_TERM_VAL
        /// UNIT_SBT=1 -> TEN_MST.SURYO_TERM_VAL"
        /// </summary>
        [Column("TERM_VAL")]
        [CustomAttribute.DefaultValue(0)]
        public double TermVal { get; set; }

        private int _kohatuKbn;
        /// <summary>
        /// 後発医薬品区分
        /// "当該医薬品が後発医薬品に該当するか否かを表す。
        /// 　0: 後発医薬品のない先発医薬品
        /// 　1: 先発医薬品がある後発医薬品である
        /// 　2: 後発医薬品がある先発医薬品である
        /// 　7: 先発医薬品のない後発医薬品である"
        /// </summary>
        [Column("KOHATU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KohatuKbn { get; set; }

        private int _syohoKbn;
        /// <summary>
        /// 処方せん記載区分
        /// "0: 指示なし（後発品のない先発品）
        /// 1: 変更不可
        /// 2: 後発品（他銘柄）への変更可 
        /// 3: 一般名処方"
        /// </summary>
        [Column("SYOHO_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SyohoKbn { get; set; }

        private int _syohoLimitKbn;
        /// <summary>
        /// 処方せん記載制限区分
        /// "0: 制限なし
        /// 1: 剤形不可
        /// 2: 含量規格不可
        /// 3: 含量規格・剤形不可"
        /// </summary>
        [Column("SYOHO_LIMIT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SyohoLimitKbn { get; set; }

        private int _drugKbn;
        /// <summary>
        /// 薬剤区分
        /// "当該医薬品の薬剤区分を表す。
        ///  0: 薬剤以外
        /// 　1: 内用薬
        /// 　3: その他
        /// 　4: 注射薬
        /// 　6: 外用薬
        /// 　8: 歯科用薬剤"
        /// </summary>
        [Column("DRUG_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int DrugKbn { get; set; }

        private int _yohoKbn;
        /// <summary>
        /// 用法区分
        /// "0: 用法以外
        /// 1: 基本用法
        /// 2: 補助用法"
        /// </summary>
        [Column("YOHO_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int YohoKbn { get; set; }

        private string _kokuji1;
        /// <summary>
        /// 告示等識別区分（１）
        /// "当該診療行為についてコンピューター運用上の取扱い（磁気媒体に記録する際の取扱い）を表す。
        /// 　1: 基本項目（告示）　※基本項目
        /// 　3: 合成項目　　　　　※基本項目
        /// 　5: 準用項目（通知）　※基本項目
        /// 　7: 加算項目　　　　　※加算項目
        /// 　9: 通則加算項目　　　※加算項目
        ///  0: 診療行為以外（薬剤、特材等）"
        /// </summary>
        [Column("KOKUJI1")]
        [MaxLength(1)]
        public string Kokuji1 { get; set; }

        private string _kokuji2;
        /// <summary>
        /// 告示等識別区分（２）
        /// "当該診療行為について点数表上の取扱いを表す。
        /// 　1: 基本項目（告示）
        /// 　3: 合成項目
        /// （削）5: 準用項目（通知）
        /// 　7: 加算項目（告示）
        /// （削）9: 通則加算項目
        ///  0: 診療行為以外（薬剤、特材等）"
        /// </summary>
        [Column("KOKUJI2")]
        [MaxLength(1)]
        public string Kokuji2 { get; set; }

        private int _isNodspRece;
        /// <summary>
        /// レセ非表示区分
        /// "0: 表示
        /// 1: 非表示"
        /// </summary>
        [Column("IS_NODSP_RECE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRece { get; set; }

        private string _ipnCd;
        /// <summary>
        /// 一般名コード
        /// 
        /// </summary>
        [Column("IPN_CD")]
        [MaxLength(12)]
        public string IpnCd { get; set; }

        private string _ipnName;
        /// <summary>
        /// 一般名
        /// 
        /// </summary>
        [Column("IPN_NAME")]
        [MaxLength(120)]
        public string IpnName { get; set; }

        private string _bunkatu;
        /// <summary>
        /// 分割調剤
        /// 7日単位の3分割の場合 "7+7+7"
        /// </summary>
        [Column("BUNKATU")]
        [MaxLength(10)]
        public string Bunkatu { get; set; }

        private string _cmtName;
        /// <summary>
        /// コメント名称
        /// "コメントマスターの名称
        /// ※当該項目がコメント項目の場合に使用"
        /// </summary>
        [Column("CMT_NAME")]
        [MaxLength(240)]
        public string CmtName { get; set; }

        private string _cmtOpt;
        /// <summary>
        /// コメント文
        /// "コメントマスターの定型文に組み合わせる文字情報
        /// ※当該項目がコメント項目の場合に使用"
        /// </summary>
        [Column("CMT_OPT")]
        [MaxLength(38)]
        public string CmtOpt { get; set; }

        private string _fontColor;
        /// <summary>
        /// 文字色
        /// 
        /// </summary>
        [Column("FONT_COLOR")]
        [MaxLength(8)]
        public string FontColor { get; set; }

        private int _commentNewline;
        /// <summary>
        /// コメント改行区分
        ///          0: 改行する
        ///          1: 改行しない
        /// </summary>
        [Column("COMMENT_NEWLINE")]
        [CustomAttribute.DefaultValue(0)]
        public int CommentNewline { get; set; }
    }
}
