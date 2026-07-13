using System;
using System.Collections.Generic;

namespace Gibe.Navigation.Core.Models
{
	public interface INavigationElement : ICloneable
	{
		string Title { get; }
		string NavTitle { get; }
		string Url { get; }
		bool IsActive { get; set; }
		IEnumerable<INavigationElement> Items { get; set; }
		string Target { get; }
		bool IsVisible { get; }
		bool IsConcrete { get; }
		bool HasVisibleChildren { get; }
		Dictionary<string, object> ExtraProperties { get; set; }
	}
}