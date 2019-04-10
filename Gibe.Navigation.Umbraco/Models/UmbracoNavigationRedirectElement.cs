using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Models;
using Gibe.UmbracoWrappers;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Gibe.Navigation.Umbraco.Models
{
	public class UmbracoNavigationRedirectElement : PublishedContentModel, INavigationElement
	{
		private readonly IUmbracoWrapper _umbracoWrapper;

		public UmbracoNavigationRedirectElement(IPublishedContent content, IUmbracoWrapper umbracoWrapper) : base(content)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		public string Title => _umbracoWrapper.Value<string>(this, "Name");

		public string NavTitle => _umbracoWrapper.Value<string>(this, "NavTitle");

		private Link Redirect => _umbracoWrapper.Value<Link>(this, "gibeNavigationRedirect");

		public new string Url => Redirect?.Url;

		public bool IsActive { get; set; }
		public IEnumerable<INavigationElement> Items { get; set; }

		public string Target => Redirect?.Target;

		public bool IsVisible => _umbracoWrapper.HasValue(this, "umbracoNaviHide") && _umbracoWrapper.Value<bool>(this, "umbracoNaviHide");
		public bool IsConcrete => false;
		public bool HasVisibleChildren => Items.Any(x => x.IsVisible);

		public object Clone()
		{
			return new UmbracoNavigationRedirectElement(this, _umbracoWrapper)
			{
				IsActive = IsActive,
				Items = Items.Select(i => (INavigationElement)i.Clone()).ToList(),
			};
		}

		public override bool Equals(object obj)
		{
			return obj is UmbracoNavigationRedirectElement && Equals((UmbracoNavigationRedirectElement)obj);
		}

		protected bool Equals(UmbracoNavigationRedirectElement other)
		{
			return
				string.Equals(Title, other.Title) &&
				string.Equals(NavTitle, other.NavTitle) &&
				string.Equals(Redirect, other.Redirect) &&
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
				hashCode = (hashCode * 397) ^ (Redirect?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ IsActive.GetHashCode();
				hashCode = (hashCode * 397) ^ (Items?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ IsVisible.GetHashCode();
				return hashCode;
			}
		}

		
	}

	[TestFixture]
	internal class UmbracoNavigationRedirectElementTests
	{
		[Test]
		public void Clone_Returns_Clone_Of_Element()
		{
			var element = new UmbracoNavigationRedirectElement(FakePublishedContent(), UmbracoWrapper())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent(), UmbracoWrapper())
				}
			};
			var clone = element.Clone();

			Assert.That(clone.Equals(element));
			Assert.That(!ReferenceEquals(clone, element));
		}

		[Test]
		public void HasVisibleChildren_Returns_True_If_At_Least_One_Visible_Child()
		{
			var element = new UmbracoNavigationRedirectElement(FakePublishedContent(), UmbracoWrapper())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent(), UmbracoWrapper()),
					new UmbracoNavigationElement(FakePublishedContent(false), UmbracoWrapper())
				}
			};

			Assert.That(element.HasVisibleChildren, Is.True);
		}

		[Test]
		public void HasVisibleChildren_Returns_False_If_At_No_Visible_Child()
		{
			var element = new UmbracoNavigationRedirectElement(FakePublishedContent(), UmbracoWrapper())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent(false), UmbracoWrapper()),
					new UmbracoNavigationElement(FakePublishedContent(false), UmbracoWrapper())
				}
			};

			Assert.That(element.HasVisibleChildren, Is.False);
		}

		public IPublishedContent FakePublishedContent(bool visible = true)
		{
			var properties = new List<IPublishedProperty>();

			if (visible)
			{
				properties.Add(VisibleProperty());
			}

			var pc = new Mock<IPublishedContent>();
			pc.Setup(p => p.Properties)
				.Returns(properties);
			return pc.Object;
		}

		public IPublishedProperty VisibleProperty()
		{
			var prop = new Mock<IPublishedProperty>();
			prop.Setup(p => p.Alias).Returns("umbracoNaviHide");
			return prop.Object;
		}

		public IUmbracoWrapper UmbracoWrapper()
		{
			var wrapper = new Mock<IUmbracoWrapper>();
			wrapper.Setup(w => w.HasValue(It.IsAny<IPublishedContent>(), "umbracoNaviHide"))
				.Returns((IPublishedContent content, string alias) => content.Properties.Any(p => p.Alias == "umbracoNaviHide"));
			wrapper.Setup(w => w.Value<bool>(It.IsAny<IPublishedContent>(), "umbracoNaviHide"))
				.Returns((IPublishedContent content, string alias) => content.Properties.Any(p => p.Alias == "umbracoNaviHide"));
			return wrapper.Object;
		}
	}
}
