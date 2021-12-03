using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public abstract class AliasNodeType : INodeType
	{
		private readonly string _alias;

		protected AliasNodeType(string alias)
		{
			_alias = alias;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			var settings = new SettingsNodeType().FindNode(rootNodes);
			return settings.DescendantOrSelf(_alias);
		}
	}
}
