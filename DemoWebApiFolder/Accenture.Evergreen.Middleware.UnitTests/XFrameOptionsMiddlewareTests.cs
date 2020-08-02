using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Accenture.Evergreen.Middleware.UnitTests
{
    public class XFrameOptionsMiddlewareTests : BaseTests
    {
        private XFrameOptionsMiddleware xFrameOptions;

        public XFrameOptionsMiddlewareTests()
        {
            this.xFrameOptions = new XFrameOptionsMiddleware(this.moqRequestDelegate.Object);
        }

        [Fact]
        public async void Should_Add_XFrameOptions_SuccesS()
        {
            // Arrange
            string expectedValue = "DENY";

            // Act
            await this.xFrameOptions.InvokeAsync(this.moqHttpContext.Object);

            // Assert
            string actualValue = this.httpResponse.Headers["X-Frame-Options"];
            Assert.Equal(expectedValue, actualValue);
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
        }
    }
}
