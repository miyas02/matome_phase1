using Json.Schema;
using System;
using System.Text.Json;
using matome_phase1.constants;

namespace matome_phase1.Core.scraper {
    public static class ScraperConfigValidator {
        public static void Validate(JsonDocument configDocument) {
            var root = configDocument.RootElement;
            ValidateJsonObject(root);
            ValidateFormat(root);
            ValidateUrl(root);
            ValidateExtract(root);
            ValidateExtractFields(root);
        }
        internal static void ValidateFormat(JsonElement root) {
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
        internal static void ValidateJsonObject(JsonElement root) {
            // jsonがオブジェクトか判定
            if (root.ValueKind != JsonValueKind.Object) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidType,
                    "$",
                    "ScraperConfig root must be a JSON object.");
            }
        }
        internal static void ValidateUrl(JsonElement root) {
            var url = root.GetProperty("URL").GetString();
            if (!Uri.TryCreate(url, UriKind.Absolute, out _)) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidFormat,
                    "URL",
                    "URL must be a valid absolute URI.");
            }
        }
        internal static void ValidateSiteName(JsonElement root) {
            var siteNameElement = root.GetProperty("SITE_NAME");

            if (siteNameElement.ValueKind != JsonValueKind.String) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidType,
                    "$.SITE_NAME",
                    "SITE_NAME must be a string.");
            }
        }

        internal static void ValidateExtract(JsonElement root) {
            var schema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Object)
                .Properties(
                    ("EXTRACT", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Object)
                        .AdditionalProperties(
                            new JsonSchemaBuilder().Type(SchemaValueType.Object)
                        ))
                );
            var result = schema.Evaluate(root);
            if (!result.IsValid) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidFormat,
                    result.EvaluationPath.ToString(),
                    string.Join(
                        Environment.NewLine,
                        result.Errors.Select(x => x.ToString())) ?? "ScraperConfig EXTRACT child must be a json object");
            }
        }
        internal static void ValidateExtractFields(JsonElement extractElement) {
            var schema = new JsonSchemaBuilder()
                .OneOf(
                    new JsonSchemaBuilder()
                        .Type(SchemaValueType.Object)
                        .Required("CONTEXT", "ITEM", "FIELDS")
                        .Properties(
                            ("CONTEXT", new JsonSchemaBuilder().Type(SchemaValueType.String)),
                            ("ITEM", new JsonSchemaBuilder().Type(SchemaValueType.String)),
                            ("FIELDS", new JsonSchemaBuilder().Type(SchemaValueType.Object))
                        )
                        .AdditionalProperties(false),

                    new JsonSchemaBuilder()
                        .Type(SchemaValueType.Object)
                        .Required("NODE", "TYPE")
                        .Properties(
                            ("NODE", new JsonSchemaBuilder().Type(SchemaValueType.String)),
                            ("TYPE", new JsonSchemaBuilder().Type(SchemaValueType.String)),
                            ("ATTRIBUTE", new JsonSchemaBuilder().Type(SchemaValueType.String)),
                            ("REGEX", new JsonSchemaBuilder().Type(SchemaValueType.String))
                        )
                        .AdditionalProperties(false)
                );
            var result = schema.Evaluate(extractElement);
            if (!result.IsValid) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidFormat,
                    result.EvaluationPath.ToString(),
                    string.Join(
                        Environment.NewLine,
                        result.Errors.Select(x => x.ToString())) ?? "ScraperConfig EXTRACT Field does not match the expected schema.");
            }
        }
    }
}
