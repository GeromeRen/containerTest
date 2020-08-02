using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Accenture.Evergreen.Middleware.UnitTests
{
    public class OtherHTTPHeadersMiddlewareTests : BaseTests
    {
        private OtherHTTPHeadersMiddleware otherHTTPHeadersMiddleware;

        public OtherHTTPHeadersMiddlewareTests()
        {
            this.otherHTTPHeadersMiddleware = new OtherHTTPHeadersMiddleware(this.moqRequestDelegate.Object);
        }

        [Theory]
        [InlineData("X-powered-By", "test")]
        [InlineData("server", "test")]
        [InlineData("x-aspnet-version", "test")]
        [InlineData("X-aspnetmvc-version", "test")]
        public async void Should_Remove_Matched_HTTPHeaders_Success(string key, string value)
        {
            // Arrange
            this.httpResponse.Headers.Add(key, value);
            this.moqHttpContext.Setup(c => c.Response).Returns(this.httpResponse);

            // Act
            await this.otherHTTPHeadersMiddleware.InvokeAsync(this.moqHttpContext.Object);

            // Assert
            Assert.False(this.httpResponse.Headers.ContainsKey(key));
            Assert.Empty(this.httpResponse.Headers);
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
        }

        [Theory]
        [InlineData("X-powered-By12", "test")]
        [InlineData("server2", "test")]
        [InlineData("x-aspnet-versio2n", "test")]
        [InlineData("X-aspnetmvc-version12", "test")]
        public async void Should_Not_Remove_Unmatched_HTTPHeaders_Success(string key, string value)
        {
            // Arrange
            this.httpResponse.Headers.Add(key, value);
            this.moqHttpContext.Setup(c => c.Response).Returns(this.httpResponse);

            // Act
            await this.otherHTTPHeadersMiddleware.InvokeAsync(this.moqHttpContext.Object);

            // Assert
            Assert.True(this.httpResponse.Headers.ContainsKey(key));
            Assert.Single(this.httpResponse.Headers);
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
        }
    }
}
