using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper {
    internal class PostsScraperConfig : AbstractScraperConfig {
        public override void Scraping() {
            
        }

        public override string URL {
            get; set;
        }
        public override string SITE_NAME {
            get; set;
        }
        public override string LOGIC {
            get; set;
        }

        public string LIST_NODE {
            get; set;
        }
        public string POST_NODE {
            get; set;
        }
        public string USER_ID_NODE {
            get; set;
        }
        public string TEXT_NODE {
            get; set;
        }
        public string DATE_NODE {
            get; set;
        }
        public string REPLY_NODE {
            get; set;
        }
        public string IMAGE_URL_NODE {
            get; set;
        }
        public string ID_KEY {
            get; set;
        }
    }
}
