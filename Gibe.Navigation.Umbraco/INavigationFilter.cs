﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco
{
	public interface INavigationFilter
	{
		bool IncludeInNavigation(IPublishedContent content);
	}

	public class FakeNavigationFilter : INavigationFilter
	{
		private readonly bool _includeInNavigation;

		public FakeNavigationFilter(bool includeInNavigation)
		{
			_includeInNavigation = includeInNavigation;
		}
		public bool IncludeInNavigation(IPublishedContent content)
		{
			return _includeInNavigation;
		}
	}
}
