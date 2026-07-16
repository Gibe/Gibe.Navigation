using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Models;
using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;


namespace Gibe.Navigation.Umbraco
{
	public class NavigationElementFactory : INavigationElementFactory
	{
		private readonly IPublishedUrlProvider _publishedUrlProvider;
		private readonly IPublishedValueFallback _publishedValueFallback;

		public NavigationElementFactory(IPublishedUrlProvider publishedUrlProvider, IPublishedValueFallback publishedValueFallback)
		{
			_publishedUrlProvider = publishedUrlProvider;
			_publishedValueFallback = publishedValueFallback;
		}

		public INavigationElement Make(IPublishedContent content)
		{
			var model = IsRedirect(content)
					? new UmbracoNavigationRedirectElement(content, _publishedValueFallback)
					: (INavigationElement)new UmbracoNavigationElement(content, _publishedUrlProvider, _publishedValueFallback);

			model = HyrdateExtraProperties(content, model);

			return model;
		}

		private bool IsRedirect(IPublishedContent content)
		{
			return content.HasValue("gibeNavigationRedirect");
		}

		public virtual INavigationElement HyrdateExtraProperties(IPublishedContent content, INavigationElement element)
		{
			return element;
		}
	}

	[TestFixture]
	internal class NavigationElementFactoryTests
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
