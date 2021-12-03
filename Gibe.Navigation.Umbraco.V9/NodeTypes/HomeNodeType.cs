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

		public HomeNodeType(IPublishedContentCache publishedContentCache, INodeTypeFactory nodeTypeFactory)
		{
			_publishedContentCache = publishedContentCache;
			_nodeTypeFactory = nodeTypeFactory;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			var settings = _nodeTypeFactory.GetNodeType<SettingsNodeType>().FindNode(rootNodes);
			var homeId = settings.Value<int>("umbracoInternalRedirectId");
			return _publishedContentCache.GetById(homeId);
		}
	}
}
