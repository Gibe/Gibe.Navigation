using Gibe.Navigation.Models;

namespace Gibe.Navigation
{
	public interface INavigationService<T> where T : INavigationElement
	{
		/// <summary>
		/// Returns a navigation tree
		/// </summary>
		Navigation<T> GetNavigation();

		/// <summary>
		/// Returns a navigation tree with the current URL and any logical parents set as active
		/// </summary>
		Navigation<T> GetNavigation(string currentUrl);

		/// <summary>
		/// Returns a model containing the parent and a child navigation tree with the current URL and any logical parents set as active
		/// </summary>
		SubNavigationModel<T> GetSubNavigation(string currentUrl);
	}
}

