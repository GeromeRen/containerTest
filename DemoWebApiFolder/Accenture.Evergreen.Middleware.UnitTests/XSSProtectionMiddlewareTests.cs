using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xunit;

namespace Accenture.Evergreen.Middleware.UnitTests
{
    public class XSSProtectionMiddlewareTests : BaseTests
    {
        /// <summary>
        /// Defines the loggingMiddleware.
        /// </summary>
        private readonly XSSProtectionMiddleware xSSProtectionMiddleware;

        public XSSProtectionMiddlewareTests()
        {
            this.xSSProtectionMiddleware = new XSSProtectionMiddleware(this.moqRequestDelegate.Object);
        }

        [Fact]
        public async void Should_Add_XSSProectionHeader()
        {
            // Arrange
            string expectedValue = "1; mode=block";
            HttpResponse fakeResponse = new DefaultHttpContext().Response;
            fakeResponse.StatusCode = (int)HttpStatusCode.OK;
            fakeResponse.Body = new MemoryStream();
            this.moqHttpContext = new Mock<HttpContext>();
            this.moqHttpContext.Setup(c => c.Response).Returns(fakeResponse);

            // Act
            await this.xSSProtectionMiddleware.InvokeAsync(this.moqHttpContext.Object);

            // Assert
            string actualValue = fakeResponse.Headers["X-Xss-Protection"];
            Assert.Equal(expectedValue, actualValue);
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
        }
    }
}
