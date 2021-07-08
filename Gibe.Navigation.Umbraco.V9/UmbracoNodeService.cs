using System.Diagnostics.CodeAnalysis;
using Gibe.Navigation.Umbraco.NodeTypes;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace Gibe.Navigation.Umbraco
{
	public class UmbracoNodeService : IUmbracoNodeService
	{
		private readonly IUmbracoContextFactory _umbracoContextFactory;
		
		public UmbracoNodeService(IUmbracoContextFactory umbracoContextFactory)
		{
			_umbracoContextFactory = umbracoContextFactory;
		}

		public IPublishedContent GetNode([NotNull]INodeType nodeType)
		{
			using (var context = _umbracoContextFactory.EnsureUmbracoContext())
			{
				return nodeType.FindNode(context.UmbracoContext.Content.GetAtRoot());
			}
		}
	}
}
