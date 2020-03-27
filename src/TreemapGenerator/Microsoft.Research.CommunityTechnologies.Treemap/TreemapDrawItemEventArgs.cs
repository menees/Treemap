using System;
using System.Diagnostics;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	/// <summary>
	/// Provides data for the <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DrawItem" /> event fired
	/// by the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" />.
	/// </summary>
	public class TreemapDrawItemEventArgs : EventArgs
	{
		private readonly Node m_oNode;

		/// <summary>
		/// Gets the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		/// object to draw.
		/// </summary>
		///
		/// <value>
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		/// object that needs to be drawn.
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
		/// Gets the rectangle to draw within.
		/// </summary>
		///
		/// <value>
		/// The rectangle to draw within.
		/// </value>
		///
		/// <remarks>
		/// The computed rectangle takes into account any padding specified with
		/// the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingPx" /> and
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingDecrementPerLevelPx" /> properties.
		/// </remarks>
		public Rectangle Bounds
		{
			get
			{
				AssertValid();
				return m_oNode.RectangleToDraw;
			}
		}

		/// <summary>
		/// Gets the pen width to use to draw the rectangle.
		/// </summary>
		///
		/// <value>
		/// The pen width to use, in pixels.
		/// </value>
		///
		/// <remarks>
		/// The returned pen width is computed using the
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthPx" /> and
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthDecrementPerLevelPx" /> properties.
		///
		/// <para>
		/// The pen width can be ignored if the owner-draw code is drawing uniform
		/// borders or no borders at all.
		/// </para>
		///
		/// </remarks>
		public int PenWidthPx
		{
			get
			{
				AssertValid();
				return m_oNode.PenWidthPx;
			}
		}

		/// <summary>
		/// Initializes a new instance of the TreemapDrawItemEventArgs class.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node object associated with the event.
		/// </param>
		protected internal TreemapDrawItemEventArgs(Node oNode)
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
