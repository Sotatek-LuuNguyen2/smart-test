﻿using Amazon;

namespace AWSSDK.Constants
{
    public static class ConfigConstant
    {
        public static string HostedZoneId = "Z09462362PXK5JFYQ59B";
        public static string Domain = "smartkarte.org";
        public static string DistributionId = "E1Q6ZVLBFAFBDX";
        public static string DedicateInstance = "db.m6g.large";
        public static string SharingInstance = "db.m6g.xlarge";
        public static int TimeoutCheckingAvailable = 15;
        public static int TypeSharing = 0;
        public static int TypeDedicate = 1;
        public static int SizeTypeMB = 1;
        public static int SizeTypeGB = 2;
        public static byte StatusNotiSuccess = 1;
        public static byte StatusNotifailure = 0;
        public static byte StatusNotiInfo = 2;
        public static List<string> LISTSYSTEMDB = new List<string>() { "rdsadmin, postgres" };
        public static string RdsSnapshotBackupTermiante = "Bak-Termiante";
        public static string RdsSnapshotUpdate = "Update";
        public static string RdsSnapshotBackupRestore = "Bak-Restore";
        public static string ManagedCachingOptimized = "658327ea-f89d-4fab-a63d-7e88639e58f6";
        public static int PgPostDefault = 5432;
        public static string PgUserDefault = "postgres";
        public static string PgPasswordDefault = "Emr!23456789";

        public static string DestinationBucketName = "phuc-test-s3";
        public static string RestoreBucketName = "phuc-test-s3";
        public static string SourceBucketName = "phuc-test-s3-replication";
        public static RegionEndpoint RegionDestination = RegionEndpoint.GetBySystemName("ap-northeast-1");
        public static RegionEndpoint RegionSource = RegionEndpoint.GetBySystemName("ap-southeast-1");

        public static byte StatusTenantPending = 1;
        public static byte StatusTenantStopping = 4;
        public static byte StatusTenantRunning = 3;
        public static byte StatusTenantFailded = 2;
        public static byte StatusTenantTeminated = 7;
        public static byte StatusTenantStopped = 5;
        public static byte StatusSuttingDown = 6;
        public static Dictionary<string, byte> StatusTenantDictionary()
        {
            Dictionary<string, byte> rdsStatusDictionary = new Dictionary<string, byte>
        {
            {"available", 1},
            {"creating", 2},
            {"modifying", 3},
            {"deleting", 4},
            {"backing-up", 5},
            {"updating", 6},
            {"failed", 7},
            {"inaccessible-encryption-credentials",8},
            {"storage-full", 9},
            {"update-failed", 10},
            {"terminating", 11},
            {"terminated", 12},
            {"terminate-failed", 13},
            {"stoped", 14},
            {"restoring", 15},
            {"restore-failed", 16},
            {"stopping", 17},
            {"starting", 18},
            {"update-schema", 19},
            {"update-schema-failed", 20}
        };

            return rdsStatusDictionary;
        }

        public static readonly List<string> listTableMaster = new List<string>()
        {
            "accounting_form_mst",
            "approval_inf",
            "audit_trail_log",
            "auto_santei_mst",
            "bui_odr_byomei_mst",
            "bui_odr_item_byomei_mst",
            "bui_odr_item_mst",
            "bui_odr_mst",
            "column_setting",
            "json_setting",
            "byomei_mst",
            "byomei_mst_aftercare",
            "byomei_set_generation_mst",
            "byomei_set_mst",
            "calc_log",
            "calc_status",
            "cmt_check_mst",
            "cmt_kbn_mst",
            "container_mst",
            "conversion_item_inf",
            "def_hoken_no",
            "densi_haihan_custom",
            "densi_haihan_day",
            "densi_haihan_karte",
            "densi_haihan_month",
            "densi_haihan_week",
            "densi_hojyo",
            "densi_houkatu",
            "densi_houkatu_grp",
            "densi_santei_kaisu",
            "doc_category_mst",
            "doc_inf",
            "dosage_mst",
            "drug_day_limit",
            "drug_inf",
            "drug_unit_conv",
            "eps_chk",
            "eps_chk_detail",
            "eps_prescription",
            "eps_reference",
            "event_mst",
            "except_hokensya",
            "filing_auto_imp",
            "filing_category_mst",
            "filing_inf",
            "function_mst",
            "hoken_mst",
            "hokensya_mst",
            "holiday_mst",
            "hp_inf",
            "item_grp_mst",
            "jihi_sbt_mst",
            "job_mst",
            "ka_mst",
            "kacode_mst",
            "kacode_rece_yousiki",
            "kacode_yousiki_mst",
            "kaikei_detail",
            "kaikei_inf",
            "kantoku_mst",
            "karte_filter_detail",
            "karte_filter_mst",
            "karte_img_inf",
            "karte_inf",
            "karte_kbn_mst",
            "kensa_center_mst",
            "kensa_cmt_mst",
            "kensa_inf",
            "kensa_inf_detail",
            "kensa_irai_log",
            "kensa_mst",
            "kensa_result_log",
            "kensa_set",
            "kensa_set_detail",
            "kensa_std_mst",
            "kinki_mst",
            "kohi_priority",
            "koui_houkatu_mst",
            "limit_cnt_list_inf",
            "limit_list_inf",
            "list_set_generation_mst",
            "list_set_mst",
            "lock_inf",
            "m01_kijyo_cmt",
            "m01_kinki",
            "m01_kinki_cmt",
            "m10_day_limit",
            "m12_food_alrgy",
            "m12_food_alrgy_kbn",
            "m14_age_check",
            "m14_cmt_code",
            "m28_drug_mst",
            "m34_ar_code",
            "m34_ar_discon",
            "m34_ar_discon_code",
            "m34_drug_info_main",
            "m34_form_code",
            "m34_indication_code",
            "m34_interaction_pat",
            "m34_interaction_pat_code",
            "m34_precaution_code",
            "m34_precautions",
            "m34_property_code",
            "m34_sar_symptom_code",
            "m38_class_code",
            "m38_ing_code",
            "m38_ingredients",
            "m38_major_div_code",
            "m38_otc_form_code",
            "m38_otc_main",
            "m38_otc_maker_code",
            "m41_supple_indexcode",
            "m41_supple_indexdef",
            "m41_supple_ingre",
            "m42_contra_cmt",
            "m42_contraindi_dis_bc",
            "m42_contraindi_dis_class",
            "m42_contraindi_dis_con",
            "m42_contraindi_drug_main_ex",
            "m46_dosage_dosage",
            "m46_dosage_drug",
            "m56_alrgy_derivatives",
            "m56_analogue_cd",
            "m56_drug_class",
            "m56_drvalrgy_code",
            "m56_ex_analogue",
            "m56_ex_ed_ingredients",
            "m56_ex_ing_code",
            "m56_ex_ingrdt_main",
            "m56_prodrug_cd",
            "m56_usage_code",
            "m56_yj_drug_class",
            "mall_renkei_inf",
            "material_mst",
            "monshin_inf",
            "odr_date_detail",
            "odr_date_inf",
            "odr_inf",
            "odr_inf_cmt",
            "odr_inf_detail",
            "path_conf",
            "payment_method_mst",
            "permission_mst",
            "physical_average",
            "pi_inf",
            "pi_inf_detail",
            "pi_product_inf",
            "post_code_mst",
            "priority_haihan_mst",
            "pt_alrgy_drug",
            "pt_alrgy_else",
            "pt_alrgy_food",
            "pt_byomei",
            "pt_cmt_inf",
            "pt_family",
            "pt_family_reki",
            "pt_grp_inf",
            "pt_grp_item",
            "pt_grp_name_mst",
            "pt_hoken_check",
            "pt_hoken_inf",
            "pt_hoken_pattern",
            "pt_hoken_scan",
            "pt_inf",
            "pt_infection",
            "pt_jibai_doc",
            "pt_kio_reki",
            "pt_kohi",
            "pt_kyusei",
            "pt_last_visit_date",
            "pt_memo",
            "pt_otc_drug",
            "pt_other_drug",
            "pt_pregnancy",
            "pt_rousai_tenki",
            "pt_santei_conf",
            "pt_supple",
            "pt_tag",
            "raiin_cmt_inf",
            "raiin_filter_kbn",
            "raiin_filter_mst",
            "raiin_filter_sort",
            "raiin_filter_state",
            "raiin_inf",
            "raiin_kbn_detail",
            "raiin_kbn_inf",
            "raiin_kbn_item",
            "raiin_kbn_koui",
            "raiin_kbn_mst",
            "raiin_kbn_yoyaku",
            "raiin_list_cmt",
            "raiin_list_detail",
            "raiin_list_doc",
            "raiin_list_file",
            "raiin_list_inf",
            "raiin_list_item",
            "raiin_list_koui",
            "raiin_list_mst",
            "raiin_list_tag",
            "rece_check_cmt",
            "rece_check_err",
            "rece_check_opt",
            "rece_cmt",
            "rece_futan_kbn",
            "rece_inf",
            "rece_inf_edit",
            "rece_inf_jd",
            "rece_inf_pre_edit",
            "rece_seikyu",
            "rece_status",
            "receden_cmt_select",
            "receden_hen_jiyuu",
            "receden_rireki_inf",
            "releasenote_read",
            "renkei_conf",
            "renkei_mst",
            "renkei_path_conf",
            "renkei_req",
            "renkei_template_mst",
            "renkei_timing_conf",
            "renkei_timing_mst",
            "roudou_mst",
            "rsv_day_comment",
            "rsv_frame_day_ptn",
            "rsv_frame_inf",
            "rsv_frame_mst",
            "rsv_frame_week_ptn",
            "rsv_frame_with",
            "rsv_grp_mst",
            "rsv_inf",
            "rsv_renkei_inf",
            "rsv_renkei_inf_tk",
            "rsvkrt_byomei",
            "rsvkrt_karte_img_inf",
            "rsvkrt_karte_inf",
            "rsvkrt_mst",
            "rsvkrt_odr_inf",
            "rsvkrt_odr_inf_cmt",
            "rsvkrt_odr_inf_detail",
            "santei_auto_order",
            "santei_auto_order_detail",
            "santei_cnt_check",
            "santei_grp_detail",
            "santei_grp_mst",
            "santei_inf",
            "santei_inf_detail",
            "schema_cmt_mst",
            "seikatureki_inf",
            "sentence_list",
            "session_inf",
            "set_byomei",
            "set_generation_mst",
            "set_karte_img_inf",
            "set_karte_inf",
            "set_kbn_mst",
            "set_mst",
            "set_odr_inf",
            "set_odr_inf_cmt",
            "set_odr_inf_detail",
            "sin_koui",
            "sin_koui_count",
            "sin_koui_detail",
            "sin_rp_inf",
            "sin_rp_no_inf",
            "single_dose_mst",
            "sinreki_filter_mst",
            "sinreki_filter_mst_detail",
            "sinreki_filter_mst_koui",
            "sta_conf",
            "sta_csv",
            "sta_menu",
            "summary_inf",
            "syobyo_keika",
            "syouki_inf",
            "syouki_kbn_mst",
            "system_conf",
            "system_conf_item",
            "system_conf_menu",
            "system_generation_conf",
            "syuno_nyukin",
            "syuno_seikyu",
            "tag_grp_mst",
            "tekiou_byomei_mst",
            "tekiou_byomei_mst_excluded",
            "template_detail",
            "template_dsp_conf",
            "template_menu_detail",
            "template_menu_mst",
            "template_mst",
            "ten_mst",
            "ten_mst_mother",
            "ten_mst_temp",
            "time_zone_conf",
            "time_zone_day_inf",
            "todo_grp_mst",
            "todo_inf",
            "todo_kbn_mst",
            "uketuke_sbt_day_inf",
            "uketuke_sbt_mst",
            "unit_mst",
            "user_conf",
            "user_mst",
            "user_permission",
            "wrk_sin_koui",
            "wrk_sin_koui_detail",
            "wrk_sin_koui_detail_del",
            "wrk_sin_rp_inf",
            "yoho_hosoku",
            "yoho_inf_mst",
            "yoho_mst",
            "yoho_set_mst",
            "yousiki1_inf",
            "yousiki1_inf_detail",
            "pt_jibkar",
            "z_doc_inf",
            "z_filing_inf",
            "z_filing_inf_temp",
            "z_kensa_inf",
            "z_kensa_inf_detail",
            "z_kensa_inf_detail_temp",
            "z_kensa_inf_temp",
            "z_limit_cnt_list_inf",
            "z_limit_list_inf",
            "z_monshin_inf",
            "z_monshin_inf_temp",
            "z_pt_alrgy_drug",
            "z_pt_alrgy_else",
            "z_pt_alrgy_food",
            "z_pt_cmt_inf",
            "z_pt_family",
            "z_pt_family_reki",
            "z_pt_grp_inf",
            "z_pt_hoken_check",
            "z_pt_hoken_inf",
            "z_pt_hoken_pattern",
            "z_pt_hoken_scan",
            "z_pt_inf",
            "z_pt_infection",
            "z_pt_kio_reki",
            "z_pt_kohi",
            "z_pt_kyusei",
            "z_pt_memo",
            "z_pt_otc_drug",
            "z_pt_other_drug",
            "z_pt_pregnancy",
            "z_pt_rousai_tenki",
            "z_pt_santei_conf",
            "z_pt_supple",
            "z_pt_tag",
            "z_raiin_cmt_inf",
            "z_raiin_cmt_inf_temp",
            "z_raiin_inf",
            "z_raiin_kbn_inf",
            "z_raiin_list_cmt",
            "z_raiin_list_cmt_temp",
            "z_raiin_list_tag",
            "z_rece_check_cmt",
            "z_rece_cmt",
            "z_rece_inf_edit",
            "z_rece_seikyu",
            "z_rece_status",
            "z_rsv_day_comment",
            "z_rsv_inf",
            "z_santei_inf_detail",
            "z_seikatureki_inf",
            "z_summary_inf",
            "z_syobyo_keika",
            "z_syouki_inf",
            "z_syuno_nyukin",
            "z_syuno_nyukin_temp",
            "z_todo_inf",
            "z_uketuke_sbt_day_inf",
            "z_yousiki1_inf",
            "z_yousiki1_inf_detail",
            "z_pt_jibkar"
        };
    }
}
