using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	/// <summary>
	/// Creates a Pen object on request and caches it for future requests.
	/// </summary>
	///
	/// <remarks>
	/// Call the Initialize() method to specify the color of all the pens that
	/// will be requested.  You can then call GetPen() to get a Pen object of a
	/// particular width.  If you call GetPen() again with the same width, the
	/// cached pen will be returned instead of being created from scratch.
	///
	/// <para>
	/// Call Dispose() when you are done using the object.
	/// </para>
	///
	/// </remarks>
	public class PenCache
	{
		private Hashtable m_oPens;

		private Color m_oPenColor;

		/// <summary>
		/// PenCache constructor.
		/// </summary>
		protected internal PenCache()
		{
			m_oPens = null;
		}

		/// <summary>
		/// Initialize method.
		/// </summary>
		///
		/// <param name="oPenColor">
		/// Color.  Color of all the pens that will be created.
		/// </param>
		///
		/// <remarks>
		/// This must be called before any other methods or properties are used.
		/// </remarks>
		public void Initialize(Color oPenColor)
		{
			if (m_oPens != null)
			{
				Dispose();
			}
			m_oPens = new Hashtable();
			m_oPenColor = oPenColor;
			AssertValid();
		}

		/// <summary>
		/// GetPen method.
		/// </summary>
		///
		/// <param name="iWidthPx">
		/// Int32.  Width of the pen, in pixels.
		/// </param>
		///
		/// <returns>
		/// Pen of the specified width.
		/// </returns>
		///
		/// <remarks>
		/// Returns a pen of the specified width.  If the pen already exists in the
		/// internal cache, the cached pen is returned.
		/// </remarks>
		public Pen GetPen(int iWidthPx)
		{
			if (iWidthPx <= 0)
			{
				throw new ArgumentOutOfRangeException("iWidthPx", iWidthPx, "PenCache.GetPen(): iWidthPx must be > 0.");
			}
			Pen pen = (Pen)m_oPens[iWidthPx];
			if (pen == null)
			{
				pen = new Pen(m_oPenColor, iWidthPx);
				pen.Alignment = PenAlignment.Inset;
				m_oPens[iWidthPx] = pen;
			}
			return pen;
		}

		/// <summary>
		/// Dispose method.
		/// </summary>
		///
		/// <remarks>
		/// Frees resources.  Call this when you are done with the object.
		/// </remarks>
		public void Dispose()
		{
			AssertValid();
			if (m_oPens != null)
			{
				foreach (object oPen in m_oPens)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)oPen;
					((Pen)dictionaryEntry.Value).Dispose();
				}
				m_oPens = null;
			}
		}

		/// <summary>
		/// AssertValid method.
		/// </summary>
		///
		/// <remarks>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </remarks>
		[Conditional("DEBUG")]
		protected internal void AssertValid()
		{
			Debug.Assert(m_oPens != null);
		}
	}
}
