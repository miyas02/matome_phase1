using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper {
    public abstract class AbstractScraperConfig {
        public abstract IScraperLogic ScraperLogic {
            get;
        }

        public abstract string URL { get; }
        public abstract string SITE_NAME {
            get;
        }
        /// <summary>
        /// 投稿のNodeの1階層上位のNode
        /// </summary>
        public abstract string LIST_NODE {
            get;
        }
        /// <summary>
        /// 投稿のNode
        /// LIST_NODEの下に複数存在する
        /// </summary>
        public abstract string POST_NODE {
            get;
        }

        public abstract string ID_KEY {
            get;
        }

        public abstract string USER_ID_NODE {
            get;
        }

        public abstract string TEXT_NODE {
            get;
        }

        public abstract string DATE_NODE {
            get;
        }

        public abstract string REPLY_NODE {
            get;
        }

        public abstract string IMAGE_URL_NODE {
            get;
        }
    }
}
