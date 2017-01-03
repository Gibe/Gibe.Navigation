using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.Navigation.Umbraco.NodeTypes;
using Umbraco.Core.Models;

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

		/// <summary>
		/// Return the corresponding nodes for the given multi-node key.
		/// </summary>
		/// <param name="nodeType">The type of nodes to find</param>
		/// <returns>The nodes represented by that key</returns>
		IEnumerable<IPublishedContent> GetNodes(IMultiNodeType nodeType);
	}

	public class FakeUmbracoNodeService : IUmbracoNodeService
	{
		private readonly IDictionary<INodeType, IPublishedContent> _contentByNodeType;

		private readonly IDictionary<IMultiNodeType, IEnumerable<IPublishedContent>> _contentsByNodeType;

		public FakeUmbracoNodeService(IDictionary<INodeType, IPublishedContent> contentByNodeType, IDictionary<IMultiNodeType, IEnumerable<IPublishedContent>> contentsByNodeType)
		{
			_contentByNodeType = contentByNodeType;
			_contentsByNodeType = contentsByNodeType;
		}

		public IPublishedContent GetNode(INodeType nodeKey) => _contentByNodeType[nodeKey];

		public IEnumerable<IPublishedContent> GetNodes(IMultiNodeType nodeKey) => _contentsByNodeType[nodeKey];
	}


}
