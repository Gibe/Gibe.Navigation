using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibe.Navigation.Models
{
	public class SubNavigationModel<T> where T : INavigationElement
	{
		public INavigationElement SectionParent { get; set; }

		public IEnumerable<T> NavigationElements { get; set; }
	}
}
