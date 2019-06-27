using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Filters;
using Gibe.UmbracoWrappers;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Gibe.Navigation.Umbraco
{
	public class GibeNavigationComposer : IUserComposer
	{
		public void Compose(Composition composition)
		{
			composition.Register<INavigationService, DefaultNavigationService>();
			composition.Register<INavigationElementFactory, NavigationElementFactory>();
			composition.Register<INodeTypeFactory, DefaultNodeTypeFactory>();
			composition.Register<INavigationProvider, UmbracoNavigationProvider>();
			composition.Register<IUmbracoNodeService, UmbracoNodeService>();
			composition.Register<INavigationFilter, TemplateOrRedirectFilter>();
		}
	}
}
