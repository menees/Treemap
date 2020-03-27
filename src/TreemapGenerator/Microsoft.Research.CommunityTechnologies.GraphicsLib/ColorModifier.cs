using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	/// <summary>
	/// Performs various modifications on Color objects.
	/// </summary>
	///
	/// <remarks>
	/// This class adds functionality to the System.Drawing.Color class.  A better
	/// design would have a new ColorPlus class inherit from Color, with new
	/// methods added to ColorPlus, but because the Color class is sealed, that
	/// isn't possible.
	/// </remarks>
	public class ColorModifier
	{
		/// <summary>
		/// Initializes a new instance of the ColorModifier class.
		/// </summary>
		protected internal ColorModifier()
		{
		}

		/// <summary>
		/// RGBToHSB method.
		/// </summary>
		///
		/// <param name="oColor">
		/// Color.  RGB color to convert to HSB.
		/// </param>
		///
		/// <param name="fHue">
		/// Single.  Where the hue component gets stored, in degrees.  Ranges from
		/// 0 to 360.
		/// </param>
		///
		/// <param name="fSaturation">
		/// Single.  Where the saturation component gets stored.  Ranges from 0
		/// to 1.0, where 0 is grayscale and 1.0 is the most saturated.
		/// </param>
		///
		/// <param name="fBrightness">
		/// Single.  Where the brightness component gets stored.  Ranges from 0
		/// to 1.0, where 0 represents black and 1.0 represents white.
		/// </param>
		///
		/// <remarks>
		/// This method converts a Color object to the HSB color space.
		/// </remarks>
		public void RGBToHSB(Color oColor, out float fHue, out float fSaturation, out float fBrightness)
		{
			ColorRGBToHLS(oColor.ToArgb(), out int pwHue, out int pwLuminance, out int pwSaturation);
			Debug.Assert(pwHue >= 0);
			Debug.Assert(pwHue <= 240);
			Debug.Assert(pwSaturation >= 0);
			Debug.Assert(pwSaturation <= 240);
			Debug.Assert(pwLuminance >= 0);
			Debug.Assert(pwLuminance <= 240);
			fHue = (float)((double)pwHue * 1.5);
			fSaturation = (float)((double)pwSaturation / 240.0);
			fBrightness = (float)((double)pwLuminance / 240.0);
		}

		/// <summary>
		/// HSBToRGB method.
		/// </summary>
		///
		/// <param name="fHue">
		/// Single.  Hue component, in degrees.  Ranges from 0 to 360.
		/// </param>
		///
		/// <param name="fSaturation">
		/// Single.  Saturation component.  Ranges from 0 to 1.0, where 0 is
		/// grayscale and 1.0 is the most saturated.
		/// </param>
		///
		/// <param name="fBrightness">
		/// Single.  Brightness component.  Ranges from 0 to 1.0, where 0
		/// represents black and 1.0 represents white.
		/// </param>
		///
		/// <returns>
		/// Color.  RGB Color object.
		/// </returns>
		///
		/// <remarks>
		/// This method converts an HSB color to an RGB Color object.
		/// </remarks>
		public Color HSBToRGB(float fHue, float fSaturation, float fBrightness)
		{
			Debug.Assert(fHue >= 0f);
			Debug.Assert(fHue <= 360f);
			Debug.Assert(fSaturation >= 0f);
			Debug.Assert(fSaturation <= 1f);
			Debug.Assert(fBrightness >= 0f);
			Debug.Assert(fBrightness <= 1f);
			Color baseColor = Color.FromArgb(ColorHLSToRGB((int)((double)fHue * (2.0 / 3.0)), (int)((double)fBrightness * 240.0), (int)((double)fSaturation * 240.0)));
			return Color.FromArgb(255, baseColor);
		}

		/// <summary>
		/// SetBrightness method.
		/// </summary>
		///
		/// <param name="oColor">
		/// Color.  RGB color to set the brightness for.
		/// </param>
		///
		/// <param name="fBrightness">
		/// Single.  Brightness to set oColor to.  Ranges from 0 to 1.0, where 0
		/// represents black and 1.0 represents white.
		/// </param>
		///
		/// <returns>
		/// Color.  Copy of oColor with modified brightness.
		/// </returns>
		///
		/// <remarks>
		/// This method modifies the brightness of an RGB Color object without
		/// affecting the hue or saturation.
		/// </remarks>
		public Color SetBrightness(Color oColor, float fBrightness)
		{
			Debug.Assert(fBrightness >= 0f);
			Debug.Assert(fBrightness <= 1f);
			RGBToHSB(oColor, out float fHue, out float fSaturation, out float _);
			return HSBToRGB(fHue, fSaturation, fBrightness);
		}

		/// <summary>
		/// ColorRGBToHLS method.
		/// </summary>
		///
		/// <param name="clrRGB">
		/// Int32.  RGB color to convert to HLS.
		/// </param>
		///
		/// <param name="pwHue">
		/// Int32.  Where the hue component gets stored, in degrees.  Ranges from
		/// 0 to 240.
		/// </param>
		///
		/// <param name="pwLuminance">
		/// Int32.  Where the luminance component gets stored.  Ranges from 0
		/// to 240, where 0 represents black and 240 represents white.
		/// </param>
		///
		/// <param name="pwSaturation">
		/// Int32.  Where the saturation component gets stored.  Ranges from 0
		/// to 240, where 0 is grayscale and 240 is the most saturated.
		/// </param>
		///
		/// <remarks>
		/// This Windows API converts an RGB color to the HLS color space.  Note
		/// the parameter order, which corresponds to H-B-S, not H-S-B.
		/// </remarks>
		[DllImport("shlwapi.dll")]
		protected static extern void ColorRGBToHLS(int clrRGB, out int pwHue, out int pwLuminance, out int pwSaturation);

		/// <summary>
		/// ColorHLSToRGB method.
		/// </summary>
		///
		/// <param name="wHue">
		/// Int32.  Hue component.  Ranges from 0 to 240.
		/// </param>
		///
		/// <param name="wLuminance">
		/// Int32.  Luminance component.  Ranges from 0 to 240, where 0 represents
		/// black and 240 represents white.
		/// </param>
		///
		/// <param name="wSaturation">
		/// Int32.  Saturation component.  Ranges from 0 to 240, where 0 is
		/// grayscale and 240 is the most saturated.
		/// </param>
		///
		/// <returns>
		/// Int32.  RGB color.
		/// </returns>
		///
		/// <remarks>
		/// This Windows API converts an HLS color to the RGB color space.
		/// </remarks>
		[DllImport("shlwapi.dll")]
		protected static extern int ColorHLSToRGB(int wHue, int wLuminance, int wSaturation);
	}
}
