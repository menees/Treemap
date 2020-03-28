using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	internal struct NodeColor
	{
		private float m_fColorMetric;

		private Color m_oAbsoluteColor;

		public float ColorMetric
		{
			get
			{
				AssertValid();
				if (float.IsNaN(m_fColorMetric))
				{
					return 0f;
				}
				return m_fColorMetric;
			}
			set
			{
				Debug.Assert(!float.IsNaN(value));
				m_fColorMetric = value;
				AssertValid();
			}
		}

		public Color AbsoluteColor
		{
			get
			{
				AssertValid();
				return m_oAbsoluteColor;
			}
			set
			{
				m_oAbsoluteColor = value;
				AssertValid();
			}
		}

		public NodeColor(float fColorMetric)
		{
			m_oAbsoluteColor = Color.Black;
			m_fColorMetric = fColorMetric;
			AssertValid();
		}

		public NodeColor(Color oAbsoluteColor)
		{
			m_fColorMetric = 0f;
			m_oAbsoluteColor = oAbsoluteColor;
		}

		[Conditional("DEBUG")]
		public void AssertValid()
		{
		}
	}
}
