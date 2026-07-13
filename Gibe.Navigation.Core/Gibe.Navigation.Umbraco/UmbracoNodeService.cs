using Gibe.Navigation.Umbraco.NodeTypes;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Extensions;

namespace Gibe.Navigation.Umbraco
{
	public class UmbracoNodeService : IUmbracoNodeService
	{
		private readonly IDocumentNavigationQueryService _documentNavigationQueryService;
		private readonly IPublishedContentCache _publishedContentCache;

		public UmbracoNodeService(
			IDocumentNavigationQueryService documentNavigationQueryService,
			IPublishedContentCache publishedContentCache)
		{
			_documentNavigationQueryService = documentNavigationQueryService;
			_publishedContentCache = publishedContentCache;
		}

		public IPublishedContent? GetNode(INodeType nodeType)
		{
			if (!_documentNavigationQueryService.TryGetRootKeys(out var rootKeys))
			{
				return null;
			}

			var rootNodes = rootKeys
				.Select(key => _publishedContentCache.GetById(key))
				.WhereNotNull();

			return nodeType.FindNode(rootNodes);
		}
	}
}
