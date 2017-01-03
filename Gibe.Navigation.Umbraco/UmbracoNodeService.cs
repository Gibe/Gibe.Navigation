using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.Navigation.Umbraco.NodeTypes;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Gibe.Navigation.Umbraco
{
	public class UmbracoNodeService : IUmbracoNodeService
	{
		private readonly IUmbracoWrapper _umbracoWrapper;

		
		public UmbracoNodeService(IUmbracoWrapper umbracoWrapper)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		public IPublishedContent GetNode(INodeType nodeType)
		{
			return nodeType.FindNode(_umbracoWrapper.TypedContentAtRoot());
		}

		public IEnumerable<IPublishedContent> GetNodes(IMultiNodeType nodeType)
		{
			return nodeType.FindNodes(_umbracoWrapper.TypedContentAtRoot());
		}
	}
}
