using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// Maintains a history of zoom actions.  The objects in the list are all
	/// derived from the ZoomAction class.
	/// </summary>
	///
	/// <remarks>
	/// This class adds an OriginalTopLevelNodes property to the base class.  This
	/// allows the top-level nodes to be stored just once, rather than duplicated
	/// in multiple ZoomAction items.
	///
	/// <para>
	/// It also adds <see cref="P:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList.HasCurrentState" />,
	/// <see cref="P:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList.PeekCurrentState" />, and <see cref="P:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList.CurrentState" />
	/// properties.  The TreemapControl uses these properties instead of the
	/// <see cref="P:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.HasPreviousState" /> and <see cref="P:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.PreviousState" /> properties implemented in the base
	/// class because it needs access to the current and next state objects, not
	/// the previous and next state objects.
	/// </para>
	///
	/// </remarks>
	public class ZoomActionHistoryList : HistoryList
	{
		/// Treemap's original top-level nodes, or null if
		protected Node[] m_aoOriginalTopLevelNodes;

		/// Treemap's original Nodes.EmptySpace.SizeMetric value, or
		/// Single.MinValue if SetOriginalTopLevelInfo() hasn't been called yet.
		protected float m_fOriginalTopLevelEmptySpaceSizeMetric;

		/// <summary>
		/// Gets a flag indicating whether there is a current state object.
		/// </summary>
		///
		/// <value>
		/// true if there is a current state object.
		/// </value>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList.PeekCurrentState" />
		public bool HasCurrentState
		{
			get
			{
				AssertValid();
				return m_iCurrentObjectIndex >= 0;
			}
		}

		/// <summary>
		/// Gets the current state object without modifying the current object
		/// pointer.
		/// </summary>
		///
		/// <value>
		/// The current object.
		/// </value>
		///
		/// <remarks>
		/// An exception is thrown if there is no current object.  Check the <see cref="P:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList.HasCurrentState" /> property before calling this.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList.HasCurrentState" />
		public ZoomAction PeekCurrentState
		{
			get
			{
				AssertValid();
				if (!HasCurrentState)
				{
					throw new InvalidOperationException("ZoomActionHistoryList.PeekCurrentState: There is no current state.  Check HasCurrentState before calling this.");
				}
				Debug.Assert(m_oStateList[m_iCurrentObjectIndex] is ZoomAction);
				return (ZoomAction)m_oStateList[m_iCurrentObjectIndex];
			}
		}

		/// <summary>
		/// Gets the current state object.
		/// </summary>
		///
		/// <value>
		/// The current state object.  The object to the left of the returned
		/// object (if there is one) then becomes the current object.
		/// </value>
		///
		/// <remarks>
		/// An exception is thrown if there is no current object.  Check the
		/// HasCurrentState property before calling this.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList.HasCurrentState" />
		public ZoomAction CurrentState
		{
			get
			{
				AssertValid();
				if (!HasCurrentState)
				{
					throw new InvalidOperationException("ZoomActionHistoryList.CurrentState: There is no current state.  Check HasCurrentState before calling this.");
				}
				object obj = m_oStateList[m_iCurrentObjectIndex];
				m_iCurrentObjectIndex--;
				AssertValid();
				FireChangeEvent();
				Debug.Assert(obj is ZoomAction);
				return (ZoomAction)obj;
			}
		}

		/// <summary>
		/// Retrieves the treemap's original top level-level nodes.
		/// </summary>
		///
		/// <value>
		/// The treemap's original top-level node or nodes.
		/// </value>
		///
		/// <remarks>
		/// Do not use this if <see cref="M:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList.SetOriginalTopLevelInfo(Microsoft.Research.CommunityTechnologies.Treemap.Node[],System.Single)" /> has not been
		/// called.
		/// </remarks>
		public Node[] OriginalTopLevelNodes
		{
			get
			{
				AssertValid();
				Debug.Assert(m_aoOriginalTopLevelNodes != null);
				Debug.Assert(m_aoOriginalTopLevelNodes.Length > 0);
				Debug.Assert(m_fOriginalTopLevelEmptySpaceSizeMetric != float.MinValue);
				return m_aoOriginalTopLevelNodes;
			}
		}

		/// <summary>
		/// Retrieves the treemap's original Nodes.EmptySpace.SizeMetric value.
		/// </summary>
		///
		/// <value>
		/// The treemap's original Nodes.EmptySpace.SizeMetric value.
		/// </value>
		///
		/// <remarks>
		/// Do not use this if <see cref="M:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList.SetOriginalTopLevelInfo(Microsoft.Research.CommunityTechnologies.Treemap.Node[],System.Single)" /> has not been
		/// called.
		/// </remarks>
		public float OriginalTopLevelEmptySpaceSizeMetric
		{
			get
			{
				AssertValid();
				Debug.Assert(m_aoOriginalTopLevelNodes != null);
				Debug.Assert(m_aoOriginalTopLevelNodes.Length > 0);
				Debug.Assert(m_fOriginalTopLevelEmptySpaceSizeMetric != float.MinValue);
				return m_fOriginalTopLevelEmptySpaceSizeMetric;
			}
		}

		/// <summary>
		/// Initializes a new instance of the ZoomActionHistoryList class.
		/// </summary>
		public ZoomActionHistoryList()
		{
			m_aoOriginalTopLevelNodes = null;
			m_fOriginalTopLevelEmptySpaceSizeMetric = float.MinValue;
			AssertValid();
		}

		/// <summary>
		/// Saves information about the treemap's original top level.
		/// </summary>
		///
		/// <param name="aoOriginalTopLevelNodes">
		/// Treemap's original top-level nodes.
		/// </param>
		///
		/// <param name="fOriginalTopLevelEmptySpaceSizeMetric">
		/// Treemap's original Nodes.EmptySpace.SizeMetric value.
		/// </param>
		public void SetOriginalTopLevelInfo(Node[] aoOriginalTopLevelNodes, float fOriginalTopLevelEmptySpaceSizeMetric)
		{
			m_aoOriginalTopLevelNodes = aoOriginalTopLevelNodes;
			m_fOriginalTopLevelEmptySpaceSizeMetric = fOriginalTopLevelEmptySpaceSizeMetric;
			AssertValid();
		}

		/// <summary>
		/// Repopulates the treemap with its original top-level nodes.
		/// </summary>
		///
		/// <param name="oTreemapGenerator">
		/// TreemapGenerator owned by the TreemapControl.
		/// </param>
		///
		/// <remarks>
		/// Do not call this if <see cref="M:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.ZoomActionHistoryList.SetOriginalTopLevelInfo(Microsoft.Research.CommunityTechnologies.Treemap.Node[],System.Single)" /> has not been
		/// called.
		/// </remarks>
		public void RedoOriginalTopLevel(TreemapGenerator oTreemapGenerator)
		{
			Debug.Assert(oTreemapGenerator != null);
			AssertValid();
			oTreemapGenerator.Clear();
			Nodes nodes = oTreemapGenerator.Nodes;
			Debug.Assert(m_aoOriginalTopLevelNodes != null);
			Debug.Assert(m_aoOriginalTopLevelNodes.Length > 0);
			Debug.Assert(m_fOriginalTopLevelEmptySpaceSizeMetric != float.MinValue);
			oTreemapGenerator.BeginUpdate();
			Node[] aoOriginalTopLevelNodes = m_aoOriginalTopLevelNodes;
			foreach (Node node in aoOriginalTopLevelNodes)
			{
				nodes.Add(node);
			}
			nodes.EmptySpace.SizeMetric = m_fOriginalTopLevelEmptySpaceSizeMetric;
			oTreemapGenerator.EndUpdate();
		}

		/// <summary>
		/// Removes all state objects and returns the ZoomActionHistoryList to the
		/// state it was in when it was first created.
		/// </summary>
		public new void Reset()
		{
			base.Reset();
			m_aoOriginalTopLevelNodes = null;
			m_fOriginalTopLevelEmptySpaceSizeMetric = float.MinValue;
			AssertValid();
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		public new void AssertValid()
		{
			base.AssertValid();
			if (m_aoOriginalTopLevelNodes != null)
			{
				Debug.Assert(m_aoOriginalTopLevelNodes.Length > 0);
				Debug.Assert(m_fOriginalTopLevelEmptySpaceSizeMetric != float.MinValue);
			}
		}
	}
}
