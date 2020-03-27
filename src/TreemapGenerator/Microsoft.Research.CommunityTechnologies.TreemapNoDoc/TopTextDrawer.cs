using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class TopTextDrawer : TextDrawerBase
	{
		protected const float TextHeightMultiplier = 1.1f;

		protected string m_sFontFamily;

		protected float m_fFontSizePt;

		protected int m_iMinimumTextHeight;

		protected Color m_oTextColor;

		protected Color m_oSelectedFontColor;

		protected Color m_oSelectedBackColor;

		public TopTextDrawer(NodeLevelsWithText eNodeLevelsWithText, int iMinNodeLevelWithText, int iMaxNodeLevelWithText, string sFontFamily, float fFontSizePt, int iMinimumTextHeight, Color oTextColor, Color oSelectedFontColor, Color oSelectedBackColor)
			: base(eNodeLevelsWithText, iMinNodeLevelWithText, iMaxNodeLevelWithText)
		{
			m_sFontFamily = sFontFamily;
			m_fFontSizePt = fFontSizePt;
			m_iMinimumTextHeight = iMinimumTextHeight;
			m_oTextColor = oTextColor;
			m_oSelectedFontColor = oSelectedFontColor;
			m_oSelectedBackColor = oSelectedBackColor;
			AssertValid();
		}

		public override void DrawTextForAllNodes(Graphics oGraphics, Rectangle oTreemapRectangle, Nodes oNodes)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oNodes != null);
			AssertValid();
			FontForRectangle fontForRectangle = null;
			SolidBrush solidBrush = null;
			TextRenderingHint textRenderingHint = TextRenderingHint.SystemDefault;
			try
			{
				fontForRectangle = new FontForRectangle(m_sFontFamily, m_fFontSizePt, oGraphics);
				textRenderingHint = SetTextRenderingHint(oGraphics, fontForRectangle.Font);
				solidBrush = new SolidBrush(m_oTextColor);
				StringFormat oNonLeafStringFormat = CreateStringFormat(bLeafNode: false);
				StringFormat oLeafStringFormat = CreateStringFormat(bLeafNode: true);
				int textHeight = GetTextHeight(oGraphics, fontForRectangle.Font, m_iMinimumTextHeight);
				DrawTextForNodes(oNodes, oGraphics, fontForRectangle, textHeight, solidBrush, null, oNonLeafStringFormat, oLeafStringFormat, 0);
			}
			finally
			{
				solidBrush?.Dispose();
				fontForRectangle?.Dispose();
				oGraphics.TextRenderingHint = textRenderingHint;
			}
		}

		public override void DrawTextForSelectedNode(Graphics oGraphics, Node oSelectedNode)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oSelectedNode != null);
			AssertValid();
			FontForRectangle fontForRectangle = null;
			SolidBrush solidBrush = null;
			SolidBrush solidBrush2 = null;
			TextRenderingHint textRenderingHint = TextRenderingHint.SystemDefault;
			try
			{
				fontForRectangle = new FontForRectangle(m_sFontFamily, m_fFontSizePt, oGraphics);
				textRenderingHint = SetTextRenderingHint(oGraphics, fontForRectangle.Font);
				solidBrush = new SolidBrush(m_oSelectedFontColor);
				solidBrush2 = new SolidBrush(m_oSelectedBackColor);
				StringFormat oNonLeafStringFormat = CreateStringFormat(bLeafNode: false);
				StringFormat oLeafStringFormat = CreateStringFormat(bLeafNode: true);
				int textHeight = GetTextHeight(oGraphics, fontForRectangle.Font, m_iMinimumTextHeight);
				DrawTextForNode(oGraphics, oSelectedNode, fontForRectangle, textHeight, solidBrush, solidBrush2, oNonLeafStringFormat, oLeafStringFormat);
			}
			finally
			{
				solidBrush?.Dispose();
				solidBrush2?.Dispose();
				fontForRectangle?.Dispose();
				oGraphics.TextRenderingHint = textRenderingHint;
			}
		}

		public static int GetTextHeight(Graphics oGraphics, string sFontFamily, float fFontSizePt, int iMinimumTextHeight)
		{
			Debug.Assert(oGraphics != null);
			StringUtil.AssertNotEmpty(sFontFamily);
			Debug.Assert(fFontSizePt > 0f);
			Debug.Assert(iMinimumTextHeight >= 0);
			FontForRectangle fontForRectangle = null;
			try
			{
				fontForRectangle = new FontForRectangle(sFontFamily, fFontSizePt, oGraphics);
				return GetTextHeight(oGraphics, fontForRectangle.Font, iMinimumTextHeight);
			}
			finally
			{
				fontForRectangle?.Dispose();
			}
		}

		protected static int GetTextHeight(Graphics oGraphics, Font oFont, int iMinimumTextHeight)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oFont != null);
			Debug.Assert(iMinimumTextHeight >= 0);
			int val = (int)Math.Ceiling(1.1f * oFont.GetHeight(oGraphics));
			return Math.Max(val, iMinimumTextHeight);
		}

		protected void DrawTextForNodes(Nodes oNodes, Graphics oGraphics, FontForRectangle oFontForRectangle, int iTextHeight, Brush oTextBrush, Brush oBackgroundBrush, StringFormat oNonLeafStringFormat, StringFormat oLeafStringFormat, int iNodeLevel)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(oGraphics != null);
			Debug.Assert(oFontForRectangle != null);
			Debug.Assert(iTextHeight >= 0);
			Debug.Assert(oTextBrush != null);
			Debug.Assert(oNonLeafStringFormat != null);
			Debug.Assert(oLeafStringFormat != null);
			Debug.Assert(iNodeLevel >= 0);
			foreach (Node oNode in oNodes)
			{
				if (!oNode.Rectangle.IsEmpty)
				{
					if (TextShouldBeDrawnForNode(oNode, iNodeLevel))
					{
						DrawTextForNode(oGraphics, oNode, oFontForRectangle, iTextHeight, oTextBrush, oBackgroundBrush, oNonLeafStringFormat, oLeafStringFormat);
					}
					DrawTextForNodes(oNode.Nodes, oGraphics, oFontForRectangle, iTextHeight, oTextBrush, oBackgroundBrush, oNonLeafStringFormat, oLeafStringFormat, iNodeLevel + 1);
				}
			}
		}

		protected void DrawTextForNode(Graphics oGraphics, Node oNode, FontForRectangle oFontForRectangle, int iTextHeight, Brush oTextBrush, Brush oBackgroundBrush, StringFormat oNonLeafStringFormat, StringFormat oLeafStringFormat)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oNode != null);
			Debug.Assert(oFontForRectangle != null);
			Debug.Assert(iTextHeight >= 0);
			Debug.Assert(oTextBrush != null);
			Debug.Assert(oNonLeafStringFormat != null);
			Debug.Assert(oLeafStringFormat != null);
			AssertValid();
			bool flag = oNode.Nodes.Count == 0;
			Rectangle rectangleToDraw = oNode.RectangleToDraw;
			int penWidthPx = oNode.PenWidthPx;
			rectangleToDraw.Inflate(-penWidthPx, -penWidthPx);
			Rectangle rectangle = (!flag) ? Rectangle.FromLTRB(rectangleToDraw.Left, rectangleToDraw.Top, rectangleToDraw.Right, rectangleToDraw.Top + iTextHeight) : rectangleToDraw;
			int width = rectangle.Width;
			int height = rectangle.Height;
			if (width > 0 && height > 0 && height <= rectangleToDraw.Height)
			{
				if (oBackgroundBrush != null)
				{
					oGraphics.FillRectangle(oBackgroundBrush, rectangle);
				}
				oGraphics.DrawString(oNode.Text, oFontForRectangle.Font, oTextBrush, rectangle, flag ? oLeafStringFormat : oNonLeafStringFormat);
			}
		}

		protected StringFormat CreateStringFormat(bool bLeafNode)
		{
			StringFormat stringFormat = new StringFormat();
			stringFormat.LineAlignment = StringAlignment.Near;
			stringFormat.Alignment = ((!bLeafNode) ? StringAlignment.Center : StringAlignment.Near);
			stringFormat.Trimming = StringTrimming.EllipsisCharacter;
			return stringFormat;
		}

		public override void AssertValid()
		{
			base.AssertValid();
			StringUtil.AssertNotEmpty(m_sFontFamily);
			Debug.Assert(m_fFontSizePt > 0f);
			Debug.Assert(m_iMinimumTextHeight >= 0);
		}
	}
}
