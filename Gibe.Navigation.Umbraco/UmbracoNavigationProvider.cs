using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation;
using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.NodeTypes;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Extensions;

namespace Gibe.Navigation.Umbraco
{
	public class UmbracoNavigationProvider : INavigationProvider
	{
		private readonly INavigationElementFactory _navigationElementFactory;
		private readonly INodeTypeFactory _nodeTypeFactory;
		private readonly Type _rootNodeType;
		private readonly IUmbracoNodeService _umbracoNodeService;
		private readonly IEnumerable<INavigationFilter> _filters;
		private readonly IDocumentNavigationQueryService _navigationQueryService;
		private readonly IPublishedStatusFilteringService _publishedStatusFilteringService;

		public UmbracoNavigationProvider(
			IUmbracoNodeService umbracoNodeService,
			INodeTypeFactory nodeTypeFactory,
			INavigationElementFactory navigationElementFactory,
			IDocumentNavigationQueryService navigationQueryService,
			IPublishedStatusFilteringService publishedStatusFilteringService)
			:
				this(umbracoNodeService, nodeTypeFactory, navigationElementFactory, navigationQueryService, publishedStatusFilteringService, null, 1)

		{

		}

		public UmbracoNavigationProvider(
				IUmbracoNodeService umbracoNodeService,
				INodeTypeFactory nodeTypeFactory,
				INavigationElementFactory navigationElementFactory,
				IDocumentNavigationQueryService navigationQueryService,
				IPublishedStatusFilteringService publishedStatusFilteringService,
				IEnumerable<INavigationFilter>? filters = null,
				int priority = 1)
				: this(
						umbracoNodeService,
						nodeTypeFactory,
						typeof(SettingsNodeType),
						filters??Enumerable.Empty<INavigationFilter>(),
						navigationElementFactory,
						navigationQueryService,
						publishedStatusFilteringService,
						priority)
		{
		}

		public UmbracoNavigationProvider(
				IUmbracoNodeService umbracoNodeService,
				INodeTypeFactory nodeTypeFactory,
				Type rootNodeType,
				IEnumerable<INavigationFilter> filters,
				INavigationElementFactory navigationElementFactory,
				IDocumentNavigationQueryService navigationQueryService,
				IPublishedStatusFilteringService publishedStatusFilteringService,
				int priority = 1)
		{
			_umbracoNodeService = umbracoNodeService;
			_nodeTypeFactory = nodeTypeFactory;
			Priority = priority;
			_rootNodeType = rootNodeType;
			_filters = filters;
			_navigationElementFactory = navigationElementFactory;
			_navigationQueryService = navigationQueryService;
			_publishedStatusFilteringService = publishedStatusFilteringService;
		}

		public int Priority { get; }

		public IEnumerable<INavigationElement> NavigationElements()
		{
			var topLevel = _umbracoNodeService.GetNode(_nodeTypeFactory.GetNodeType(_rootNodeType));
			if (topLevel == null)
			{
				return Enumerable.Empty<INavigationElement>();
			}
			return NavigationElements(topLevel);
		}

		public IEnumerable<INavigationElement> NavigationElements(IPublishedContent content)
		{
			var children = content
				.Children(_navigationQueryService, _publishedStatusFilteringService)
				.Where(IncludeInNavigation);
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
