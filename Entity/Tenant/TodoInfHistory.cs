using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "todo_inf_history")]
    public class TodoInfHistory : EmrCloneable<TodoInf>
    {
        /// <summary>
        /// 履歴番号
        ///     変更していく旅に増えていく
        /// </summary>
        
        [Column(name: "revision", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Revision { get; set; }

        /// <summary>
        /// 医療機関識別ID 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// TODO番号 
        /// </summary>
        
        [Column("todo_no", Order = 3)]
        public int TodoNo { get; set; }

        /// <summary>
        /// TODO枝番 
        /// </summary>
        
        [Column("todo_eda_no", Order = 4)]
        public int TodoEdaNo { get; set; }

        /// <summary>
        /// 患者ID 
        /// </summary>
        
        [Column("pt_id", Order = 5)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日 
        /// </summary>
        [Column("sin_date")]
        public int SinDate { get; set; }

        /// <summary>
        /// 予約番号 
        /// </summary>
        [Column("raiin_no")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// TODO区分番号 
        /// </summary>
        [Column("todo_kbn_no")]
        public int TodoKbnNo { get; set; }

        /// <summary>
        /// TODO分類番号 
        /// </summary>
        [Column("todo_grp_no")]
        public int TodoGrpNo { get; set; }

        /// <summary>
        /// 担当 
        /// </summary>
        [Column("tanto")]
        [CustomAttribute.DefaultValue(0)]
        public int Tanto { get; set; }

        /// <summary>
        /// 期限 
        /// </summary>
        [Column("term")]
        public int Term { get; set; }

        /// <summary>
        /// コメント１ 
        /// </summary>
        [Column("cmt1")]
        public string? Cmt1 { get; set; } = string.Empty;

        /// <summary>
        /// コメント２ 
        /// </summary>
        [Column("cmt2")]
        public string? Cmt2 { get; set; } = string.Empty;

        /// <summary>
        /// 済
        /// 1: 済み
        /// </summary>
        [Column("is_done")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDone { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 1: 削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;

        /// <summary>
        /// Update type: 
        /// Insert: 挿入
        /// Update: 更新
        /// Delete: 削除
        /// </summary>
        [Column(name: "update_type")]
        [MaxLength(6)]
        public string? UpdateType { get; set; } = string.Empty;
    }
}
