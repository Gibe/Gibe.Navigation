using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.UmbracoWrappers;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class HomeNodeType : INodeType
	{
		private readonly IUmbracoWrapper _umbracoWrapper;
		private readonly INodeTypeFactory _nodeTypeFactory;
		
		public HomeNodeType(IUmbracoWrapper umbracoWrapper, INodeTypeFactory nodeTypeFactory)
		{
			_umbracoWrapper = umbracoWrapper;
			_nodeTypeFactory = nodeTypeFactory;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes, IPublishedContent currentNode)
		{
			var settings = _nodeTypeFactory.GetNodeType<SettingsNodeType>().FindNode(rootNodes, currentNode);
			var homeId = _umbracoWrapper.GetPropertyValue<int>(settings, "umbracoInternalRedirectId");
			return _umbracoWrapper.TypedContent(homeId);
		}
	}

	[TestFixture]
	internal class HomeNodeTypeTests
	{
		[Test]
		public void FindNode_Uses_UmbracoInternalRedirectId_On_Settings_Node_To_Find_Node()
		{
			const int homeId = 1234;

			var settingsMock = new Mock<IPublishedContent>().Object;
			var homeMock = new Mock<IPublishedContent>().Object;

			var wrapperMock = new Mock<IUmbracoWrapper>();
			wrapperMock.Setup(w => w.GetPropertyValue<int>(It.IsAny<IPublishedContent>(), "umbracoInternalRedirectId"))
				.Returns(homeId);
			wrapperMock.Setup(w => w.TypedContent(homeId))
				.Returns(homeMock);
			
			var nodeTypeFactoryMock = new Mock<INodeTypeFactory>();
			nodeTypeFactoryMock.Setup(n => n.GetNodeType<SettingsNodeType>())
				.Returns(new FakeNodeType(settingsMock));

			var nodeType = new HomeNodeType(wrapperMock.Object, nodeTypeFactoryMock.Object);
			var foundNode = nodeType.FindNode(new List<IPublishedContent>(), null);

			Assert.That(foundNode, Is.EqualTo(homeMock));
		}
	}
}
