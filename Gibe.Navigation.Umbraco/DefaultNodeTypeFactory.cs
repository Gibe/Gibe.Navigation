using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.Navigation.Umbraco.NodeTypes;

namespace Gibe.Navigation.Umbraco
{
	public class DefaultNodeTypeFactory : INodeTypeFactory
	{
		private readonly Dictionary<Type, INodeType> _nodeTypes;

		public DefaultNodeTypeFactory(IEnumerable<INodeType> nodeTypes)
		{
			_nodeTypes = nodeTypes.ToDictionary(n => n.GetType(), n => n);
		}
		public INodeType GetNodeType<T>() where T : INodeType
		{
			return _nodeTypes[typeof(T)];
		}
	}
}
