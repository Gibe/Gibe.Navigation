using System;
using Gibe.Navigation.Umbraco.NodeTypes;

namespace Gibe.Navigation.Umbraco
{
	public interface INodeTypeFactory
	{
		INodeType GetNodeType<T>() where T : INodeType;

		INodeType GetNodeType(Type t);
	}

	public class FakeNodeTypeFactory : INodeTypeFactory
	{
		private readonly INodeType _nodeType;

		public FakeNodeTypeFactory(INodeType nodeType)
		{
			_nodeType = nodeType;
		}

		public INodeType GetNodeType<T>() where T : INodeType
		{
			return _nodeType;
		}

		public INodeType GetNodeType(Type t)
		{
			return _nodeType;
		}
	}
}
