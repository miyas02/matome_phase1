using HtmlAgilityPack;
using matome_phase1.constants;
using matome_phase1.scraper;
using matome_phase1.scraper.Configs.Base;
using matome_phase1.scraper.Configs.EC;
using matome_phase1.scraper.Models;
using matome_phase1.scraper.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace matome_phase1.Tests.ECServiceTest {
    internal class DocsPaths {
        internal string targetHtml { get; init; }
        internal string ConfigPath { get; init; }
        internal string DocParseItems_expectPath { get; init; }
        internal string DocParseItems_actualPath { get; init; }
        internal string GetItems_expectPath { get; init; }
        internal string GetItems_actualPath { get; init; }
        internal DocsPaths(string type, string site, string target) {
            targetHtml = @$"..\..\..\docs\{type}\{site}\{target}\targetHtml.html";
            ConfigPath = @$"..\..\..\docs\{type}\{site}\{target}\Config.json";
            DocParseItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\DocParseItems_Expect.json";
            DocParseItems_actualPath = @$"..\..\..\log\{site}_DocParseItems_actual.json";
            GetItems_expectPath = @$"..\..\..\docs\{type}\{site}\{target}\GetItems_Expect.json";
            GetItems_actualPath = @$"..\..\..\log\{site}_GetItems_actual.json";
        }
    }
    public class ECServiceTest {
        [Fact]
        public void mercari_GetItemsTest() {
        string type = "EC";
        string site = "mercari";
        string target = "メンズ";
        DocsPaths docs = new(type, site, target);

        AbstractScraperConfig AConfig;
        ECService service;
            //Arrange
            //testConfigの読み込みとAConfigとServiceのインスタンス化
            string config = File.ReadAllText(docs.ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (ECService)ScraperFactory.Create(AConfig);
            //expectedの作成
            string expect = File.ReadAllText(docs.GetItems_expectPath);
            var expectList = JsonSerializer.Deserialize<List<EC>>(expect);

            //Act
            List<Object> Items = service.GetItems(AConfig);
            List<EC> actualItems = Items.Cast<EC>().ToList();
            var options = new System.Text.Json.JsonSerializerOptions {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            //actualItemsの書き出し
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(docs.GetItems_actualPath, json); //書き出し

            //Assert
            Assert.Equal(expectList.Count, Items.Count);

        }
        [Fact]
        public void mercari_DocParseItemsTest() {
            string type = "EC";
            string site = "mercari";
            string target = "メンズ";
            DocsPaths docs = new(type, site, target);
            AbstractScraperConfig AConfig;
            ECService service;
            //Arrange
            //testConfigの読み込みとAConfigとServiceのインスタンス化
            string config = File.ReadAllText(docs.ConfigPath);
            AConfig = ScraperFactory.Create(config);
            if (AConfig.LOGIC == null) {
                throw new ConfigException(ScraperExceptionType.ConfigJsonLogicNotFound, AConfig);
            }
            service = (ECService)ScraperFactory.Create(AConfig);
            //ターゲットhtml読み込み
            string htmlText = File.ReadAllText(docs.targetHtml);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            //expectedの作成
            string expect = File.ReadAllText(docs.DocParseItems_expectPath);
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
            string json = JsonSerializer.Serialize(actualItems, options); //Listをjsonにシリアライズ
            File.WriteAllText(docs.DocParseItems_actualPath, json); //書き出し

            //Assert
            Assert.Equal(expectList, actualItems);
        }
    }
}
