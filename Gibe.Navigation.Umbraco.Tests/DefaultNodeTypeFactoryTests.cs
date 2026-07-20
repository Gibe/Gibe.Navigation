using System.Collections.Generic;
using Gibe.Navigation.Umbraco;
using Gibe.Navigation.Umbraco.NodeTypes;
using NUnit.Framework;

namespace Gibe.Navigation.Umbraco.Tests
{
	[TestFixture]
	public class DefaultNodeTypeFactoryTests
	{
		[Test]
		public void GetNodeType_Returns_Matching_NodeType()
		{
			var fakeRootNodeType = new FakeRootNodeType("root");
			var settingsNodeType = new SettingsNodeType();

			var nodeTypeFactory = new DefaultNodeTypeFactory(new INodeType[] { fakeRootNodeType, settingsNodeType });

			var nodeType = nodeTypeFactory.GetNodeType(settingsNodeType.GetType());
			Assert.That(nodeType is SettingsNodeType);
		}

		[Test]
		public void GetNodeType_Throws_KeyNotFoundException_When_Not_Found()
		{
			var nodeTypeFactory = new DefaultNodeTypeFactory(new INodeType[] { });
			Assert.Throws<KeyNotFoundException>(() => nodeTypeFactory.GetNodeType(typeof(SettingsNodeType)));
		}

		[Test]
		public void GetNodeTypeGeneric_Returns_Matching_NodeType()
		{
			var fakeRootNodeType = new FakeRootNodeType("root");
			var settingsNodeType = new SettingsNodeType();

			var nodeTypeFactory = new DefaultNodeTypeFactory(new INodeType[] { fakeRootNodeType, settingsNodeType });

			var nodeType = nodeTypeFactory.GetNodeType<SettingsNodeType>();
			Assert.That(nodeType.GetType(), Is.EqualTo(typeof(SettingsNodeType)));
		}

		[Test]
		public void GetNodeTypeGeneric_Throws_KeyNotFoundException_When_Not_Found()
		{
			var nodeTypeFactory = new DefaultNodeTypeFactory(new INodeType[] { });
			Assert.Throws<KeyNotFoundException>(() => nodeTypeFactory.GetNodeType<SettingsNodeType>());
		}
	}
}
