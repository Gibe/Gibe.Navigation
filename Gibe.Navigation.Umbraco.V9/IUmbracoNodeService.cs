﻿using System.Collections.Generic;
using Gibe.Navigation.Umbraco.NodeTypes;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco
{
	public interface IUmbracoNodeService
	{
		/// <summary>
		/// Return the correct node for the given key.
		/// </summary>
		/// <param name="nodeType">The type of node to find</param>
		/// <returns>The node represented by that key</returns>
		IPublishedContent GetNode(INodeType nodeType);
	}

	public class FakeUmbracoNodeService : IUmbracoNodeService
	{
		private readonly IDictionary<INodeType, IPublishedContent> _contentByNodeType;

		public FakeUmbracoNodeService(IDictionary<INodeType, IPublishedContent> contentByNodeType)
		{
			_contentByNodeType = contentByNodeType;
		}

		public IPublishedContent GetNode(INodeType nodeKey) => _contentByNodeType[nodeKey];
	}


}
