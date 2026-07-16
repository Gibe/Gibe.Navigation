using Gibe.Navigation.Umbraco.Filters;
using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco.Tests.Filters
{
	[TestFixture]
	public class TemplateOrRedirectFilterFilterTests
	{
		[Test]
		public void IncludeInNavigation_Returns_False_When_Content_Has_No_Template()
		{
			var filter = new TemplateOrRedirectFilter();
			var result = filter.IncludeInNavigation(GetMockContent(0, false));

			Assert.That(result, Is.False);
		}

		[Test]
		public void IncludeInNavigation_Returns_True_When_Content_Has_Template()
		{
			var filter = new TemplateOrRedirectFilter();
			var result = filter.IncludeInNavigation(GetMockContent(1, false));

			Assert.That(result, Is.True);
		}

		[Test]
		public void IncludeInNavigation_Returns_True_When_Content_Has_No_Template_And_Is_Redirect()
		{
			var filter = new TemplateOrRedirectFilter();
			var result = filter.IncludeInNavigation(GetMockContent(0, true));

			Assert.That(result, Is.True);
		}

		[Test]
		public void IncludeInNavigation_Returns_True_When_Content_Has_Template_And_Is_Redirect()
		{
			var filter = new TemplateOrRedirectFilter();
			var result = filter.IncludeInNavigation(GetMockContent(1, true));

			Assert.That(result, Is.True);
		}

		private IPublishedContent GetMockContent(int templateId, bool redirect)
		{
			var contentMock = new Mock<IPublishedContent>();
			contentMock.Setup(c => c.TemplateId).Returns(templateId);

			if (redirect)
			{
				var redirectPropertyMock = new Mock<IPublishedProperty>();
				redirectPropertyMock.Setup(p => p.HasValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
				contentMock.Setup(c => c.GetProperty("gibeNavigationRedirect")).Returns(redirectPropertyMock.Object);
			}
			else
			{
				contentMock.Setup(c => c.GetProperty("gibeNavigationRedirect")).Returns((IPublishedProperty?)null);
			}

			return contentMock.Object;
		}
	}
}
