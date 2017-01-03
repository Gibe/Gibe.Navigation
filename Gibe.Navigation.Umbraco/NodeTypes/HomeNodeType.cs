using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class HomeNodeType : INodeType
	{
		private readonly IUmbracoWrapper _umbracoWrapper;
		
		public HomeNodeType(IUmbracoWrapper umbracoWrapper)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			var settings = new SettingsNodeType().FindNode(rootNodes);
			var homeId = _umbracoWrapper.GetPropertyValue<int>(settings, "umbracoInternalRedirectId");
			return _umbracoWrapper.TypedContent(homeId);
		}

		public IEnumerable<IPublishedContent> FindNodes(IEnumerable<IPublishedContent> rootNodes)
		{
			return new[] { FindNode(rootNodes) };
		}
	}
}
