using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;


namespace Gibe.Navigation.Umbraco
{
	public class NavigationElementFactory : INavigationElementFactory
	{
		private readonly IPublishedUrlProvider _publishedUrlProvider;
		private readonly IPublishedValueFallback _publishedValueFallback;

		public NavigationElementFactory(IPublishedUrlProvider publishedUrlProvider, IPublishedValueFallback publishedValueFallback)
		{
			_publishedUrlProvider = publishedUrlProvider;
			_publishedValueFallback = publishedValueFallback;
		}

		public INavigationElement Make(IPublishedContent content)
		{
			var model = IsRedirect(content)
					? new UmbracoNavigationRedirectElement(content, _publishedValueFallback)
					: (INavigationElement)new UmbracoNavigationElement(content, _publishedUrlProvider, _publishedValueFallback);

			model = HyrdateExtraProperties(content, model);

			return model;
		}

		private bool IsRedirect(IPublishedContent content)
		{
			return content.HasValue("gibeNavigationRedirect");
		}

		public virtual INavigationElement HyrdateExtraProperties(IPublishedContent content, INavigationElement element)
		{
			return element;
		}
	}

}
