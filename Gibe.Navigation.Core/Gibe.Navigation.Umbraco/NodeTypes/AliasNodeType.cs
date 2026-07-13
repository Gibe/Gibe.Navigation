using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Extensions;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public abstract class AliasNodeType : INodeType
	{
		private readonly string _alias;
		private readonly INavigationQueryService _navigationQueryService;
		private readonly IPublishedStatusFilteringService _publishedStatusFilteringService;

		protected AliasNodeType(
			string alias,
			INavigationQueryService navigationQueryService,
			IPublishedStatusFilteringService publishedStatusFilteringService)
		{
			_alias = alias;
			_navigationQueryService = navigationQueryService;
			_publishedStatusFilteringService = publishedStatusFilteringService;
		}

		public IPublishedContent? FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			var settings = new SettingsNodeType().FindNode(rootNodes);
			return settings?.DescendantOrSelf(_navigationQueryService, _publishedStatusFilteringService, _alias);
		}
	}
}
