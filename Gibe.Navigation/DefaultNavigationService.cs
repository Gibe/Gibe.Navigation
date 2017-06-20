﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.Caching.Interfaces;
using Gibe.Navigation.Models;

namespace Gibe.Navigation
{
	public class DefaultNavigationService<T> : INavigationService<T> where T : INavigationElement
	{
		private readonly ICache _cache;
		private readonly IEnumerable<INavigationProvider<T>> _providers;
		
		private readonly string _cacheKey;

		public DefaultNavigationService(ICache cache, IEnumerable<INavigationProvider<T>> providers, string cacheKey = "navigation")
		{
			_cache = cache;
			_providers = providers;
			_cacheKey = cacheKey;
		}

		public Navigation<T> GetNavigation()
		{
			return GetNavigation(null);
		}

		public Navigation<T> GetNavigation(string currentUrl)
		{
			List<T> navElements;

			if (_cache.Exists(_cacheKey))
			{
				navElements = _cache.Get<List<T>>(_cacheKey);
			}
			else
			{
				navElements = _providers.OrderBy(p => p.Priority).SelectMany(p => p.GetNavigationElements()).ToList();
				_cache.Add(_cacheKey, navElements, new TimeSpan(0, 10, 0));
			}
			
			return new Navigation<T>
			{
				Items = Active(navElements, currentUrl)
			};
		}


		public SubNavigationModel<T> GetSubNavigation(string currentUrl)
		{
			var navigation = GetNavigation(currentUrl);
			var section = navigation.Items.FirstOrDefault(i => i.IsActive);
			
			return new SubNavigationModel<T>
			{
				SectionParent = section,
				NavigationElements = (IEnumerable<T>)section.Items
			};
		}

		private List<T> Active(List<T> elements, string currentUrl)
		{
			var clone = Clone(elements).ToList();
			if (currentUrl != null)
			{
				SelectActiveChildren(clone, currentUrl);
			}
			return clone.ToList();
		}

		private bool SelectActiveChildren(IEnumerable<T> elements, string currentUrl)
		{
			foreach (var navigationElement in elements)
			{
				if (navigationElement.IsConcrete && IsActive(currentUrl, navigationElement))
				{
					navigationElement.IsActive = true;
					return true;
				}

				navigationElement.IsActive = SelectActiveChildren(navigationElement.Items.Select(i => (T)i).ToList(), currentUrl);
				if (navigationElement.IsActive)
				{
					return true;
				}
			}
			return false;
		}

		protected virtual bool IsActive(string currentUrl, T navigationElement) =>
			navigationElement.Url.TrimEnd('/') == currentUrl.TrimEnd('/');

		private IEnumerable<T> Clone(IEnumerable<T> elements)
			=> elements.Select(e => (T)e.Clone()).ToList();

		private IEnumerable<T> Visible(List<T> elements)
			=> elements.Where(e => e.IsVisible).Select(e => (T)e.Clone());
	}
	
}
