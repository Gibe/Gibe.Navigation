using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Models;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco
{
	public class NavigationElementFactory : INavigationElementFactory
	{

		public NavigationElementFactory()
		{

		}

		public INavigationElement Make(IPublishedContent content)
		{
			var model = IsRedirect(content)
					? new UmbracoNavigationRedirectElement(content)
					: (INavigationElement)new UmbracoNavigationElement(content);

			model.IsVisible = ShowInNavigation(content);
			return model;
		}

		private bool IsRedirect(IPublishedContent content)
		{
			return content.DocumentTypeAlias.Equals("redirect", StringComparison.CurrentCultureIgnoreCase);
		}

		private bool ShowInNavigation(IPublishedContent content)
		{
			return _umbracoWrapper.HasValue(content, "umbracoNaviHide") &&
						 !_umbracoWrapper.GetPropertyValue<bool>(content, "umbracoNaviHide");
		}
	}

	[TestFixture]
	internal class NavigationElementFactoryTests
	{
		[Test]
		public void Make_Returns_UmbracoNavigationElement_For_Page()
		{
			var element = new UmbracoNavigationElement();

			var wrapperMock = new Mock<IUmbracoWrapper>();
			var contentMock = new Mock<IPublishedContent>();
			contentMock.Setup(c => c.ContentType.Alias).Returns("page");

			var modelConverterMock = new Mock<IModelConverter>();
			modelConverterMock.Setup(c => c.ToModel<UmbracoNavigationElement>(It.IsAny<IPublishedContent>(), null))
				.Returns(element);

			var factory = new NavigationElementFactory(modelConverterMock.Object, wrapperMock.Object);
			var result = factory.Make(contentMock.Object);

			Assert.That(result, Is.EqualTo(element));
		}

		[Test]
		public void Make_Returns_UmbracoNavigationRedirectElement_For_Redirect()
		{
			var element = new UmbracoNavigationRedirectElement();

			var wrapperMock = new Mock<IUmbracoWrapper>();
			var contentMock = new Mock<IPublishedContent>();
			contentMock.Setup(c => c.DocumentTypeAlias).Returns("redirect");

			var modelConverterMock = new Mock<IModelConverter>();
			modelConverterMock.Setup(c => c.ToModel<UmbracoNavigationRedirectElement>(It.IsAny<IPublishedContent>(), null))
				.Returns(element);

			var factory = new NavigationElementFactory(modelConverterMock.Object, wrapperMock.Object);
			var result = factory.Make(contentMock.Object);

			Assert.That(result, Is.EqualTo(element));
		}
	}
}
