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
        [Fact]
        public void mercari_GetItemsTest() {
        string type = "EC";
        string site = "mercari";
        string target = "メンズ";
        string targetHtml = @$"..\..\..\docs\{type}\{site}\{target}\targetHtml.html";
        string ConfigPath = @$"..\..\..\docs\{type}\{site}\{target}\Config.json";
        string DocParseItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\DocParseItems_Expect.json";
        string GetItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\GetItems_Expect.json";

        AbstractScraperConfig AConfig;
        ECService service;
            //Arrange
            //testConfigの読み込みとAConfigとServiceのインスタンス化
            string config = File.ReadAllText(ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (ECService)ScraperFactory.Create(AConfig);
            //expectedの作成
            string expect = File.ReadAllText(GetItems_expectPath);
            var expectList = JsonSerializer.Deserialize<List<EC>>(expect);

            //Act
            List<Object> Items = service.GetItems(AConfig);
            List<EC> actualItems = Items.Cast<EC>().ToList();
            var options = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            //actualItemsの書き出し
            string filePath = @"..\..\..\log\mercari_GetItemsTest_actualItems.json"; //出力パスの定義
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(filePath, json); //書き出し

            //Assert
            Assert.Equal(expectList.Count, Items.Count);

        }
        [Fact]
        public void mercari_DocParseItemsTest() {
            string type = "EC";
            string site = "mercari";
            string target = "メンズ";
            string targetHtml = @$"..\..\..\docs\{type}\{site}\{target}\targetHtml.html";
            string ConfigPath = @$"..\..\..\docs\{type}\{site}\{target}\Config.json";
            string DocParseItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\DocParseItems_Expect.json";
            string GetItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\GetItems_Expect.json";
            AbstractScraperConfig AConfig;
            ECService service;
            //Arrange
            //testConfigの読み込みとAConfigとServiceのインスタンス化
            string config = File.ReadAllText(ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (ECService)ScraperFactory.Create(AConfig);
            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(targetHtml);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(DocParseItems_expectPath);
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
            string filePath = @"..\..\..\log\mercari_DocParseItems_actualItems.json"; //出力パスの定義
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(filePath, json); //書き出し

            //Assert
            Assert.Equal(expectList, actualItems);
        }
    }
}
