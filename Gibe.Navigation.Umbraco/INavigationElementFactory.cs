using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.Navigation.Models;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco
{
	public interface INavigationElementFactory
	{
		INavigationElement Make(IPublishedContent content);
	}
}
