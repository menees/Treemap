using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	/// <summary>
	/// ToolTipTrackerEventArgs class.
	/// </summary>
	///
	/// <remarks>
	/// Events fired by the ToolTipTracker object include a ToolTipTrackerEventArgs
	/// object as one of the event arguments.
	/// </remarks>
	internal class ToolTipTrackerEventArgs : EventArgs
	{
		private readonly object m_oObject;

		/// <summary>
		/// Object property.
		/// </summary>
		///
		/// <value>
		/// Object.  Returns the Object for which a tooltip window should be shown
		/// or hidden.  Read-only.
		/// </value>
		public object Object
		{
			get
			{
				AssertValid();
				return m_oObject;
			}
		}

		/// <summary>
		/// ToolTipTrackerEventArgs constructor.
		/// </summary>
		///
		/// <param name="oObject">
		/// Object.  Object for which a tooltip window should be shown or hidden.
		/// </param>
		public ToolTipTrackerEventArgs(object oObject)
		{
			m_oObject = oObject;
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
			Debug.Assert(m_oObject != null);
		}
	}
}
