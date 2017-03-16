using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Filters;
using Ninject.Modules;

namespace Gibe.Navigation.Umbraco
{
	public class DefaultBindings : NinjectModule
	{
		public override void Load()
		{
			Bind<INodeTypeFactory>().To<DefaultNodeTypeFactory>();
			Bind<INavigationElementFactory>().To<NavigationElementFactory>();
			Bind<INavigationProvider<INavigationElement>>().To<UmbracoNavigationProvider<INavigationElement>>();
			Bind<IUmbracoNodeService>().To<UmbracoNodeService>();
			Bind<INavigationFilter>().To<NotRedirectAndNoTemplateFilter>();
		}
	}
}
