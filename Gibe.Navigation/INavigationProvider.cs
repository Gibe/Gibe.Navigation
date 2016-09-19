using System.Collections.Generic;
using Gibe.Navigation.Models;

namespace Gibe.Navigation
{
	/// <summary>
	/// A Navigation Provider provides navigation elements from a particular source, e.g. Umbraco, GibeCommerce, or a custom configuration
	/// It should provide the entire set of items that it needs to. 
	/// It may cache internally, and if so should manage its own cache invalidation.
	/// It must be valid to reconfigure the active states of the returned objects.
	/// </summary>
	public interface INavigationProvider<T> where T : INavigationElement
	{
		/// <summary>
		/// Returns an ordered enumerable of navigation elements from the given source
		/// </summary>
		/// <returns>An ordered enumberable of navigation elements</returns>
		IEnumerable<T> GetNavigationElements();

		/// <summary>
		/// Returns a model containing the parent and a child navigation tree
		/// </summary>
		SubNavigationModel<T> GetSubNavigation(string url);

		int Priority { get; }
	}
}
