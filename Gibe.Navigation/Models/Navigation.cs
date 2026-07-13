using System.Collections.Generic;

namespace Gibe.Navigation.Models
{
	public class Navigation<T> where T : INavigationElement
	{
		public required IEnumerable<T> Items { get; set; }
	}
}
