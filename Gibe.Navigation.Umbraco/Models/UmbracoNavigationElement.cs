using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Models;
using Umbraco.Web;
using Gibe.UmbracoWrappers;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco.Models
{
	public class UmbracoNavigationElement : PublishedContentModel, INavigationElement
	{
		public UmbracoNavigationElement(IPublishedContent content) : base(content)
		{
			Items = new List<INavigationElement>();
		}

		public string Title => this.Value<string>("Name");
		public string NavTitle => this.Value<string>("NavTitle");
		public bool IsActive { get; set; }
		public IEnumerable<INavigationElement> Items { get; set; }
		public string Target => "_self";
		public bool IsVisible => this.HasValue("umbracoNaviHide") && this.Value<bool>("umbracoNaviHide");
		public bool IsConcrete => true;
		public bool HasVisibleChildren => Items.Any(x => x.IsVisible);

		public object Clone()
		{
			return new UmbracoNavigationElement(this)
			{
				IsActive = IsActive,
				Items = Items.Select(i => (INavigationElement)i.Clone()).ToList(),
			};
		}

		public override bool Equals(object obj)
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
			var element = new UmbracoNavigationElement(FakePublishedContent())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent())
				}
			};
			var clone = element.Clone();

			Assert.That(clone.Equals(element));
			Assert.That(!ReferenceEquals(clone, element));
		}

		[Test]
		public void HasVisibleChildren_Returns_True_If_At_Least_One_Visible_Child()
		{
			var element = new UmbracoNavigationElement(FakePublishedContent())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent()),
					new UmbracoNavigationElement(FakePublishedContent(false))
				}
			};

			Assert.That(element.HasVisibleChildren, Is.True);
		}

		[Test]
		public void HasVisibleChildren_Returns_False_If_At_No_Visible_Child()
		{
			var element = new UmbracoNavigationElement(FakePublishedContent())
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement(FakePublishedContent(false)),
					new UmbracoNavigationElement(FakePublishedContent(false))
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