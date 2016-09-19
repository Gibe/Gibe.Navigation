using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibe.Navigation.Models
{
	public interface INavigationElement : ICloneable
	{
		string Title { get; set; }
		string NavTitle { get; }
		string Url { get; }
		bool IsActive { get; set; }
		List<INavigationElement> Items { get; set; }
		string Target { get; }
		bool IsVisible { get; set; }
		bool IsConcrete { get; }
	}
}
