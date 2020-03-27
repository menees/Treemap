using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	/// <summary>
	/// Represents the empty space within a parent rectangle.
	/// </summary>
	///
	/// <remarks>
	/// The <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.Nodes" /> collection owned by each <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
	/// object has an <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Nodes.EmptySpace" /> property.  EmptySpace has a
	/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.EmptySpace.SizeMetric" /> property that determines how much empty space
	/// appears in the upper right corner of the parent rectangle corresponding to
	/// the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />.
	///
	/// <para>
	/// See <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" /> for details on how the size of each
	/// node rectangle is computed and how EmptySpace is involved in the
	/// computations.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" />
	public class EmptySpace
	{
		/// Owner of this object, or null if the TreemapGenerator property hasn't
		/// been set yet.
		protected TreemapGenerator m_oTreemapGenerator;

		/// Determines the size of the empty space.
		protected float m_fSizeMetric;

		/// <summary>
		/// Gets or sets the size of the empty space in the parent rectangle.
		/// </summary>
		///
		/// <value>
		/// A metric that determines the size of the empty space in the parent
		/// rectangle.  Must be greater than or equal to zero.
		/// </value>
		public float SizeMetric
		{
			get
			{
				AssertValid();
				return m_fSizeMetric;
			}
			set
			{
				Node.ValidateSizeMetric(value, "EmptySpace.SizeMetric");
				if (m_fSizeMetric != value)
				{
					m_fSizeMetric = value;
					FireRedrawRequired();
				}
			}
		}

		/// <summary>
		/// Sets the object that owns this object.
		/// </summary>
		///
		/// <value>
		/// The TreemapGenerator object that owns this object.
		/// </value>
		///
		/// <remarks>
		/// This method must be called after this object is added to the
		///             TreemapGenerator.
		/// </remarks>
		protected internal TreemapGenerator TreemapGenerator
		{
			set
			{
				m_oTreemapGenerator = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Initializes a new instance of the EmptySpace class.
		/// </summary>
		protected internal EmptySpace()
		{
			m_oTreemapGenerator = null;
			m_fSizeMetric = 0f;
		}

		/// <summary>
		/// Fires the TreemapGenerator.RedrawRequired event if appropriate.
		/// </summary>
		///
		/// <remarks>
		/// This should be called when something occurs that affects the treemap's
		/// appearance.
		/// </remarks>
		protected void FireRedrawRequired()
		{
			if (m_oTreemapGenerator != null)
			{
				m_oTreemapGenerator.FireRedrawRequired();
			}
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Node.ValidateSizeMetric(m_fSizeMetric, "EmptySpace.m_fSizeMetric");
		}
	}
}
