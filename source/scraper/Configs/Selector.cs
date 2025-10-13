using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    public class Selector {
        public string NODE {
            get; set;
        }
        public string TYPE {
            get; set;
        }
        public string ATTRIBUTE {
            get; set;
        }  // nullの場合もあるのでnullable対応
    }
}
