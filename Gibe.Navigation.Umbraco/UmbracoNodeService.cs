using System.Collections.Generic;
using Gibe.Navigation.Umbraco.NodeTypes;
using JetBrains.Annotations;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.PublishedCache;

namespace Gibe.Navigation.Umbraco
{
	public class UmbracoNodeService : IUmbracoNodeService
	{
		private readonly IPublishedSnapshotService _publishedContentCache;
		
		public UmbracoNodeService(IPublishedSnapshotService publishedContentCache)
		{
			_publishedContentCache = publishedContentCache;
		}

		[NotNull]
		public IPublishedContent GetNode([NotNull]INodeType nodeType)
		{
			return nodeType.FindNode(_publishedContentCache.PublishedSnapshotAccessor.PublishedSnapshot.Content.GetAtRoot());
		}
	}
	
	[TestFixture]
	internal class UmbracoNodeServiceTests
	{
		[Test]
		public void GetNode_Calls_FindNode_On_NodeType()
		{
			var mockContent = new Mock<IPublishedContent>().Object;

			var publishedContentCache = new Mock<IPublishedContentCache>();
			publishedContentCache.Setup(p => p.GetAtRoot(null))
				.Returns(new List<IPublishedContent> { mockContent});

			var publishedSnapshot = new Mock<IPublishedSnapshot>();
			publishedSnapshot.Setup(p => p.Content)
				.Returns(publishedContentCache.Object);

			var publishedContentAccessor = new Mock<IPublishedSnapshotAccessor>();
			publishedContentAccessor.Setup(p => p.PublishedSnapshot)
				.Returns(publishedSnapshot.Object);

			var publishedSnapshotService = new Mock<IPublishedSnapshotService>();
			publishedSnapshotService.Setup(u => u.PublishedSnapshotAccessor)
				.Returns(publishedContentAccessor.Object);
			
			var nodeService = new UmbracoNodeService(publishedSnapshotService.Object);
			var result = nodeService.GetNode(new FakeNodeType(mockContent));

			Assert.That(result, Is.EqualTo(mockContent));
		}
	}
	

}
