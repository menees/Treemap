using Microsoft.Research.CommunityTechnologies.ControlLib;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
	/// <summary>
	/// Panel that displays a tooltip.
	/// </summary>
	///
	/// <remarks>
	/// This class can be used in conjunction with the ToolTipTracker class by a
	/// Control object that displays various objects within its window and wants to
	/// show a tooltip for each object.  (The ToolTip class in the FCL makes it
	/// easy to show a single tooltip for an entire control, but it does not
	/// support different tooltips for different parts of the control's window.
	/// See the ToolTipTracker class for more details.)
	///
	/// <para>
	/// To use this class, have the Control object create a child <see cref="T:Microsoft.Research.CommunityTechnologies.AppLib.ToolTipPanel" /> object.  Call <see cref="M:Microsoft.Research.CommunityTechnologies.AppLib.ToolTipPanel.ShowToolTip(System.String,System.Windows.Forms.Control)" /> to show a
	/// tooltip.  Call <see cref="M:Microsoft.Research.CommunityTechnologies.AppLib.ToolTipPanel.HideToolTip" /> to hide it.
	/// </para>
	///
	/// <para>
	/// <see cref="T:Microsoft.Research.CommunityTechnologies.AppLib.ToolTipPanel" /> is better than using a standard Label control.
	/// A Label doesn't pay attention to embedded line breaks and therefore can't
	/// display a multiline tooltip.
	/// </para>
	///
	/// </remarks>
	public class ToolTipPanel : Panel
	{
		/// Margin between the tooltip text and the tooltip rectangle, and between
		/// the tooltip rectangle and the rectangle of the parent control.  (These
		/// two margins are arbitrarily set to the same value.)
		protected const int InternalMargin = 1;

		/// Tooltip text.  Can contain multiple lines separated by "\r\n".  Can be
		/// null.
		protected string m_sText;

		/// <summary>
		/// Initializes a new instance of the ToolTipPanel class.
		/// </summary>
		public ToolTipPanel()
		{
			m_sText = null;
			ForeColor = SystemColors.InfoText;
			base.Visible = false;
			base.Enabled = false;
			AssertValid();
		}

		/// <summary>
		/// Shows a tooltip.
		/// </summary>
		///
		/// <param name="sText">
		/// Tooltip text.  Can contain multiple lines separated by "\r\n".  Can be
		/// null.
		/// </param>
		///
		/// <param name="oParentControl">
		/// Parent control that is displaying the tooltip.
		/// </param>
		///
		/// <remarks>
		/// This method positions, sizes, and shows the tooltip panel.  Call <see cref="M:Microsoft.Research.CommunityTechnologies.AppLib.ToolTipPanel.HideToolTip" /> to hide the tooltip.
		/// </remarks>
		public void ShowToolTip(string sText, Control oParentControl)
		{
			Debug.Assert(oParentControl != null);
			AssertValid();
			m_sText = sText;
			Rectangle rectangle = ComputePanelRectangle(sText, oParentControl);
			base.Location = rectangle.Location;
			base.Size = rectangle.Size;
			base.Visible = true;
		}

		/// <summary>
		/// Hides the tooltip.
		/// </summary>
		///
		/// <remarks>
		/// This method hides the tooltip shown by <see cref="M:Microsoft.Research.CommunityTechnologies.AppLib.ToolTipPanel.ShowToolTip(System.String,System.Windows.Forms.Control)" />.
		/// It's okay to call this method before <see cref="M:Microsoft.Research.CommunityTechnologies.AppLib.ToolTipPanel.ShowToolTip(System.String,System.Windows.Forms.Control)" /> has
		/// been called.
		/// </remarks>
		public void HideToolTip()
		{
			AssertValid();
			base.Visible = false;
		}

		/// <summary>
		/// Computes the rectangle where the panel should be displayed.
		/// </summary>
		///
		/// <param name="sText">
		/// Tooltip text.  Can contain multiple lines separated by "\r\n".  Can be
		/// null.
		/// </param>
		///
		/// <param name="oParentControl">
		/// Parent control that is displaying the tooltip.
		/// </param>
		///
		/// <returns>
		/// The rectangle where the panel should be displayed.
		/// </returns>
		protected Rectangle ComputePanelRectangle(string sText, Control oParentControl)
		{
			Debug.Assert(oParentControl != null);
			AssertValid();
			Rectangle clientRectangle = oParentControl.ClientRectangle;
			PointF pointF = ControlUtil.GetClientMousePosition(oParentControl);
			Size size = oParentControl.Cursor.Size;
			int val = (int)pointF.X;
			int val2 = (int)pointF.Y + size.Height;
			Size size2 = clientRectangle.Size;
			val = Math.Max(val, 0);
			val = Math.Min(size2.Width, val);
			val2 = Math.Max(val2, 0);
			val2 = Math.Min(size2.Height, val2);
			Graphics graphics = CreateGraphics();
			SizeF sizeF = graphics.MeasureString(sText, Font);
			graphics.Dispose();
			Rectangle result = new Rectangle(val, val2, (int)Math.Ceiling(sizeF.Width + 2f + 2f), (int)Math.Ceiling(sizeF.Height + 2f + 2f));
			if (result.Bottom + 1 > size2.Height)
			{
				result.Offset(0, size2.Height - result.Bottom - 1);
			}
			if (result.Right + 1 > size2.Width)
			{
				result.Offset(size2.Width - result.Right - 1, 0);
			}
			result.Intersect(clientRectangle);
			Debug.Assert(result.Left >= 0);
			Debug.Assert(result.Top >= 0);
			Debug.Assert(result.Right <= size2.Width);
			Debug.Assert(result.Bottom <= size2.Height);
			return result;
		}

		/// <summary>
		/// Paints the control.
		/// </summary>
		///
		/// <param name="e">
		/// Standard event argument.
		/// </param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Debug.Assert(e != null);
			AssertValid();
			Graphics graphics = e.Graphics;
			Brush brush = new SolidBrush(ForeColor);
			graphics.DrawString(m_sText, Font, brush, new Point(1, 1));
			brush.Dispose();
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		public void AssertValid()
		{
		}
	}
}
