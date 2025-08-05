using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    public class TargetLinkConfig {
        public NodeSelector Selector {
            get; set;
        } = new NodeSelector();
        public string? Text {
            get; set;
        }
    }
}
