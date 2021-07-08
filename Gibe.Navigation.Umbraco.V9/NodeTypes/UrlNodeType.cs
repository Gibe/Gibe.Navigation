using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class UrlNodeType : INodeType
	{
		private readonly IPublishedContentCache _publishedContentCache;
		private readonly string _url;

		public UrlNodeType(IPublishedContentCache publishedContentCache, string url)
		{
			_publishedContentCache = publishedContentCache;
			_url = url;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{

			return _publishedContentCache.GetByRoute(_url);

		}
	}
}
