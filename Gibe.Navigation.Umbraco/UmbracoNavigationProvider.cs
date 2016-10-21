using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.DittoServices.ModelConverters;
using Gibe.Navigation.Models;
using Gibe.Navigation.Umbraco.Models;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;

namespace Gibe.Navigation.Umbraco
{
    public class UmbracoNavigationProvider<T> : INavigationProvider<T> where T : INavigationElement
    {
        private readonly IModelConverter _modelConverter;
        private readonly INodeTypeFactory _nodeTypeFactory;
        private readonly Type _rootNode;
        private readonly IUmbracoNodeService _umbracoNodeService;
        private readonly IUmbracoWrapper _umbracoWrapper;

        public UmbracoNavigationProvider(
            IModelConverter modelConverter,
            IUmbracoNodeService umbracoNodeService,
            IUmbracoWrapper umbracoWrapper,
            int priority, Type rootNode, INodeTypeFactory nodeTypeFactory)
        {
            _modelConverter = modelConverter;
            _umbracoNodeService = umbracoNodeService;
            _umbracoWrapper = umbracoWrapper;
            Priority = priority;
            _rootNode = rootNode;
            _nodeTypeFactory = nodeTypeFactory;
        }

        public int Priority { get; }

        public IEnumerable<T> GetNavigationElements()
        {
            var topLevel = _umbracoNodeService.GetNode(_nodeTypeFactory.GetNodeType(_rootNode));
            return GetNavigationElements(topLevel);
        }

        public SubNavigationModel<T> GetSubNavigation(string url)
        {
            var content = _umbracoWrapper.TypedContent(url.TrimEnd('/'));

            if (content == null) return null;

            var model = new SubNavigationModel<T>();
            var topLevelParent = _umbracoWrapper.AncestorOrSelf(content, 2);
            model.SectionParent = _modelConverter.ToModel<UmbracoNavigationElement>(topLevelParent);
            model.NavigationElements = GetNavigationElements(topLevelParent);
            return model;
        }

        public IEnumerable<T> GetNavigationElements(IPublishedContent content)
        {
            var children = content.Children.Where(
                c => (HasTemplate(c) || IsRedirect(c))
                     && IncludeInNavigation(c));

            var navItems = children.Select(ToNavigationElement);

            return navItems.ToList();
        }

        private T ToNavigationElement(IPublishedContent content)
        {
            var model = IsRedirect(content)
                ? _modelConverter.ToModel<UmbracoNavigationRedirectElement>(content)
                : (INavigationElement) _modelConverter.ToModel<UmbracoNavigationElement>(content);
            model.Title = string.IsNullOrEmpty(model.NavTitle) ? content.Name : model.NavTitle;
            model.Items = GetNavigationElements(content).Select(e => (INavigationElement) e).ToList();
            model.IsVisible = ShowInNavigation(content);
            return (T) model;
        }

        private bool IsRedirect(IPublishedContent content)
        {
            return content.DocumentTypeAlias == "redirect";
        }

        private bool HasTemplate(IPublishedContent content)
        {
            return content.TemplateId != 0;
        }

        protected virtual bool IncludeInNavigation(IPublishedContent content)
        {
            return true;
        }

        protected virtual bool ShowInNavigation(IPublishedContent content)
        {
            return _umbracoWrapper.HasValue(content, "umbracoNaviHide") &&
                   !_umbracoWrapper.GetPropertyValue<bool>(content, "umbracoNaviHide");
        }
    }
}