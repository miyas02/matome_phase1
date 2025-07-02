using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace matome_phase1.constants
{
    public class Constants {
        //Erorrログ
        public const string contentNodeNotFound = "Content node not found in the document.";

        //zawazawaの定数
        public const string zawazawa = "https://zawazawa.jp/spla3/";
        public const string chatLog3 = "https://zawazawa.jp/spla3/chatlog/3";
        public const string nodes = "//div[@id='main-list-view-widget']";
        public const string postNode = ".//*[contains(@class, 'list-view-item')]";
        public const string textNode = ".//*[contains(@class, 'body')]";
        public const string date = "data-key";
        public const string userIdNode = ".//*[contains(@class, 'hashed-track-info')]";
        public const string userIdNode_2 = ".//*[contains(@class, 'posted-user-account')]";



    }
}
