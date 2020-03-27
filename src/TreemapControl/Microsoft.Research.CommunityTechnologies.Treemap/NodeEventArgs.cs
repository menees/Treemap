using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	/// <summary>
	/// Provides data for the NodeDoubleClick and NodeMouseHover events fired by
	/// the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" />.
	/// </summary>
	public class NodeEventArgs : EventArgs
	{
		private readonly Node m_oNode;

		/// <summary>
		/// Gets the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		/// object associated with the event.
		/// </summary>
		///
		/// <value>
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		/// object associated with the event.
		/// </value>
		///
		/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		public Node Node
		{
			get
			{
				AssertValid();
				return m_oNode;
			}
		}

		/// <summary>
		/// Initializes a new instance of the NodeEventArgs class.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node object associated with the event.
		/// </param>
		protected internal NodeEventArgs(Node oNode)
		{
			m_oNode = oNode;
			AssertValid();
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(m_oNode != null);
			m_oNode.AssertValid();
		}
	}
}
