using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.DittoServices.ModelConverters;
using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Models;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco
{
	public class NavigationElementFactory : INavigationElementFactory
	{
		private readonly IModelConverter _modelConverter;
		private readonly IUmbracoWrapper _umbracoWrapper;

		public NavigationElementFactory(IModelConverter modelConverter, IUmbracoWrapper umbracoWrapper)
		{
			_modelConverter = modelConverter;
			_umbracoWrapper = umbracoWrapper;
		}

		public INavigationElement Make(IPublishedContent content)
		{
			var model = IsRedirect(content)
					? _modelConverter.ToModel<UmbracoNavigationRedirectElement>(content)
					: (INavigationElement)_modelConverter.ToModel<UmbracoNavigationElement>(content);

			model.IsVisible = ShowInNavigation(content);
			return model;
		}

		private bool IsRedirect(IPublishedContent content)
		{
			return content.DocumentTypeAlias.Equals("redirect", StringComparison.CurrentCultureIgnoreCase);
		}

		private bool ShowInNavigation(IPublishedContent content)
		{
			return _umbracoWrapper.HasValue(content, "umbracoNaviHide") &&
						 !_umbracoWrapper.GetPropertyValue<bool>(content, "umbracoNaviHide");
		}

	}
}
