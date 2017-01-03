using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Umbraco.NodeTypes;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco
{
	public interface IUmbracoNodeService
	{
		/// <summary>
		///     Return the correct node for the given key.
		/// </summary>
		/// <param name="nodeType">The type of node to find</param>
		/// <returns>The node represented by that key</returns>
		IPublishedContent GetNode(INodeType nodeType);

		/// <summary>
		///     Return the correct nodes for the given key.
		/// </summary>
		/// <param name="nodeType">The type of node to find</param>
		/// <returns>The nodes represented by that key</returns>
		IEnumerable<IPublishedContent> GetNodes(INodeType nodeType);
	}

	public class FakeUmbracoNodeService : IUmbracoNodeService
	{
		private readonly IDictionary<INodeType, IEnumerable<IPublishedContent>> _contentByNodeType;

		public FakeUmbracoNodeService(IDictionary<INodeType, IEnumerable<IPublishedContent>> contentByNodeType)
		{
			_contentByNodeType = contentByNodeType;
		}

		public IPublishedContent GetNode(INodeType nodeKey) => _contentByNodeType[nodeKey].First();

		public IEnumerable<IPublishedContent> GetNodes(INodeType nodeKey) => _contentByNodeType[nodeKey];
	}
}