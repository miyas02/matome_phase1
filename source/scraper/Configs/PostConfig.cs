using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    public class PostConfig : AbstractScraperConfig {
        public override string ToString() {
            return JsonSerializer.Serialize(this);
        }
        [JsonPropertyName("LIST_NODE")]
        public string? LIST_NODE { get; set; }

        [JsonPropertyName("POST_NODE")]
        public string? POST_NODE { get; set; }

        [JsonPropertyName("USER_ID")]
        public USERID? USER_ID { get; set; }

        [JsonPropertyName("TEXT")]
        public TEXT? TEXT { get; set; }

        [JsonPropertyName("DATE")]
        public DATE? DATE { get; set; }

        [JsonPropertyName("REPLY")]
        public REPLY? REPLY { get; set; }

        [JsonPropertyName("IMAGE")]
        public IMAGE? IMAGE { get; set; }

        [JsonPropertyName("POST_ID")]
        public POSTID? POST_ID { get; set; }
    }
    public class DATE : ConfigNodeBase {

    }

    public class IMAGE : ConfigNodeBase {

    }

    public class NAVIGATEPAGE {
        [JsonPropertyName("TYPE")]
        public NavigatePageTypes TYPE { get; set; }

        [JsonPropertyName("TARGET_LINK")]
        public TARGETLINK? TARGET_LINK { get; set; }

        [JsonPropertyName("PAGINATION")]
        public PAGINATION? PAGINATION { get; set; }
    }

    public class PAGINATION {
        [JsonPropertyName("NODE")]
        public string? NODE { get; set; }

        [JsonPropertyName("ACTION")]
        public string? ACTION { get; set; }

        [JsonPropertyName("MAX_LOOP")]
        public int? MAX_LOOP { get; set; }
    }

    public class POSTID : ConfigNodeBase {

    }

    public class REPLY : ConfigNodeBase {

    }

    public class TARGETLINK : ConfigNodeBase {
        [JsonPropertyName("TEXT")]
        public string? TEXT { get; set; }

    }

    public class TEXT : ConfigNodeBase {

    }

    public class USERID : ConfigNodeBase {

    }
}
