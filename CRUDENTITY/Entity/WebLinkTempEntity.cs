using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Entity
{
    [Table("web_link_temp")]
    public class WebLinkTempEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int link_temp_id { get; set; }

        public int lang_id { get; set; }

        public int type_id { get; set; }

        public string? link_name { get; set; }

        public string? link_alias { get; set; }

        public string? title { get; set; }

        public string? link_bdesc { get; set; }

        public string? keywords { get; set; }

        public string? meta_tag { get; set; }

        public string? source { get; set; }

        public short? cont_show { get; set; }

        public short? feed_req { get; set; }

        public string? file_name { get; set; }

        public string? url { get; set; }

        public string? details { get; set; }   // nvarchar

        public int? creator_id { get; set; }

        public DateTime? creation_date { get; set; }

        public short? app_reject { get; set; }

        public short? app_rej_user_id { get; set; }

        public DateTime? app_rej_action_on { get; set; }

        public int lid { get; set; }

        public int? entry_type { get; set; }

        public string? current_status { get; set; }

        public int? continuous_content { get; set; }

        public int? main_link_temp_id { get; set; }

        public string? author_name { get; set; }

        public DateTime? pub_date { get; set; }

        public int? publish_by { get; set; }

        public DateTime? publish_on { get; set; }

        public DateTime? publish_date { get; set; }

        public DateTime? expiry_date { get; set; }

        public DateTime? review_date { get; set; }

        public int? pos { get; set; }

        public short? revive { get; set; }

        public DateTime? revive_on { get; set; }

        public int? revive_by { get; set; }

        public string? revive_details { get; set; } // nvarchar

        public DateTime? new_icon_date { get; set; }

        public string? status { get; set; }

        public int? entry_by { get; set; }

        public DateTime? entry_date { get; set; }

        public string? ip_addr { get; set; }

        public short? l_sub_type { get; set; }

        public short? content_type { get; set; }

        public string? event_name { get; set; }

        public DateTime? evef_date { get; set; }

        public DateTime? evet_date { get; set; }

        public int? eng_id { get; set; }

        public string? header_img { get; set; }
    }
}
