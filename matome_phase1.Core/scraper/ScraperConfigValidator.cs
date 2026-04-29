using Json.Schema;
using System;
using System.Text.Json;
using matome_phase1.constants;
using OpenQA.Selenium.DevTools.V136.Page;

namespace matome_phase1.Core.scraper {
    public static class ScraperConfigValidator {
        public static void Validate(JsonDocument configDocument) {
            var root = configDocument.RootElement;
            ValidateRoot(root);
            var isExtractPresent = root.TryGetProperty("EXTRACT", out var extractElement);
            if (isExtractPresent) {
                ValidateExtract(extractElement);
            }
            foreach(var extractProperty in extractElement.EnumerateObject()) {
                var childElement = extractProperty.Value;
                var isFieldsPresent = childElement.TryGetProperty("FIELDS", out var fieldsElement);
                if (isFieldsPresent) {
                    ValidateExtractFields(fieldsElement);
                }
            }
        }
        internal static void ValidateRoot(JsonElement root) {
            //schema作成
            var schema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Object)
                .Required("URL", "EXTRACT");
            var result = schema.Evaluate(root);
            if (!result.IsValid) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidFormat,
                    result.EvaluationPath.ToString(),
                    string.Join(
                        Environment.NewLine,
                        result.Errors.Select(x => x.ToString())) ?? "ScraperConfig json does not match the expected schema.");
            }
        }
        internal static void ValidateExtract(JsonElement Extract) {
            var schema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Object)
                .AdditionalProperties(
                    new JsonSchemaBuilder().OneOf(
                        new JsonSchemaBuilder()
                            .Type(SchemaValueType.Object)
                            .Required("CONTEXT", "ITEM", "FIELDS")
                            .Properties(
                                ("CONTEXT", new JsonSchemaBuilder()
                                    .Type(SchemaValueType.String)
                                    .MinLength(1)))
                            .Properties(
                                ("ITEM", new JsonSchemaBuilder()
                                    .Type(SchemaValueType.String)
                                    .MinLength(1)))
                            .Properties(
                                ("FIELDS", new JsonSchemaBuilder()
                                    .Type(SchemaValueType.Object)
                                )
                            ),
                        new JsonSchemaBuilder()
                            .Type(SchemaValueType.Object)
                            .Required("NODE", "TYPE")
                    )
                );
            var result = schema.Evaluate(Extract);
            if (!result.IsValid) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidFormat,
                    result.EvaluationPath.ToString(),
                    string.Join(
                        Environment.NewLine,
                        result.Errors.Select(x => x.ToString())) ?? "ScraperConfig EXTRACT child must be a json object");
            }
        }
        internal static void ValidateExtractFields(JsonElement fields) {
            var schema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Object)
                .AdditionalProperties(
                    new JsonSchemaBuilder()
                        .Type(SchemaValueType.Object)
                        .Required("NODE", "TYPE")
                        .Properties(
                            ("NODE", new JsonSchemaBuilder()
                                .Type(SchemaValueType.String)
                                .MinLength(1)),
                            ("TYPE", new JsonSchemaBuilder()
                                .Type(SchemaValueType.String)
                                .MinLength(1))
                        )
                );
            var result = schema.Evaluate(fields);
            if (!result.IsValid) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidFormat,
                    result.EvaluationPath.ToString(),
                    string.Join(
                        Environment.NewLine,
                        result.Errors.Select(x => x.ToString())) ?? "ScraperConfig EXTRACT child must be a json object");
            }
        }
    }
}
