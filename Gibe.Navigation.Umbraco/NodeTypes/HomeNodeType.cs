using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class HomeNodeType : INodeType
	{
		private readonly IPublishedContentCache _publishedContentCache;
		private readonly INodeTypeFactory _nodeTypeFactory;
		private readonly IPublishedValueFallback _publishedValueFallback;

		public HomeNodeType(
			IPublishedContentCache publishedContentCache,
			INodeTypeFactory nodeTypeFactory,
			IPublishedValueFallback publishedValueFallback)
		{
			_publishedContentCache = publishedContentCache;
			_nodeTypeFactory = nodeTypeFactory;
			_publishedValueFallback = publishedValueFallback;
		}

		public IPublishedContent? FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			var settings = _nodeTypeFactory.GetNodeType<SettingsNodeType>().FindNode(rootNodes);
			if (settings == null)
			{
				return null;
			}
			var homeId = settings.Value<int>(_publishedValueFallback, "umbracoInternalRedirectId");
			return _publishedContentCache.GetById(homeId);
		}
	}
}
