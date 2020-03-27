using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// Provides a Font object to use for drawing a node's text.
	/// </summary>
	///
	/// <remarks>
	/// Specify a font family and a treemap rectangle in the constructor, then call
	/// <see cref="M:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.PerLevelFontMapper.NodeToFont(Microsoft.Research.CommunityTechnologies.Treemap.Node,System.Int32,System.Drawing.Graphics,System.Drawing.Font@,System.String@)" /> to get a Font object for drawing a node's text.
	/// The size of the font is determined by the node's level and by the size of
	/// the treemap rectangle.
	///
	/// <para>
	/// Call <see cref="M:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.PerLevelFontMapper.Dispose" /> when you are done using the object.
	/// </para>
	///
	/// </remarks>
	public class PerLevelFontMapper : IFontMapper, IDisposable
	{
		private ArrayList m_oFontForRectangles;

		/// Used to implement IDispose.
		protected bool m_bDisposed;

		/// <summary>
		/// Initializes a new instance of the PerLevelFontMapper class.
		/// </summary>
		///
		/// <param name="sFamily">
		/// Font family.
		/// </param>
		///
		/// <param name="oTreemapRectangle">
		/// Treemap's outer rectangle.
		/// </param>
		///
		/// <param name="fTreemapRectangleDivisor">
		/// The font size for level 0 is the treemap rectangle height divided by
		/// fTreemapRectangleDivisor.
		/// </param>
		///
		/// <param name="fPerLevelDivisor">
		/// The font size for level N is the font size for level N-1 divided by
		/// fPerLevelDivisor.
		/// </param>
		///
		/// <param name="fMinimumFontSize">
		/// Minimum font size.
		/// </param>
		///
		/// <param name="oGraphics">
		/// Object the caller will use to draw the node's text.
		/// </param>
		protected internal PerLevelFontMapper(string sFamily, Rectangle oTreemapRectangle, float fTreemapRectangleDivisor, float fPerLevelDivisor, float fMinimumFontSize, Graphics oGraphics)
		{
			StringUtil.AssertNotEmpty(sFamily);
			Debug.Assert(fTreemapRectangleDivisor > 0f);
			Debug.Assert(fPerLevelDivisor > 0f);
			Debug.Assert(fMinimumFontSize > 0f);
			Debug.Assert(oGraphics != null);
			float num = (float)oTreemapRectangle.Height / fTreemapRectangleDivisor;
			m_oFontForRectangles = new ArrayList();
			while (num > fMinimumFontSize)
			{
				FontForRectangle value = new FontForRectangle(sFamily, num, oGraphics);
				m_oFontForRectangles.Add(value);
				num /= fPerLevelDivisor;
			}
			m_bDisposed = false;
			AssertValid();
		}

		/// <summary>
		/// PerLevelFontMapper destructor.
		/// </summary>
		~PerLevelFontMapper()
		{
			Dispose(bDisposing: false);
		}

		/// <summary>
		/// Returns a Font object to use for drawing a node's text.
		/// </summary>
		///
		/// <param name="oNode">
		/// The node that needs to be drawn.
		/// </param>
		///
		/// <param name="iNodeLevel">
		/// Node level.  Top-level nodes are at level 0.
		/// </param>
		///
		/// <param name="oGraphics">
		/// Object the caller will use to draw the node's text.
		/// </param>
		///
		/// <param name="oFont">
		/// Where the font gets stored.  The font is owned by the object
		/// implementing this interface.  Do not call the font's Dispose() method.
		/// </param>
		///
		/// <param name="sTextToDraw">
		/// Where the text to draw gets stored.  This is either the node's text or
		/// an abbreviated form of the node's text.
		/// </param>
		///
		/// <returns>
		/// true if an appropriate font was found, false if not.
		/// </returns>
		///
		/// <remarks>
		/// If a Font object suitable for drawing the text for <paramref name="oNode" /> is available, the font is stored at <paramref name="oFont" />, the text to draw is stored at <paramref name="sTextToDraw" />, and true is returned.  false if returned
		/// otherwise.
		/// </remarks>
		public bool NodeToFont(Node oNode, int iNodeLevel, Graphics oGraphics, out Font oFont, out string sTextToDraw)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(iNodeLevel >= 0);
			Debug.Assert(oGraphics != null);
			AssertValid();
			if (iNodeLevel < m_oFontForRectangles.Count)
			{
				FontForRectangle fontForRectangle = (FontForRectangle)m_oFontForRectangles[iNodeLevel];
				string text = oNode.Text;
				if (fontForRectangle.CanFitInRectangle(text, oNode.Rectangle, oGraphics))
				{
					oFont = fontForRectangle.Font;
					sTextToDraw = text;
					return true;
				}
			}
			oFont = null;
			sTextToDraw = null;
			return false;
		}

		/// <summary>
		/// Dispose method.
		/// </summary>
		///
		/// <remarks>
		/// Performs application-defined tasks associated with freeing, releasing,
		/// or resetting unmanaged resources.
		/// </remarks>
		public void Dispose()
		{
			Dispose(bDisposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing,
		/// or resetting unmanaged resources.
		/// </summary>
		///
		/// <param name="bDisposing">
		/// See IDisposable.
		/// </param>
		protected void Dispose(bool bDisposing)
		{
			if (!m_bDisposed && bDisposing)
			{
				if (m_oFontForRectangles != null)
				{
					foreach (FontForRectangle oFontForRectangle in m_oFontForRectangles)
					{
						oFontForRectangle.Dispose();
					}
				}
				m_oFontForRectangles = null;
			}
			m_bDisposed = true;
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(m_oFontForRectangles != null);
		}
	}
}
