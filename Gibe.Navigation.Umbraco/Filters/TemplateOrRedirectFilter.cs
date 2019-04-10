using System;
using Gibe.UmbracoWrappers;
using Moq;
using NUnit.Framework;
using Umbraco.Core.Models.PublishedContent;

namespace Gibe.Navigation.Umbraco.Filters
{
	public class TemplateOrRedirectFilter : INavigationFilter
	{
		private readonly IUmbracoWrapper _umbracoWrapper;

		public TemplateOrRedirectFilter(IUmbracoWrapper umbracoWrapper)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		public bool IncludeInNavigation(IPublishedContent content)
		{
			return HasTemplate(content) || IsRedirect(content);
		}

		private bool IsRedirect(IPublishedContent content)
		{
			return _umbracoWrapper.HasValue(content, "gibeNavigationRedirect");
		}

		private bool HasTemplate(IPublishedContent content)
		{
			return content.TemplateId != 0;
		}
	}

	[TestFixture]
	internal class TemplateOrRedirectFilterFilterTests
	{
		[Test]
		public void IncludeInNavigation_Returns_False_When_Content_Has_No_Template()
		{
			var filter = TemplateOrRedirectFilter(false);
			var result = filter.IncludeInNavigation(GetMockContent(0));

			Assert.That(result,Is.False);
		}

		[Test]
		public void IncludeInNavigation_Returns_True_When_Content_Has_Template()
		{
			var filter = TemplateOrRedirectFilter(false);
			var result = filter.IncludeInNavigation(GetMockContent(1));

			Assert.That(result, Is.True);
		}

		[Test]
		public void IncludeInNavigation_Returns_True_When_Content_Has_No_Template_And_Is_Redirect()
		{
			var filter = TemplateOrRedirectFilter(true);
			var result = filter.IncludeInNavigation(GetMockContent(0));

			Assert.That(result, Is.True);
		}

		[Test]
		public void IncludeInNavigation_Returns_True_When_Content_Has_Template_And_Is_Redirect()
		{
			var filter = TemplateOrRedirectFilter(true);
			var result = filter.IncludeInNavigation(GetMockContent(1));

			Assert.That(result, Is.True);
		}

		private IPublishedContent GetMockContent(int templateId = 0)
		{
			var contentMock = new Mock<IPublishedContent>();
			contentMock.Setup(c => c.TemplateId).Returns(templateId);
			return contentMock.Object;
		}

		private TemplateOrRedirectFilter TemplateOrRedirectFilter(bool redirect)
		{
			var wrapperMock = new Mock<IUmbracoWrapper>();
			wrapperMock.Setup(w => w.HasValue(It.IsAny<IPublishedContent>(), "gibeNavigationRedirect"))
				.Returns(redirect);

			return new TemplateOrRedirectFilter(wrapperMock.Object);
		}
	}
}
