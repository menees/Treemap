using System.Diagnostics;
using System.Windows.Forms;

namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	/// <summary>
	/// Provides data for the MouseDown and MouseUp events fired by the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" />.
	/// </summary>
	public class NodeMouseEventArgs : MouseEventArgs
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
		/// Initializes a new instance of the NodeMouseEventArgs class.
		/// </summary>
		///
		/// <param name="oMouseEventArgs">
		/// Mouse event arguments.
		/// </param>
		///
		/// <param name="oNode">
		/// Node object associated with the event.
		/// </param>
		protected internal NodeMouseEventArgs(MouseEventArgs oMouseEventArgs, Node oNode)
			: base(oMouseEventArgs.Button, oMouseEventArgs.Clicks, oMouseEventArgs.X, oMouseEventArgs.Y, oMouseEventArgs.Delta)
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
