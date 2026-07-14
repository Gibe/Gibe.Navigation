using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Models;
using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace Gibe.Navigation.Umbraco.Models
{
	public class UmbracoNavigationElement : PublishedContentModel, INavigationElement
	{
		private static readonly NoopPublishedValueFallback Fallback = new();

		private readonly IPublishedUrlProvider _publishedUrlProvider;

		public UmbracoNavigationElement(IPublishedContent content, IPublishedUrlProvider publishedUrlProvider) : base(content, Fallback)
		{
			_publishedUrlProvider = publishedUrlProvider;
			Items = new List<INavigationElement>();
			ExtraProperties = new Dictionary<string, object>();
		}

		public string Title => base.Name;
		public string NavTitle => this.Value<string>(Fallback, "NavTitle")!;
		public bool IsActive { get; set; }
		public IEnumerable<INavigationElement> Items { get; set; }
		public string Target => "_self";
		public bool IsVisible => !this.HasValue("umbracoNaviHide") ||
		                         !this.Value<bool>(Fallback, "umbracoNaviHide");
		public bool IsConcrete => true;
		public bool HasVisibleChildren => Items.Any(x => x.IsVisible);
		public string Url => this.Url(_publishedUrlProvider);

		public Dictionary<string, object> ExtraProperties { get; set; }

		public object Clone()
		{
			return new UmbracoNavigationElement(this, _publishedUrlProvider)
			{
				IsActive = IsActive,
				Items = Items.Select(i => (INavigationElement)i.Clone()).ToList(),
				ExtraProperties = ExtraProperties
			};
		}

		public override bool Equals(object? obj)
		{
			return obj is UmbracoNavigationElement && Equals((UmbracoNavigationElement)obj);
		}

		protected bool Equals(UmbracoNavigationElement other)
		{
			return
				string.Equals(Title, other.Title) &&
				string.Equals(NavTitle, other.NavTitle) &&
				string.Equals(Url, other.Url) &&
				IsActive == other.IsActive &&
				Items.SequenceEqual(other.Items) &&
				IsVisible == other.IsVisible;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Title?.GetHashCode() ?? 0;
				hashCode = (hashCode * 397) ^ (NavTitle?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Url?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ IsActive.GetHashCode();
				hashCode = (hashCode * 397) ^ (Items?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ IsVisible.GetHashCode();
				return hashCode;
			}
		}
	}

	[TestFixture]
	internal class UmbracoNavigationElementTests
	{
		[Test]
		public void Clone_Returns_Clone_Of_Element()
		{
			var element = new UmbracoNavigationElement(FakePublishedContent(), UrlProvider())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent(), UrlProvider())
				}
			};
			var clone = (UmbracoNavigationElement)element.Clone();

			Assert.That(clone.Equals(element));
			Assert.That(!ReferenceEquals(clone, element));
		}

		[Test]
		public void HasVisibleChildren_Returns_True_If_At_Least_One_Visible_Child()
		{
			var element = new UmbracoNavigationElement(FakePublishedContent(), UrlProvider())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent(), UrlProvider()),
					new UmbracoNavigationElement(FakePublishedContent(false), UrlProvider())
				}
			};

			Assert.That(element.HasVisibleChildren, Is.True);
		}

		[Test]
		public void HasVisibleChildren_Returns_False_If_At_No_Visible_Child()
		{
			var element = new UmbracoNavigationElement(FakePublishedContent(), UrlProvider())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent(false), UrlProvider()),
					new UmbracoNavigationElement(FakePublishedContent(false), UrlProvider())
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
	}

}
