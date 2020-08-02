namespace Accenture.Evergreen.Middleware.UnitTests
{
    using Accenture.Evergreen.Middleware.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System;
    using System.IO;
    using System.Net;
    using System.Text.Json;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="ExceptionHandlingMiddlewareTests" />.
    /// </summary>
    public class ExceptionHandlingMiddlewareTests
    {
        /// <summary>
        /// Defines the exceptionHandlingMiddleware.
        /// </summary>
        private readonly ExceptionHandlingMiddleware exceptionHandlingMiddleware;

        /// <summary>
        /// Defines the moqRequestDelegate.
        /// </summary>
        private Mock<RequestDelegate> moqRequestDelegate;

        /// <summary>
        /// Defines the moqHttpContext.
        /// </summary>
        private Mock<HttpContext> moqHttpContext;

        /// <summary>
        /// Defines the moqLogger.
        /// </summary>
        private Mock<ILogger<ExceptionHandlingMiddleware>> moqLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddlewareTests"/> class.
        /// </summary>
        public ExceptionHandlingMiddlewareTests()
        {
            this.moqHttpContext = new Mock<HttpContext>();
            this.moqRequestDelegate = new Mock<RequestDelegate>();
            this.moqLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            this.exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(this.moqRequestDelegate.Object, this.moqLogger.Object);
        }

        /// <summary>
        /// The Should_ThrowException_OnNullArguments_Success.
        /// </summary>
        [Fact]
        public void Should_ThrowException_OnNullArguments_Success()
        {
            Assert.Throws<ArgumentNullException>(() => new ExceptionHandlingMiddleware(this.moqRequestDelegate.Object, null));
            Assert.Throws<ArgumentNullException>(() => new ExceptionHandlingMiddleware(null, this.moqLogger.Object));
            Assert.Throws<ArgumentNullException>(() => new ExceptionHandlingMiddleware(null, null));
        }

        /// <summary>
        /// The Should_ThrowException_NullHttpContext_Success.
        /// </summary>
        [Fact]
        public void Should_ThrowException_NullHttpContext_Success()
        {
            // Act
            var actual = Assert.ThrowsAsync<ArgumentNullException>(async () => { await this.exceptionHandlingMiddleware.InvokeAsync(null); });
            // Assert
            Assert.NotNull(actual);
        }

        /// <summary>
        /// The Should_InvokeExceptionMiddleware_Success.
        /// </summary>
        [Fact]
        public async void Should_InvokeExceptionMiddleware_Success()
        {
            // Arrange
            this.moqHttpContext = new Mock<HttpContext>();

            // Act
            await this.exceptionHandlingMiddleware.InvokeAsync(this.moqHttpContext.Object);

            // Verify
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
        }

        /// <summary>
        /// The Should_HandleException_OnInvokeNextMiddleware_Success.
        /// </summary>
        /// <param name="env">The env<see cref="string"/>.</param>
        [Theory]
        [InlineData("Development")]
        [InlineData("Staging")]
        [InlineData("Production")]
        public async void Should_HandleException_OnInvokeNextMiddleware_Success(string env)
        {
            // Arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", env);
            string expectedExpcetionMessage = $"This is a fake exception for {env}";
            string expectedTraceIdentifier = "fake_traceIdentifier_123123123";
            if(env != "Development")
            {
                expectedExpcetionMessage = $"An error has occurred. Please provide the following ID to the Support Team: {expectedTraceIdentifier}";
            }

            ExceptionHandlingResponseModel expectedexceptionModel = new ExceptionHandlingResponseModel()
            {
                CorrelationId = expectedTraceIdentifier,
                Message = expectedExpcetionMessage,
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };

            HttpResponse fakeResponse = new DefaultHttpContext().Response;
            fakeResponse.StatusCode = (int)HttpStatusCode.OK;
            fakeResponse.Body = new MemoryStream();
            this.moqHttpContext = new Mock<HttpContext>();
            this.moqHttpContext.Setup(c => c.Response).Returns(fakeResponse);
            this.moqHttpContext.Setup(c => c.TraceIdentifier).Returns("fake_traceIdentifier_123123123");
            this.moqRequestDelegate.Setup(c => c.Invoke(this.moqHttpContext.Object)).Throws(new Exception(expectedExpcetionMessage));

            // Act
            await this.exceptionHandlingMiddleware.InvokeAsync(this.moqHttpContext.Object);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, fakeResponse.StatusCode);
            Assert.Equal("application/json", fakeResponse.ContentType);
            fakeResponse.Body.Position = 0;
            using(StreamReader streamReader = new StreamReader(fakeResponse.Body))
            {
                string actualResponseText = await streamReader.ReadToEndAsync();
                Assert.Equal(JsonSerializer.Serialize<ExceptionHandlingResponseModel>(expectedexceptionModel), actualResponseText);
            }
        }
    }
}
