using System.Collections.Generic;
using System.Linq;
using Gibe.DittoProcessors.Processors;
using Gibe.DittoProcessors.Processors.Models;
using Gibe.Navigation.Models;
using NUnit.Framework;
using Our.Umbraco.Ditto;

namespace Gibe.Navigation.Umbraco.Models
{
	public class UmbracoNavigationRedirectElement : INavigationElement
	{
		[UmbracoProperty("Name")]
		public string Title { get; set; }

		public string NavTitle { get; set; }

		[UmbracoProperty("redirect")]
		[LinkPicker]
		public LinkPickerModel Redirect { get; set; }

		[DittoIgnore]
		public string Url => Redirect?.Url;

		public bool IsActive { get; set; }
		public IEnumerable<INavigationElement> Items { get; set; }

		[DittoIgnore]
		public string Target => Redirect?.Target;

		public bool IsVisible { get; set; }
		public bool IsConcrete => false;
		public bool HasVisibleChildren => Items.Any(x => x.IsVisible);

		public object Clone()
		{
			return new UmbracoNavigationRedirectElement
			{
				Title = Title,
				NavTitle = NavTitle,
				Redirect = Redirect,
				IsActive = IsActive,
				Items = Items.Select(i => (INavigationElement)i.Clone()).ToList(),
				IsVisible = IsVisible,
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
			var element = new UmbracoNavigationRedirectElement
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
			var element = new UmbracoNavigationRedirectElement
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
			var element = new UmbracoNavigationRedirectElement
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
