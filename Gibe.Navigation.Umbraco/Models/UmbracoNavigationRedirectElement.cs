using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.DittoProcessors.Processors;
using Gibe.DittoProcessors.Processors.Models;
using Gibe.Navigation.Models;
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
		public string Url => Redirect.Url;

		public bool IsActive { get; set; }
		public IEnumerable<INavigationElement> Items { get; set; }

		[DittoIgnore]
		public string Target => Redirect.Target;

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
	}
}
