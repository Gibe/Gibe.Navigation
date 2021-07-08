using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Gibe.Navigation.Umbraco.Filters
{
	public class TemplateOrRedirectFilter : INavigationFilter
	{

		public bool IncludeInNavigation(IPublishedContent content)
		{
			return HasTemplate(content) || IsRedirect(content);
		}

		private bool IsRedirect(IPublishedContent content)
		{
			return content.HasValue("gibeNavigationRedirect");
		}

		private bool HasTemplate(IPublishedContent content)
		{
			return content.TemplateId != 0;
		}
	}
	
}
