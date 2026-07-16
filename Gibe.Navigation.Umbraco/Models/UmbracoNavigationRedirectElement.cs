using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace Gibe.Navigation.Umbraco.Models
{
	public class UmbracoNavigationRedirectElement : PublishedContentModel, INavigationElement
	{
		private readonly IPublishedValueFallback _publishedValueFallback;

		public UmbracoNavigationRedirectElement(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
			Items = new List<INavigationElement>();
			ExtraProperties = new Dictionary<string, object>();
		}

		public string Title => base.Name;

		public string NavTitle => this.Value<string>(_publishedValueFallback, "NavTitle")!;

		private Link? Redirect => this.Value<Link>(_publishedValueFallback, "gibeNavigationRedirect");

		public string Url => Redirect?.Url!;

		public bool IsActive { get; set; }
		public IEnumerable<INavigationElement> Items { get; set; }

		public string Target => Redirect?.Target!;

		public bool IsVisible => !this.HasValue("umbracoNaviHide") ||
		                         !this.Value<bool>(_publishedValueFallback, "umbracoNaviHide");
		public bool IsConcrete => false;
		public bool HasVisibleChildren => Items.Any(x => x.IsVisible);

		public Dictionary<string, object> ExtraProperties { get; set; }

		public object Clone()
		{
			return new UmbracoNavigationRedirectElement(this, _publishedValueFallback)
			{
				IsActive = IsActive,
				Items = Items.Select(i => (INavigationElement)i.Clone()).ToList(),
			};
		}

		public override bool Equals(object? obj)
		{
			return obj is UmbracoNavigationRedirectElement && Equals((UmbracoNavigationRedirectElement)obj);
		}

		protected bool Equals(UmbracoNavigationRedirectElement other)
		{
			return
				string.Equals(Title, other.Title) &&
				string.Equals(NavTitle, other.NavTitle) &&
				Equals(Redirect, other.Redirect) &&
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

}
