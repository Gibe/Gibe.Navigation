﻿using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Models;
using Gibe.UmbracoWrappers;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco
{
	public class NavigationElementFactory : INavigationElementFactory
	{
		private IUmbracoWrapper _umbracoWrapper;

		public NavigationElementFactory(IUmbracoWrapper umbracoWrapper)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		public INavigationElement Make(IPublishedContent content)
		{
			var model = IsRedirect(content)
					? new UmbracoNavigationRedirectElement(content, _umbracoWrapper)
					: (INavigationElement)new UmbracoNavigationElement(content, _umbracoWrapper);

			model = HyrdateExtraProperties(content, model);

			return model;
		}

		private bool IsRedirect(IPublishedContent content)
		{
			return _umbracoWrapper.HasValue(content, "gibeNavigationRedirect");
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
			var wrapperMock = new Mock<IUmbracoWrapper>();
			wrapperMock.Setup(w => w.HasValue(It.IsAny<IPublishedContent>(), "gibeNavigationRedirect"))
				.Returns(false);

			var contentMock = new Mock<IPublishedContent>();
			
			var factory = new NavigationElementFactory(wrapperMock.Object);
			var result = factory.Make(contentMock.Object);

			Assert.That(result, Is.TypeOf<UmbracoNavigationElement>());
		}

		[Test]
		public void Make_Returns_UmbracoNavigationRedirectElement_For_Redirect()
		{
			var wrapperMock = new Mock<IUmbracoWrapper>();
			wrapperMock.Setup(w => w.HasValue(It.IsAny<IPublishedContent>(), "gibeNavigationRedirect"))
				.Returns(true);

			var contentMock = new Mock<IPublishedContent>();
			
			var factory = new NavigationElementFactory(wrapperMock.Object);
			var result = factory.Make(contentMock.Object);

			Assert.That(result, Is.TypeOf<UmbracoNavigationRedirectElement>());
		}
	}
}
