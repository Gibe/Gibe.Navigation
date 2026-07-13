using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Core.Models;
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

}
