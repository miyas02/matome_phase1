using matome_phase1.scraper.Configs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs.Post {
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
    public class POSTID : ConfigNodeBase {

    }

    public class REPLY : ConfigNodeBase {

    }
    public class TEXT : ConfigNodeBase {

    }
    public class USERID : ConfigNodeBase {

    }
}
