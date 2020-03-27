using Microsoft.Research.CommunityTechnologies.Treemap;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// Represents a zoom from one of the treemap's original top-level nodes.
	/// </summary>
	///
	/// <remarks>
	/// An object of this class gets created when the treemap is zoomed at a time
	/// when its top level contains just one of its original, multiple top-level
	/// nodes.  Example: The treemap's original top-level nodes are A and B, and
	/// the treemap's top level currently contains B.
	/// </remarks>
	public class ZoomedFromOneTopLevelNodeAction : ZoomAction
	{
		/// One of the treemap's original, multiple top-level nodes.
		protected Node m_oOriginalTopLevelNode;

		/// <summary>
		/// Initializes a new instance of the ZoomedFromOneTopLevelNodeAction
		/// class.
		/// </summary>
		///
		/// <param name="oZoomActionHistoryList">
		/// History list to which this object will be added.
		/// </param>
		///
		/// <param name="oZoomedNode">
		/// Node that is being zoomed to, or null if zooming to all of the
		/// treemap's original top-level nodes.
		/// </param>
		///
		/// <param name="oOriginalTopLevelNode">
		/// One of the treemap's original, multiple top-level nodes.
		/// </param>
		public ZoomedFromOneTopLevelNodeAction(ZoomActionHistoryList oZoomActionHistoryList, Node oZoomedNode, Node oOriginalTopLevelNode)
			: base(oZoomActionHistoryList, oZoomedNode)
		{
			m_oOriginalTopLevelNode = oOriginalTopLevelNode;
			AssertValid();
		}

		/// <summary>
		/// Determines whether the treemap can be zoomed out one level from the
		/// zoomed node.
		/// </summary>
		///
		/// <returns>
		/// true if the treemap can be zoomed out one level from the zoomed node.
		/// </returns>
		public override bool CanZoomOutFromZoomedNode()
		{
			AssertValid();
			return m_oParentOfZoomedNode != null;
		}

		/// <summary>
		/// Undoes this zoom action.
		/// </summary>
		///
		/// <param name="oTreemapGenerator">
		/// TreemapGenerator owned by the TreemapControl.
		/// </param>
		public override void Undo(TreemapGenerator oTreemapGenerator)
		{
			AssertValid();
			base.Undo(oTreemapGenerator);
			oTreemapGenerator.Clear();
			oTreemapGenerator.Nodes.Add(m_oOriginalTopLevelNode);
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		public override void AssertValid()
		{
			base.AssertValid();
			Debug.Assert(m_oOriginalTopLevelNode != null);
		}
	}
}
