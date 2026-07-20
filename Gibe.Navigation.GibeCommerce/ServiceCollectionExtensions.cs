using Gibe.Navigation;
using GibeCommerce.CatalogSystem;
using GibeCommerce.SiteServices.UrlProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Gibe.Navigation.GibeCommerce
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddGibeCommerceNavigation(this IServiceCollection services, int priority)
		{
			services.AddSingleton<INavigationProvider>(sp => new GibeCommerceNavigationProvider(
				sp.GetRequiredService<ICatalogService>(),
				priority,
				sp.GetRequiredService<IUrlProvider>()));

			return services;
		}
	}
}
