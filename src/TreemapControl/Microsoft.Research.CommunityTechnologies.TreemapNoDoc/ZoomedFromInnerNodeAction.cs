using Microsoft.Research.CommunityTechnologies.Treemap;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// Represents a zoom from one of the treemap's original inner nodes.
	/// </summary>
	///
	/// <remarks>
	/// An object of this class gets created when the treemap is zoomed at a time
	/// when its single top-level node is one of the treemap's original inner
	/// (non-top-level) nodes.
	/// </remarks>
	public class ZoomedFromInnerNodeAction : ZoomAction
	{
		/// Node that was originally an inner node and is currently the treemap's
		/// single top-level node.
		protected Node m_oInnerNode;

		/// <summary>
		/// Initializes a new instance of the ZoomedFromInnerNodeAction class.
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
		/// <param name="oInnerNode">
		/// Node that was originally an inner node and is currently the treemap's
		/// single top-level node.
		/// </param>
		public ZoomedFromInnerNodeAction(ZoomActionHistoryList oZoomActionHistoryList, Node oZoomedNode, Node oInnerNode)
			: base(oZoomActionHistoryList, oZoomedNode)
		{
			m_oInnerNode = oInnerNode;
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
			if (m_oParentOfZoomedNode != null)
			{
				return true;
			}
			if (m_oZoomActionHistoryList.OriginalTopLevelNodes.Length == 1)
			{
				return false;
			}
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
			oTreemapGenerator.Clear();
			oTreemapGenerator.Nodes.Add(m_oInnerNode);
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		public override void AssertValid()
		{
			base.AssertValid();
			Debug.Assert(m_oInnerNode != null);
		}
	}
}
