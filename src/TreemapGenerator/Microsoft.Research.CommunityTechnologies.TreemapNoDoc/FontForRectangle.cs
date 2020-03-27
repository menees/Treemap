using Microsoft.Research.CommunityTechnologies.AppLib;
using System;
using System.Diagnostics;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// Provides a Font object to use for drawing text inside a rectangle.
	/// </summary>
	///
	/// <remarks>
	/// Specify font information in the constructor.  You can then call <see cref="M:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.FontForRectangle.CanFitInRectangle(System.String,System.Drawing.RectangleF,System.Drawing.Graphics)" /> or <see cref="M:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.FontForRectangle.CanFitInRectangleTruncate(System.String@,System.Drawing.RectangleF,System.Drawing.Graphics)" /> to
	/// determine whether the font can be used to draw specified text inside a
	/// specified rectangle without exceeding the rectangle's bounds.
	///
	/// <para>
	/// Call <see cref="M:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.FontForRectangle.Dispose" /> when you are done using the object.
	/// </para>
	///
	/// </remarks>
	public class FontForRectangle : IDisposable
	{
		/// Minimum length of text that TruncateText() will truncate.
		protected const int MinTruncatableTextLength = 4;

		/// Font provided by this object.
		protected Font m_oFont;

		/// Used to implement IDispose.
		protected bool m_bDisposed;

		/// <summary>
		/// Gets the font represented by the object.
		/// </summary>
		///
		/// <value>
		/// The <see cref="P:Microsoft.Research.CommunityTechnologies.TreemapNoDoc.FontForRectangle.Font" /> represented by the object.
		/// </value>
		public Font Font
		{
			get
			{
				AssertValid();
				return m_oFont;
			}
		}

		/// <summary>
		/// Initializes a new instance of the FontForRectangle class.
		/// </summary>
		///
		/// <param name="sFamily">
		/// Font family.
		/// </param>
		///
		/// <param name="fEmSize">
		/// Font size.
		/// </param>
		///
		/// <param name="oGraphics">
		/// Object that will use the font.
		/// </param>
		protected internal FontForRectangle(string sFamily, float fEmSize, Graphics oGraphics)
		{
			StringUtil.AssertNotEmpty(sFamily);
			Debug.Assert(fEmSize > 0f);
			Debug.Assert(oGraphics != null);
			m_oFont = new Font(sFamily, fEmSize);
			m_bDisposed = false;
			AssertValid();
		}

		/// <summary>
		/// FontForRectangle destructor.
		/// </summary>
		~FontForRectangle()
		{
			Dispose(bDisposing: false);
		}

		/// <summary>
		/// Determines whether the font can be used to draw the specified text
		/// inside the specified rectangle without exceeding the rectangle's
		/// bounds.
		/// </summary>
		///
		/// <param name="sText">
		/// Text that will be drawn in <paramref name="oRectangle" />.
		/// </param>
		///
		/// <param name="oRectangle">
		/// Rectangle that <paramref name="sText" /> will be drawn in.
		/// </param>
		///
		/// <param name="oGraphics">
		/// Object the caller will use to draw the text.
		/// </param>
		///
		/// <returns>
		/// true if the font can be used to draw <paramref name="sText" /> into
		/// <paramref name="oRectangle" />.
		/// </returns>
		public bool CanFitInRectangle(string sText, RectangleF oRectangle, Graphics oGraphics)
		{
			Debug.Assert(sText != null);
			Debug.Assert(oGraphics != null);
			AssertValid();
			SizeF sizeF = oGraphics.MeasureString(sText, Font);
			return sizeF.Width < oRectangle.Width && sizeF.Height < oRectangle.Height;
		}

		/// <summary>
		/// Determines whether the font can be used to draw the specified text or
		/// a truncated version of the text inside the specified rectangle without
		/// exceeding the rectangle's bounds.
		/// </summary>
		///
		/// <param name="sText">
		/// Text that will be drawn in <paramref name="oRectangle" />.  This may
		/// get replaced with a truncated version of the text.
		/// </param>
		///
		/// <param name="oRectangle">
		/// Rectangle that <paramref name="sText" /> will be drawn in.
		/// </param>
		///
		/// <param name="oGraphics">
		/// Object the caller will use to draw the text.
		/// </param>
		///
		/// <returns>
		/// true if the font can be used to draw <paramref name="sText" /> or a
		/// truncated version of <paramref name="sText" /> into <paramref name="oRectangle" />.
		/// </returns>
		///
		/// <remarks>
		/// If the font can be used to draw <paramref name="sText" /> into
		/// <paramref name="oRectangle" />, true is returned.  Otherwise, if a
		/// truncated version of <paramref name="sText" /> fits into <paramref name="oRectangle" />, the truncated text is stored at <paramref name="sText" /> and true is returned.  Otherwise, false is returned.
		/// </remarks>
		public bool CanFitInRectangleTruncate(ref string sText, RectangleF oRectangle, Graphics oGraphics)
		{
			Debug.Assert(sText != null);
			Debug.Assert(oGraphics != null);
			AssertValid();
			if (CanFitInRectangle(sText, oRectangle, oGraphics))
			{
				return true;
			}
			if (TruncateText(sText, out string sTruncatedText) && CanFitInRectangle(sTruncatedText, oRectangle, oGraphics))
			{
				sText = sTruncatedText;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing,
		/// or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(bDisposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Attempts to truncate text.
		/// </summary>
		///
		/// <param name="sText">
		/// Text to truncate.
		/// </param>
		///
		/// <param name="sTruncatedText">
		/// Where the truncated text gets stored if true is returned.
		/// </param>
		///
		/// <returns>
		/// true if the text can be truncated.
		/// </returns>
		///
		/// <remarks>
		/// If <paramref name="sText" /> can be truncated, the truncated version is
		/// stored at <paramref name="sTruncatedText" /> and true is returned.
		/// false is returned otherwise.
		/// </remarks>
		protected bool TruncateText(string sText, out string sTruncatedText)
		{
			Debug.Assert(sText != null);
			AssertValid();
			if (sText.Length < 4)
			{
				sTruncatedText = null;
				return false;
			}
			sTruncatedText = sText.Substring(0, 3) + "...";
			return true;
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
			if (!m_bDisposed && bDisposing && m_oFont != null)
			{
				m_oFont.Dispose();
				m_oFont = null;
			}
			m_bDisposed = true;
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(m_oFont != null);
		}
	}
}
