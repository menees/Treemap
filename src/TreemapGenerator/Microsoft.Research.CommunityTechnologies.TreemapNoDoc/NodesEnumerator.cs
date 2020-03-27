using Microsoft.Research.CommunityTechnologies.Treemap;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// Supports iterating over a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" /> collection.
	/// </summary>
	public class NodesEnumerator : IEnumerator
	{
		/// Collection being enumerated;
		protected Nodes m_oNodes;

		/// Current index into m_oNodes;
		protected int m_iZeroBasedIndex;

		/// <summary>
		/// Gets the object at the current position.
		/// </summary>
		///
		/// <value>
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object at the enumerator's current position.
		/// </value>
		object IEnumerator.Current
		{
			get
			{
				AssertValid();
				return m_oNodes[m_iZeroBasedIndex];
			}
		}

		/// <summary>
		/// Gets the object at the current position.
		/// </summary>
		///
		/// <value>
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object at the enumerator's current position.
		/// </value>
		public Node Current
		{
			get
			{
				AssertValid();
				return m_oNodes[m_iZeroBasedIndex];
			}
		}

		/// <summary>
		/// Initializes a new instance of the NodesEnumerator class.
		/// </summary>
		///
		/// <param name="nodes">
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" /> object to enumerate.
		/// </param>
		public NodesEnumerator(Nodes nodes)
		{
			nodes.AssertValid();
			m_iZeroBasedIndex = -1;
			m_oNodes = nodes;
		}

		/// <summary>
		/// Moves to the next object in the enumeration.
		/// </summary>
		public bool MoveNext()
		{
			AssertValid();
			if (m_iZeroBasedIndex < m_oNodes.Count - 1)
			{
				m_iZeroBasedIndex++;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Resets the current position so it points to the beginning of the
		/// enumeration.
		/// </summary>
		public void Reset()
		{
			AssertValid();
			m_iZeroBasedIndex = -1;
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		protected internal void AssertValid()
		{
			Debug.Assert(m_oNodes != null);
			m_oNodes.AssertValid();
			Debug.Assert(m_iZeroBasedIndex >= -1);
			Debug.Assert(m_iZeroBasedIndex <= m_oNodes.Count);
		}
	}
}
