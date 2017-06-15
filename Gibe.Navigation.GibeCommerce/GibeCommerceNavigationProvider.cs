using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.GibeCommerce.Models;
using Gibe.Navigation.Models;
using GibeCommerce.CatalogSystem;
using GibeCommerce.SiteServices.UrlProviders;

namespace Gibe.Navigation.GibeCommerce
{
	public class GibeCommerceNavigationProvider<T> : INavigationProvider<T> where T : INavigationElement
	{
		private readonly ICatalogService _catalogService;
		private readonly IUrlProvider _urlProvider;

		public GibeCommerceNavigationProvider(ICatalogService catalogService, int priority, IUrlProvider urlProvider)
		{
			_catalogService = catalogService;
			Priority = priority;
			_urlProvider = urlProvider;
		}

		public IEnumerable<T> GetNavigationElements()
		{
			var categories = _catalogService.GetSubCategories("root")
				.Where(x => x.Name != "root" && IncludeInNavigation(x));

			return categories.OrderBy(x => x.Rank).Select(x => (T) ToNavigationElement(x)).ToList();
		}
		
		public int Priority { get; }

		private INavigationElement ToNavigationElement(ICategory category)
		{
			return new GibeCommerceNavigationElement
			{
				Title = string.IsNullOrEmpty(NavTitle(category)) ? category.DisplayName : NavTitle(category),
				IsVisible = ShowInNavigation(category),
				NavTitle = NavTitle(category),
				Items = _catalogService.GetSubCategories(category.Name).OrderBy(x => x.Rank).Select(ToNavigationElement).ToList(),
				Url = _urlProvider.GetUrl(category, UrlProviderMode.Relative)
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