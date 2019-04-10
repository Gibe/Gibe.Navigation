using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.PublishedCache;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class HomeNodeType : INodeType
	{
		private readonly IPublishedContentCache _umbracoWrapper;
		private readonly INodeTypeFactory _nodeTypeFactory;
		
		public HomeNodeType(IPublishedContentCache umbracoWrapper, INodeTypeFactory nodeTypeFactory)
		{
			_umbracoWrapper = umbracoWrapper;
			_nodeTypeFactory = nodeTypeFactory;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			var settings = _nodeTypeFactory.GetNodeType<SettingsNodeType>().FindNode(rootNodes);
			var homeId = settings.Value<int>("umbracoInternalRedirectId");
			return _umbracoWrapper.GetById(homeId);
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

			var wrapperMock = new Mock<IPublishedContentCache>();
			wrapperMock.Setup(w => w.GetPropertyValue<int>(It.IsAny<IPublishedContent>(), "umbracoInternalRedirectId"))
				.Returns(homeId);
			wrapperMock.Setup(w => w.TypedContent(homeId))
				.Returns(homeMock);
			
			var nodeTypeFactoryMock = new Mock<INodeTypeFactory>();
			nodeTypeFactoryMock.Setup(n => n.GetNodeType<SettingsNodeType>())
				.Returns(new FakeNodeType(settingsMock));

			var nodeType = new HomeNodeType(wrapperMock.Object, nodeTypeFactoryMock.Object);
			var foundNode = nodeType.FindNode(new List<IPublishedContent>());

			Assert.That(foundNode, Is.EqualTo(homeMock));
		}
	}
}
