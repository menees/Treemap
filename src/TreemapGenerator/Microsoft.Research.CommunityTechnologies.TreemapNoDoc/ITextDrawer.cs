using Microsoft.Research.CommunityTechnologies.Treemap;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public interface ITextDrawer
	{
		void DrawTextForAllNodes(Graphics oGraphics, Rectangle oTreemapRectangle, Nodes oNodes);

		void DrawTextForSelectedNode(Graphics oGraphics, Node oSelectedNode);
	}
}
