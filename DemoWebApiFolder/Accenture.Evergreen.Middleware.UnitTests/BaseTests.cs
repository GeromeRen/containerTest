using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accenture.Evergreen.Middleware.UnitTests
{
    public class BaseTests
    {

        /// <summary>
        /// Defines the moqHttpContext.
        /// </summary>
        protected Mock<HttpContext> moqHttpContext;

        /// <summary>
        /// Defines the moqRequestDelegate.
        /// </summary>
        protected Mock<RequestDelegate> moqRequestDelegate;

        protected DefaultHttpContext defaultHttpContext;
        protected HttpResponse httpResponse;

        public BaseTests()
        {
            this.moqRequestDelegate = new Mock<RequestDelegate>();
            this.defaultHttpContext = new DefaultHttpContext();
            this.httpResponse = this.defaultHttpContext.Response;
            this.moqHttpContext = new Mock<HttpContext>();
            this.moqHttpContext.Setup(c => c.Response).Returns(this.httpResponse);
        }
    }
}
