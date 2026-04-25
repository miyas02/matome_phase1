using Json.Schema;
using System;
using System.Text.Json;
using matome_phase1.constants;

namespace matome_phase1.Core.scraper {
    internal static class ScraperConfigValidator {
        private static readonly JsonSchema Schema = BuildSchema();

        internal static void Validate(JsonDocument configDocument) {
            var root = configDocument.RootElement;
            if (root.ValueKind != JsonValueKind.Object) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidType,
                    "$",
                    "ScraperConfig root must be a JSON object.");
            }

            var result = Schema.Evaluate(root);
            if (!result.IsValid) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidValue,
                    result.EvaluationPath.ToString(),
                    result.Details[0].ToString() ?? "ScraperConfig json does not match the expected schema.");
            }

            ValidateUrl(root);
            ValidateExtract(root);
        }

        private static JsonSchema BuildSchema() {
            using var schemaDocument = JsonDocument.Parse(
                """
                {
                  "$schema": "https://json-schema.org/draft/2020-12/schema",
                  "type": "object",
                  "required": ["SITE_NAME", "URL", "EXTRACT"],
                  "additionalProperties": false,
                  "properties": {
                    "SITE_NAME": {
                      "type": "string",
                      "minLength": 1
                    },
                    "URL": {
                      "type": "string",
                      "minLength": 1
                    },
                    "EXTRACT": {
                      "type": "object",
                      "minProperties": 1,
                      "additionalProperties": {
                        "$ref": "#/$defs/listExtract"
                      }
                    }
                  },
                  "$defs": {
                    "listExtract": {
                      "type": "object",
                      "required": ["CONTEXT", "ITEM", "FIELDS"],
                      "additionalProperties": false,
                      "properties": {
                        "CONTEXT": { "type": "string", "minLength": 1 },
                        "ITEM": { "type": "string", "minLength": 1 },
                        "FIELDS": {
                          "type": "object",
                          "minProperties": 1,
                          "additionalProperties": {
                            "$ref": "#/$defs/singleExtract"
                          }
                        }
                      }
                    },
                    "singleExtract": {
                      "type": "object",
                      "required": ["NODE", "TYPE"],
                      "additionalProperties": false,
                      "properties": {
                        "NODE": { "type": "string", "minLength": 1 },
                        "TYPE": { "type": "string", "minLength": 1 },
                        "ATTRIBUTE": { "type": "string", "minLength": 1 },
                        "REGEX": { "type": "string", "minLength": 1 }
                      }
                    }
                  }
                }
                """);

            return JsonSchema.Build(schemaDocument.RootElement);
        }

        private static void ValidateUrl(JsonElement root) {
            var url = root.GetProperty("URL").GetString();
            if (!Uri.TryCreate(url, UriKind.Absolute, out _)) {
                throw new ScraperConfigValidationException(
                    ScraperConfigValidationErrorCode.InvalidFormat,
                    "URL",
                    "URL must be a valid absolute URI.");
            }
        }

        private static void ValidateExtract(JsonElement root) {
            var extract = root.GetProperty("EXTRACT");

        }
    }
}
