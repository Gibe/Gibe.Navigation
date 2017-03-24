using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public interface INodeType
	{
		IPublishedContent FindNode([NotNull]IEnumerable<IPublishedContent> rootNodes, IPublishedContent currentNode);
	}

	public class FakeNodeType : INodeType
	{
		private readonly IPublishedContent _contentToReturn;

		public FakeNodeType(IPublishedContent contentToReturn)
		{
			_contentToReturn = contentToReturn;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes, IPublishedContent currentNode)
		{
			return _contentToReturn;
		}
	}

	public class FakeRootNodeType : INodeType
	{
		private readonly string _rootDocumentTypeAlias;

		public FakeRootNodeType(string rootDocumentTypeAlias)
		{
			_rootDocumentTypeAlias = rootDocumentTypeAlias;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes, IPublishedContent currentNode)
		{
			return rootNodes.First(x => x.DocumentTypeAlias == _rootDocumentTypeAlias);
		}
	}
}