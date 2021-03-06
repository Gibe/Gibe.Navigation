﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class SettingsNodeType : INodeType
	{
		private readonly string _docTypeAlias;

		public SettingsNodeType() : this("site")
		{

		}

		public SettingsNodeType(string docTypeAlias)
		{
			_docTypeAlias = docTypeAlias;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			var settingsNodes = rootNodes.Where(r => r.ContentType.Alias == _docTypeAlias).ToList();
			if (settingsNodes.Count > 1)
			{
				throw new InvalidOperationException("More than one matching node");
			}
			return settingsNodes.First();
		}
	}

	[Ignore("Until figure out contenttype.alias"), TestFixture]
	internal class SettingsNodeTypeTests
	{
		[Test]
		public void FindNode_Returns_Settings_Node()
		{
			const string docType1 = "docType1";
			const string docType2 = "docType2";

			var rootNodeMock1 = new Mock<IPublishedContent>();
			rootNodeMock1.Setup(r => r.ContentType.Alias).Returns(docType1);
			var rootNodeMock2 = new Mock<IPublishedContent>();
			rootNodeMock2.Setup(r => r.ContentType.Alias).Returns(docType2);

			var settingsNodeType = new SettingsNodeType(docType2);
			var settingsNode = settingsNodeType.FindNode(new[] { rootNodeMock1.Object, rootNodeMock2.Object });

			Assert.That(settingsNode.ContentType.Alias, Is.EqualTo(docType2));
		}

		[Test]
		public void FindNode_Throws_InvalidOperationException_If_More_Than_One_Settings_Node()
		{
			const string docType1 = "docType1";

			var rootNodeMock1 = new Mock<IPublishedContent>();
			rootNodeMock1.Setup(r => r.ContentType.Alias).Returns(docType1);
			var rootNodeMock2 = new Mock<IPublishedContent>();
			rootNodeMock2.Setup(r => r.ContentType.Alias).Returns(docType1);

			var settingsNodeType = new SettingsNodeType(docType1);
			Assert.Throws<InvalidOperationException>(() => settingsNodeType.FindNode(new[] { rootNodeMock1.Object, rootNodeMock2.Object }));
		}
	}
}
