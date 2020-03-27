using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using System;
using System.Diagnostics;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	/// <summary>
	/// Represents one rectangle within a treemap.
	/// </summary>
	///
	/// <remarks>
	/// Node objects are arranged in a hierarchy.  The
	/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent.Nodes" /> property on <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent" /> returns a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
	/// collection containing the top-level Node objects.  Each Node object has a
	/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
	/// collection containing its child Node objects.  A leaf Node object has an
	/// empty <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
	/// collection.
	///
	/// <para>
	/// To add a Node to the hierarchy, you can use an Add method on the
	/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
	/// collection that creates the Node and adds it in one step.  You can also
	/// explicitly create a new Node object and add it to the collection using the
	/// Add method that takes a Node object.
	/// </para>
	///
	/// </remarks>
	public class Node : IComparable
	{
		/// Owner of this object, or null if the TreemapGenerator property hasn't
		/// been set yet.
		protected TreemapGenerator m_oTreemapGenerator;

		/// Parent node, or null if this node is in the top-level nodes collection
		/// or the node hasn't been added to a nodes collection yet.
		protected Node m_oParentNode;

		/// Text to display within the node's rectangle.
		protected string m_sText;

		/// Determines the size of the node's rectangle.
		protected float m_fSizeMetric;

		private NodeColor m_oNodeColor;

		/// Object the caller wants to associate with this node, or null for none.
		protected object m_oTag;

		/// Tooltip to display for this node.  This is used by the TreemapControl
		/// object.
		protected string m_sToolTip;

		/// Collection of this node's child Node objects.
		protected Nodes m_oNodes;

		/// Where the node's rectangle will be drawn.
		protected RectangleF m_oRectangle;

		/// Saved version of m_oRectangle.  See SaveRectangle().
		protected RectangleF m_oSavedRectangle;

		/// Width of the pen that should be used to draw the rectangle.  This is
		/// stored in the Node object along with the rectangle because it's one of
		/// the factors used to calculate the rectangle, and it's needed again when
		/// the rectangle is drawn.
		protected int m_iPenWidthPx;

		/// true if the Rectangle property has been set.
		protected bool m_bRectangleSet;

		/// true if SaveRectangle() has been called.
		protected bool m_bRectangleSaved;

		/// <summary>
		/// Gets or sets the node's text.
		/// </summary>
		///
		/// <value>
		/// The text that gets displayed in the center of the node's rectangle.
		/// </value>
		public string Text
		{
			get
			{
				AssertValid();
				return m_sText;
			}
			set
			{
				if (m_sText != value)
				{
					m_sText = value;
					FireRedrawRequired();
				}
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the metric that determines the size of the node's
		/// rectangle.
		/// </summary>
		///
		/// <value>
		/// The metric that determines the size of the node's rectangle within the
		/// parent rectangle.  Must be greater than or equal to zero.
		/// </value>
		///
		/// <remarks>
		/// To compute the rectangle sizes for the nodes in a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
		/// collection, the drawing engine adds up the SizeMetric value for the
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects, adds the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.EmptySpace.SizeMetric" /> value from the
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.EmptySpace" /> object owned by the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
		/// collection, and divides the size of the parent rectangle by the
		/// resulting sum.  This "area per SizeMetric" factor is multiplied by each
		/// node's SizeMetric value to obtain the size of the node's rectangle.
		///
		/// <para>
		/// Thus, the size of the node's rectangle depends on the following:
		///
		/// <list type="bullet">
		///
		/// <item>
		/// <description>The node's SizeMetric value</description>
		/// </item>
		///
		/// <item>
		/// <description>The SizeMetric values of the node's siblings</description>
		/// </item>
		///
		/// <item>
		/// <description>The SizeMetric value of the Nodes.EmptySpace object
		/// </description>
		/// </item>
		///
		/// <item>
		/// <description>The size of the parent rectangle</description>
		/// </item>
		///
		/// </list>
		///
		/// </para>
		///
		/// </remarks>
		public float SizeMetric
		{
			get
			{
				AssertValid();
				return m_fSizeMetric;
			}
			set
			{
				ValidateSizeMetric(value, "Node.SizeMetric");
				if (m_fSizeMetric != value)
				{
					m_fSizeMetric = value;
					FireRedrawRequired();
				}
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the metric that determines the fill color of the node's
		/// rectangle.
		/// </summary>
		///
		/// <value>
		/// The metric that determines fill color of the node's rectangle.
		/// </value>
		///
		/// <remarks>
		/// The fill color of the node's rectangle is determined as follows.  Note
		/// that TreemapGenerator.MinColorMetric is a negative number and
		/// TreemapGenerator.MaxColorMetric is positive.
		///
		/// <list type="table">
		///
		/// <listheader>
		/// <term>ColorMetric Value</term>
		/// <description>Color</description>
		/// </listheader>
		///
		/// <item>
		/// <term>&lt;= TreemapGenerator.MinColorMetric</term>
		/// <description>TreemapGenerator.MinColor</description>
		/// </item>
		///
		/// <item>
		/// <term>&gt; TreemapGenerator.MinColorMetric and &lt; 0</term>
		/// <description>A color on the gradient between TreemapGenerator.MinColor
		/// and white</description>
		/// </item>
		///
		/// <item>
		/// <term>0</term>
		/// <description>White</description>
		/// </item>
		///
		/// <item>
		/// <term>&gt; 0 and &lt; TreemapGenerator.MaxColorMetric</term>
		/// <description>A color on the gradient between white and
		/// TreemapGenerator.MaxColor</description>
		/// </item>
		///
		/// <item>
		/// <term>&gt;= TreemapGenerator.MaxColorMetric</term>
		/// <description>TreemapGenerator.MaxColor</description>
		/// </item>
		///
		/// </list>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColor" />
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColorMetric" />
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColor" />
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColorMetric" />
		public float ColorMetric
		{
			get
			{
				AssertValid();
				return m_oNodeColor.ColorMetric;
			}
			set
			{
				ValidateColorMetric(value, "Node.ColorMetric");
				if (m_oNodeColor.ColorMetric != value)
				{
					m_oNodeColor.ColorMetric = value;
					FireRedrawRequired();
				}
				AssertValid();
			}
		}

		public Color AbsoluteColor
		{
			get
			{
				AssertValid();
				return m_oNodeColor.AbsoluteColor;
			}
			set
			{
				if (m_oNodeColor.AbsoluteColor != value)
				{
					m_oNodeColor.AbsoluteColor = value;
					FireRedrawRequired();
				}
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the arbitrary object associated with the node.
		/// </summary>
		///
		/// <value>
		/// The arbitrary object associated with the node, as an
		/// <see cref="T:System.Object" />.  The default value is null.
		/// </value>
		///
		/// <remarks>
		/// This property exists only for the caller's convenience.  It is not used
		/// by the treemap components.
		///
		/// <para>
		/// By default, this property is not serialized in the XML string returned
		/// by <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent.NodesXml" />.  If you want this
		/// property to be serialized, you must set <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent.NodesXmlAttributeOverrides" />.
		/// </para>
		///
		/// </remarks>
		public object Tag
		{
			get
			{
				AssertValid();
				return m_oTag;
			}
			set
			{
				m_oTag = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the node's tooltip.
		/// </summary>
		///
		/// <value>
		/// The tooltip to display for the node.  Use "\n" or "\r\n" to separate
		/// multiple lines.  Can be null.
		/// </value>
		///
		/// <remarks>
		/// The tooltip is used by the TreemapControl.  It is not used by the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.TreemapGenerator" />.
		/// </remarks>
		public string ToolTip
		{
			get
			{
				AssertValid();
				return m_sToolTip;
			}
			set
			{
				m_sToolTip = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets the collection of child <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.
		/// </summary>
		///
		/// <value>
		/// A <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
		/// collection of child <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.
		/// </value>
		public Nodes Nodes
		{
			get
			{
				AssertValid();
				return m_oNodes;
			}
		}

		/// <summary>
		/// Gets the parent <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object.
		/// </summary>
		///
		/// <value>
		/// The parent <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object, or null if this node belongs to
		/// the top-level <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.Nodes" /> collection or the node hasn't been
		/// added to a collection.
		/// </value>
		public Node Parent
		{
			get
			{
				AssertValid();
				return m_oParentNode;
			}
		}

		/// <summary>
		/// Gets the node's level.
		/// </summary>
		///
		/// <value>
		/// The node's level.  Top-level nodes are at level 0.
		/// </value>
		///
		/// <remarks>
		/// 0 is returned if the node hasn't been added to a collection.
		/// </remarks>
		public int Level
		{
			get
			{
				AssertValid();
				Node parent = Parent;
				int num = 0;
				while (parent != null)
				{
					parent = parent.Parent;
					num++;
				}
				return num;
			}
		}

		/// <summary>
		/// HasBeenDrawn property.
		/// </summary>
		///
		/// <value>
		/// Boolean.  true if the rectangle has been drawn.
		/// </value>
		protected internal bool HasBeenDrawn
		{
			get
			{
				AssertValid();
				if (!m_oRectangle.IsEmpty)
				{
					Debug.Assert(m_bRectangleSet);
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Rectangle property.
		/// </summary>
		///
		/// <value>
		/// RectangleF.  The node's rectangle.
		/// </value>
		///
		/// <remarks>
		/// This property provides the floating-point rectangle that should be
		/// used in all rectangle calculations.  To draw the rectangle, use the
		/// RectangleToDraw property instead.
		///
		/// <para>
		/// Do not get this property without setting it first.
		/// </para>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.RectangleToDraw" />
		///
		/// </remarks>
		protected internal RectangleF Rectangle
		{
			get
			{
				AssertValid();
				Debug.Assert(m_bRectangleSet);
				return m_oRectangle;
			}
			set
			{
				m_oRectangle = value;
				m_bRectangleSet = true;
				AssertValid();
			}
		}

		/// <summary>
		/// RectangleToDraw property.
		/// </summary>
		///
		/// <value>
		/// Rectangle.  The integer rectangle to draw.  Read-only.
		/// </value>
		///
		/// <remarks>
		/// This property provides the integer rectangle that should be used for
		/// drawing the node's rectangle.  All rectangle calculations should use
		/// the floating-point Rectangle property instead.
		///
		/// <para>
		/// Do not get this property without setting Rectangle first.
		/// </para>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.Rectangle" />
		///
		/// </remarks>
		protected internal Rectangle RectangleToDraw
		{
			get
			{
				AssertValid();
				return GraphicsUtil.RectangleFToRectangle(Rectangle, PenWidthPx);
			}
		}

		/// <summary>
		/// AspectRatio property.
		/// </summary>
		///
		/// <value>
		/// Double.  Aspect ratio of the node's rectangle.  Read-only.
		/// </value>
		///
		/// <remarks>
		/// The aspect ratio is the ratio of the rectangle's longer side to its
		/// shorter side.  Do not get this property without setting
		/// Node.Rectangle first.  If the shorter side has a length of zero,
		/// Double.MaxValue is returned.
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.Rectangle" />
		///
		/// </remarks>
		protected internal double AspectRatio
		{
			get
			{
				AssertValid();
				Debug.Assert(m_bRectangleSet);
				float width = m_oRectangle.Width;
				float height = m_oRectangle.Height;
				if (width > height)
				{
					if (height == 0f)
					{
						return double.MaxValue;
					}
					return width / height;
				}
				if (width == 0f)
				{
					return double.MaxValue;
				}
				return height / width;
			}
		}

		/// <summary>
		/// PenWidthPx property.
		/// </summary>
		///
		/// <value>
		/// Int32.  Width of the pen used to draw the node's rectangle, in pixels.
		/// </value>
		///
		/// <remarks>
		/// Do not get this property without setting it first.
		/// </remarks>
		protected internal int PenWidthPx
		{
			get
			{
				AssertValid();
				Debug.Assert(m_iPenWidthPx != -1);
				return m_iPenWidthPx;
			}
			set
			{
				m_iPenWidthPx = value;
				AssertValid();
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
		/// This method must be called after this node object is added to the
		/// TreemapGenerator.
		/// </remarks>
		protected internal TreemapGenerator TreemapGenerator
		{
			set
			{
				m_oTreemapGenerator = value;
				m_oNodes.TreemapGenerator = value;
				AssertValid();
			}
		}

		/// <overloads>
		/// Initializes a new instance of the Node class.
		/// </overloads>
		///
		/// <summary>
		/// Initializes a new instance of the Node class with the specified text,
		/// size metric, and color metric.
		/// </summary>
		///
		/// <param name="text">
		/// Text to display within the node's rectangle.
		/// </param>
		///
		/// <param name="sizeMetric">
		/// Determines the size of the node's rectangle.  See
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" />.
		/// </param>
		///
		/// <param name="colorMetric">
		/// Determines the fill color of the node's rectangle.  See
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />.
		/// </param>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		public Node(string text, float sizeMetric, float colorMetric)
		{
			InitializeWithValidation(text, sizeMetric, colorMetric);
			AssertValid();
		}

		public Node(string text, float sizeMetric, Color absoluteColor)
		{
			InitializeWithValidation(text, sizeMetric, 0f);
			m_oNodeColor.AbsoluteColor = absoluteColor;
			AssertValid();
		}

		/// <summary>
		/// Initializes a new instance of the Node class with the specified text,
		/// size metric, color metric, and tag.
		/// </summary>
		///
		/// <param name="text">
		/// Text to display within the node's rectangle.
		/// </param>
		///
		/// <param name="sizeMetric">
		/// Determines the size of the node's rectangle.  See
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" />.
		/// </param>
		///
		/// <param name="colorMetric">
		/// Determines the fill color of the node's rectangle.  See
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />.
		/// </param>
		///
		/// <param name="tag">
		/// Arbitrary object to associate with the node.
		/// </param>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		public Node(string text, float sizeMetric, float colorMetric, object tag)
		{
			InitializeWithValidation(text, sizeMetric, colorMetric);
			m_oTag = tag;
			AssertValid();
		}

		/// <summary>
		/// Initializes a new instance of the Node class with the specified text,
		/// size metric, color metric, tag, and tooltip.
		/// </summary>
		///
		/// <param name="text">
		/// Text to display within the node's rectangle.
		/// </param>
		///
		/// <param name="sizeMetric">
		/// Determines the size of the node's rectangle.  See
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" />.
		/// </param>
		///
		/// <param name="colorMetric">
		/// Determines the fill color of the node's rectangle.  See
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />.
		/// </param>
		///
		/// <param name="tag">
		/// Arbitrary object to associate with the node.
		/// </param>
		///
		/// <param name="toolTip">
		/// Tooltip to show for the node.  This is used by the TreemapControl.  It
		/// is not used by the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.TreemapGenerator" />.
		/// </param>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		public Node(string text, float sizeMetric, float colorMetric, object tag, string toolTip)
		{
			InitializeWithValidation(text, sizeMetric, colorMetric);
			m_oTag = tag;
			m_sToolTip = toolTip;
			AssertValid();
		}

		/// <summary>
		/// Compares this instance to a specified object and returns an indication
		/// of their relative values.
		/// </summary>
		///
		/// <param name="otherNode">
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> to compare this one to.
		/// </param>
		///
		/// <returns>
		/// See <see cref="M:System.Single.CompareTo(System.Object)" />.
		/// </returns>
		public int CompareTo(object otherNode)
		{
			AssertValid();
			return -m_fSizeMetric.CompareTo((object)((Node)otherNode).m_fSizeMetric);
		}

		/// <summary>
		/// This member overrides <see cref="M:System.Object.ToString" />.
		/// </summary>
		public override string ToString()
		{
			AssertValid();
			return string.Format("Node object: Text=\"{0}\",  SizeMetric={1}, Tag={2}, Rectangle={{L={3}, R={4}, T={5}, B={6},  W={7}, H={8}}}, Size={9}", m_sText, m_fSizeMetric, (m_oTag == null) ? "null" : m_oTag.ToString(), m_oRectangle.Left, m_oRectangle.Right, m_oRectangle.Top, m_oRectangle.Bottom, m_oRectangle.Width, m_oRectangle.Height, m_oRectangle.Width * m_oRectangle.Height);
		}

		/// <summary>
		/// Do not call this method.
		/// </summary>
		///
		/// <param name="oParentNode">
		/// Do not call this method.  It is for use only by other treemap
		/// components.
		/// </param>
		public void PrivateSetParent(Node oParentNode)
		{
			SetParent(oParentNode);
			AssertValid();
		}

		/// <summary>
		/// InitializeWithValidation() method.
		/// </summary>
		///
		/// <param name="sText">
		/// String.  Text to display within the node's rectangle.
		/// </param>
		///
		/// <param name="fSizeMetric">
		/// Single.  Determines the size of the node's rectangle.
		/// </param>
		///
		/// <param name="fColorMetric">
		/// Single.  Determines the fill color of the node's rectangle.
		/// </param>
		///
		/// <remarks>
		/// Validates the arguments and initializes the object.  This is used by
		/// several constructors.
		/// </remarks>
		protected void InitializeWithValidation(string sText, float fSizeMetric, float fColorMetric)
		{
			ValidateSizeMetric(fSizeMetric, "Node");
			ValidateColorMetric(fColorMetric, "Node");
			m_oTreemapGenerator = null;
			m_oParentNode = null;
			m_sText = sText;
			m_fSizeMetric = fSizeMetric;
			m_oNodeColor = new NodeColor(fColorMetric);
			m_oTag = null;
			m_sToolTip = null;
			m_oNodes = new Nodes(this);
			m_iPenWidthPx = -1;
			m_bRectangleSet = false;
			m_bRectangleSaved = false;
		}

		/// <summary>
		/// Sets the parent node.
		/// </summary>
		///
		/// <param name="oParentNode">
		/// The parent <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object, or null if this node belongs to
		/// the top-level <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.Nodes" /> collection or the node hasn't been
		/// added to a collection.
		/// </param>
		protected internal void SetParent(Node oParentNode)
		{
			Debug.Assert(oParentNode != this);
			m_oParentNode = oParentNode;
			AssertValid();
		}

		/// <summary>
		/// GetNodeFromPoint method.
		/// </summary>
		///
		/// <param name="oPointF">
		/// PointF.  Point to get a Node object for.
		/// </param>
		///
		/// <param name="oNode">
		/// Node.  Where the Node object gets stored.
		/// </param>
		///
		/// <returns>
		/// Boolean.  true if a Node object was found, false if not.
		/// </returns>
		///
		/// <remarks>
		/// Looks for the innermost node whose rectangle contains the specified
		/// point.  If found, the Node object is stored in oNode and true is
		/// returned.  false is returned otherwise.
		/// </remarks>
		protected internal bool GetNodeFromPoint(PointF oPointF, out Node oNode)
		{
			AssertValid();
			if (m_oRectangle.Contains(oPointF))
			{
				if (!m_oNodes.GetNodeFromPoint(oPointF, out oNode))
				{
					oNode = this;
				}
				return true;
			}
			oNode = null;
			return false;
		}

		/// <summary>
		/// ValidateSizeMetric method.
		/// </summary>
		///
		/// <param name="fValue">
		/// Single.  Value to validate.
		/// </param>
		///
		/// <param name="sCaller">
		/// String.  Name of the caller.  Used in exception messages.  Sample:
		/// "Nodes.Add()".
		/// </param>
		///
		/// <remarks>
		/// Throws an exception if iValue is not a valid SizeMetric value.
		/// </remarks>
		protected internal static void ValidateSizeMetric(float fValue, string sCaller)
		{
			if (fValue < 0f)
			{
				throw new ArgumentOutOfRangeException(sCaller, fValue, sCaller + ": SizeMetric must be >= 0.");
			}
		}

		/// <summary>
		/// ValidateColorMetric method.
		/// </summary>
		///
		/// <param name="fValue">
		/// Single.  Value to validate.
		/// </param>
		///
		/// <param name="sCaller">
		/// String.  Name of the caller.  Used in exception messages.  Sample:
		/// "Nodes.Add()".
		/// </param>
		///
		/// <remarks>
		/// Throws an exception if iValue is not a valid ColorMetric value.
		/// </remarks>
		protected internal static void ValidateColorMetric(float fValue, string sCaller)
		{
			if (float.IsNaN(fValue))
			{
				throw new ArgumentOutOfRangeException(sCaller, fValue, sCaller + ": ColorMetric can't be NaN.");
			}
		}

		/// <summary>
		/// SaveRectangle method.
		/// </summary>
		///
		/// <remarks>
		/// Saves the Rectangle property in an internal variable.  The saved
		/// value can be restored by calling RestoreRectangle().  These methods
		/// are provided as helpers for the NodeRectangleCalculator object.
		/// </remarks>
		protected internal void SaveRectangle()
		{
			m_oSavedRectangle = m_oRectangle;
			m_bRectangleSaved = true;
			AssertValid();
		}

		/// <summary>
		/// RestoreRectangle method.
		/// </summary>
		///
		/// <remarks>
		/// Restores the Rectangle property saved by SaveRectangle().  There can
		/// be only one RestoreRectangle() call for each SaveRectangle() call.
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.Node.SaveRectangle" />
		/// </remarks>
		protected internal void RestoreRectangle()
		{
			Debug.Assert(m_bRectangleSaved);
			m_oRectangle = m_oSavedRectangle;
			m_bRectangleSaved = false;
			AssertValid();
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
			AssertValid();
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
			Debug.Assert(m_fSizeMetric >= 0f);
			m_oNodeColor.AssertValid();
			Debug.Assert(m_oNodes != null);
			m_oNodes.AssertValid();
			Debug.Assert(m_iPenWidthPx == -1 || m_iPenWidthPx >= 0);
		}
	}
}
