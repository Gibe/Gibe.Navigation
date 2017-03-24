using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class SettingsNodeType : INodeType
	{
		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes, IPublishedContent currentNode)
		{
			return rootNodes.First(r => r.DocumentTypeAlias == "site");
		}
	}
}
