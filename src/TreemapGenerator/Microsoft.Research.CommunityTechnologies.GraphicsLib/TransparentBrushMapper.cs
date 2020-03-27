using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	/// <summary>
	/// Provides a solid brush that uses a transparent color.
	/// </summary>
	///
	/// <remarks>
	/// This object creates a set of transparent brushes, with a transparency that
	/// varies from a maximum to a minimum in equal steps.  Each brush has a
	/// "level" that varies from 0 to the number of brushes minus 1.  The lowest-
	/// level brush has the maximum transparency and the highest-level has the
	/// minimum transparency.
	///
	/// <para>
	/// Call the Initialize() method to specify a brush color and the transparency
	/// levels to use.  Initialize() creates all the brushes at once and saves them
	/// in an internal array.  You can then call LevelToBrush() to get a Brush
	/// object for a specified level.  If the specified level is greater than the
	/// level of the highest-level (minimum transparency) brush, the highest-level
	/// brush is returned.
	/// </para>
	///
	/// <para>
	/// Call Dispose() when you are done using the object.
	/// </para>
	///
	/// </remarks>
	public class TransparentBrushMapper
	{
		private ArrayList m_oTransparentBrushes;

		private int m_iTransparentBrushes;

		/// <summary>
		/// TransparentBrushMapper constructor.
		/// </summary>
		protected internal TransparentBrushMapper()
		{
			m_oTransparentBrushes = null;
			m_iTransparentBrushes = 0;
		}

		/// <summary>
		/// Initialize method.
		/// </summary>
		///
		/// <param name="oSolidColor">
		/// Color.  Color to use for all brushes.  Must have alpha set to 255.
		/// </param>
		///
		/// <param name="iMinAlpha">
		/// Int32.  Alpha value to use for the level with maximum transparency.
		/// Must be between 0 and 255.
		/// </param>
		///
		/// <param name="iMaxAlpha">
		/// Int32.  Alpha value to use for the level with minimum transparency.
		/// Must be between 0 and 255.  Must be &gt;= iMinAlpha.
		/// </param>
		///
		/// <param name="iAlphaIncrementPerLevel">
		/// Int32.  Amount that alpha should be incremented from level to level.
		/// Must be &gt; 0.
		/// </param>
		///
		/// <remarks>
		/// This must be called before any other methods or properties are used.
		/// </remarks>
		public void Initialize(Color oSolidColor, int iMinAlpha, int iMaxAlpha, int iAlphaIncrementPerLevel)
		{
			if (oSolidColor.A != byte.MaxValue)
			{
				throw new ArgumentOutOfRangeException("oSolidColor", oSolidColor, "TransparentBrushMapper.Initialize(): oSolidColor must not be transparent.");
			}
			ValidateAlphaRange(iMinAlpha, iMaxAlpha, iAlphaIncrementPerLevel, "TransparentBrushMapper.Initialize()");
			m_oTransparentBrushes = new ArrayList();
			for (int i = iMinAlpha; i <= iMaxAlpha; i += iAlphaIncrementPerLevel)
			{
				Color color = Color.FromArgb(i, oSolidColor);
				Brush value = new SolidBrush(color);
				m_oTransparentBrushes.Add(value);
			}
			m_iTransparentBrushes = m_oTransparentBrushes.Count;
			AssertValid();
		}

		/// <summary>
		/// LevelToTransparentBrush method.
		/// </summary>
		///
		/// <param name="iLevel">
		/// Int32.  Level of transparency.  Must be &gt;= 0.  If this is greater than
		/// the level of the highest-level (minimum transparency) brush, the
		/// highest-level brush is returned.
		/// </param>
		///
		/// <returns>
		/// Brush with the specified level of transparency.
		/// </returns>
		///
		/// <remarks>
		/// Returns a Brush object with a specified level of transparency.
		/// </remarks>
		public Brush LevelToTransparentBrush(int iLevel)
		{
			AssertValid();
			if (iLevel < 0)
			{
				throw new ArgumentOutOfRangeException("iLevel", iLevel, "TransparentBrushMapper.LevelToTransparentBrush: iLevel must be >= 0.");
			}
			if (iLevel >= m_iTransparentBrushes)
			{
				iLevel = m_iTransparentBrushes - 1;
			}
			return (Brush)m_oTransparentBrushes[iLevel];
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
			if (m_oTransparentBrushes != null)
			{
				foreach (Brush oTransparentBrush in m_oTransparentBrushes)
				{
					oTransparentBrush.Dispose();
				}
			}
		}

		/// <summary>
		/// ValidateAlphaRange method.
		/// </summary>
		///
		/// <param name="iMinAlpha">
		/// Int32.  Alpha value to use for the level with maximum transparency.
		/// Must be between 0 and 255.
		/// </param>
		///
		/// <param name="iMaxAlpha">
		/// Int32.  Alpha value to use for the level with minimum transparency.
		/// Must be between 0 and 255.  Must be &gt;= iMinAlpha.
		/// </param>
		///
		/// <param name="iIncrementPerLevel">
		/// Int32.  Amount that alpha should be incremented from level to level.
		/// Must be &gt; 0.
		/// </param>
		///
		/// <param name="sCaller">
		/// String.  Name of the caller.  Used in exception messages.  Sample:
		/// "TransparentBrushMapper.Initialize()".
		/// </param>
		///
		/// <remarks>
		/// Throws an exception if one of the parameters is invalid.
		/// </remarks>
		protected internal static void ValidateAlphaRange(int iMinAlpha, int iMaxAlpha, int iIncrementPerLevel, string sCaller)
		{
			if (iMinAlpha < 0 || iMinAlpha > 255)
			{
				throw new ArgumentOutOfRangeException("iMinAlpha", iMinAlpha, sCaller + ": iMinAlpha must be between 0 and 255.");
			}
			if (iMaxAlpha < 0 || iMaxAlpha > 255)
			{
				throw new ArgumentOutOfRangeException("iMaxAlpha", iMaxAlpha, sCaller + ": iMaxAlpha must be between 0 and 255.");
			}
			if (iMaxAlpha < iMinAlpha)
			{
				throw new ArgumentOutOfRangeException("iMaxAlpha", iMaxAlpha, sCaller + ": iMaxAlpha must be >= iMinAlpha.");
			}
			if (iIncrementPerLevel <= 0)
			{
				throw new ArgumentOutOfRangeException("iIncrementPerLevel", iIncrementPerLevel, sCaller + ": iIncrementPerLevel must be > 0.");
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
			Debug.Assert(m_oTransparentBrushes != null);
			Debug.Assert(m_iTransparentBrushes != 0);
		}
	}
}
