using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace Gibe.Navigation.Umbraco.Models
{
	public class UmbracoNavigationElement : PublishedContentModel, INavigationElement
	{
		private readonly IPublishedUrlProvider _publishedUrlProvider;
		private readonly IPublishedValueFallback _publishedValueFallback;

		public UmbracoNavigationElement(IPublishedContent content, IPublishedUrlProvider publishedUrlProvider, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
		{
			_publishedUrlProvider = publishedUrlProvider;
			_publishedValueFallback = publishedValueFallback;
			Items = new List<INavigationElement>();
			ExtraProperties = new Dictionary<string, object>();
		}

		public string Title => base.Name;
		public string NavTitle => this.Value<string>(_publishedValueFallback, "NavTitle")!;
		public bool IsActive { get; set; }
		public IEnumerable<INavigationElement> Items { get; set; }
		public string Target => "_self";
		public bool IsVisible => !this.HasValue("umbracoNaviHide") ||
		                         !this.Value<bool>(_publishedValueFallback, "umbracoNaviHide");
		public bool IsConcrete => true;
		public bool HasVisibleChildren => Items.Any(x => x.IsVisible);
		public string Url => this.Url(_publishedUrlProvider);

		public Dictionary<string, object> ExtraProperties { get; set; }

		public object Clone()
		{
			return new UmbracoNavigationElement(this, _publishedUrlProvider, _publishedValueFallback)
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

}
