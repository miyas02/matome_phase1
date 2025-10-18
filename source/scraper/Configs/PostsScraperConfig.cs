using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace matome_phase1.scraper.Configs {
    internal class PostsScraperConfig : AbstractScraperConfig {
        //public override string URL {
        //    get; set;
        //}
        //public override string SITE_NAME {
        //    get; set;
        //}
        //public override string LOGIC {
        //    get; set;
        //}

        public string LIST_NODE {
            get; set;
        }
        public string POST_NODE {
            get; set;
        }
        public NodeSelector USER_ID {
            get; set;
        }
        public NodeSelector TEXT {
            get; set;
        }
        public NodeSelector DATE {
            get; set;
        }
        public NodeSelector REPLY {
            get; set;
        }
        public NodeSelector IMAGE {
            get; set;
        }
        public NodeSelector POST_ID {
            get; set;
        }
    }
}
