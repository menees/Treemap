using System;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
	/// <summary>
	/// Maintains a history list that can be used to implement back and forward
	/// navigation controls in the style of Internet Explorer.
	/// </summary>
	///
	/// <remarks>
	/// This class maintains a list of objects that store the state of something
	/// at various points in time.  The nature of the objects is entirely up to the
	/// application.  If the class were used in a browser, for example, each state
	/// object would store an URL the user visited.
	///
	/// <para>
	/// The object list can be thought of as organized from left to right, with
	/// older objects to the left and newer ones to the right.  The class supports
	/// the insertion of new state objects and back/forward navigation through
	/// existing objects.
	/// </para>
	///
	/// <para>
	/// Call <see cref="M:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.InsertState(System.Object)" /> to insert the first state object.  An
	/// internal current state object pointer gets set to the new object.  Call
	/// <see cref="M:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.InsertState(System.Object)" /> again to insert another state object.  This adds
	/// the new object to the right of the current object and points the current
	/// state to the new object.
	/// </para>
	///
	/// <para>
	/// Call <see cref="P:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.PreviousState" /> to retrieve the state object to the left
	/// of the current object, if there is one.  The retrieved object then becomes
	/// the current object.  You can determine whether there is a previous object
	/// by checking the <see cref="P:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.HasPreviousState" /> property.  <see cref="P:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.NextState" /> and <see cref="P:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.HasNextState" /> work the same way, but
	/// in the other direction.
	/// </para>
	///
	/// <para>
	/// <see crerf="Reset" /> can be used to remove all state objects and return
	/// the HistoryList to the state it was in when it was first created.
	/// </para>
	///
	/// <para>
	/// <see cref="M:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.InsertState(System.Object)" /> deletes all state objects to the right of the
	/// current object.
	/// </para>
	///
	/// <para>
	/// A <see cref="E:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.Change" /> event is fired whenever the number of state objects
	/// changes or the current state changes.  You can use this to enable
	/// back/forward toolbar buttons.
	/// </para>
	///
	/// </remarks>
	public class HistoryList
	{
		/// <summary>
		/// Represents the method that handles the <see cref="E:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.Change" /> event.
		/// </summary>
		///
		/// <param name="oSource">
		/// Standard event argument.
		/// </param>
		///
		/// <param name="oEventArgs">
		/// Standard event argument.
		/// </param>
		public delegate void ChangeEventHandler(object oSource, EventArgs oEventArgs);

		/// List of state objects.
		protected ArrayList m_oStateList;

		/// Index of the current state object, or -1 if there is none.
		protected int m_iCurrentObjectIndex;

		/// <summary>
		/// Gets the state object to the right of the current object.
		/// </summary>
		///
		/// <value>
		/// The state object to the right of the current object.  The returned
		/// object then becomes the current object.
		/// </value>
		///
		/// <remarks>
		/// An exception is thrown if there is no object to the right of the
		/// current object.  Check the HasNextState property before calling this.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.HasNextState" />
		public object NextState
		{
			get
			{
				AssertValid();
				if (!HasNextState)
				{
					throw new InvalidOperationException("HistoryList.NextState: There is no next state.  Check HasNextState before calling this.");
				}
				object result = m_oStateList[m_iCurrentObjectIndex + 1];
				m_iCurrentObjectIndex++;
				AssertValid();
				FireChangeEvent();
				return result;
			}
		}

		/// <summary>
		/// Gets the state object to the left of the current object.
		/// </summary>
		///
		/// <value>
		/// The state object to the left of the current object.  The returned
		/// object then becomes the current object.
		/// </value>
		///
		/// <remarks>
		/// An exception is thrown if there is no object to the left of the
		/// current object.  Check the HasPreviousState property before calling
		/// this.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.HasPreviousState" />
		public object PreviousState
		{
			get
			{
				AssertValid();
				if (!HasPreviousState)
				{
					throw new InvalidOperationException("HistoryList.PreviousState: There is no previous state.  Check HasPreviousState before calling this.");
				}
				object result = m_oStateList[m_iCurrentObjectIndex - 1];
				m_iCurrentObjectIndex--;
				AssertValid();
				FireChangeEvent();
				return result;
			}
		}

		/// <summary>
		/// Gets a flag indicating whether there is a state object to the right of
		/// the current object.
		/// </summary>
		///
		/// <value>
		/// true if there is a state object to the right of the current object.
		/// </value>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.NextState" />
		public bool HasNextState
		{
			get
			{
				AssertValid();
				return m_iCurrentObjectIndex < m_oStateList.Count - 1;
			}
		}

		/// <summary>
		/// Gets a flag indicating whether there is a state object to the left of
		/// the current object.
		/// </summary>
		///
		/// <value>
		/// true if there is a state object to the left of the current object.
		/// </value>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.AppLib.HistoryList.PreviousState" />
		public bool HasPreviousState
		{
			get
			{
				AssertValid();
				return m_iCurrentObjectIndex > 0;
			}
		}

		/// <summary>
		/// Gets fired when the number of state objects changes or the current
		/// state changes.
		/// </summary>
		public event ChangeEventHandler Change;

		/// <summary>
		/// Initializes a new instance of the HistoryList class.
		/// </summary>
		public HistoryList()
		{
			m_oStateList = new ArrayList();
			m_iCurrentObjectIndex = -1;
			AssertValid();
		}

		/// <summary>
		/// Inserts a state object to the right of the current object.
		/// </summary>
		///
		/// <param name="oState">
		/// State object to insert.  The nature of the object is entirely up to the
		/// application.
		/// </param>
		///
		/// <returns>
		/// Inserted <paramref name="oState" /> object.
		/// </returns>
		///
		/// <remarks>
		/// This method inserts a state object to the right of the current object.
		/// The inserted object then becomes the current object.  Any objects to
		/// the right of the inserted object are deleted.
		/// </remarks>
		public object InsertState(object oState)
		{
			Debug.Assert(oState != null);
			AssertValid();
			m_oStateList.RemoveRange(m_iCurrentObjectIndex + 1, m_oStateList.Count - m_iCurrentObjectIndex - 1);
			m_oStateList.Add(oState);
			m_iCurrentObjectIndex++;
			AssertValid();
			Debug.Assert(m_iCurrentObjectIndex == m_oStateList.Count - 1);
			FireChangeEvent();
			return oState;
		}

		/// <summary>
		/// Removes all state objects and returns the HistoryList to the state it
		/// was in when it was first created.
		/// </summary>
		public void Reset()
		{
			m_oStateList.Clear();
			m_iCurrentObjectIndex = -1;
			FireChangeEvent();
			AssertValid();
		}

		/// <summary>
		/// Fires the Change event.
		/// </summary>
		protected void FireChangeEvent()
		{
			if (this.Change != null)
			{
				EventArgs oEventArgs = new EventArgs();
				this.Change(this, oEventArgs);
			}
		}

		/// <summary>
		/// Override of base class method.
		/// </summary>
		public override string ToString()
		{
			AssertValid();
			return "HistoryList object: Number of state objects: " + m_oStateList.Count + ".  Current object:" + m_iCurrentObjectIndex + ".";
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(m_oStateList != null);
			Debug.Assert(m_iCurrentObjectIndex >= -1);
			if (m_iCurrentObjectIndex >= 0)
			{
				Debug.Assert(m_iCurrentObjectIndex < m_oStateList.Count);
			}
		}
	}
}
