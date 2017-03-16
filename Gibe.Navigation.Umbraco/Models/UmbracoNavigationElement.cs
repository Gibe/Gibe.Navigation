using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Models;
using Our.Umbraco.Ditto;

namespace Gibe.Navigation.Umbraco.Models
{
	public class UmbracoNavigationElement : INavigationElement
	{
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
	}
}