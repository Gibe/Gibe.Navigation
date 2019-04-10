using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Filters;
using Umbraco.Core.Composing;

namespace Gibe.Navigation.Umbraco
{
	public class GibeNavigationComposer : IComposer
	{
		public void Compose(Composition composition)
		{
			composition.RegisterFor<INodeTypeFactory, DefaultNodeTypeFactory>();
			composition.RegisterFor<INavigationElementFactory, NavigationElementFactory>();
			composition.RegisterFor<INavigationProvider<INavigationElement>, UmbracoNavigationProvider<INavigationElement>>();
			composition.RegisterFor<IUmbracoNodeService, UmbracoNodeService>();
			composition.RegisterFor<INavigationFilter, TemplateOrRedirectFilter>();
		}
	}
}
