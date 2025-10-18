using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class DATE {
        [JsonPropertyName("NODE")]
        public string NODE { get; set; }

        [JsonPropertyName("TYPE")]
        public string TYPE { get; set; }
    }

    public class IMAGE {
        [JsonPropertyName("NODE")]
        public string NODE { get; set; }

        [JsonPropertyName("TYPE")]
        public string TYPE { get; set; }
    }

    public class NAVIGATEPAGE {
        [JsonPropertyName("TYPE")]
        public NavigatePageTypes TYPE { get; set; }

        [JsonPropertyName("TARGET_LINK")]
        public TARGETLINK TARGET_LINK { get; set; }

        [JsonPropertyName("PAGINATION")]
        public PAGINATION PAGINATION { get; set; }
    }

    public class PAGINATION {
        [JsonPropertyName("NODE")]
        public string NODE { get; set; }

        [JsonPropertyName("ACTION")]
        public string ACTION { get; set; }

        [JsonPropertyName("MAX_LOOP")]
        public int MAX_LOOP { get; set; }
    }

    public class POSTID {
        [JsonPropertyName("NODE")]
        public string NODE { get; set; }

        [JsonPropertyName("TYPE")]
        public string TYPE { get; set; }

        [JsonPropertyName("ATTRIBUTE")]
        public string ATTRIBUTE { get; set; }
    }

    public class REPLY {
        [JsonPropertyName("NODE")]
        public string NODE { get; set; }

        [JsonPropertyName("TYPE")]
        public string TYPE { get; set; }
    }

    public class PostConfig : AbstractScraperConfig {
        [JsonPropertyName("LIST_NODE")]
        public string LIST_NODE { get; set; }

        [JsonPropertyName("POST_NODE")]
        public string POST_NODE { get; set; }

        [JsonPropertyName("USER_ID")]
        public USERID USER_ID { get; set; }

        [JsonPropertyName("TEXT")]
        public TEXT TEXT { get; set; }

        [JsonPropertyName("DATE")]
        public DATE DATE { get; set; }

        [JsonPropertyName("REPLY")]
        public REPLY REPLY { get; set; }

        [JsonPropertyName("IMAGE")]
        public IMAGE IMAGE { get; set; }

        [JsonPropertyName("POST_ID")]
        public POSTID POST_ID { get; set; }
    }

    public class TARGETLINK {
        [JsonPropertyName("NODE")]
        public string NODE { get; set; }

        [JsonPropertyName("TEXT")]
        public string TEXT { get; set; }
    }

    public class TEXT {
        [JsonPropertyName("NODE")]
        public string NODE { get; set; }

        [JsonPropertyName("TYPE")]
        public string TYPE { get; set; }
    }

    public class USERID {
        [JsonPropertyName("NODE")]
        public string NODE { get; set; }

        [JsonPropertyName("TYPE")]
        public string TYPE { get; set; }
    }


}
