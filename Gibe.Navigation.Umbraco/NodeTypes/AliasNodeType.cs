using System.Collections.Generic;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public abstract class AliasNodeType : INodeType
	{
		private readonly IUmbracoWrapper _umbracoWrapper;
		private readonly string _alias;

		protected AliasNodeType(IUmbracoWrapper umbracoWrapper, string alias)
		{
			_umbracoWrapper = umbracoWrapper;
			_alias = alias;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			var settings = new SettingsNodeType().FindNode(rootNodes);
			return _umbracoWrapper.DescendantOrSelf(settings, _alias);
		}
	}
}
