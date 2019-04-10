using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Filters;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Gibe.Navigation.Umbraco
{
	public class GibeNavigationComposer : IComposer
	{
		public void Compose(Composition composition)
		{
			composition.Register<INodeTypeFactory, DefaultNodeTypeFactory>();
			composition.Register<INavigationElementFactory, NavigationElementFactory>();
			composition.Register<INavigationProvider<INavigationElement>, UmbracoNavigationProvider<INavigationElement>>();
			composition.Register<IUmbracoNodeService, UmbracoNodeService>();
			composition.Register<INavigationFilter, TemplateOrRedirectFilter>();
		}
	}
}
