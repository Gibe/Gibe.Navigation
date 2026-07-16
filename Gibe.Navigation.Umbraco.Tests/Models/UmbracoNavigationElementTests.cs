using System.Collections.Generic;
using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Models;
using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace Gibe.Navigation.Umbraco.Tests.Models
{
	[TestFixture]
	public class UmbracoNavigationElementTests
	{
		[Test]
		public void Clone_Returns_Clone_Of_Element()
		{
			var element = new UmbracoNavigationElement(FakePublishedContent(), UrlProvider(), Fallback())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent(), UrlProvider(), Fallback())
				}
			};
			var clone = (UmbracoNavigationElement)element.Clone();

			Assert.That(clone.Equals(element));
			Assert.That(!ReferenceEquals(clone, element));
		}

		[Test]
		public void HasVisibleChildren_Returns_True_If_At_Least_One_Visible_Child()
		{
			var element = new UmbracoNavigationElement(FakePublishedContent(), UrlProvider(), Fallback())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent(), UrlProvider(), Fallback()),
					new UmbracoNavigationElement(FakePublishedContent(false), UrlProvider(), Fallback())
				}
			};

			Assert.That(element.HasVisibleChildren, Is.True);
		}

		[Test]
		public void HasVisibleChildren_Returns_False_If_At_No_Visible_Child()
		{
			var element = new UmbracoNavigationElement(FakePublishedContent(), UrlProvider(), Fallback())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent(false), UrlProvider(), Fallback()),
					new UmbracoNavigationElement(FakePublishedContent(false), UrlProvider(), Fallback())
				}
			};

			Assert.That(element.HasVisibleChildren, Is.False);
		}

		public static IPublishedContent FakePublishedContent(bool visible = true)
		{
			var contentMock = new Mock<IPublishedContent>();
			contentMock.Setup(c => c.ContentType.ItemType).Returns(PublishedItemType.Content);

			if (visible)
			{
				contentMock.Setup(c => c.GetProperty("umbracoNaviHide")).Returns((IPublishedProperty?)null);
			}
			else
			{
				var hiddenPropertyMock = new Mock<IPublishedProperty>();
				hiddenPropertyMock.Setup(p => p.HasValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
				hiddenPropertyMock.Setup(p => p.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
				contentMock.Setup(c => c.GetProperty("umbracoNaviHide")).Returns(hiddenPropertyMock.Object);
			}

			return contentMock.Object;
		}

		public static IPublishedUrlProvider UrlProvider()
		{
			return new Mock<IPublishedUrlProvider>().Object;
		}

		public static IPublishedValueFallback Fallback()
		{
			return new NoopPublishedValueFallback();
		}
	}

}
