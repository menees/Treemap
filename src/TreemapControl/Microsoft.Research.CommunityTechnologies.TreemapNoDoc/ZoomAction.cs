using Microsoft.Research.CommunityTechnologies.Treemap;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// This class is used in conjunction with the <see cref="T:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList" /> class to maintain a history of treemap zoom
	/// actions.
	/// </summary>
	///
	/// <remarks>
	/// This is an abstract base class.  An object of one of the classes derived
	/// from this one gets inserted in to the TreemapControl's <see cref="T:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList" /> when the user zooms in to a treemap node or
	/// zooms out one level.  The object stores enough information to undo the
	/// zoom action or redo the zoom action.
	/// </remarks>
	public abstract class ZoomAction
	{
		/// History list to which this object will be added.
		protected ZoomActionHistoryList m_oZoomActionHistoryList;

		/// Node that is being zoomed to, or null if zooming to all of the
		/// treemap's original top-level nodes.
		protected Node m_oZoomedNode;

		/// Parent of the node that is being zoomed to, or null if zooming to one
		/// or all of the treemap's original top-level nodes.
		protected Node m_oParentOfZoomedNode;

		/// <summary>
		/// Gets the parent of the node that is being zoomed to.
		/// </summary>
		///
		/// <value>
		/// Parent of the node that is being zoomed to, or null if zooming to one
		/// or all of the treemap's original top-level nodes.
		/// </value>
		public Node ParentOfZoomedNode
		{
			get
			{
				AssertValid();
				return m_oParentOfZoomedNode;
			}
		}

		/// <summary>
		/// Initializes a new instance of the ZoomAction class.
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
		protected ZoomAction(ZoomActionHistoryList oZoomActionHistoryList, Node oZoomedNode)
		{
			m_oZoomActionHistoryList = oZoomActionHistoryList;
			m_oZoomedNode = oZoomedNode;
			if (m_oZoomedNode != null)
			{
				m_oParentOfZoomedNode = oZoomedNode.Parent;
			}
			else
			{
				m_oParentOfZoomedNode = null;
			}
		}

		/// <summary>
		/// Determines whether the treemap can be zoomed out one level from the
		/// zoomed node.
		/// </summary>
		///
		/// <returns>
		/// true if the treemap can be zoomed out one level from the zoomed node.
		/// </returns>
		public abstract bool CanZoomOutFromZoomedNode();

		/// <summary>
		/// Undoes this zoom action.
		/// </summary>
		///
		/// <param name="oTreemapGenerator">
		/// TreemapGenerator owned by the TreemapControl.
		/// </param>
		///
		/// <remarks>
		/// This must be overridden in the derived class, which should call this
		/// base method first and then repopulate the treemap to complete the undo.
		/// </remarks>
		public virtual void Undo(TreemapGenerator oTreemapGenerator)
		{
			AssertValid();
			if (m_oParentOfZoomedNode != null)
			{
				Debug.Assert(oTreemapGenerator.Nodes.Count == 1);
				oTreemapGenerator.Nodes[0].PrivateSetParent(m_oParentOfZoomedNode);
			}
		}

		/// <summary>
		/// Redoes this zoom action.
		/// </summary>
		///
		/// <param name="oTreemapGenerator">
		/// TreemapGenerator owned by the TreemapControl.
		/// </param>
		///
		/// <remarks>
		/// This must be overridden in the derived class, which should call this
		/// base method first and then repopulate the treemap to complete the
		/// reversal.
		/// </remarks>
		public void Redo(TreemapGenerator oTreemapGenerator)
		{
			AssertValid();
			Nodes nodes = oTreemapGenerator.Nodes;
			if (m_oZoomedNode == null)
			{
				m_oZoomActionHistoryList.RedoOriginalTopLevel(oTreemapGenerator);
				return;
			}
			oTreemapGenerator.Clear();
			nodes.Add(m_oZoomedNode);
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		public virtual void AssertValid()
		{
			Debug.Assert(m_oZoomActionHistoryList != null);
		}
	}
}
