using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    internal class NewsConfig : AbstractScraperConfig {
        public override string ToString() {
            return JsonSerializer.Serialize(this);
        }
        [JsonPropertyName("LIST_NODE")]
        public string? LIST_NODE { get; set; }

        [JsonPropertyName("POST_NODE")]
        public string? POST_NODE { get; set; }

        [JsonPropertyName("NAME")]
        public NAME? NAME { get; set; }

        [JsonPropertyName("LINK")]
        public LINK? LINK { get; set; }

        [JsonPropertyName("IMAGE")]
        public IMAGE? IMAGE { get; set; }
    }

    public class NAME : ConfigNodeBase {

    }
    public class LINK : ConfigNodeBase {

    }
    public class IMAGE : ConfigNodeBase {

    }
}
