using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibe.Navigation.Umbraco.NodeTypes;

namespace Gibe.Navigation.Umbraco
{
	public interface INodeTypeFactory
	{
		INodeType GetNodeType<T>() where T : INodeType;

	    INodeType GetNodeType(Type T);
	}
}
