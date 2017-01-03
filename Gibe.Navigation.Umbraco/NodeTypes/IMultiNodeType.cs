using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public interface IMultiNodeType
	{
		IEnumerable<IPublishedContent> FindNodes(IEnumerable<IPublishedContent> rootNodes);
	}
}