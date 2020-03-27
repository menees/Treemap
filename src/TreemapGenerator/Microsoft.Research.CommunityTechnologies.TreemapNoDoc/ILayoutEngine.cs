using Microsoft.Research.CommunityTechnologies.Treemap;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public interface ILayoutEngine
	{
		void CalculateNodeRectangles(Nodes oNodes, RectangleF oParentRectangle, Node oParentNode, EmptySpaceLocation eEmptySpaceLocation);

		void SetNodeRectanglesToEmpty(Node oNode);

		void SetNodeRectanglesToEmpty(Nodes oNodes, bool bRecursive);
	}
}
