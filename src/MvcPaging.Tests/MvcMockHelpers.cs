using System;
using System.Web;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace MvcPaging.Tests
{
	public static class MvcMockHelpers
	{
		public static HttpContextBase FakeHttpContext()
		{
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var response = new Mock<HttpResponseBase>();
			var session = new Mock<HttpSessionStateBase>();
			var server = new Mock<HttpServerUtilityBase>();

			context.Expect(ctx => ctx.Request).Returns(request.Object);
			context.Expect(ctx => ctx.Response).Returns(response.Object);
			context.Expect(ctx => ctx.Session).Returns(session.Object);
			context.Expect(ctx => ctx.Server).Returns(server.Object);

			return context.Object;
		}

		public static HttpContextBase FakeHttpContext(string url)
		{
			HttpContextBase context = FakeHttpContext();
			context.Request.SetupRequestUrl(url);
			return context;
		}

		public static void SetFakeControllerContext(this Controller controller)
		{
			var httpContext = FakeHttpContext();
			ControllerContext context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
			controller.ControllerContext = context;
		}

		static string GetUrlFileName(string url)
		{
			if (url.Contains("?"))
				return url.Substring(0, url.IndexOf("?"));
			else
				return url;
		}

		static NameValueCollection GetQueryStringParameters(string url)
		{
			if (url.Contains("?"))
			{
				NameValueCollection parameters = new NameValueCollection();

				string[] parts = url.Split("?".ToCharArray());
				string[] keys = parts[1].Split("&".ToCharArray());

				foreach (string key in keys)
				{
					string[] part = key.Split("=".ToCharArray());
					parameters.Add(part[0], part[1]);
				}

				return parameters;
			}
			else
			{
				return null;
			}
		}

		public static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
		{
			Mock.Get(request)
				.Expect(req => req.HttpMethod)
				.Returns(httpMethod);
		}

		public static void SetupRequestUrl(this HttpRequestBase request, string url)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			if (!url.StartsWith("~/"))
				throw new ArgumentException("Sorry, we expect a virtual url starting with \"~/\".");

			var mock = Mock.Get(request);

			mock.Expect(req => req.QueryString)
				.Returns(GetQueryStringParameters(url));
			mock.Expect(req => req.AppRelativeCurrentExecutionFilePath)
				.Returns(GetUrlFileName(url));
			mock.Expect(req => req.PathInfo)
				.Returns(string.Empty);
		}
	}
}