using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.scraper.zawazawa {
    internal class ZawazawaScraperLogic : IScraperLogic{
        public HtmlNode GetPostsNode(HtmlDocument doc) {
        
        };
        HtmlNode GetPostNode(HtmlNode postsNode);
        string GetText(HtmlNode textNode);
        DateTime GetDate(HtmlNode dateNode);
        string GetUserId(HtmlNode userIdNode);
    }
}
