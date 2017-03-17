using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Models;
using NUnit.Framework;
using Our.Umbraco.Ditto;

namespace Gibe.Navigation.Umbraco.Models
{
	public class UmbracoNavigationElement : INavigationElement
	{
		public UmbracoNavigationElement()
		{
			Items = new List<INavigationElement>();
		}

		[UmbracoProperty("Name")]
		public string Title { get; set; }
		public string NavTitle { get; set; }
		public string Url { get; set; }
		public bool IsActive { get; set; }
		public IEnumerable<INavigationElement> Items { get; set; }
		public string Target => "_self";
		public bool IsVisible { get; set; }
		public bool IsConcrete => true;
		public bool HasVisibleChildren => Items.Any(x => x.IsVisible);

		public object Clone()
		{
			return new UmbracoNavigationElement
			{
				Title = Title,
				NavTitle = NavTitle,
				Url = Url,
				IsActive = IsActive,
				Items = Items.Select(i => (INavigationElement)i.Clone()).ToList(),
				IsVisible = IsVisible
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
			var element = new UmbracoNavigationElement
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement()
				}
			};
			var clone = element.Clone();

			Assert.That(clone.Equals(element));
			Assert.That(!ReferenceEquals(clone, element));
		}

		[Test]
		public void HasVisibleChildren_Returns_True_If_At_Least_One_Visible_Child()
		{
			var element = new UmbracoNavigationElement
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement {IsVisible = true},
					new UmbracoNavigationElement {IsVisible = false}
				}
			};

			Assert.That(element.HasVisibleChildren, Is.True);
		}

		[Test]
		public void HasVisibleChildren_Returns_False_If_At_No_Visible_Child()
		{
			var element = new UmbracoNavigationElement
			{
				Items = new List<INavigationElement>
				{
					new UmbracoNavigationElement {IsVisible = false},
					new UmbracoNavigationElement {IsVisible = false}
				}
			};

			Assert.That(element.HasVisibleChildren, Is.False);
		}
	}
}