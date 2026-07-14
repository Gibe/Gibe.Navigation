using Gibe.Navigation.Umbraco.Filters;
using Gibe.Navigation.Umbraco.NodeTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Gibe.Navigation.Umbraco
{
	public static class GibeNavigationServiceCollectionExtensions
	{
		public static void AddNavigation(this IServiceCollection services)
		{
			services.AddGibeNavigation();

			services.AddTransient<INodeTypeFactory, DefaultNodeTypeFactory>();
			services.AddTransient<INavigationProvider, UmbracoNavigationProvider>();
			services.AddTransient<IUmbracoNodeService, UmbracoNodeService>();
			services.AddTransient<INavigationFilter, TemplateOrRedirectFilter>();
			services.AddTransient<INavigationElementFactory, NavigationElementFactory>();

			services.AddTransient<INodeType, SettingsNodeType>();
		}
	}
}
