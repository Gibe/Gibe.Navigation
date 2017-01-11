using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public interface INodeType
	{
		IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes);
	}

	public class FakeRootNodeType : INodeType
	{
		private readonly string _rootDocumentTypeAlias;

		public FakeRootNodeType(string rootDocumentTypeAlias)
		{
			_rootDocumentTypeAlias = rootDocumentTypeAlias;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			return rootNodes.First(x => x.DocumentTypeAlias == _rootDocumentTypeAlias);
		}
	}
}