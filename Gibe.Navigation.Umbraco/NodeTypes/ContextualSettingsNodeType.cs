using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class ContextualSettingsNodeType : INodeType
	{
		private readonly IUmbracoWrapper _umbracoWrapper;

		public ContextualSettingsNodeType(IUmbracoWrapper umbracoWrapper)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes, IPublishedContent currentNode)
		{
			return _umbracoWrapper.AncestorOrSelf(currentNode, 1);
		}
	}
}
