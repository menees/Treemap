using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	/// <summary>
	/// Helper class for displaying tooltips.
	/// </summary>
	///
	/// <remarks>
	/// This is meant for use by a Control object that displays various objects
	/// within its window and wants to show a tooltip for each object.  The ToolTip
	/// class in the FCL makes it easy to show a single tooltip for an entire
	/// control, but it does not support different tooltips for different parts of
	/// the control's window.
	///
	/// <para>
	/// To use ToolTipTracker, call <see cref="M:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.OnMouseMoveOverObject(System.Object)" /> from the
	/// control's MouseMove event handler.  If the mouse is currently over an
	/// object that has a tooltip associated with it, pass the object as the
	/// method's oObjectToTrack parameter.  Otherwise, pass null.  Also, call
	/// <see cref="M:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.OnMouseMoveOverObject(System.Object)" /> with a null parameter from the
	/// control's MouseLeave event handler.
	/// </para>
	///
	/// <para>
	/// If the mouse remains over an object for a period of <see cref="P:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.ShowDelayMs" /> milliseconds, ToolTipTracker fires a <see cref="E:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.ShowToolTip" /> event.  The event arguments include the object being
	/// tracked.
	/// </para>
	///
	/// <para>
	/// A <see cref="E:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.HideToolTip" /> event is fired when the tooltip should be
	/// hidden.  This occurs <see cref="P:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.HideDelayMs" /> after the <see cref="E:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.ShowToolTip" /> event fires if the mouse remains over the object, or
	/// immediately if OnMouseMoveOverObject(null) is called.
	/// </para>
	///
	/// <para>
	/// Note that ToolTipTracker does not actually show or hide the tooltip; that's
	/// up to the application.  The easiest way to do this is to create a child
	/// ToolTipPanel control and call its Show and Hide methods in response to the
	/// <see cref="E:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.ShowToolTip" /> and <see cref="E:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.HideToolTip" /> events.
	/// </para>
	///
	/// <para>
	/// If the mouse is moved to another object within <see cref="P:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.ReshowDelayMs" />
	/// milliseconds, another <see cref="E:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.ShowToolTip" /> event is fired.
	/// Otherwise, the waiting period reverts to <see cref="P:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.ShowDelayMs" />.
	/// </para>
	///
	/// <para>
	/// Call <see cref="M:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.Reset" /> to reset ToolTipTracker to its initial state.
	/// This forces a <see cref="E:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.HideToolTip" /> event if a tooltip is showing.
	/// </para>
	///
	/// <para>
	/// <b>IMPORTANT</b>
	/// </para>
	///
	/// <para>
	/// The control must call <see cref="M:Microsoft.Research.CommunityTechnologies.GraphicsLib.ToolTipTracker.Dispose" /> from its own Dispose method.
	/// This prevents timer-based events from firing after the control no longer
	/// has a handle.
	/// </para>
	///
	/// </remarks>
	internal class ToolTipTracker : IDisposable
	{
		/// <summary>
		/// ToolTipTrackerEvent delegate.
		/// </summary>
		///
		/// <param name="oSource">
		/// Object.  Source of the event.
		/// </param>
		///
		/// <param name="oToolTipTrackerEventArgs">
		/// ToolTipTrackerEventArgs.  Provides information about the object for
		/// which a tooltip window should be shown or hidden.
		/// </param>
		///
		/// <remarks>
		/// This delegate is used in all events fired by ToolTipTracker.
		/// </remarks>
		public delegate void ToolTipTrackerEvent(object oSource, ToolTipTrackerEventArgs oToolTipTrackerEventArgs);

		protected enum State
		{
			NotDoingAnything,
			WaitingForShowTimeout,
			WaitingForHideTimeout,
			WaitingForReshowTimeout
		}

		public const int MinDelayMs = 1;

		public const int MaxDelayMs = 10000;

		/// Default value for the ShowDelayMs property.
		protected const int DefaultShowDelayMs = 500;

		/// Default value for the HideDelayMs property.
		protected const int DefaultHideDelayMs = 5000;

		/// Default value for the ReshowDelayMs property.
		protected const int DefaultReshowDelayMs = 50;

		protected int m_iShowDelayMs;

		protected int m_iHideDelayMs;

		protected int m_iReshowDelayMs;

		protected State m_iState;

		protected object m_oObjectBeingTracked;

		protected Timer m_oTimer;

		protected bool m_bDisposed;

		/// <summary>
		/// ShowDelayMs property.
		/// </summary>
		///
		/// <value>
		/// Int32.  Number of milliseconds to wait to fire the ShowToolTip event
		/// after OnMouseMoveOverObject(oObjectToTrack) is first called.
		/// </value>
		public int ShowDelayMs
		{
			get
			{
				AssertValid();
				return m_iShowDelayMs;
			}
			set
			{
				ValidateDelayProperty(value, "ShowDelayMs");
				m_iShowDelayMs = value;
			}
		}

		/// <summary>
		/// HideDelayMs property.
		/// </summary>
		///
		/// <value>
		/// Int32.  Number of milliseconds to wait to fire the HideToolTip event
		/// after ShowToolTip is fired.
		/// </value>
		public int HideDelayMs
		{
			get
			{
				AssertValid();
				return m_iHideDelayMs;
			}
			set
			{
				ValidateDelayProperty(value, "HideDelayMs");
				m_iHideDelayMs = value;
			}
		}

		/// <summary>
		/// ReshowDelayMs property.
		/// </summary>
		///
		/// <value>
		/// Int32.  Period after a HideToolTip event during which a ShowToolTip
		/// event will be fired immediately if the mouse is moved over another
		/// object.  If this period elapses without
		/// OnMouseMoveOverObject(oObjectToTrack) being called, the waiting period
		/// reverts to m_iShowDelayMs.
		/// </value>
		public int ReshowDelayMs
		{
			get
			{
				AssertValid();
				return m_iReshowDelayMs;
			}
			set
			{
				ValidateDelayProperty(value, "ReshowDelayMs");
				m_iReshowDelayMs = value;
			}
		}

		/// <summary>
		/// ShowToolTip event.
		/// </summary>
		///
		/// <remarks>
		/// Fired when a tooltip window should be shown.
		/// </remarks>
		public event ToolTipTrackerEvent ShowToolTip;

		/// <summary>
		/// HideToolTip event.
		/// </summary>
		///
		/// <remarks>
		/// Fired when a tooltip window should be hidden.
		/// </remarks>
		public event ToolTipTrackerEvent HideToolTip;

		/// <summary>
		/// ToolTipTracker constructor.
		/// </summary>
		public ToolTipTracker()
		{
			m_iShowDelayMs = 500;
			m_iHideDelayMs = 5000;
			m_iReshowDelayMs = 50;
			m_iState = State.NotDoingAnything;
			m_oObjectBeingTracked = null;
			m_bDisposed = false;
			m_oTimer = new Timer();
			m_oTimer.Tick += TimerTick;
		}

		/// <summary>
		/// ToolTipTracker destructor.
		/// </summary>
		~ToolTipTracker()
		{
			Dispose(bDisposing: false);
		}

		/// <summary>
		/// OnMouseMoveOverObject method.
		/// </summary>
		///
		/// <param name="oObjectToTrack">
		/// Object.  Object to track, or null to stop tracking.
		/// </param>
		///
		/// <remarks>
		/// Call this with an Object parameter when the mouse moves over an object
		/// that should be tracked.  Call it with a null parameter when the mouse
		/// moves over an area of the control where there is no object, and when
		/// the mouse leaves the control.
		/// </remarks>
		public void OnMouseMoveOverObject(object oObjectToTrack)
		{
			AssertValid();
			switch (m_iState)
			{
			case State.NotDoingAnything:
				if (oObjectToTrack != null)
				{
					ChangeState(State.WaitingForShowTimeout, oObjectToTrack);
				}
				break;
			case State.WaitingForShowTimeout:
				if (oObjectToTrack == null)
				{
					ChangeState(State.NotDoingAnything, null);
				}
				else if (oObjectToTrack != m_oObjectBeingTracked)
				{
					ChangeState(State.WaitingForShowTimeout, oObjectToTrack);
				}
				break;
			case State.WaitingForHideTimeout:
				if (oObjectToTrack == null)
				{
					FireHideToolTipEvent(m_oObjectBeingTracked);
					ChangeState(State.WaitingForReshowTimeout, null);
				}
				else if (oObjectToTrack == m_oObjectBeingTracked)
				{
					ChangeState(State.WaitingForHideTimeout, oObjectToTrack);
				}
				else
				{
					FireHideToolTipEvent(m_oObjectBeingTracked);
					FireShowToolTipEvent(oObjectToTrack);
					ChangeState(State.WaitingForHideTimeout, oObjectToTrack);
				}
				break;
			case State.WaitingForReshowTimeout:
				if (oObjectToTrack != null)
				{
					FireShowToolTipEvent(oObjectToTrack);
					ChangeState(State.WaitingForHideTimeout, oObjectToTrack);
				}
				break;
			default:
				Debug.Assert(condition: false);
				break;
			}
		}

		/// <summary>
		/// Reset method.
		/// </summary>
		///
		/// <remarks>
		/// Resets the object to its initial state.  This forces a HideToolTip
		/// event if a tooltip is showing.
		/// </remarks>
		public void Reset()
		{
			AssertValid();
			m_oTimer.Stop();
			if (m_iState == State.WaitingForHideTimeout)
			{
				FireHideToolTipEvent(m_oObjectBeingTracked);
			}
			ChangeState(State.NotDoingAnything, null);
		}

		/// <summary>
		/// Dispose method.
		/// </summary>
		///
		/// <remarks>
		/// Performs application-defined tasks associated with freeing, releasing,
		/// or resetting unmanaged resources.
		/// </remarks>
		public void Dispose()
		{
			Dispose(bDisposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// ValidateDelayProperty method.
		/// </summary>
		///
		/// <param name="iValue">
		/// Int32.  Property value.
		/// </param>
		///
		/// <param name="sPropertyName">
		/// String.  Name of the property being validated.  Sample: "ShowDelayMs".
		/// </param>
		///
		/// <remarks>
		/// Validates one of the xxDelayMs properties.  Throws an exception if the
		/// value is out of range.
		/// </remarks>
		protected void ValidateDelayProperty(int iValue, string sPropertyName)
		{
			if (iValue < 1 || iValue > 10000)
			{
				throw new ArgumentOutOfRangeException(sPropertyName, iValue, "ToolTipTracker." + sPropertyName + ": Must be between " + 1 + " and " + 10000 + ".");
			}
		}

		/// <summary>
		/// ChangeState method.
		/// </summary>
		///
		/// <param name="iState">
		/// State.  New object state.
		/// </param>
		///
		/// <param name="oObjectToTrack">
		/// Object.  Object to track, or null to stop tracking.
		/// </param>
		///
		/// <remarks>
		/// Changes the state of the object.
		/// </remarks>
		protected void ChangeState(State iState, object oObjectToTrack)
		{
			AssertValid();
			m_oTimer.Stop();
			m_iState = iState;
			m_oObjectBeingTracked = oObjectToTrack;
			int num;
			switch (iState)
			{
			case State.NotDoingAnything:
				num = -1;
				break;
			case State.WaitingForShowTimeout:
				num = m_iShowDelayMs;
				break;
			case State.WaitingForHideTimeout:
				num = m_iHideDelayMs;
				break;
			case State.WaitingForReshowTimeout:
				num = m_iReshowDelayMs;
				break;
			default:
				Debug.Assert(condition: false);
				num = -1;
				break;
			}
			if (num != -1)
			{
				m_oTimer.Interval = num;
				m_oTimer.Start();
			}
			AssertValid();
		}

		/// <summary>
		/// FireShowToolTipEvent method.
		/// </summary>
		///
		/// <param name="oObject">
		/// Object.  Object to show a tooltip for.
		/// </param>
		///
		/// <remarks>
		/// Fires the ShowToolTip event.
		/// </remarks>
		protected void FireShowToolTipEvent(object oObject)
		{
			Debug.Assert(oObject != null);
			this.ShowToolTip?.Invoke(this, new ToolTipTrackerEventArgs(oObject));
		}

		/// <summary>
		/// FireHideToolTipEvent method.
		/// </summary>
		///
		/// <param name="oObject">
		/// Object.  Object to hide the tooltip for.
		/// </param>
		///
		/// <remarks>
		/// Fires the HideToolTip event.
		/// </remarks>
		protected void FireHideToolTipEvent(object oObject)
		{
			Debug.Assert(oObject != null);
			this.HideToolTip?.Invoke(this, new ToolTipTrackerEventArgs(oObject));
		}

		/// <summary>
		/// TimerTick method.
		/// </summary>
		///
		/// <param name="oSource">
		/// Object.  Source of the event.
		/// </param>
		///
		/// <param name="oEventArgs">
		/// Standard event arguments.
		/// </param>
		///
		/// <remarks>
		/// Timer event handler.
		/// </remarks>
		protected void TimerTick(object oSource, EventArgs oEventArgs)
		{
			AssertValid();
			m_oTimer.Stop();
			switch (m_iState)
			{
			case State.NotDoingAnything:
				Debug.Assert(condition: false);
				break;
			case State.WaitingForShowTimeout:
				FireShowToolTipEvent(m_oObjectBeingTracked);
				ChangeState(State.WaitingForHideTimeout, m_oObjectBeingTracked);
				break;
			case State.WaitingForHideTimeout:
				FireHideToolTipEvent(m_oObjectBeingTracked);
				ChangeState(State.WaitingForReshowTimeout, null);
				break;
			case State.WaitingForReshowTimeout:
				ChangeState(State.NotDoingAnything, null);
				break;
			default:
				Debug.Assert(condition: false);
				break;
			}
		}

		/// <summary>
		/// Dispose method.
		/// </summary>
		///
		/// <remarks>
		/// Performs application-defined tasks associated with freeing, releasing,
		/// or resetting unmanaged resources.
		/// </remarks>
		///
		/// <param name="bDisposing">
		/// See IDisposable.
		/// </param>
		protected void Dispose(bool bDisposing)
		{
			if (!m_bDisposed && bDisposing)
			{
				m_oTimer.Stop();
				m_oTimer.Dispose();
			}
			m_bDisposed = true;
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
			Debug.Assert(m_iShowDelayMs >= 1);
			Debug.Assert(m_iShowDelayMs <= 10000);
			Debug.Assert(m_iHideDelayMs >= 1);
			Debug.Assert(m_iHideDelayMs <= 10000);
			Debug.Assert(m_iReshowDelayMs >= 1);
			Debug.Assert(m_iReshowDelayMs <= 10000);
			switch (m_iState)
			{
			case State.NotDoingAnything:
				Debug.Assert(m_oObjectBeingTracked == null);
				break;
			case State.WaitingForShowTimeout:
				Debug.Assert(m_oObjectBeingTracked != null);
				break;
			case State.WaitingForHideTimeout:
				Debug.Assert(m_oObjectBeingTracked != null);
				break;
			case State.WaitingForReshowTimeout:
				Debug.Assert(m_oObjectBeingTracked == null);
				break;
			default:
				Debug.Assert(condition: false);
				break;
			}
		}
	}
}
