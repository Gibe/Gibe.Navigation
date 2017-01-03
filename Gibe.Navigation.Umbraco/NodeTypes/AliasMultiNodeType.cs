using System.Collections.Generic;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public abstract class AliasMultiNodeType : IMultiNodeType
	{
		private readonly string _alias;
		private readonly IUmbracoWrapper _umbracoWrapper;

		protected AliasMultiNodeType(IUmbracoWrapper umbracoWrapper, string alias)
		{
			_umbracoWrapper = umbracoWrapper;
			_alias = alias;
		}
		
		public IEnumerable<IPublishedContent> FindNodes(IEnumerable<IPublishedContent> rootNodes)
		{
			var settings = new SettingsNodeType().FindNode(rootNodes);
			return _umbracoWrapper.DescendantsOrSelf(settings, _alias);
		}
	}
}