using Gibe.Navigation.Umbraco.Filters;
using Gibe.Navigation.Umbraco.NodeTypes;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Gibe.Navigation.Umbraco.Composers
{
	public class GibeNavigationComposer : IUserComposer
	{
		public void Compose(Composition composition)
		{
			composition.Register<INavigationService, DefaultNavigationService>();
			composition.Register<INodeTypeFactory, DefaultNodeTypeFactory>(Lifetime.Singleton);
			composition.Register<INavigationProvider, UmbracoNavigationProvider>();
			composition.Register<IUmbracoNodeService, UmbracoNodeService>();
			composition.Register<INavigationFilter, TemplateOrRedirectFilter>();

			composition.Register<INodeType, SettingsNodeType>();

		}
	}
}
