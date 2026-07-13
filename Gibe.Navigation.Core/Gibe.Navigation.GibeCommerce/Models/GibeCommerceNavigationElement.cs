using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Core.Models;

namespace Gibe.Navigation.GibeCommerce.Models
{
	public class GibeCommerceNavigationElement : INavigationElement
	{
		public object Clone() => new GibeCommerceNavigationElement
		{
			Title = Title,
			NavTitle = NavTitle,
			Url = Url,
			IsActive = IsActive,
			Items = Items.Select(i => (INavigationElement) i.Clone()).ToList(),
			IsVisible = IsVisible
		};

		public required string Title { get; set; }
		public required string NavTitle { get; set; }
		public required string Url { get; set; }
		public bool IsActive { get; set; }
		public required IEnumerable<INavigationElement> Items { get; set; }
		public string Target => "_self";
		public bool IsVisible { get; set; }
		public bool IsConcrete => true;
		public bool HasVisibleChildren => Items.Any(x => x.IsVisible);
		public Dictionary<string, object> ExtraProperties { get; set; } = new();
	}
}