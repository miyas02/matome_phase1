using matome_phase1.constants;

namespace matome_phase1.Tests {
    /// <summary>
    /// Tests for scraper config validation exceptions.
    /// </summary>
    public class ScraperConfigValidationExceptionTest {
        /// <summary>
        /// Constructor should expose validation details and format the exception message.
        /// </summary>
        [Fact]
        public void ConstructorSetsValidationDetails() {
            var innerException = new InvalidOperationException("parse failed");

            var exception = new ScraperConfigValidationException(
                ScraperConfigValidationErrorCode.InvalidFormat,
                "EXTRACT.posts.CONTEXT",
                "CONTEXT is not valid XPath.",
                innerException);

            Assert.Equal(ScraperConfigValidationErrorCode.InvalidFormat, exception.ErrorCode);
            Assert.Equal("EXTRACT.posts.CONTEXT", exception.FieldPath);
            Assert.Equal("[InvalidFormat] EXTRACT.posts.CONTEXT: CONTEXT is not valid XPath.", exception.Message);
            Assert.Same(innerException, exception.InnerException);
        }
    }
}
