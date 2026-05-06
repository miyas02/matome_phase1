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
        static string E2 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E2_ValidationExtract_ExtractEmptyObject.json";
        static string E3 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E3_ValidationExtract_ExtractString.json";
        static string E4 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E4_ValidationExtractChildProp_FieldsString.json";
        static string E5 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E5_ValidationExtractChildProp_InvalidContextXPath.json";
        static string E6 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E6_ValidationExtractChildProp_InvalidItemXPath.json";
        static string E7 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E7_ValidationExtractFields_FieldsEmptyObject.json";
        static string E8 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E8_ValidationExtractFields_FieldString.json";
        static string E9 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E9_ValidationExtractFields_InvalidNodeXPath.json";
        static string E10 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E10_ValidationExtractFields_InvalidTypeXPath.json";
        static string E11 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E11_ValidationExtractFields_MissingNode.json";
        static string E12 = @$"{projectRoot}/TestFiles/ScraperConfigValidationTestFiles/E12_ValidationExtractFields_MissingType.json";
        public static TheoryData<string, ScraperConfigValidationErrorCode> TestCases => new (){
            {E1, ScraperConfigValidationErrorCode.InvalidFormat},
            {E2, ScraperConfigValidationErrorCode.InvalidFormat},
            {E3, ScraperConfigValidationErrorCode.InvalidFormat},
            {E4, ScraperConfigValidationErrorCode.InvalidFormat},
            {E5, ScraperConfigValidationErrorCode.InvalidFormat},
            {E6, ScraperConfigValidationErrorCode.InvalidFormat},
            {E7, ScraperConfigValidationErrorCode.InvalidFormat},
            {E8, ScraperConfigValidationErrorCode.InvalidFormat},
            {E9, ScraperConfigValidationErrorCode.InvalidFormat},
            {E10, ScraperConfigValidationErrorCode.InvalidFormat},
            {E11, ScraperConfigValidationErrorCode.InvalidFormat},
            {E12, ScraperConfigValidationErrorCode.InvalidFormat}
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
