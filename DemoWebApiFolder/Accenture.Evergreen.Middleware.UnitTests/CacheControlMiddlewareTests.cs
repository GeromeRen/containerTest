using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Accenture.Evergreen.Middleware.UnitTests
{
    public class CacheControlMiddlewareTests : BaseTests
    {
        private CacheControlMiddleware cacheControlMiddleware;

        public CacheControlMiddlewareTests()
        {
            this.cacheControlMiddleware = new CacheControlMiddleware(this.moqRequestDelegate.Object);
        }

        [Fact]
        public async void Should_Add_CacheControlMiddleware_Success()
        {
            // Arrange
            string expectedCacheControlValue = "public, max-age=31536000";
            string expectedVaryValue = "Accept-Encoding";

            // Act
            await this.cacheControlMiddleware.InvokeAsync(this.moqHttpContext.Object);

            // Assert
            string actualCacheControlValue = this.httpResponse.Headers["Cache-Control"];
            string actualVaryValue = this.httpResponse.Headers["Vary"];
            Assert.Equal(expectedCacheControlValue, actualCacheControlValue);
            Assert.Equal(expectedVaryValue, actualVaryValue);
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
        }
    }
}
