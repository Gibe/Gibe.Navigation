using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public interface INodeType
	{
		IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes, IPublishedContent currentNode);
	}

	
}
