using Gibe.Navigation.Umbraco;
using Gibe.Navigation.Umbraco.Models;
using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace Gibe.Navigation.Umbraco.Tests
{
	[TestFixture]
	public class NavigationElementFactoryTests
	{
		[Test]
		public void Make_Returns_UmbracoNavigationElement_For_Page()
		{
			var contentMock = new Mock<IPublishedContent>();
			contentMock.Setup(c => c.GetProperty("gibeNavigationRedirect")).Returns((IPublishedProperty?)null);

			var factory = new NavigationElementFactory(new Mock<IPublishedUrlProvider>().Object, new NoopPublishedValueFallback());
			var result = factory.Make(contentMock.Object);

			Assert.That(result, Is.TypeOf<UmbracoNavigationElement>());
		}

		[Test]
		public void Make_Returns_UmbracoNavigationRedirectElement_For_Redirect()
		{
			var redirectPropertyMock = new Mock<IPublishedProperty>();
			redirectPropertyMock.Setup(p => p.HasValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

			var contentMock = new Mock<IPublishedContent>();
			contentMock.Setup(c => c.GetProperty("gibeNavigationRedirect")).Returns(redirectPropertyMock.Object);

			var factory = new NavigationElementFactory(new Mock<IPublishedUrlProvider>().Object, new NoopPublishedValueFallback());
			var result = factory.Make(contentMock.Object);

			Assert.That(result, Is.TypeOf<UmbracoNavigationRedirectElement>());
		}
	}
}
