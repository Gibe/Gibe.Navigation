using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class UrlNodeType : INodeType
	{
		private readonly IDocumentUrlService _documentUrlService;
		private readonly IPublishedContentCache _publishedContentCache;
		private readonly string _url;

		public UrlNodeType(IDocumentUrlService documentUrlService, IPublishedContentCache publishedContentCache, string url)
		{
			_documentUrlService = documentUrlService;
			_publishedContentCache = publishedContentCache;
			_url = url;
		}

		public IPublishedContent? FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			var key = _documentUrlService.GetDocumentKeyByRoute(_url, culture: null, documentStartNodeId: null, isDraft: false);
			return key.HasValue ? _publishedContentCache.GetById(key.Value) : null;
		}
	}
}
