using System;
using System.Collections.Generic;

namespace Gibe.Navigation.Models
{
	public interface INavigationElement : ICloneable
	{
		string Title { get; }
		string NavTitle { get; }
		string Url { get; }
		bool IsActive { get; }
		IEnumerable<INavigationElement> Items { get; }
		string Target { get; }
		bool IsVisible { get; }
		bool IsConcrete { get; }
		bool HasVisibleChildren { get; }
	}
}