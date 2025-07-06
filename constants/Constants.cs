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
        public const string ZAWAZAWA = "https://zawazawa.jp/spla3/";
        public const string CHATLOG3 = "https://zawazawa.jp/spla3/chatlog/3";
        public const string NODES = "//div[@id='main-list-view-widget']";
        public const string POSTNODE = ".//*[contains(@class, 'list-view-item')]";
        public const string TEXTNODE = ".//*[contains(@class, 'body')]";
        public const string DATE = "data-key";
        public const string USERIDNODE = ".//*[contains(@class, 'hashed-track-info')]";
        public const string USERIDNODE_2 = ".//*[contains(@class, 'posted-user-account')]";



    }
}
