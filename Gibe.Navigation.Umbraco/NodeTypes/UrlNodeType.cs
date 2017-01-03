using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class UrlNodeType : INodeType
	{
		private readonly IUmbracoWrapper _umbracoWrapper;
		private readonly string _url;

		public UrlNodeType(IUmbracoWrapper umbracoWrapper, string url)
		{
			_umbracoWrapper = umbracoWrapper;
			_url = url;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			return _umbracoWrapper.TypedContent(_url);
		}

		public IEnumerable<IPublishedContent> FindNodes(IEnumerable<IPublishedContent> rootNodes)
		{
			return new[] { FindNode(rootNodes) };
		}
	}
}
