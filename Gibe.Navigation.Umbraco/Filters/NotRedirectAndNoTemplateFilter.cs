using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.Filters
{
	public class NotRedirectAndNoTemplateFilter : INavigationFilter
	{
		public bool IncludeInNavigation(IPublishedContent content)
		{
			return HasTemplate(content) || IsRedirect(content);
		}

		private bool IsRedirect(IPublishedContent content)
		{
			return content.DocumentTypeAlias.Equals("redirect", StringComparison.CurrentCultureIgnoreCase);
		}

		private bool HasTemplate(IPublishedContent content)
		{
			return content.TemplateId != 0;
		}
	}
}
