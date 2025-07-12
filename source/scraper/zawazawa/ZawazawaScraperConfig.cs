using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.zawazawa {
    internal class ZawazawaScraperConfig: AbstractScraperConfig {
        public override string URL => "https://zawazawa.jp/spla3/chatlog/3";
        public override string SITE_NAME => "Zawazawa";

        public override string LIST_NODE => "//div[@id='main-list-view-widget']";
        public override string POST_NODE => ".//*[contains(@class, 'list-view-item')]";
        public override string ID_KEY => "data-key";
        public override string USER_ID_NODE => ".//*[contains(@class, 'hashed-track-info')]";
        // public string USER_ID_NODE_2 = ".//*[contains(@class, 'posted-user-account')]";
        //public override string USER_ID_KEY => Constants.USERIDNODE;
        public override string TEXT_NODE => ".//div[contains(@class, 'body')]//p";
        public override string DATE_NODE => ".//div[contains(@class, 'comment-timestamp')]//a";
        public override string REPLY_NODE => ".//div[contains(@class, 'comment-parent-link')]";
        public override string IMAGE_URL_NODE => ".//div[contains(@class, 'media-modal')]";
    }
}
