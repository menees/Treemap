using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Research.CommunityTechnologies.ControlLib
{
	/// <summary>
	/// Control utility methods.
	/// </summary>
	///
	/// <remarks>
	/// This class contains utility methods for dealing with Control-derived
	/// objects.  All methods are static.
	/// </remarks>
	public class ControlUtil
	{
		/// <summary>
		/// Do not use this constructor.
		/// </summary>
		///
		/// <remarks>
		/// Do not use this constructor.  All ControlUtil methods are static.
		/// </remarks>
		private ControlUtil()
		{
		}

		/// <summary>
		/// Gets the current mouse position in client coordinates.
		/// </summary>
		///
		/// <param name="oControl">
		/// Control to use for the client coordinates.
		/// </param>
		///
		/// <returns>
		/// Mouse position in client coordinates.
		/// </returns>
		///
		/// <remarks>
		/// NOTE: The point returned by this method can be outside the control's
		/// client area.  This can happen if the user is moving the mouse quickly.
		/// The caller should check for this.
		/// </remarks>
		public static Point GetClientMousePosition(Control oControl)
		{
			Debug.Assert(oControl != null);
			Point mousePosition = Control.MousePosition;
			return oControl.PointToClient(mousePosition);
		}
	}
}
