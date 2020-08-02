namespace Accenture.Evergreen.Middleware.UnitTests
{
    using Microsoft.AspNetCore.Http;
    using Moq;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="AuthValidationMiddlewareTests" />.
    /// </summary>
    public class AuthValidationMiddlewareTests
    {
        /// <summary>
        /// Defines the fake_TraceIdentitifer.
        /// </summary>
        private const string fake_TraceIdentitifer = "fake_Id_123123123";

        /// <summary>
        /// Defines the authValidationMiddleware.
        /// </summary>
        private readonly AuthValidationMiddleware authValidationMiddleware;

        /// <summary>
        /// Defines the moqRequestDelegate.
        /// </summary>
        private Mock<RequestDelegate> moqRequestDelegate;

        /// <summary>
        /// Defines the moqHttpContext.
        /// </summary>
        private Mock<HttpContext> moqHttpContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthValidationMiddlewareTests"/> class.
        /// </summary>
        public AuthValidationMiddlewareTests()
        {
            this.moqRequestDelegate = new Mock<RequestDelegate>();

            this.authValidationMiddleware = new AuthValidationMiddleware(this.moqRequestDelegate.Object);
        }

        /// <summary>
        /// The Should_Invoke_Success.
        /// </summary>
        [Fact]
        public async void Should_Invoke_Success()
        {
            // Arrange
            this.moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.TraceIdentifier).Returns(fake_TraceIdentitifer);
            moqHttpContext.Setup(c => c.Items).Returns(new Dictionary<object, object>());

            // Act
            await this.authValidationMiddleware.Invoke(moqHttpContext.Object);

            // Verify
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
        }
    }
}
