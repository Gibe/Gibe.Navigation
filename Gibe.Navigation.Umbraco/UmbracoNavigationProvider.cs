using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.NodeTypes;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco
{
	public class UmbracoNavigationProvider<T> : INavigationProvider<T> where T : INavigationElement
	{
		private readonly INavigationElementFactory _navigationElementFactory;
		private readonly INodeTypeFactory _nodeTypeFactory;
		private readonly Type _rootNodeType;
		private readonly IUmbracoNodeService _umbracoNodeService;
		private readonly IUmbracoWrapper _umbracoWrapper;
		private readonly IEnumerable<INavigationFilter> _filters;

		public UmbracoNavigationProvider(
				IUmbracoNodeService umbracoNodeService,
				IUmbracoWrapper umbracoWrapper,
				INodeTypeFactory nodeTypeFactory,
				int priority,
				IEnumerable<INavigationFilter> filters,
				INavigationElementFactory navigationElementFactory)
				: this(
						umbracoNodeService, 
						umbracoWrapper,
						nodeTypeFactory, 
						priority, 
						typeof(SettingsNodeType), 
						filters, 
						navigationElementFactory)
		{
		}

		public UmbracoNavigationProvider(
				IUmbracoNodeService umbracoNodeService,
				IUmbracoWrapper umbracoWrapper,
				INodeTypeFactory nodeTypeFactory,
				int priority,
				Type rootNodeType,
				IEnumerable<INavigationFilter> filters,
				INavigationElementFactory navigationElementFactory)
		{
			_umbracoNodeService = umbracoNodeService;
			_umbracoWrapper = umbracoWrapper;
			_nodeTypeFactory = nodeTypeFactory;
			Priority = priority;
			_rootNodeType = rootNodeType;
			_filters = filters;
			_navigationElementFactory = navigationElementFactory;
		}

		public int Priority { get; }

		public IEnumerable<T> GetNavigationElements()
		{
			var topLevel = _umbracoNodeService.GetNode(_nodeTypeFactory.GetNodeType(_rootNodeType));
			return GetNavigationElements(topLevel);
		}

		public SubNavigationModel<T> GetSubNavigation(string url)
		{
			var content = _umbracoWrapper.TypedContent(url.TrimEnd('/'));

			if (content == null) return null;

			var model = new SubNavigationModel<T>();
			var topLevelParent = _umbracoWrapper.AncestorOrSelf(content, 2);
			model.SectionParent = _navigationElementFactory.Make(topLevelParent);
			model.NavigationElements = GetNavigationElements(topLevelParent);
			return model;
		}

		public IEnumerable<T> GetNavigationElements(IPublishedContent content)
		{
			var children = content.Children.Where(IncludeInNavigation);
			var navItems = children.Select(ToNavigationElement);
			return navItems.ToList();
		}

		private T ToNavigationElement(IPublishedContent content)
		{
			var model = _navigationElementFactory.Make(content);
			model.Title = string.IsNullOrEmpty(model.NavTitle) ? content.Name : model.NavTitle;
			model.Items = GetNavigationElements(content).Select(e => (INavigationElement)e).ToList();
			return (T)model;
		}

		private bool IncludeInNavigation(IPublishedContent content)
		{
			return _filters.All(filter => filter.IncludeInNavigation(content));
		}
	}
}