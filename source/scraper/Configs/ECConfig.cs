using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs.EC {
    public class ECConfig : AbstractScraperConfig {
        public override string ToString() {
            return JsonSerializer.Serialize(this);
        }
        [JsonPropertyName("LIST_NODE")]
        public string? LIST_NODE { get; set; }

        [JsonPropertyName("ITEM_NODE")]
        public string? ITEM_NODE { get; set; }
        [JsonPropertyName("ITEM_NAME")]
        public ITEM_NAME? ITEM_NAME { get; set; }
        [JsonPropertyName("PRICE")]
        public PRICE? PRICE { get; set; }
        [JsonPropertyName("IMAGE")]
        public IMAGE? IMAGE { get; set; }
    }
    public class ITEM_NAME : ConfigNodeBase {

    }
    public class PRICE : ConfigNodeBase {

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
}
