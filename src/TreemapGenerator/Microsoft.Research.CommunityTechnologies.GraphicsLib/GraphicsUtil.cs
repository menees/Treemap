using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	/// <summary>
	/// Set of static methods that perform graphics operations not available
	/// directly through GDI+.
	/// </summary>
	///
	/// <remarks>
	/// Do not try to instantiate an object of this type.  All methods are static.
	/// </remarks>
	internal class GraphicsUtil
	{
		/// <summary>
		/// Do not use this contructor.
		/// </summary>
		///
		/// <remarks>
		/// All methods on this class are static, so there is no need to create a
		/// GraphicsUtil object.
		/// </remarks>
		private GraphicsUtil()
		{
		}

		/// <summary>
		/// Converts a height and width from pixels to points.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Object that will do the conversion.
		/// </param>
		///
		/// <param name="fWidthPx">
		/// Width in pixels.  Must be greater than or equal to zero.
		/// </param>
		///
		/// <param name="fHeightPx">
		/// Height in pixels.  Must be greater than or equal to zero.
		/// </param>
		///
		/// <param name="fWidthPt">
		/// Where the width in points gets stored.
		/// </param>
		///
		/// <param name="fHeightPt">
		/// Where the height in points gets stored.
		/// </param>
		public static void PixelsToPoints(Graphics oGraphics, float fWidthPx, float fHeightPx, out float fWidthPt, out float fHeightPt)
		{
			ConvertPixelsAndPoints(bPixelsToPoints: true, oGraphics, fWidthPx, fHeightPx, out fWidthPt, out fHeightPt);
		}

		/// <summary>
		/// Converts a height and width from points to pixels.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Object that will do the conversion.
		/// </param>
		///
		/// <param name="fWidthPt">
		/// Width in points.  Must be greater than or equal to zero.
		/// </param>
		///
		/// <param name="fHeightPt">
		/// Height in points.  Must be greater than or equal to zero.
		/// </param>
		///
		/// <param name="fWidthPx">
		/// Where the width in pixels gets stored.
		/// </param>
		///
		/// <param name="fHeightPx">
		/// Where the height in pixels gets stored.
		/// </param>
		///
		/// <remarks>
		/// There are two versions of this method.  This version converts to
		/// floating-point pixels.  The other converts to integer pixels.
		/// </remarks>
		public static void PointsToPixels(Graphics oGraphics, float fWidthPt, float fHeightPt, out float fWidthPx, out float fHeightPx)
		{
			ConvertPixelsAndPoints(bPixelsToPoints: false, oGraphics, fWidthPt, fHeightPt, out fWidthPx, out fHeightPx);
		}

		/// <summary>
		/// Converts a height and width from points to pixels.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Object that will do the conversion.
		/// </param>
		///
		/// <param name="fWidthPt">
		/// Width in points.  Must be greater than or equal to zero.
		/// </param>
		///
		/// <param name="fHeightPt">
		/// Height in points.  Must be greater than or equal to zero.
		/// </param>
		///
		/// <param name="iWidthPx">
		/// Where the width in pixels gets stored.
		/// </param>
		///
		/// <param name="iHeightPx">
		/// Where the height in pixels gets stored.
		/// </param>
		///
		/// <remarks>
		/// There are two versions of this method.  This version converts to
		/// integer pixels.  The other converts to floating-point pixels.
		/// </remarks>
		public static void PointsToPixels(Graphics oGraphics, float fWidthPt, float fHeightPt, out int iWidthPx, out int iHeightPx)
		{
			PointsToPixels(oGraphics, fWidthPt, fHeightPt, out float fWidthPx, out float fHeightPx);
			iWidthPx = (int)((double)fWidthPx + 0.5);
			iHeightPx = (int)((double)fHeightPx + 0.5);
		}

		/// <summary>
		/// Draws a circle defined by a center point and radius.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Object to draw on.
		/// </param>
		///
		/// <param name="oPen">
		/// Pen to draw with.
		/// </param>
		///
		/// <param name="fXCenter">
		/// x-coordinate of the circle's center.
		/// </param>
		///
		/// <param name="fYCenter">
		/// y-coordinate of the circle's center.
		/// </param>
		///
		/// <param name="fRadius">
		/// Radius of the circle.
		/// </param>
		public static void DrawCircle(Graphics oGraphics, Pen oPen, float fXCenter, float fYCenter, float fRadius)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oPen != null);
			Debug.Assert(fRadius >= 0f);
			float num = 2f * fRadius;
			oGraphics.DrawEllipse(oPen, fXCenter - fRadius, fYCenter - fRadius, num, num);
		}

		/// <summary>
		/// Fills the interior of a circle defined by a center point and radius.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Object to draw on.
		/// </param>
		///
		/// <param name="oBrush">
		/// Brush to draw with.
		/// </param>
		///
		/// <param name="fXCenter">
		/// x-coordinate of the circle's center.
		/// </param>
		///
		/// <param name="fYCenter">
		/// y-coordinate of the circle's center.
		/// </param>
		///
		/// <param name="fRadius">
		/// Radius of the circle.
		/// </param>
		public static void FillCircle(Graphics oGraphics, Brush oBrush, float fXCenter, float fYCenter, float fRadius)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(fRadius >= 0f);
			float num = 2f * fRadius;
			oGraphics.FillEllipse(oBrush, fXCenter - fRadius, fYCenter - fRadius, num, num);
		}

		/// <summary>
		/// Fills the interior of a circle defined by a center point and radius
		/// using 3-D shading.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Object to draw on.
		/// </param>
		///
		/// <param name="oColor">
		/// Color to use.
		/// </param>
		///
		/// <param name="fXCenter">
		/// x-coordinate of the circle's center.
		/// </param>
		///
		/// <param name="fYCenter">
		/// y-coordinate of the circle's center.
		/// </param>
		///
		/// <param name="fRadius">
		/// Radius of the circle.
		/// </param>
		///
		/// <remarks>
		/// The circle is shaded to make it look as if it's a 3-D sphere.
		/// </remarks>
		public static void FillCircle3D(Graphics oGraphics, Color oColor, float fXCenter, float fYCenter, float fRadius)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(fRadius >= 0f);
			GraphicsPath graphicsPath = new GraphicsPath();
			RectangleF rect = SquareFromCenterAndHalfWidth(fXCenter, fYCenter, fRadius);
			graphicsPath.AddEllipse(rect);
			PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);
			pathGradientBrush.CenterPoint = new PointF(rect.Left + rect.Width / 3f, rect.Top + rect.Height / 3f);
			pathGradientBrush.CenterColor = Color.White;
			pathGradientBrush.SurroundColors = new Color[1]
			{
				oColor
			};
			oGraphics.FillRectangle(pathGradientBrush, rect);
			pathGradientBrush.Dispose();
			graphicsPath.Dispose();
		}

		/// <summary>
		/// Fills the interior of a square defined by a center point and
		/// half-width.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Object to draw on.
		/// </param>
		///
		/// <param name="oBrush">
		/// Brush to draw with.
		/// </param>
		///
		/// <param name="fXCenter">
		/// x-coordinate of the square's center.
		/// </param>
		///
		/// <param name="fYCenter">
		/// y-coordinate of the square's center.
		/// </param>
		///
		/// <param name="fHalfWidth">
		/// One half of the width of the square.
		/// </param>
		///
		/// <remarks>
		/// The square is specified as a center point and half-width to make this
		/// method compatible with <see cref="M:Microsoft.Research.CommunityTechnologies.GraphicsLib.GraphicsUtil.FillCircle(System.Drawing.Graphics,System.Drawing.Brush,System.Single,System.Single,System.Single)" />.
		/// </remarks>
		public static void FillSquare(Graphics oGraphics, Brush oBrush, float fXCenter, float fYCenter, float fHalfWidth)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(fHalfWidth >= 0f);
			RectangleF oRectangle = SquareFromCenterAndHalfWidth(fXCenter, fYCenter, fHalfWidth);
			Rectangle rect = RectangleFToRectangle(oRectangle, 1);
			oGraphics.FillRectangle(oBrush, rect);
		}

		/// <summary>
		/// Fills the interior of a square using 3-D shading.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Object to draw on.
		/// </param>
		///
		/// <param name="oColor">
		/// Color to use.
		/// </param>
		///
		/// <param name="fXCenter">
		/// x-coordinate of the square's center.
		/// </param>
		///
		/// <param name="fYCenter">
		/// y-coordinate of the square's center.
		/// </param>
		///
		/// <param name="fHalfWidth">
		/// One half of the width of the square.
		/// </param>
		///
		/// <remarks>
		/// The square is shaded to make it look as if it's 3-D.
		///
		/// <para>
		/// The square is specified as a center point and half-width to make this
		/// method compatible with <see cref="M:Microsoft.Research.CommunityTechnologies.GraphicsLib.GraphicsUtil.FillCircle(System.Drawing.Graphics,System.Drawing.Brush,System.Single,System.Single,System.Single)" />.
		/// </para>
		///
		/// </remarks>
		public static void FillSquare3D(Graphics oGraphics, Color oColor, float fXCenter, float fYCenter, float fHalfWidth)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(fHalfWidth >= 0f);
			GraphicsPath graphicsPath = new GraphicsPath();
			RectangleF rect = SquareFromCenterAndHalfWidth(fXCenter, fYCenter, fHalfWidth);
			graphicsPath.AddRectangle(rect);
			PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);
			pathGradientBrush.CenterPoint = new PointF(fXCenter, fYCenter);
			pathGradientBrush.CenterColor = Color.White;
			pathGradientBrush.SurroundColors = new Color[1]
			{
				oColor
			};
			oGraphics.FillRectangle(pathGradientBrush, rect);
			pathGradientBrush.Dispose();
			graphicsPath.Dispose();
		}

		/// <summary>
		/// Creates a GraphicsPath that describes a rectangle with rounded corners.
		/// </summary>
		///
		/// <param name="oRectangle">
		/// Rectangle to add rounded corners to.
		/// </param>
		///
		/// <param name="iCornerRadius">
		/// Radius of the rectangle's corners.
		/// </param>
		///
		/// <returns>
		/// A new GraphicsPath.
		/// </returns>
		public static GraphicsPath CreateRoundedRectangleGraphicsPath(Rectangle oRectangle, int iCornerRadius)
		{
			Debug.Assert(iCornerRadius > 0);
			GraphicsPath graphicsPath = new GraphicsPath();
			int num = 2 * iCornerRadius;
			Rectangle rect = new Rectangle(oRectangle.Location, new Size(num, num));
			graphicsPath.AddArc(rect, 180f, 90f);
			rect.X = oRectangle.Right - num;
			graphicsPath.AddArc(rect, 270f, 90f);
			rect.Y = oRectangle.Bottom - num;
			graphicsPath.AddArc(rect, 0f, 90f);
			rect.X = oRectangle.Left;
			graphicsPath.AddArc(rect, 90f, 90f);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		/// <summary>
		/// Fills the interior of a rectangle that will contain text.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Object to draw on.
		/// </param>
		///
		/// <param name="oRectangle">
		/// Rectangle to draw on.  If empty, this method does nothing.
		/// </param>
		///
		/// <param name="bTextIsSelected">
		/// true if the text is selected.
		/// </param>
		///
		/// <remarks>
		/// This method fills the interior of a rectangle with either the system
		/// window or system highlight color, depending on whether the text is
		/// selected.  Call this method before you draw the text.  When you draw
		/// the text, use SystemBrushes.HighlightText or SystemBrushes.WindowText
		/// as the text color.
		/// </remarks>
		public static void FillTextRectangle(Graphics oGraphics, Rectangle oRectangle, bool bTextIsSelected)
		{
			Debug.Assert(oGraphics != null);
			if (oRectangle.Width > 0 && oRectangle.Height > 0)
			{
				oGraphics.FillRectangle(bTextIsSelected ? SystemBrushes.Highlight : SystemBrushes.Window, oRectangle);
			}
		}

		/// <summary>
		/// Returns the area of a circle given its radius.
		/// </summary>
		///
		/// <param name="dRadius">
		/// The circle's radius.
		/// </param>
		///
		/// <returns>
		/// The circle's area.
		/// </returns>
		public static double RadiusToArea(double dRadius)
		{
			Debug.Assert(dRadius >= 0.0);
			return Math.PI * dRadius * dRadius;
		}

		/// <summary>
		/// Returns the radius of a circle given its area.
		/// </summary>
		///
		/// <param name="dArea">
		/// The circle's area.
		/// </param>
		///
		/// <returns>
		/// The circle's radius.
		/// </returns>
		public static double AreaToRadius(double dArea)
		{
			Debug.Assert(dArea >= 0.0);
			return Math.Sqrt(dArea / Math.PI);
		}

		/// <summary>
		/// Returns a square given a center point and half-width.
		/// </summary>
		///
		/// <param name="fXCenter">
		/// x-coordinate of the square's center.
		/// </param>
		///
		/// <param name="fYCenter">
		/// y-coordinate of the square's center.
		/// </param>
		///
		/// <param name="fHalfWidth">
		/// One half of the width of the square.
		/// </param>
		public static RectangleF SquareFromCenterAndHalfWidth(float fXCenter, float fYCenter, float fHalfWidth)
		{
			Debug.Assert(fHalfWidth >= 0f);
			return RectangleF.FromLTRB(fXCenter - fHalfWidth, fYCenter - fHalfWidth, fXCenter + fHalfWidth, fYCenter + fHalfWidth);
		}

		/// <summary>
		/// Converts a RectangleF to a Rectangle.
		/// </summary>
		///
		/// <param name="oRectangle">
		/// Rectangle to convert.
		/// </param>
		///
		/// <param name="iPenWidthPx">
		/// Width of the pen that will be used to draw the rectangle.
		/// </param>
		///
		/// <returns>
		/// Converted rectangle.
		/// </returns>
		///
		/// <remarks>
		/// This method converts a floating-point RectangleF to an integer
		/// Rectangle, compensating for some GDI oddities in the process.
		///
		/// <para>
		/// If precise rectangle drawing is required, the caller should convert
		/// all RectangleF objects to Rectangles using this method, then use the
		/// Graphics.DrawRectangle(Pen, Rectangle) method to draw them.  The
		/// floating-point version of Graphics.DrawRectangle() should not be used.
		/// </para>
		///
		/// </remarks>
		public static Rectangle RectangleFToRectangle(RectangleF oRectangle, int iPenWidthPx)
		{
			int left = (int)(oRectangle.Left + 0.5f);
			int num = (int)(oRectangle.Right + 0.5f);
			int top = (int)(oRectangle.Top + 0.5f);
			int num2 = (int)(oRectangle.Bottom + 0.5f);
			if (iPenWidthPx > 1)
			{
				num++;
				num2++;
			}
			return Rectangle.FromLTRB(left, top, num, num2);
		}

		/// <summary>
		/// Saves an Image object to a specified file in a specified format using
		/// high quality settings.
		/// </summary>
		///
		/// <param name="oImage">
		/// Image to save.
		/// </param>
		///
		/// <param name="sFileName">
		/// Full path of the file to save to.
		/// </param>
		///
		/// <param name="eImageFormat">
		/// File format.
		/// </param>
		///
		/// <remarks>
		/// Use this instead of Image.Save(filename, format) if you want a high-
		/// quality image.
		/// </remarks>
		public static void SaveHighQualityImage(Image oImage, string sFileName, ImageFormat eImageFormat)
		{
			Debug.Assert(oImage != null);
			Debug.Assert(sFileName != null);
			Debug.Assert(sFileName != "");
			if (eImageFormat == ImageFormat.Jpeg)
			{
				SaveJpegImage(oImage, sFileName, 100);
			}
			else
			{
				oImage.Save(sFileName, eImageFormat);
			}
		}

		/// <summary>
		/// Saves an image to a JPEG file with a specified quality level.
		/// </summary>
		///
		/// <param name="oImage">
		/// Image to save.
		/// </param>
		///
		/// <param name="sFileName">
		/// Full path of the file to save to.
		/// </param>
		///
		/// <param name="iQuality">
		/// Quality level to use.  I THINK this can be from 1 to 100; the
		/// documentation is not clear.
		/// </param>
		///
		/// <remarks>
		/// Image.Save(..., ImageFormat.Jpeg) uses a low quality by default.  This
		/// method allows the quality to be specified.
		/// </remarks>
		public static void SaveJpegImage(Image oImage, string sFileName, int iQuality)
		{
			Debug.Assert(oImage != null);
			Debug.Assert(sFileName != null);
			Debug.Assert(sFileName != "");
			Debug.Assert(iQuality >= 1);
			Debug.Assert(iQuality <= 100);
			ImageCodecInfo imageCodecInfoForMimeType = GetImageCodecInfoForMimeType("image/jpeg");
			EncoderParameters encoderParameters = new EncoderParameters(1);
			encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, iQuality);
			oImage.Save(sFileName, imageCodecInfoForMimeType, encoderParameters);
		}

		/// <summary>
		/// Draws an error string on a Graphics object.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Object to draw on.
		/// </param>
		///
		/// <param name="oRectangle">
		/// Rectangle to draw on.
		/// </param>
		///
		/// <param name="sString">
		/// String to draw.
		/// </param>
		///
		/// <remarks>
		/// This can be used to draw error strings on bitmaps in a uniform manner.
		/// </remarks>
		public static void DrawErrorStringOnGraphics(Graphics oGraphics, Rectangle oRectangle, string sString)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(sString != null);
			oGraphics.DrawString(sString, new Font("Arial", 11f), Brushes.Black, oRectangle);
		}

		/// <summary>
		/// Gets the ImageCodecInfo object for a specified MIME type.
		/// </summary>
		///
		/// <param name="sMimeType">
		/// MIME type.
		/// </param>
		///
		/// <remarks>
		/// An exception is thrown if the requested object isn't found.
		/// </remarks>
		public static ImageCodecInfo GetImageCodecInfoForMimeType(string sMimeType)
		{
			Debug.Assert(sMimeType != null);
			Debug.Assert(sMimeType != "");
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			foreach (ImageCodecInfo imageCodecInfo in imageEncoders)
			{
				if (imageCodecInfo.MimeType == sMimeType)
				{
					return imageCodecInfo;
				}
			}
			throw new Exception("GraphicsUtil.GetImageCodecInfoForMimeType: Can't find " + sMimeType + ".");
		}

		/// <summary>
		/// Converts a height and width from pixels to points, or vice versa.
		/// </summary>
		///
		/// <param name="bPixelsToPoints">
		/// true to convert pixels to points, false to convert points to pixels.
		/// </param>
		///
		/// <param name="oGraphics">
		/// Object that will do the conversion.
		/// </param>
		///
		/// <param name="fWidthIn">
		/// Input width.  Must be greater than or equal to zero.
		/// </param>
		///
		/// <param name="fHeightIn">
		/// Input height.  Must be greater than or equal to zero.
		/// </param>
		///
		/// <param name="fWidthOut">
		/// Where the converted width gets stored.
		/// </param>
		///
		/// <param name="fHeightOut">
		/// Where the converted height gets stored.
		/// </param>
		protected static void ConvertPixelsAndPoints(bool bPixelsToPoints, Graphics oGraphics, float fWidthIn, float fHeightIn, out float fWidthOut, out float fHeightOut)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(fWidthIn >= 0f);
			Debug.Assert(fHeightIn >= 0f);
			GraphicsUnit pageUnit = oGraphics.PageUnit;
			oGraphics.PageUnit = GraphicsUnit.Point;
			PointF[] array = new PointF[1]
			{
				new PointF(fWidthIn, fHeightIn)
			};
			if (bPixelsToPoints)
			{
				oGraphics.TransformPoints(CoordinateSpace.Page, CoordinateSpace.Device, array);
			}
			else
			{
				oGraphics.TransformPoints(CoordinateSpace.Device, CoordinateSpace.Page, array);
			}
			fWidthOut = array[0].X;
			fHeightOut = array[0].Y;
			oGraphics.PageUnit = pageUnit;
		}

		/// <summary>
		/// Disposes of a pen.
		/// </summary>
		///
		/// <param name="oPen">
		/// Pen to dispose.  Can be null.  Gets set to null.
		/// </param>
		///
		/// <remarks>
		/// If <paramref name="oPen" /> isn't null, this method calls the Dispose
		/// method on <paramref name="oPen" />, then sets it to null.
		/// </remarks>
		public static void DisposePen(ref Pen oPen)
		{
			if (oPen != null)
			{
				oPen.Dispose();
				oPen = null;
			}
		}

		/// <summary>
		/// Disposes of a brush.
		/// </summary>
		///
		/// <param name="oBrush">
		/// Brush to dispose.  Can be null.  Gets set to null.
		/// </param>
		///
		/// <remarks>
		/// If <paramref name="oBrush" /> isn't null, this method calls the Dispose
		/// method on <paramref name="oBrush" />, then sets it to null.
		/// </remarks>
		public static void DisposeBrush(ref Brush oBrush)
		{
			if (oBrush != null)
			{
				oBrush.Dispose();
				oBrush = null;
			}
		}
	}
}
