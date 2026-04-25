using System;

namespace matome_phase1.constants {
    public enum ScraperConfigValidationErrorCode {
        MissingField,
        InvalidType,
        InvalidFormat,
        InvalidValue,
        InvalidCombination,
        MutuallyExclusive,
        EmptyCollection,
        DuplicateKey
    }

    public class ScraperConfigValidationException : Exception {
        public ScraperConfigValidationErrorCode ErrorCode { get; }
        public string FieldPath { get; }

        public ScraperConfigValidationException(
            ScraperConfigValidationErrorCode errorCode,
            string fieldPath,
            string message,
            Exception? innerException = null)
            : base(FormatMessage(errorCode, fieldPath, message), innerException) {
            ErrorCode = errorCode;
            FieldPath = fieldPath;
        }

        private static string FormatMessage(
            ScraperConfigValidationErrorCode errorCode,
            string fieldPath,
            string message) {
            return $"[{errorCode}] {fieldPath}: {message}";
        }
    }
}
