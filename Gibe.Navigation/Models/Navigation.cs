using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibe.Navigation.Models
{
	public class Navigation<T> where T : INavigationElement
	{
		public IEnumerable<T> Items { get; set; }
	}
}
