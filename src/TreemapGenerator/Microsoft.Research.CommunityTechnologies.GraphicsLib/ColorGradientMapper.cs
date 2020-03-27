using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	/// <summary>
	/// Maps a range of color metric values to colors within a gradient.
	/// </summary>
	///
	/// <remarks>
	/// Call the Initialize() method to specify the color metric range, the color
	/// gradient to map the range to, and the number of discrete colors to split
	/// the gradient into.  You can then call ColorMetricToColor() to map a color
	/// metric within the range to one of the discrete colors within the gradient,
	/// or ColorMetricToBrush() to get a corresponding brush.
	///
	/// <para>
	/// Call Dispose() when you are done using the object.
	/// </para>
	///
	/// </remarks>
	public class ColorGradientMapper
	{
		/// Array of discrete colors that the color gradient has been split into.
		protected Color[] m_aoDiscreteColors;

		/// Array of preallocated brushes corresponding to the discrete colors in
		/// m_aoDiscreteColors.  Used only if the bCreateBrushes argument to
		/// Initialize() is true.
		protected Brush[] m_aoDiscreteBrushes;

		/// Number of discrete colors in the arrays.
		protected int m_iDiscreteColorCount;

		/// Color metric that maps to the first element in m_aoDiscreteColors;
		protected float m_fMinColorMetric;

		/// Color metric that maps to the last element in m_aoDiscreteColors;
		protected float m_fMaxColorMetric;

		/// Value used by ColorMetricToColor(), saved in member data so it doesn't
		/// have to be computed with every call.
		protected float m_fColorMetricsPerDivision;

		/// <summary>
		/// Initializes a new instance of the ColorGradientMapper class.
		/// </summary>
		protected internal ColorGradientMapper()
		{
			m_aoDiscreteColors = null;
			m_aoDiscreteBrushes = null;
			m_iDiscreteColorCount = 0;
			m_fMinColorMetric = 0f;
			m_fMaxColorMetric = 0f;
			m_fColorMetricsPerDivision = 0f;
		}

		/// <summary>
		/// Initializes the object.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Graphics object that will use the colors created by this object.
		/// </param>
		///
		/// <param name="fMinColorMetric">
		/// Minimum color metric to map.
		/// </param>
		///
		/// <param name="fMaxColorMetric">
		/// Maximum color metric to map.
		/// </param>
		///
		/// <param name="oMinColor">
		/// Color to map to fMinColorMetric.
		/// </param>
		///
		/// <param name="oMaxColor">
		/// Color to map to fMaxColorMetric.
		/// </param>
		///
		/// <param name="iDiscreteColorCount">
		/// Number of discrete colors to split the color gradient into.  Must be
		/// between 2 and 256.
		/// </param>
		///
		/// <param name="bCreateBrushes">
		/// true to create an array of brushes so that ColorMetricToBrush() can be
		/// called.
		/// </param>
		///
		/// <remarks>
		/// This must be called before any other methods or properties are used.
		/// </remarks>
		public void Initialize(Graphics oGraphics, float fMinColorMetric, float fMaxColorMetric, Color oMinColor, Color oMaxColor, int iDiscreteColorCount, bool bCreateBrushes)
		{
			Debug.Assert(oGraphics != null);
			if (fMaxColorMetric <= fMinColorMetric)
			{
				throw new ArgumentOutOfRangeException("fMaxColorMetric", fMaxColorMetric, "ColorGradientMapper.Initialize: fMaxColorMetric must be > fMinColorMetric.");
			}
			if (iDiscreteColorCount < 2 || iDiscreteColorCount > 256)
			{
				throw new ArgumentOutOfRangeException("iDiscreteColorCount", iDiscreteColorCount, "ColorGradientMapper.Initialize: iDiscreteColorCount must be between 2 and 256.");
			}
			m_fMinColorMetric = fMinColorMetric;
			m_fMaxColorMetric = fMaxColorMetric;
			m_iDiscreteColorCount = iDiscreteColorCount;
			m_fColorMetricsPerDivision = (m_fMaxColorMetric - m_fMinColorMetric) / (float)m_iDiscreteColorCount;
			m_aoDiscreteColors = CreateDiscreteColors(oGraphics, oMinColor, oMaxColor, iDiscreteColorCount);
			if (bCreateBrushes)
			{
				m_aoDiscreteBrushes = CreateDiscreteBrushes(m_aoDiscreteColors);
			}
		}

		/// <summary>
		/// Maps a color metric to a color.
		/// </summary>
		///
		/// <param name="fColorMetric">
		/// Color metric to map.
		/// </param>
		///
		/// <returns>
		/// Color within the gradient specified in the Initialize() method.
		/// </returns>
		///
		/// <remarks>
		/// This method maps a color metric to a discrete color within the
		/// gradient specified in the Initialize() call.
		///
		/// <para>
		/// If fColorMetric is less than the fMinColorMetric value specified in
		/// Initialize(), the minimum color is returned.  If fColorMetric is
		/// greater than the fMaxColorMetric value, the maximum color is returned.
		/// </para>
		///
		/// </remarks>
		public Color ColorMetricToColor(float fColorMetric)
		{
			if (m_iDiscreteColorCount == 0)
			{
				throw new InvalidOperationException("ColorGradientMapper.ColorMetricToColor: Must call Initialize() first.");
			}
			int num = ColorMetricToArrayIndex(fColorMetric);
			return m_aoDiscreteColors[num];
		}

		/// <summary>
		/// Maps a color metric to a brush.
		/// </summary>
		///
		/// <param name="fColorMetric">
		/// Color metric to map.
		/// </param>
		///
		/// <returns>
		/// Brush with a color within the gradient specified in the Initialize()
		/// method.
		/// </returns>
		///
		/// <remarks>
		/// This method maps a color metric to a brush with a discrete color within
		/// the gradient specified in the Initialize() call.
		///
		/// <para>
		/// The returned brush is owned by the ColorGradientMapper object.  Do
		/// not call the brush's Dispose() method.
		/// </para>
		///
		/// <para>
		/// If fColorMetric is less than the fMinColorMetric value specified in
		/// Initialize(), the minimum color brush is returned.  If fColorMetric is
		/// greater than the fMaxColorMetric value, the maximum color brush is
		/// returned.
		/// </para>
		///
		/// </remarks>
		public Brush ColorMetricToBrush(float fColorMetric)
		{
			if (m_iDiscreteColorCount == 0)
			{
				throw new InvalidOperationException("ColorGradientMapper.ColorMetricToBrush: Must call Initialize() first.");
			}
			if (m_aoDiscreteBrushes == null)
			{
				throw new InvalidOperationException("ColorGradientMapper.ColorMetricToBrush: Must specify bCreateBrushes=true in Initialize() call.");
			}
			int num = ColorMetricToArrayIndex(fColorMetric);
			return m_aoDiscreteBrushes[num];
		}

		/// <summary>
		/// Releases all resources used by this object.
		/// </summary>
		///
		/// <remarks>
		/// Call this when you are done with the object.
		/// </remarks>
		public void Dispose()
		{
			if (m_aoDiscreteBrushes != null)
			{
				Brush[] aoDiscreteBrushes = m_aoDiscreteBrushes;
				foreach (Brush brush in aoDiscreteBrushes)
				{
					brush.Dispose();
				}
			}
		}

		/// <summary>
		/// Creates an array of discrete colors.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Graphics object that will use the colors created by this object.
		/// </param>
		///
		/// <param name="oMinColor">
		/// Color to map to fMinColorMetric.
		/// </param>
		///
		/// <param name="oMaxColor">
		/// Color to map to fMaxColorMetric.
		/// </param>
		///
		/// <param name="iDiscreteColorCount">
		/// Number of discrete colors to split the color gradient into.  Must be
		/// between 2 and 256.
		/// </param>
		///
		/// <returns>
		/// Array of discrete colors.
		/// </returns>
		protected Color[] CreateDiscreteColors(Graphics oGraphics, Color oMinColor, Color oMaxColor, int iDiscreteColorCount)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(iDiscreteColorCount > 1);
			Color[] array = new Color[iDiscreteColorCount];
			Bitmap bitmap = new Bitmap(1, iDiscreteColorCount, oGraphics);
			Graphics graphics = Graphics.FromImage(bitmap);
			Rectangle rect = Rectangle.FromLTRB(0, 0, 1, iDiscreteColorCount - 1);
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, oMinColor, oMaxColor, LinearGradientMode.Vertical);
			graphics.FillRectangle(linearGradientBrush, new Rectangle(Point.Empty, bitmap.Size));
			linearGradientBrush.Dispose();
			int i;
			for (i = 0; i < iDiscreteColorCount - 1; i++)
			{
				array[i] = bitmap.GetPixel(0, i);
			}
			array[i] = oMaxColor;
			bitmap.Dispose();
			return array;
		}

		/// <summary>
		/// Creates an array of brushes with the specified colors.
		/// </summary>
		///
		/// <param name="aoDiscreteColors">
		/// Array of colors.  One brush is created for each color.
		/// </param>
		///
		/// <returns>
		/// Array of brushes.
		/// </returns>
		protected Brush[] CreateDiscreteBrushes(Color[] aoDiscreteColors)
		{
			int num = aoDiscreteColors.Length;
			Brush[] array = new Brush[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new SolidBrush(aoDiscreteColors[i]);
			}
			return array;
		}

		/// <summary>
		/// Maps a color metric to an index into the m_aoDiscreteColors and
		/// m_aoDiscreteBrushes arrays.
		/// </summary>
		///
		/// <param name="fColorMetric">
		/// Color metric to map.
		/// </param>
		///
		/// <returns>
		/// Index into the m_aoDiscreteColors and m_aoDiscreteBrushes arrays.
		/// </returns>
		protected int ColorMetricToArrayIndex(float fColorMetric)
		{
			Debug.Assert(m_iDiscreteColorCount != 0);
			int num = (!(fColorMetric <= m_fMinColorMetric)) ? ((!(fColorMetric >= m_fMaxColorMetric)) ? ((int)((fColorMetric - m_fMinColorMetric) / m_fColorMetricsPerDivision)) : (m_iDiscreteColorCount - 1)) : 0;
			Debug.Assert(num >= 0);
			Debug.Assert(num < m_iDiscreteColorCount);
			return num;
		}
	}
}
