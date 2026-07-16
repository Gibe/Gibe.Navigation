using System.Collections.Generic;
using Gibe.Navigation.Umbraco;
using Gibe.Navigation.Umbraco.NodeTypes;
using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;

namespace Gibe.Navigation.Umbraco.Tests.NodeTypes
{
	[TestFixture]
	public class HomeNodeTypeTests
	{
		[Test]
		public void FindNode_Uses_UmbracoInternalRedirectId_On_Settings_Node_To_Find_Node()
		{
			const int homeId = 1234;

			var homePropertyMock = new Mock<IPublishedProperty>();
			homePropertyMock.Setup(p => p.HasValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
			homePropertyMock.Setup(p => p.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(homeId);

			var settingsMock = new Mock<IPublishedContent>();
			settingsMock.Setup(s => s.GetProperty("umbracoInternalRedirectId")).Returns(homePropertyMock.Object);

			var homeMock = new Mock<IPublishedContent>().Object;

			var publishedContentCache = new Mock<IPublishedContentCache>();
			publishedContentCache.Setup(w => w.GetById(homeId))
				.Returns(homeMock);

			var nodeTypeFactoryMock = new Mock<INodeTypeFactory>();
			nodeTypeFactoryMock.Setup(n => n.GetNodeType<SettingsNodeType>())
				.Returns(new FakeNodeType(settingsMock.Object));

			var fallbackMock = new Mock<IPublishedValueFallback>();

			var nodeType = new HomeNodeType(publishedContentCache.Object, nodeTypeFactoryMock.Object, fallbackMock.Object);
			var foundNode = nodeType.FindNode(new List<IPublishedContent>());

			Assert.That(foundNode, Is.EqualTo(homeMock));
		}
	}
}
