using matome_phase1.constants;
using matome_phase1.Core.scraper;
using matome_phase1.scraper.Configs;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace matome_phase1.Tests.ScraperConfigValidatorUnitTest {
    public class ScraperConfigValidatorTest {
        static string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        static string E1 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E1_ValidationExtract_ChildMissingRequiredSet.json";
        public static TheoryData<string, ScraperConfigValidationErrorCode> TestCases => new (){
            {E1, ScraperConfigValidationErrorCode.InvalidFormat}
        };
        
        [Theory]
        [MemberData(nameof(TestCases))]
        public void ValidateTest(string inputJson, ScraperConfigValidationErrorCode expectedErrorCode) {
            //Arrange
            //logger
            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectRoot)
                .AddJsonFile("appsettings.json")
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var inputFile = File.ReadAllText(inputJson);
            var inputDocument = JsonDocument.Parse(inputFile);
            try {
                //Act
                Log.Information($"Validate実行：{inputJson}");
                ScraperConfigValidator.Validate(inputDocument);
            } catch (ScraperConfigValidationException ex) {
                //Assert
                Log.Information(
                    "ScraperConfig validation result. Expected={Expected}, Actual={Actual}",
                    expectedErrorCode,
                    ex.ErrorCode);
                Assert.Equal(ex.ErrorCode, expectedErrorCode);
            }
            Log.CloseAndFlush();
        }
    }
}
