using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Accenture.Evergreen.Middleware.UnitTests
{
    public class ContentSinffingMiddlewareTests : BaseTests
    {
        private ContentSinffingMiddleware sinffingMiddleware;

        public ContentSinffingMiddlewareTests()
        {
            this.sinffingMiddleware = new ContentSinffingMiddleware(this.moqRequestDelegate.Object);
        }

        [Fact]
        public async void Should_Add_ContentSinffing_MiddlewareTests_()
        {
            // Arrange
            string expectedValue = "nosniff";

            // Act
            await this.sinffingMiddleware.InvokeAsync(this.moqHttpContext.Object);
            // Assert

            Assert.Equal(expectedValue, this.moqHttpContext.Object.Response.Headers["X-Content-Type-Options"]);
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
        }
    }
}
