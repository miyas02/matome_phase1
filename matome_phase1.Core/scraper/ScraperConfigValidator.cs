using Json.Schema;
using System;
using System.Text.Json;
using matome_phase1.constants;
using OpenQA.Selenium.DevTools.V136.Page;
using System.Xml.XPath;
using matome_phase1.Core.scraper.Configs;
using System.Net.Http.Headers;

namespace matome_phase1.Core.scraper {
    public static class ScraperConfigValidator {
        public static void Validate(JsonDocument configDocument) {
            var root = configDocument.RootElement;
            ValidateSchemaRoot(root);
            ValidateSchemaExtract(root, out ScraperConfigType type);

            root.TryGetProperty("EXTRACT", out var extract);
            switch (type) {
                case ScraperConfigType.list:
                    foreach (var extractProperty in extract.EnumerateObject()) {
                        var childElement = extractProperty.Value;
                        ValidateValues(childElement, "CONTEXT");
                        ValidateValues(childElement, "ITEM");
                        childElement.TryGetProperty("FIELDS", out var fieldsElement);
                        ValidateSchemaExtractFields(fieldsElement);
                        ValidateValues(fieldsElement, "NODE");
                        ValidateValues(fieldsElement, "TYPE");
                    }
                    break;
                case ScraperConfigType.single:
                    ValidateValues(extract, "NODE");
                    ValidateValues(extract, "TYPE");
                    break;
            }
            //root.TryGetProperty("EXTRACT", out var extract);
            //foreach (var extractProperty in extract.EnumerateObject()) {
            //    var childElement = extractProperty.Value;
            //    var isFieldsPresent = childElement.TryGetProperty("FIELDS", out var fieldsElement);
            //    if (isFieldsPresent) {
            //        ValidateValues(childElement, "CONTEXT");
            //        ValidateValues(childElement, "ITEM");
            //        ValidateSchemaExtractFields(fieldsElement);
            //        foreach (var fieldProperty in fieldsElement.EnumerateObject()) {
            //            ValidateValues(fieldProperty.Value, "NODE");
            //        }
            //        continue;
            //    }
            //    ValidateValues(childElement, "NODE");
        }

        internal static void ValidateValues(JsonElement element, string target) {
            element.TryGetProperty(target,out var ele);
            try {
                XPathExpression.Compile(ele.ToString());
            } catch {
                throw new ScraperConfigValidationException(ScraperConfigValidationErrorCode.InvalidFormat, element.ToString(), $"The {ele} in the ScraperConfig json does not match the XPath");
            }
        }
        internal static void ValidateValuesFields(JsonElement element) {
            element.TryGetProperty("NODE", out var node);
            try {
                XPathExpression.Compile(node.ToString());
            } catch {
                throw new ScraperConfigValidationException(ScraperConfigValidationErrorCode.InvalidFormat, element.ToString(), "The NODE in the ScraperConfig json does not match the XPath");
            }
        }

        internal static void ValidateSchemaRoot(JsonElement root) {
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
        internal static void ValidateSchemaExtract(JsonElement root,out ScraperConfigType type) {
            var isExtractPresent = root.TryGetProperty("EXTRACT", out var extractElement);
            if (!isExtractPresent) throw new ScraperConfigValidationException(ScraperConfigValidationErrorCode.InvalidFormat, extractElement.ToString(), "ScraperConfig json does not exists the EXTRACT element");

            var listSchema = new JsonSchemaBuilder()
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
                );
            var singleSchema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Object)
                .Required("NODE", "TYPE");
            if (listSchema.Evaluate(extractElement).IsValid) type = ScraperConfigType.list;
            else if (singleSchema.Evaluate(extractElement).IsValid) type = ScraperConfigType.single;
            else 
                throw new ScraperConfigValidationException(
                ScraperConfigValidationErrorCode.InvalidFormat,
                extractElement.ToString(),
                "ScraperConfig EXTRACT child must be either a list or a single object");
        }
        internal static void ValidateSchemaExtractFields(JsonElement fields) {
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
