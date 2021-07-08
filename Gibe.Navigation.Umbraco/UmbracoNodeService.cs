using Gibe.Navigation.Umbraco.NodeTypes;
using JetBrains.Annotations;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Gibe.Navigation.Umbraco
{
	public class UmbracoNodeService : IUmbracoNodeService
	{
		private readonly IUmbracoContextFactory _umbracoContextFactory;
		
		public UmbracoNodeService(IUmbracoContextFactory umbracoContextFactory)
		{
			_umbracoContextFactory = umbracoContextFactory;
		}

		[NotNull]
		public IPublishedContent GetNode([NotNull]INodeType nodeType)
		{
			using (var context = _umbracoContextFactory.EnsureUmbracoContext())
			{
				return nodeType.FindNode(context.UmbracoContext.Content.GetAtRoot());
			}
		}
	}
}
