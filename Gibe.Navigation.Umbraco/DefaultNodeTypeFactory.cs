using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.Navigation.Umbraco.NodeTypes;

namespace Gibe.Navigation.Umbraco
{
	public class DefaultNodeTypeFactory : INodeTypeFactory
	{
		private readonly Dictionary<Type, INodeType> _nodeTypes;

		public DefaultNodeTypeFactory(IEnumerable<INodeType> nodeTypes)
		{
			_nodeTypes = new Dictionary<Type, INodeType>();
			foreach (var nodeType in nodeTypes)
			{
				_nodeTypes.TryAdd(nodeType.GetType(), nodeType);
			}
		}

		public INodeType GetNodeType<T>() where T : INodeType
		{
			return _nodeTypes[typeof(T)];
		}

		public INodeType GetNodeType(Type t)
		{
			return _nodeTypes[t];
		}
	}
}