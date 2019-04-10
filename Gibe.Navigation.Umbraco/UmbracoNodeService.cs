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
		private readonly IPublishedContentCache _publishedContentCache;
		
		public UmbracoNodeService(IPublishedContentCache publishedContentCache)
		{
			_publishedContentCache = publishedContentCache;
		}

		[NotNull]
		public IPublishedContent GetNode([NotNull]INodeType nodeType)
		{
			return nodeType.FindNode(_publishedContentCache.GetAtRoot());
		}
	}

	[TestFixture]
	internal class UmbracoNodeServiceTests
	{
		[Test]
		public void GetNode_Calls_FindNode_On_NodeType()
		{
			var mockContent = new Mock<IPublishedContent>().Object;

			var umbracoWrapper = new Mock<IPublishedContentCache>();
			umbracoWrapper.Setup(u => u.GetAtRoot())
				.Returns(new List<IPublishedContent>());
			
			var nodeService = new UmbracoNodeService(umbracoWrapper.Object);
			var result = nodeService.GetNode(new FakeNodeType(mockContent));

			Assert.That(result, Is.EqualTo(mockContent));
		}
	}
	

}
