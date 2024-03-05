using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Serializable()]
    /// <summary>
    /// 患者病名
    /// </summary>
    [Table("pt_byomei")]
    public class PtByomei : EmrCloneable<PtByomei>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 基本病名コード
        ///     コードを使用しない場合、「0000999」をセット
        /// </summary>
        [Column("byomei_cd")]
        [MaxLength(7)]
        public string? ByomeiCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        ///     患者の病名を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("id", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 修飾語コード１
        /// </summary>
        [Column("syusyoku_cd1")]
        [MaxLength(7)]
        public string? SyusyokuCd1 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード２
        /// </summary>
        [Column("syusyoku_cd2")]
        [MaxLength(7)]
        public string? SyusyokuCd2 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード３
        /// </summary>
        [Column("syusyoku_cd3")]
        [MaxLength(7)]
        public string? SyusyokuCd3 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード４
        /// </summary>
        [Column("syusyoku_cd4")]
        [MaxLength(7)]
        public string? SyusyokuCd4 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード５
        /// </summary>
        [Column("syusyoku_cd5")]
        [MaxLength(7)]
        public string? SyusyokuCd5 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード６
        /// </summary>
        [Column("syusyoku_cd6")]
        [MaxLength(7)]
        public string? SyusyokuCd6 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード７
        /// </summary>
        [Column("syusyoku_cd7")]
        [MaxLength(7)]
        public string? SyusyokuCd7 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード８
        /// </summary>
        [Column("syusyoku_cd8")]
        [MaxLength(7)]
        public string? SyusyokuCd8 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード９
        /// </summary>
        [Column("syusyoku_cd9")]
        [MaxLength(7)]
        public string? SyusyokuCd9 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード１０
        /// </summary>
        [Column("syusyoku_cd10")]
        [MaxLength(7)]
        public string? SyusyokuCd10 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード１１
        /// </summary>
        [Column("syusyoku_cd11")]
        [MaxLength(7)]
        public string? SyusyokuCd11 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード１２
        /// </summary>
        [Column("syusyoku_cd12")]
        [MaxLength(7)]
        public string? SyusyokuCd12 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード１３
        /// </summary>
        [Column("syusyoku_cd13")]
        [MaxLength(7)]
        public string? SyusyokuCd13 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード１４
        /// </summary>
        [Column("syusyoku_cd14")]
        [MaxLength(7)]
        public string? SyusyokuCd14 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード１５
        /// </summary>
        [Column("syusyoku_cd15")]
        [MaxLength(7)]
        public string? SyusyokuCd15 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード１６
        /// </summary>
        [Column("syusyoku_cd16")]
        [MaxLength(7)]
        public string? SyusyokuCd16 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード１７
        /// </summary>
        [Column("syusyoku_cd17")]
        [MaxLength(7)]
        public string? SyusyokuCd17 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード１８
        /// </summary>
        [Column("syusyoku_cd18")]
        [MaxLength(7)]
        public string? SyusyokuCd18 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード１９
        /// </summary>
        [Column("syusyoku_cd19")]
        [MaxLength(7)]
        public string? SyusyokuCd19 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード２０
        /// </summary>
        [Column("syusyoku_cd20")]
        [MaxLength(7)]
        public string? SyusyokuCd20 { get; set; } = string.Empty;

        /// <summary>
        /// 修飾語コード２１
        /// </summary>
        [Column("syusyoku_cd21")]
        [MaxLength(7)]
        public string? SyusyokuCd21 { get; set; } = string.Empty;

        /// <summary>
        /// 病名
        /// </summary>
        [Column("byomei")]
        [MaxLength(160)]
        public string? Byomei { get; set; } = string.Empty;

        /// <summary>
        /// 開始日
        /// </summary>
        [Column("start_date")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 転帰区分
        /// 転帰区分を表す。
        ///      0: 下記以外
        ///      1: 治ゆ
        ///      2: 中止
        ///      3: 死亡
        ///      9: その他
        /// </summary>
        [Column("tenki_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int TenkiKbn { get; set; }

        /// <summary>
        /// 転帰日
        /// </summary>
        [Column("tenki_date")]
        [CustomAttribute.DefaultValue(0)]
        public int TenkiDate { get; set; }

        /// <summary>
        /// 主病名区分
        ///     0: 主病名以外
        ///     1: 主病名
        /// </summary>
        [Column("syubyo_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SyubyoKbn { get; set; }

        /// <summary>
        /// 慢性疾患区分
        ///     特定疾患療養指導料等の算定対象であるか否かを表す
        ///     00: 対象外
        ///     03: 皮膚科特定疾患指導管理料（１）算定対象
        ///     04: 皮膚科特定疾患指導管理料（２）算定対象
        ///     05: 特定疾患療養指導料／老人慢性疾患生活指導料算定対象
        ///     07: てんかん指導料算定対象 
        ///     08: 特定疾患療養管理料又はてんかん指導料算
        ///     定対象 
        /// </summary>
        [Column("sikkan_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SikkanKbn { get; set; }

        /// <summary>
        /// 難病外来コード
        ///     当該傷病名が難病外来指導管理料の算定対象であるか否かを表す。
        ///     00: 算定対象外
        ///     09: 難病外来指導管理料算定対象
        /// </summary>
        [Column("nanbyo_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int NanByoCd { get; set; }

        /// <summary>
        /// 補足コメント
        /// </summary>
        [Column("hosoku_cmt")]
        [MaxLength(80)]
        public string? HosokuCmt { get; set; } = string.Empty;

        /// <summary>
        /// 保険組み合わせ番号
        ///     0: 共通病名
        /// </summary>
        [Column("hoken_pid")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 当月病名区分
        ///     1: 当月病名
        /// </summary>
        [Column("togetu_byomei")]
        [CustomAttribute.DefaultValue(0)]
        public int TogetuByomei { get; set; }

        /// <summary>
        /// レセプト非表示区分
        ///     1: 非表示
        /// </summary>
        [Column("is_nodsp_rece")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRece { get; set; }

        /// <summary>
        /// カルテ非表示区分
        ///     1: 非表示
        /// </summary>
        [Column("is_nodsp_karte")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspKarte { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末	
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時	
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末	
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;

        [Column("seq_no")]
        [CustomAttribute.DefaultValue(0)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 重要
        ///     1:重要
        /// </summary>
        [Column("is_important")]
        [CustomAttribute.DefaultValue(0)]
        public int IsImportant { get; set; }
    }
}