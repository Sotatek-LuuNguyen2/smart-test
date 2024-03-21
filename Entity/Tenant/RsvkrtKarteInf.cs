using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rsvkrt_karte_inf")]
    public class RsvkrtKarteInf : EmrCloneable<RsvkrtKarteInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 予約日
        /// yyyymmdd
        /// </summary>
        [Column("rsv_date")]
        public int RsvDate { get; set; }

        /// <summary>
        /// 予約カルテ番号
        /// 
        /// </summary>
        
        [Column("rsvkrt_no", Order = 3)]
        public long RsvkrtNo { get; set; }

        /// <summary>
        /// カルテ区分
        /// KARTE_KBN_MST.KARTE_KBN
        /// </summary>
        
        [Column("karte_kbn", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public long SeqNo { get; set; }

        /// <summary>
        /// テキスト
        /// 
        /// </summary>
        [Column("text")]
        public string? Text { get; set; } = string.Empty;

        /// <summary>
        /// リッチテキスト
        /// 
        /// </summary>
        [Column("rich_text")]
        public byte[]? RichText { get; set; } = default!;

        /// <summary>
        /// 削除区分
        /// "1: 削除
        /// 2: 実施"
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}