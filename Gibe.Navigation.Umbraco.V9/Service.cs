using Gibe.Navigation.Umbraco.Filters;
using Gibe.Navigation.Umbraco.NodeTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Gibe.Navigation.Umbraco
{
	public static class Services
	{
		public static void AddNavigation(this IServiceCollection services)
		{
			services.AddTransient<INavigationService, DefaultNavigationService>();
			services.AddTransient<INodeTypeFactory, DefaultNodeTypeFactory>();
			services.AddTransient<INavigationProvider, UmbracoNavigationProvider>();
			services.AddTransient<IUmbracoNodeService, UmbracoNodeService>();
			services.AddTransient<INavigationFilter, TemplateOrRedirectFilter>();
			services.AddTransient<INavigationElementFactory, NavigationElementFactory>();

			services.AddTransient<INodeType, SettingsNodeType>();
			services.AddTransient<ICache, MemoryCacheWrapper>();
		}
	}
}
