using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// Provides the largest possible Font object to use for drawing a node's text.
	/// </summary>
	///
	/// <remarks>
	/// Specify a range of font sizes to use in the constructor.  You can then call
	/// <see cref="M:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.MaximizingFontMapper.NodeToFont(Microsoft.Research.CommunityTechnologies.Treemap.Node,System.Int32,System.Drawing.Graphics,System.Drawing.Font@,System.String@)" /> to get a Font object for drawing a node's text.
	/// The returned font is the largest within the specified font range that
	/// doesn't exceed the bounds of the font's rectangle.
	///
	/// <para>
	/// Call <see cref="M:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.MaximizingFontMapper.Dispose" /> when you are done using the object.
	/// </para>
	///
	/// </remarks>
	public class MaximizingFontMapper : IFontMapper, IDisposable
	{
		private ArrayList m_oFontForRectangles;

		/// Used to implement IDispose.
		protected bool m_bDisposed;

		/// <summary>
		/// Initializes a new instance of the MaximizingFontMapper class.
		/// </summary>
		///
		/// <param name="sFamily">
		/// Font family.
		/// </param>
		///
		/// <param name="fMinSizePt">
		/// Minimum font size, in points.
		/// </param>
		///
		/// <param name="fMaxSizePt">
		/// Maximum font size, in points.
		/// </param>
		///
		/// <param name="fIncrementPt">
		/// Increment between fonts.
		/// </param>
		///
		/// <param name="oGraphics">
		/// Object the caller will use to draw the node's text.
		/// </param>
		protected internal MaximizingFontMapper(string sFamily, float fMinSizePt, float fMaxSizePt, float fIncrementPt, Graphics oGraphics)
		{
			StringUtil.AssertNotEmpty(sFamily);
			Debug.Assert(oGraphics != null);
			ValidateSizeRange(fMinSizePt, fMaxSizePt, fIncrementPt, "MaximizingFontMapper.Initialize()");
			m_oFontForRectangles = new ArrayList();
			for (float num = fMinSizePt; num <= fMaxSizePt; num += fIncrementPt)
			{
				FontForRectangle value = new FontForRectangle(sFamily, num, oGraphics);
				m_oFontForRectangles.Insert(0, value);
			}
			m_bDisposed = false;
			AssertValid();
		}

		/// <summary>
		/// MaximizingFontMapper destructor.
		/// </summary>
		~MaximizingFontMapper()
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
			string text = oNode.Text;
			RectangleF rectangle = oNode.Rectangle;
			foreach (FontForRectangle oFontForRectangle in m_oFontForRectangles)
			{
				if (oFontForRectangle.CanFitInRectangle(text, rectangle, oGraphics))
				{
					oFont = oFontForRectangle.Font;
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
		/// Throws an exception if one of the parameters is invalid.
		/// </summary>
		///
		/// <param name="fMinSizePt">
		/// Minimum font size, in points.  Must be &gt; 0.
		/// </param>
		///
		/// <param name="fMaxSizePt">
		/// Maximum font size, in points.  Must be &gt; 0 and &gt;= fMinSizePt.
		/// </param>
		///
		/// <param name="fIncrementPt">
		/// Increment between fonts.  Must be &gt; 0.
		/// </param>
		///
		/// <param name="sCaller">
		/// Name of the caller.  Used in exception messages.  Sample:
		/// "MaximizingFontMapper.Initialize()".
		/// </param>
		protected internal static void ValidateSizeRange(float fMinSizePt, float fMaxSizePt, float fIncrementPt, string sCaller)
		{
			if (fMinSizePt <= 0f)
			{
				throw new ArgumentOutOfRangeException("fMinSizePt", fMinSizePt, sCaller + ": fMinSizePt must be > 0.");
			}
			if (fMaxSizePt <= 0f)
			{
				throw new ArgumentOutOfRangeException("fMaxSizePt", fMaxSizePt, sCaller + ": fMaxSizePt must be > 0.");
			}
			if (fMaxSizePt < fMinSizePt)
			{
				throw new ArgumentOutOfRangeException("fMaxSizePt", fMaxSizePt, sCaller + ": fMaxSizePt must be >= fMinSizePt.");
			}
			if (fIncrementPt <= 0f)
			{
				throw new ArgumentOutOfRangeException("fIncrementPt", fIncrementPt, sCaller + ": fIncrementPt must be > 0.");
			}
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
		/// AssertValid method.
		/// </summary>
		///
		/// <remarks>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </remarks>
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(m_oFontForRectangles != null);
		}
	}
}
