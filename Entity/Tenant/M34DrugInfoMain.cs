﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_DRUG_INFO_MAIN")]
    public class M34DrugInfoMain : EmrCloneable<M34DrugInfoMain>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        
        [Column("YJ_CD", Order = 1)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 剤形コード
        /// 
        /// </summary>
        [Column("FORM_CD")]
        public string? FormCd { get; set; } = string.Empty;

        /// <summary>
        /// 色調
        /// 
        /// </summary>
        [Column("COLOR")]
        [MaxLength(20)]
        public string? Color { get; set; } = string.Empty;

        /// <summary>
        /// 本体記号
        /// 
        /// </summary>
        [Column("MARK")]
        [MaxLength(20)]
        public string? Mark { get; set; } = string.Empty;

        /// <summary>
        /// 効能効果コード
        /// 
        /// </summary>
        [Column("KONO_CD")]
        public string? KonoCd { get; set; } = string.Empty;

        /// <summary>
        /// 副作用コード
        /// 
        /// </summary>
        [Column("FUKUSAYO_CD")]
        public string? FukusayoCd { get; set; } = string.Empty;

        /// <summary>
        /// 副作用初期症状コード
        /// 
        /// </summary>
        [Column("FUKUSAYO_INIT_CD")]
        public string? FukusayoInitCd { get; set; } = string.Empty;

    }
}
