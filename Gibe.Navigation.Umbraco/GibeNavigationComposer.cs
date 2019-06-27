﻿using Gibe.Navigation.Models;
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
			composition.Register<INavigationElementFactory, NavigationElementFactory>();
			composition.Register<INodeTypeFactory, DefaultNodeTypeFactory>();
			composition.Register<IUmbracoNodeService, UmbracoNodeService>();
			composition.Register<INavigationFilter, TemplateOrRedirectFilter>();
			composition.Register<INavigationProvider<INavigationElement>, UmbracoNavigationProvider<INavigationElement>>();
		}
	}
}
