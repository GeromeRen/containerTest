using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Accenture.Evergreen.Middleware.UnitTests
{
    public class ContentSecurityPolicyMiddlewareTests : BaseTests
    {
        private ContentSecurityPolicyMiddleware securityPolicyMiddleware;

        public ContentSecurityPolicyMiddlewareTests()
        {
            this.securityPolicyMiddleware = new ContentSecurityPolicyMiddleware(this.moqRequestDelegate.Object);
        }

        [Fact]
        public async void Should_Add_ContentSecurityPolicy_Success()
        {
            // Arrange
            string expectedValue = "default-src 'self' 'unsafe-eval' 'unsafe-inline' *.accenture.com; " +
            "img-src 'self' *.accenture.com data:; connect-src 'self' *.accenture.com;  " +
            "font-src 'self' *.accenture.com report-uri *.accenture.com/csp_report; " +
            "connect-src 'self' *.accenture.com; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
            "form-action 'self';";

            // Act
            await this.securityPolicyMiddleware.InvokeAsync(this.moqHttpContext.Object);

            // Assert
            string actualValue = this.httpResponse.Headers["Content-Security-Policy"];
            Assert.Equal(expectedValue, actualValue);
            this.moqRequestDelegate.Verify(c => c(It.Is<HttpContext>(t => t == this.moqHttpContext.Object)), Times.Once);
        }
    }
}
