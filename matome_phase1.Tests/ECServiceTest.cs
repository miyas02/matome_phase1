using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper;
using matome_phase1.scraper.Configs;
using matome_phase1.scraper.Configs.EC;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace matome_phase1.Tests {
    public class ECServiceTest {
        //書き換える
        private static string type = "EC";
        private static string site = "mercari";
        private static string target = "メンズ";

        private string targetHtml = @$"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\{type}\{site}\{target}\targetHtml.html";
        private string ConfigPath = @$"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\{type}\{site}\{target}\Config.json";
        private string expectPath = @$"C:\work\MyApps\matome_phase1\matome_phase1.Tests\docs\{type}\{site}\{target}\Expect.json";

        AbstractScraperConfig AConfig;
        ECService service;
        public ECServiceTest() {
            //testConfigの読み込みとAConfigとServiceのインスタンス化
            string config = File.ReadAllText(ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (ECService)ScraperFactory.Create(AConfig);

        }

        [Fact]
        public void GetItemsTest() {
            //Arrange
            //expectedの作成
            string expect = File.ReadAllText(expectPath);
            var expectList = JsonSerializer.Deserialize<List<EC>>(expect);

            //Act
            List<Object> Items = service.GetItems(AConfig);
            List<EC> actualItems = Items.Cast<EC>().ToList();
            var options = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            //actualItemsの書き出し
            string filePath = @"C:\work\MyApps\matome_phase1\matome_phase1.Tests\log\EC_GetItemsTest_actualItems.json"; //出力パスの定義
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(filePath, json); //書き出し

            //Assert
            Assert.Equal(expectList, Items);

        }
        [Fact]
        public void DocParseItemsTest() {
            //Arrange
            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(targetHtml);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(expectPath);
            var expectList = JsonSerializer.Deserialize<List<EC>>(expect);

            //Act
            // actualの作成
            List<Object> Items = service.DocParseItems(AConfig, doc);
            List<EC> actualItems = Items.Cast<EC>().ToList();
            var options = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            //actualItemsの書き出し
            string filePath = @"C:\work\MyApps\matome_phase1\matome_phase1.Tests\log\EC_DocParseItems_actualItems.json"; //出力パスの定義
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(filePath, json); //書き出し

            //Assert
            Assert.Equal(expectList, actualItems);
        }
    }
}
