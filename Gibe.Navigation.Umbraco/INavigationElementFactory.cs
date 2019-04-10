using Gibe.Navigation.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco
{
	public interface INavigationElementFactory
	{
		INavigationElement Make(IPublishedContent content);
	}

	public class FakeNavigationElementFactory : INavigationElementFactory
	{
		private readonly INavigationElement _navigationElement;

		public FakeNavigationElementFactory(INavigationElement navigationElement)
		{
			_navigationElement = navigationElement;
		}

		public INavigationElement Make(IPublishedContent content)
		{
			return _navigationElement;
		}
	}
}
