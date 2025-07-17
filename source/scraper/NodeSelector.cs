using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper {
    public class NodeSelector {
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
