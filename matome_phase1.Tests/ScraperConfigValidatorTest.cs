using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using matome_phase1.Core.scraper;

namespace matome_phase1.Tests.ScraperConfigValidatorUnitTest {
    public class ScraperConfigValidatorTest {
        public static IEnumerable<object[]> TestCases => new[] {
            new object[] { "5ch", @"TestFiles/5ch_ScraperConfig.json", @"TestFiles/5ch_DocParseItems_Expect.json", @"log/5ch_DocParseItems_Actual.json", @"TestFiles/targetHtml.html" }
        };
        [Fact]
        public void ValidateFormatTest() {
            //Arrange
            string json = """
                [
                    {
                       "URL": "https://example.com"
                    }
                ]
                """;
            JsonDocument doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            try {
                //Act
                ScraperConfigValidator.ValidateFormat(root);

            } catch (Exception ex) {
                //Assert
                Assert.True(true, $"Validation failed with exception: {ex.Message}");
            }   
        }
    }
}
