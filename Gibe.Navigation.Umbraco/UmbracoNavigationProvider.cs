using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.NodeTypes;
using Umbraco.Core.Models.PublishedContent;

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
			INavigationElementFactory navigationElementFactory
			)
			: this(
				umbracoNodeService,
				nodeTypeFactory,
				1,
				typeof(SettingsNodeType),
				Enumerable.Empty<INavigationFilter>(),
				navigationElementFactory)
		{
		}

		public UmbracoNavigationProvider(
				IUmbracoNodeService umbracoNodeService,
				INodeTypeFactory nodeTypeFactory,
				IEnumerable<INavigationFilter> filters,
				INavigationElementFactory navigationElementFactory,
				int priority)
				: this(
						umbracoNodeService, 
						nodeTypeFactory, 
						priority, 
						typeof(SettingsNodeType), 
						filters, 
						navigationElementFactory)
		{
		}

		public UmbracoNavigationProvider(
				IUmbracoNodeService umbracoNodeService,
				INodeTypeFactory nodeTypeFactory,
				int priority,
				Type rootNodeType,
				IEnumerable<INavigationFilter> filters,
				INavigationElementFactory navigationElementFactory)
		{
			_umbracoNodeService = umbracoNodeService;
			_nodeTypeFactory = nodeTypeFactory;
			Priority = priority;
			_rootNodeType = rootNodeType;
			_filters = filters;
			_navigationElementFactory = navigationElementFactory;
		}

		public int Priority { get; }

		public IEnumerable<INavigationElement> GetNavigationElements()
		{
			var topLevel = _umbracoNodeService.GetNode(_nodeTypeFactory.GetNodeType(_rootNodeType));
			return GetNavigationElements(topLevel);
		}
		
		public IEnumerable<INavigationElement> GetNavigationElements(IPublishedContent content)
		{
			var children = content.Children.Where(IncludeInNavigation);
			var navItems = children.Select(ToNavigationElement);
			return navItems.ToList();
		}

		private INavigationElement ToNavigationElement(IPublishedContent content)
		{
			var model = _navigationElementFactory.Make(content);
			model.Items = GetNavigationElements(content).ToList();
			return model;
		}

		private bool IncludeInNavigation(IPublishedContent content)
		{
			return _filters.All(filter => filter.IncludeInNavigation(content));
		}
	}
}