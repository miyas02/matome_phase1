using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using matome_phase1.constants;

namespace matome_phase1.scraper.zawazawa {
    internal class ZawazawaScraperLogic : IScraperLogic{
        public static HtmlNode GetPostsNode(HtmlDocument doc) {
            var contentNode = doc.DocumentNode.SelectSingleNode(Constants.nodes);
            if (contentNode == null) {
                throw new Exception(Constants.contentNodeNotFound);
            }
            return contentNode;
        }
        HtmlNode GetPostNode(HtmlNode postsNode);
        string GetText(HtmlNode textNode);
        DateTime GetDate(HtmlNode dateNode);
        string GetUserId(HtmlNode userIdNode);
    }
}
