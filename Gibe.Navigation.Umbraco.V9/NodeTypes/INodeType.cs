using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public interface INodeType
	{
		IPublishedContent FindNode([NotNull]IEnumerable<IPublishedContent> rootNodes);
	}

	public class FakeNodeType : INodeType
	{
		private readonly IPublishedContent _contentToReturn;

		public FakeNodeType(IPublishedContent contentToReturn)
		{
			_contentToReturn = contentToReturn;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
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

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			return rootNodes.First(x => x.ContentType.Alias == _rootDocumentTypeAlias);
		}
	}
}