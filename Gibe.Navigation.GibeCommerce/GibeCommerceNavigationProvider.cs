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

		public SubNavigationModel<T> GetSubNavigation(string url)
		{
			var categories = _catalogService.GetAllCategories();
			var parent = categories.First(x => IncludeInNavigation(x) && _urlProvider.GetUrl(x, UrlProviderMode.Relative) == url);
			var children = _catalogService.GetSubCategories(parent.Name).Where(IncludeInNavigation);

			return new SubNavigationModel<T>
			{
				SectionParent = ToNavigationElement(parent),
				NavigationElements = children.OrderBy(x => x.Rank).Select(x => (T) ToNavigationElement(x)).ToList()
			};
		}

		public int Priority { get; }

		private INavigationElement ToNavigationElement(ICategory category)
		{
			return new GibeCommerceNavigationElement
			{
				Title = category.GetAttribute("PageTitle", string.Empty),
				IsVisible = ShowInNavigation(category),
				NavTitle = category.DisplayName,
				Items = _catalogService.GetSubCategories(category.Name).OrderBy(x => x.Rank).Select(ToNavigationElement).ToList(),
				Url = _urlProvider.GetUrl(category, UrlProviderMode.Relative)
			};
		}

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