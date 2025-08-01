using matome_phase1.scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace matome_phase1.test {
    /// <summary>
    /// UIのテスト用クラス
    /// </summary>
    internal class Test_main {
        //config.json取得
        string path = "C:\\Users\\aabca\\source\\repos\\matome_phase1\\source\\docs\\SampleConfig.json";

        public Test_main() {
            IScraperOwner scraperOwner = new ScraperOwner();
            scraperOwner.LoadConfig(path);
        }

        //scraperOwnerにconfig.jsonを渡す
    }
}
