using System.Collections.Generic;

namespace Gibe.Navigation.Core.Models
{
	public class SubNavigationModel<T> where T : INavigationElement
	{
		public INavigationElement? SectionParent { get; set; }

		public required IEnumerable<T> NavigationElements { get; set; }
	}
}
