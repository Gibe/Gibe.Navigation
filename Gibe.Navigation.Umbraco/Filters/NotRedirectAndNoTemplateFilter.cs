using System;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco.Filters
{
	public class NotRedirectAndNoTemplateFilter : INavigationFilter
	{
		public bool IncludeInNavigation(IPublishedContent content)
		{
			return HasTemplate(content) || IsRedirect(content);
		}

		private bool IsRedirect(IPublishedContent content)
		{
			return content.DocumentTypeAlias.Equals("redirect", StringComparison.CurrentCultureIgnoreCase);
		}

		private bool HasTemplate(IPublishedContent content)
		{
			return content.TemplateId != 0;
		}
	}

	[TestFixture]
	internal class NotRedirectAndNoTemplateFilterTests
	{
		[Test]
		public void IncludeInNavigation_Returns_False_When_Content_Has_No_Template()
		{
			var filter = new NotRedirectAndNoTemplateFilter();
			var result = filter.IncludeInNavigation(GetMockContent(0));

			Assert.That(result,Is.False);
		}

		[Test]
		public void IncludeInNavigation_Returns_True_When_Content_Has_Template()
		{
			var filter = new NotRedirectAndNoTemplateFilter();
			var result = filter.IncludeInNavigation(GetMockContent(1));

			Assert.That(result, Is.True);
		}

		[Test]
		public void IncludeInNavigation_Returns_True_When_Content_Has_No_Template_And_Is_Redirect()
		{
			var filter = new NotRedirectAndNoTemplateFilter();
			var result = filter.IncludeInNavigation(GetMockContent(0, "redirect"));

			Assert.That(result, Is.True);
		}

		[Test]
		public void IncludeInNavigation_Returns_True_When_Content_Has_Template_And_Is_Redirect()
		{
			var filter = new NotRedirectAndNoTemplateFilter();
			var result = filter.IncludeInNavigation(GetMockContent(1, "redirect"));

			Assert.That(result, Is.True);
		}

		private IPublishedContent GetMockContent(int templateId = 0, string docTypeAlias = "page")
		{
			var contentMock = new Mock<IPublishedContent>();
			contentMock.Setup(c => c.TemplateId).Returns(templateId);
			contentMock.Setup(c => c.DocumentTypeAlias).Returns(docTypeAlias);
			return contentMock.Object;
		}
	}
}
