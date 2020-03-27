using Microsoft.Research.CommunityTechnologies.Treemap;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// Represents a zoom from the treemap's original top level.
	/// </summary>
	///
	/// <remarks>
	/// An object of this class gets created when the treemap is zoomed at a time
	/// when it contains all of its original top-level nodes.
	/// </remarks>
	public class ZoomedFromTopLevelAction : ZoomAction
	{
		/// <summary>
		/// Initializes a new instance of the ZoomedFromTopLevelAction class.
		/// </summary>
		///
		/// <param name="oZoomActionHistoryList">
		/// History list to which this object will be added.
		/// </param>
		///
		/// <param name="oZoomedNode">
		/// Node that is being zoomed to.
		/// </param>
		///
		/// <param name="oOriginalTopLevelNodes">
		/// Treemap's original top-level nodes.  Must contain one or more nodes.
		/// </param>
		public ZoomedFromTopLevelAction(ZoomActionHistoryList oZoomActionHistoryList, Node oZoomedNode, Nodes oOriginalTopLevelNodes)
			: base(oZoomActionHistoryList, oZoomedNode)
		{
			Debug.Assert(oZoomedNode != null);
			Debug.Assert(oOriginalTopLevelNodes != null);
			oZoomActionHistoryList.SetOriginalTopLevelInfo(oOriginalTopLevelNodes.ToArray(), oOriginalTopLevelNodes.EmptySpace.SizeMetric);
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
			return true;
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
			m_oZoomActionHistoryList.RedoOriginalTopLevel(oTreemapGenerator);
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		public override void AssertValid()
		{
			base.AssertValid();
			Node[] originalTopLevelNodes = m_oZoomActionHistoryList.OriginalTopLevelNodes;
			float originalTopLevelEmptySpaceSizeMetric = m_oZoomActionHistoryList.OriginalTopLevelEmptySpaceSizeMetric;
			Debug.Assert(originalTopLevelNodes != null);
			Debug.Assert(originalTopLevelNodes.Length > 0);
			Debug.Assert(originalTopLevelEmptySpaceSizeMetric != float.MinValue);
		}
	}
}
