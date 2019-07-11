using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.Caching.Interfaces;
using Gibe.Navigation.Models;

namespace Gibe.Navigation
{
	public class DefaultNavigationService : INavigationService
	{
		private readonly ICache _cache;
		private readonly IEnumerable<INavigationProvider> _providers;
		
		private readonly string _cacheKey;

		public DefaultNavigationService(ICache cache, IEnumerable<INavigationProvider> providers) : this(cache, providers, "navigation")
		{

		}

		public DefaultNavigationService(ICache cache, IEnumerable<INavigationProvider> providers, string cacheKey)
		{
			_cache = cache;
			_providers = providers;
			_cacheKey = cacheKey;
		}

		public Navigation<INavigationElement> Navigation()
		{
			return Navigation(null);
		}

		public Navigation<INavigationElement> Navigation(string currentUrl)
		{
			List<INavigationElement> navElements;

			if (_cache.Exists(_cacheKey))
			{
				navElements = _cache.Get<List<INavigationElement>>(_cacheKey);
			}
			else
			{
				navElements = _providers.OrderBy(p => p.Priority)
					.SelectMany(p => p.NavigationElements())
					.ToList();
				_cache.Add(_cacheKey, navElements, new TimeSpan(0, 10, 0));
			}
			
			return new Navigation<INavigationElement>
			{
				Items = Active(navElements, currentUrl)
			};
		}


		public SubNavigationModel<INavigationElement> SubNavigation(string currentUrl)
		{
			var navigation = Navigation(currentUrl);
			var section = navigation.Items.FirstOrDefault(i => i.IsActive);
			
			return new SubNavigationModel<INavigationElement>
			{
				SectionParent = section,
				NavigationElements = section.Items
			};
		}

		private List<INavigationElement> Active(List<INavigationElement> elements, string currentUrl)
		{
			var clone = Clone(elements).ToList();
			if (currentUrl != null)
			{
				SelectActiveChildren(clone, currentUrl);
			}
			return clone.ToList();
		}

		private bool SelectActiveChildren(IEnumerable<INavigationElement> elements, string currentUrl)
		{
			foreach (var navigationElement in elements)
			{
				if (navigationElement.IsConcrete && IsActive(currentUrl, navigationElement))
				{
					navigationElement.IsActive = true;
					return true;
				}

				navigationElement.IsActive = SelectActiveChildren(navigationElement.Items.ToList(), currentUrl);
				if (navigationElement.IsActive)
				{
					return true;
				}
			}
			return false;
		}

		protected virtual bool IsActive(string currentUrl, INavigationElement navigationElement) =>
			navigationElement.Url.TrimEnd('/') == currentUrl.TrimEnd('/');

		private IEnumerable<INavigationElement> Clone(IEnumerable<INavigationElement> elements)
			=> elements.Select(e => (INavigationElement)e.Clone()).ToList();

		private IEnumerable<INavigationElement> Visible(List<INavigationElement> elements)
			=> elements.Where(e => e.IsVisible).Select(e => (INavigationElement)e.Clone());
	}
	
}
