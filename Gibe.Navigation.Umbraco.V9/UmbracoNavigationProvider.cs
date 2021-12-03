using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.NodeTypes;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco
{
	public class UmbracoNavigationProvider : INavigationProvider
	{
		private readonly INavigationElementFactory _navigationElementFactory;
		private readonly INodeTypeFactory _nodeTypeFactory;
		private readonly Type _rootNodeType;
		private readonly IUmbracoNodeService _umbracoNodeService;
		private readonly IEnumerable<INavigationFilter> _filters;

		public UmbracoNavigationProvider(
			IUmbracoNodeService umbracoNodeService,
			INodeTypeFactory nodeTypeFactory,
			INavigationElementFactory navigationElementFactory)
			: 
				this(umbracoNodeService, nodeTypeFactory, navigationElementFactory, null, 1)
		
		{

		}

		public UmbracoNavigationProvider(
				IUmbracoNodeService umbracoNodeService,
				INodeTypeFactory nodeTypeFactory,
				INavigationElementFactory navigationElementFactory,
				IEnumerable<INavigationFilter> filters = null,
				int priority = 1)
				: this(
						umbracoNodeService, 
						nodeTypeFactory, 
						typeof(SettingsNodeType), 
						filters??Enumerable.Empty<INavigationFilter>(), 
						navigationElementFactory,
						priority)
		{
		}

		public UmbracoNavigationProvider(
				IUmbracoNodeService umbracoNodeService,
				INodeTypeFactory nodeTypeFactory,
				Type rootNodeType,
				IEnumerable<INavigationFilter> filters,
				INavigationElementFactory navigationElementFactory,
				int priority = 1)
		{
			_umbracoNodeService = umbracoNodeService;
			_nodeTypeFactory = nodeTypeFactory;
			Priority = priority;
			_rootNodeType = rootNodeType;
			_filters = filters;
			_navigationElementFactory = navigationElementFactory;
		}

		public int Priority { get; }

		public IEnumerable<INavigationElement> NavigationElements()
		{
			var topLevel = _umbracoNodeService.GetNode(_nodeTypeFactory.GetNodeType(_rootNodeType));
			return NavigationElements(topLevel);
		}
		
		public IEnumerable<INavigationElement> NavigationElements(IPublishedContent content)
		{
			var children = content.Children.Where(IncludeInNavigation);
			var navItems = children.Select(ToNavigationElement);
			return navItems.ToList();
		}

		private INavigationElement ToNavigationElement(IPublishedContent content)
		{
			var model = _navigationElementFactory.Make(content);
			model.Items = NavigationElements(content).ToList();
			return model;
		}

		private bool IncludeInNavigation(IPublishedContent content)
		{
			return _filters.All(filter => filter.IncludeInNavigation(content));
		}
	}
}