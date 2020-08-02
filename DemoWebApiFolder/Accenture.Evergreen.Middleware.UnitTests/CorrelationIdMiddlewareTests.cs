namespace Accenture.Evergreen.Middleware.UnitTests
{
    using Accenture.Evergreen.Middleware.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="CorrelationIdMiddlewareTests" />.
    /// </summary>
    public class CorrelationIdMiddlewareTests
    {
        /// <summary>
        /// Defines the correlationIdMiddleware.
        /// </summary>
        private CorrelationIdMiddleware correlationIdMiddleware;

        /// <summary>
        /// Defines the moqHttpContext.
        /// </summary>
        private Mock<HttpContext> moqHttpContext;

        /// <summary>
        /// Defines the moqRequestDelegate.
        /// </summary>
        private Mock<RequestDelegate> moqRequestDelegate;

        /// <summary>
        /// Defines the moqOptions.
        /// </summary>
        private Mock<IOptions<CorrelationIdMiddlewareOptions>> moqOptions;

        /// <summary>
        /// Defines the fake_TraceIdentitifer.
        /// </summary>
        private const string fake_TraceIdentitifer = "fake_Id_123123123";

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationIdMiddlewareTests"/> class.
        /// </summary>
        public CorrelationIdMiddlewareTests()
        {
            this.moqRequestDelegate = new Mock<RequestDelegate>();
            this.moqOptions = new Mock<IOptions<CorrelationIdMiddlewareOptions>>();
            this.moqOptions.Setup(c => c.Value).Returns(new CorrelationIdMiddlewareOptions());

            this.correlationIdMiddleware = new CorrelationIdMiddleware(this.moqRequestDelegate.Object, this.moqOptions.Object);
        }

        /// <summary>
        /// The Should_ThrowException_NullArguments.
        /// </summary>
        [Fact]
        public void Should_ThrowException_NullArguments()
        {
            Assert.Throws<ArgumentNullException>(() => new CorrelationIdMiddleware(null, this.moqOptions.Object));
            Assert.Throws<ArgumentNullException>(() => new CorrelationIdMiddleware(this.moqRequestDelegate.Object, null));
            Assert.Throws<ArgumentNullException>(() => new CorrelationIdMiddleware(null, null));
        }

        /// <summary>
        /// The Should_Invoke_With_IncludeInResponse_Success.
        /// </summary>
        /// <param name="includeInResponse">The includeInResponse<see cref="bool"/>.</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void Should_Invoke_With_IncludeInResponse_Success(bool includeInResponse)
        {
            // Arrange
            this.moqOptions.Setup(c => c.Value).Returns(new CorrelationIdMiddlewareOptions() { IncludeInResponse = includeInResponse });
            this.correlationIdMiddleware = new CorrelationIdMiddleware(this.moqRequestDelegate.Object, this.moqOptions.Object);

            var fakeHttpContext = new DefaultHttpContext();
            fakeHttpContext.Request.Headers.Add("X-Correlation-ID", fake_TraceIdentitifer);

            var moqResponse = new Mock<HttpResponse>();
            moqResponse.Setup(c => c.Headers).Returns(fakeHttpContext.Response.Headers);
            moqResponse.Setup(c => c.OnStarting(It.IsAny<Func<Task>>()));

            this.moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request).Returns(fakeHttpContext.Request);
            moqHttpContext.Setup(c => c.Response).Returns(moqResponse.Object);
            moqHttpContext.SetupProperty(c => c.TraceIdentifier);


            // Act
            await this.correlationIdMiddleware.Invoke(moqHttpContext.Object);

            // Assert
            Assert.Equal(fake_TraceIdentitifer, moqHttpContext.Object.TraceIdentifier);

            // Verify
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
            if(includeInResponse)
            {
                moqResponse.Verify(c => c.OnStarting(It.IsAny<Func<Task>>()), Times.Once);
            }
            else
            {
                moqResponse.Verify(c => c.OnStarting(It.IsAny<Func<Task>>()), Times.Never);
            }
        }
    }
}
