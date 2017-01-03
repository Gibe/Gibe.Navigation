using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;

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

		public IEnumerable<IPublishedContent> FindNodes(IEnumerable<IPublishedContent> rootNodes)
		{
			var settings = new SettingsNodeType().FindNode(rootNodes);
			return _umbracoWrapper.DescendantsOrSelf(settings, _alias);
		}
	}
}
