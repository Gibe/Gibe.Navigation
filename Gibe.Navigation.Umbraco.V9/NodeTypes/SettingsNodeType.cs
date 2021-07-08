using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;


namespace Gibe.Navigation.Umbraco.NodeTypes
{
	public class SettingsNodeType : INodeType
	{
		private readonly string _docTypeAlias;

		public SettingsNodeType() : this("site")
		{

		}

		public SettingsNodeType(string docTypeAlias)
		{
			_docTypeAlias = docTypeAlias;
		}

		public IPublishedContent FindNode(IEnumerable<IPublishedContent> rootNodes)
		{
			var settingsNodes = rootNodes.Where(r => r.ContentType.Alias == _docTypeAlias).ToList();
			if (settingsNodes.Count > 1)
			{
				throw new InvalidOperationException("More than one matching node");
			}
			return settingsNodes.First();
		}
	}
}
