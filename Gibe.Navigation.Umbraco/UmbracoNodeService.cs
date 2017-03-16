﻿using System.Collections.Generic;
using Gibe.Navigation.Umbraco.NodeTypes;
using Gibe.UmbracoWrappers;
using JetBrains.Annotations;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco
{
	public class UmbracoNodeService : IUmbracoNodeService
	{
		private readonly IUmbracoWrapper _umbracoWrapper;
		
		public UmbracoNodeService(IUmbracoWrapper umbracoWrapper)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		[NotNull]
		public IPublishedContent GetNode([NotNull]INodeType nodeType)
		{
			return nodeType.FindNode(_umbracoWrapper.TypedContentAtRoot());
		}
	}

	[TestFixture]
	internal class UmbracoNodeServiceTests
	{
		[Test]
		public void GetNode_Calls_FindNode_On_NodeType()
		{
			var mockContent = new Mock<IPublishedContent>().Object;

			var umbracoWrapper = new Mock<IUmbracoWrapper>();
			umbracoWrapper.Setup(u => u.TypedContentAtRoot())
				.Returns(new List<IPublishedContent>());

			var nodeType = new Mock<INodeType>();
			nodeType.Setup(n => n.FindNode(It.IsAny<IEnumerable<IPublishedContent>>()))
				.Returns(mockContent)
				.Verifiable();

			var nodeService = new UmbracoNodeService(umbracoWrapper.Object);
			var result = nodeService.GetNode(nodeType.Object);

			Assert.That(result, Is.EqualTo(mockContent));
		}
	}
	

}
