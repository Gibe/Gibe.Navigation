using Gibe.Navigation.Models;

namespace Gibe.Navigation
{
	public interface INavigationService
	{
		/// <summary>
		/// Returns a navigation tree
		/// </summary>
		Navigation<INavigationElement> Navigation();

		/// <summary>
		/// Returns a navigation tree with the current URL and any logical parents set as active
		/// </summary>
		Navigation<INavigationElement> Navigation(string currentUrl);

		/// <summary>
		/// Returns a model containing the parent and a child navigation tree with the current URL and any logical parents set as active
		/// </summary>
		SubNavigationModel<INavigationElement> SubNavigation(string currentUrl);
	}
}

