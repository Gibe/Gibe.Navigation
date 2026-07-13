using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Gibe.Navigation.Umbraco
{
	public class NavigationComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Services.AddNavigation();
		}
	}
}
