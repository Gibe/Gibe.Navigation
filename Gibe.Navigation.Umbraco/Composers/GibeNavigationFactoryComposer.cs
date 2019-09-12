using Umbraco.Core.Composing;
using Umbraco.Core;

namespace Gibe.Navigation.Umbraco.Composers
{
	public class GibeNavigationFactoryComposer : IUserComposer
	{
		public void Compose(Composition composition)
		{
			composition.Register<INavigationElementFactory, NavigationElementFactory>();
		}
	}
}
