using Microsoft.Extensions.DependencyInjection;

namespace Gibe.Navigation.Core
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddGibeNavigation(this IServiceCollection services)
		{
			services.AddMemoryCache();
			services.AddSingleton<ICache, MemoryCacheWrapper>();
			services.AddScoped<INavigationService, DefaultNavigationService>();

			return services;
		}
	}
}
