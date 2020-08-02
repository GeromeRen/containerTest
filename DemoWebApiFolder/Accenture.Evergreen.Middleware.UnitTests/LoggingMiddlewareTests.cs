namespace Accenture.Evergreen.Middleware.UnitTests
{
    using Microsoft.AspNetCore.Http;
    using Moq;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="LoggingMiddlewareTests" />.
    /// </summary>
    public class LoggingMiddlewareTests
    {
        /// <summary>
        /// Defines the loggingMiddleware.
        /// </summary>
        private readonly LoggingMiddleware loggingMiddleware;

        /// <summary>
        /// Defines the moqHttpContext.
        /// </summary>
        private Mock<HttpContext> moqHttpContext;

        /// <summary>
        /// Defines the moqRequestDelegate.
        /// </summary>
        private Mock<RequestDelegate> moqRequestDelegate;

        /// <summary>
        /// Defines the fake_TraceIdentitifer.
        /// </summary>
        private const string fake_TraceIdentitifer = "fake_Id_123123123";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingMiddlewareTests"/> class.
        /// </summary>
        public LoggingMiddlewareTests()
        {
            this.moqRequestDelegate = new Mock<RequestDelegate>();
            this.loggingMiddleware = new LoggingMiddleware(this.moqRequestDelegate.Object);
        }

        /// <summary>
        /// The Should_Invoke_LoggingMiddleware_Success.
        /// </summary>
        [Fact]
        public async void Should_Invoke_LoggingMiddleware_Success()
        {
            // Arrange
            this.moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.TraceIdentifier).Returns(fake_TraceIdentitifer);
            moqHttpContext.Setup(c => c.Items).Returns(new Dictionary<object, object>());

            // Act
            await this.loggingMiddleware.Invoke(moqHttpContext.Object);

            // Assert
            Assert.Equal(2, moqHttpContext.Object.Items.Count);
            Assert.NotNull(moqHttpContext.Object.Items["RequestTime"]);
            Assert.Equal(fake_TraceIdentitifer, moqHttpContext.Object.Items["CorrelationId"]);

            // Verify
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
        }
    }

;
}
