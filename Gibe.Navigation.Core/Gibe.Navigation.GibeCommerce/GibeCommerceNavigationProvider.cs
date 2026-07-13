using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Core;
using Gibe.Navigation.Core.Models;
using Gibe.Navigation.GibeCommerce.Models;
using GibeCommerce.CatalogSystem;
using GibeCommerce.SiteServices.UrlProviders;

namespace Gibe.Navigation.GibeCommerce
{
	public class GibeCommerceNavigationProvider(
		ICatalogService catalogService, 
		int priority, 
		IUrlProvider urlProvider)
		: INavigationProvider
	{
		public IEnumerable<INavigationElement> NavigationElements()
		{
			var categories = catalogService.GetSubCategories("root")
				.Where(x => x.Name != "root" && IncludeInNavigation(x));

			return categories.OrderBy(x => x.Rank).Select(x => ToNavigationElement(x)).ToList();
		}
		
		public int Priority { get; } = priority;

		private INavigationElement ToNavigationElement(ICategory category)
		{
			return new GibeCommerceNavigationElement
			{
				Title = string.IsNullOrEmpty(NavTitle(category)) ? category.DisplayName : NavTitle(category),
				IsVisible = ShowInNavigation(category),
				NavTitle = NavTitle(category),
				Items = catalogService.GetSubCategories(category.Name).OrderBy(x => x.Rank).Select(ToNavigationElement).ToList(),
				Url = urlProvider.GetUrl(category, UrlProviderMode.Relative)
			};
		}

		private static string NavTitle(ICategory category) => category.GetAttribute("NavigationTitle", String.Empty);

		protected virtual bool IncludeInNavigation(ICategory category)
		{
			return true;
		}

		protected virtual bool ShowInNavigation(ICategory category)
		{
			return true;
		}
	}
}