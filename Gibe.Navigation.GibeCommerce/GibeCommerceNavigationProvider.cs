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
        // URL provider

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
            var parent =
                categories.First(x => IncludeInNavigation(x) && _urlProvider.GetUrl(x, UrlProviderMode.Relative) == url);
            var children = _catalogService.GetSubCategories(parent.Name);

            return new SubNavigationModel<T>
            {
                SectionParent = ToNavigationElement(parent),
                NavigationElements = children.Select(x => (T) ToNavigationElement(x))
            };
        }

        public int Priority { get; }

        private INavigationElement ToNavigationElement(Category category)
        {
            return new GibeCommerceNavigationElement
            {
                Title = category.PageTitle,
                IsActive = false,
                IsVisible = ShowInNavigation(category),
                NavTitle = category.DisplayName,
                Items = _catalogService.GetSubCategories(category.Name).Select(ToNavigationElement),
                Url = _urlProvider.GetUrl(category, UrlProviderMode.Relative)
            };
        }

        protected virtual bool IncludeInNavigation(Category category)
        {
            return true;
        }

        protected virtual bool ShowInNavigation(Category category)
        {
            return true;
        }
    }
}