using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.ControlLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.Research.CommunityTechnologies.TreemapNoDoc;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	/// <summary>
	/// Treemap control.
	/// </summary>
	///
	/// <remarks>
	///
	/// TreemapControl is one of two components that render a hierarchical data
	/// set as a treemap, which is a set of nested rectangles.  The size of each
	/// rectangle is determined by a property of each item in the data set, and the
	/// rectangle's fill color is determined by another property.
	///
	/// <para>
	/// The following table summarizes the two treemap components.
	/// </para>
	///
	/// <list type="table">
	///
	/// <listheader>
	/// <term>Component</term>
	/// <term>For Use In</term>
	/// <term>Required Assemblies</term>
	/// </listheader>
	///
	/// <item>
	/// <term><see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /></term>
	/// <term>
	/// Any application that wants to draw a treemap onto a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Bitmap" />
	/// or <see cref="T:System.Drawing.Graphics" /> object, or do custom drawing
	/// </term>
	/// <term>
	/// TreemapGenerator.dll
	/// </term>
	/// </item>
	///
	/// <item>
	/// <term>TreemapControl</term>
	/// <term>
	/// Windows Forms applications
	/// </term>
	/// <term>
	/// TreemapControl.dll, TreemapGenerator.dll
	/// </term>
	/// </item>
	///
	/// </list>
	///
	/// <para>
	/// TreemapControl inherits from the standard <see cref="T:System.Windows.Forms.Panel" /> control,
	/// which provides support for scrolling.  Internally, TreemapControl uses a
	/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> object to draw a treemap onto a
	/// <see cref="T:System.Windows.Forms.PictureBox" /> control placed on the <see cref="T:System.Windows.Forms.Panel" />.
	/// </para>
	///
	/// <para>
	/// Note that an application using TreemapControl requires both the
	/// TreemapControl.dll and TreemapGenerator.dll assemblies.
	/// </para>
	///
	/// <para>
	/// Using the TreemapControl in an application involves two steps:
	/// <list type="bullet">
	///
	/// <item>
	/// <description>Populate the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Nodes" /> collection
	/// </description>
	/// </item>
	///
	/// <item>
	/// <description>
	/// Set properties that determine how the treemap is drawn
	/// </description>
	/// </item>
	///
	/// </list>
	/// </para>
	///
	/// <para>
	/// The <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Nodes" /> property on TreemapControl
	/// returns a collection of top-level <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.  Each
	/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object in turn has a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.Nodes" /> property
	/// that returns a collection of child <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.  These
	/// nested collections are directly analogous to the Nodes collections in the
	/// standard .NET <see cref="T:System.Windows.Forms.TreeView" /> control.
	/// </para>
	///
	/// <para>
	/// Each <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object has a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" />
	/// property that determines the size of the node's rectangle relative to other
	/// nodes in the collection, a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> property that
	/// determines the rectangle's fill color, and a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.Text" />
	/// property that determines the text that is drawn in the center of the
	/// rectangle.
	/// </para>
	///
	/// <para>
	/// To improve performance, call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.BeginUpdate" /> before populating
	/// the treemap with nodes.  This prevents the control from being immediately
	/// updated.  Call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.EndUpdate" /> when you are done.
	/// </para>
	///
	/// </remarks>
	///
	/// <example>
	/// Here is sample C# code that populates a TreemapControl
	/// with two top-level nodes, each of which has two child nodes.  A few
	/// properties that determine how the treemap is drawn are set.
	///
	/// <code>
	///
	/// // InitializeTreemap() should be called from the form's constructor.
	///
	/// protected void
	/// InitializeTreemap()
	/// {
	///     // A TreemapControl has been placed on this form using the Visual
	///     // Studio designer.
	///
	///     // Improve performance by turning off updating while the control is
	///     // being populated.
	///
	///     oTreemapControl.BeginUpdate();
	///     PopulateTreemap(oTreemapControl);
	///     oTreemapControl.EndUpdate();
	///
	///     SetTreemapProperties(oTreemapControl);
	/// }
	///
	/// protected void
	/// PopulateTreemap(TreemapControl oTreemapControl)
	/// {
	///     Nodes oNodes;
	///     Node oNode;
	///     Nodes oChildNodes;
	///     Node oChildNode;
	///
	///     // Get the collection of top-level nodes.
	///
	///     oNodes = oTreemapControl.Nodes;
	///
	///     // Add a top-level node to the collection.
	///
	///     oNode = oNodes.Add("Top Level 1", 25F, 100F);
	///
	///     // Add child nodes to the top-level node.
	///
	///     oChildNodes = oNode.Nodes;
	///     oChildNode = oChildNodes.Add("Child 1-1", 90F, 2.5F);
	///     oChildNode = oChildNodes.Add("Child 1-2", 10F, -34.5F);
	///
	///     // Add another top-level node.
	///
	///     oNode = oNodes.Add("Top Level 2", 50F, -40.1F);
	///
	///     // Add child nodes to the second top-level node.
	///
	///     oChildNodes = oNode.Nodes;
	///     oChildNode = oChildNodes.Add("Child 2-1", 61F, 0F);
	///     oChildNode = oChildNodes.Add("Child 2-2", 100F, 200F);
	///
	///     // (As an alternative to making multiple calls to the Nodes.Add
	///     // method, the control can be populated via an XML string
	///     // passed to the TreemapControl.NodesXml property.)
	/// }
	///
	/// protected void
	/// SetTreemapProperties(TreemapControl oTreemapControl)
	/// {
	///     // All TreemapControl properties have default values that yield
	///     // reasonable results in many cases.  We want to change the
	///     // range of colors for this example.
	///
	///     // Make Node.ColorMetric values of -200 to 200 map to a color
	///     // range between blue and yellow.
	///
	///     oTreemapControl.MinColorMetric = -200F;
	///     oTreemapControl.MaxColorMetric = 200F;
	///
	///     oTreemapControl.MinColor = Color.Blue;
	///     oTreemapControl.MaxColor = Color.Yellow;
	///
	///     // (Set other properties that determine border width, spacing
	///     // between boxes, fonts, etc., if desired.)
	/// }
	///
	/// </code>
	///
	/// </example>
	///
	/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" />
	public class TreemapControl : Panel, ITreemapComponent
	{
		/// <summary>
		/// Represents the method that will handle a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> event that involves a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object.
		/// </summary>
		///
		/// <param name="sender">
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> that fired the event.
		/// </param>
		///
		/// <param name="nodeEventArgs">
		/// Provides information about the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object involved in
		/// the event.
		/// </param>
		public delegate void NodeEventHandler(object sender, NodeEventArgs nodeEventArgs);

		/// <summary>
		/// Represents the method that will handle a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> event that involves the mouse and a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object.
		/// </summary>
		///
		/// <param name="sender">
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> that fired the event.
		/// </param>
		///
		/// <param name="nodeMouseEventArgs">
		/// Provides information about the mouse and the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		/// object involved in the event.
		/// </param>
		public delegate void NodeMouseEventHandler(object sender, NodeMouseEventArgs nodeMouseEventArgs);

		/// Treemap drawing engine.
		protected TreemapGenerator m_oTreemapGenerator;

		/// The control's internal bitmap, or null if the treemap hasn't been drawn
		/// yet.
		protected Bitmap m_oBitmap;

		/// true if tooltips should be shown.
		protected bool m_bShowToolTips;

		protected bool m_bAllowDrag;

		/// true if the treemap is zoomable.  See the Zoomable property for details
		/// on the zooming scheme.
		protected bool m_bIsZoomable;

		/// If m_bIsZoomable is true, this is an object that maintains a history of
		/// the treemap's zoom actions.  A ZoomAction object is inserted into the
		/// list each time the treemap is zoomed in or out.   The ZoomAction object
		/// stores enough information to restore the treemap to the state it was in
		/// before it was zoomed.
		///
		/// If m_bIsZoomable is false, this is null.
		protected ZoomActionHistoryList m_oZoomActionHistoryList;

		/// Helper object for figuring out when to show tooltips.
		private ToolTipTracker m_oToolTipTracker;

		/// Most recent point passed into picPictureBox_MouseMove().
		protected Point m_oLastMouseMovePoint;

		protected Point m_oLastDraggableMouseDownPoint;

#pragma warning disable IDE0044 // Add readonly modifier
		private Container components = null;
#pragma warning restore IDE0044 // Add readonly modifier

		private PictureBox picPictureBox;

		private ToolTipPanel pnlToolTip;

		/// <summary>
		/// Gets the collection of top-level <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.
		/// </summary>
		///
		/// <value>
		/// A <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
		/// collection of top-level <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.
		/// </value>
		///
		/// <remarks>
		/// Items in the collection can be accessed by a zero-based index.  They
		/// can also be enumerated.
		/// </remarks>
		///
		/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
		/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		[ReadOnly(true)]
		[Browsable(false)]
		public Nodes Nodes
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.Nodes;
			}
		}

		/// <summary>
		/// Gets or sets the entire node hierarchy as an XML string.
		/// </summary>
		///
		/// <value>
		/// XML string containing the entire node hierarchy.
		/// </value>
		///
		/// <remarks>
		/// Setting this property is an alternative to repeatedly calling the
		/// Nodes.Add method.  This property also serves as a convenient
		/// persistence mechanism.  Once you populate the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Nodes" />
		/// collection, you can save the contents as an XML string.  Later, you can
		/// reload the collection by passing that string to this property.
		///
		/// <para>
		/// Setting this property causes any existing <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects
		/// to be deleted.
		/// </para>
		///
		/// <para>
		/// This is what the XML returned by this property looks like.  (The values
		/// of the SizeMetric, ColorMetric, and ToolTip attributes are omitted here
		/// but are present in real XML.)
		/// </para>
		///
		/// <code>
		/// &lt;?xml version="1.0" encoding="utf-16"?&gt;
		/// &lt;Nodes EmptySizeMetric="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"&gt;
		///   &lt;Node Text="TopLevel1" SizeMetric="" ColorMetric="" ToolTip=""&gt;
		///     &lt;Nodes EmptySizeMetric=""&gt;
		///       &lt;Node Text="Child1" SizeMetric="" ColorMetric="" ToolTip=""&gt;
		///         &lt;Nodes EmptySizeMetric=""&gt;
		///           &lt;Node Text="Grandchild1" SizeMetric="" ColorMetric="" ToolTip=""&gt;
		///             &lt;Nodes EmptySizeMetric=""/&gt;
		///           &lt;/Node&gt;
		///           &lt;Node Text="Grandchild2" SizeMetric="" ColorMetric="" ToolTip=""&gt;
		///             &lt;Nodes EmptySizeMetric=""/&gt;
		///           &lt;/Node&gt;
		///       ...
		///         &lt;/Nodes&gt;
		///       &lt;/Node&gt;
		///       &lt;Node Text="Child2" SizeMetric="" ColorMetric="" ToolTip=""&gt;
		///         &lt;Nodes EmptySizeMetric=""/&gt;
		///       &lt;/Node&gt;
		///     ...
		///     &lt;/Nodes&gt;
		///   &lt;/Node&gt;
		///   &lt;Node Text="TopLevel2" SizeMetric="" ColorMetric="" ToolTip=""&gt;
		///     &lt;Nodes EmptySizeMetric=""/&gt;
		///   &lt;/Node&gt;
		///   ...
		/// &lt;/Nodes&gt;
		/// </code>
		///
		/// <para>
		/// By default, the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.Tag" /> property is not serialized.
		/// </para>
		///
		/// <para>
		/// You can customize the way the nodes are serialized to XML by using the
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.NodesXmlAttributeOverrides" /> property.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Nodes" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.NodesXmlAttributeOverrides" />
		[ReadOnly(true)]
		[Browsable(false)]
		public string NodesXml
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.NodesXml;
			}
			set
			{
				m_oTreemapGenerator.NodesXml = value;
				AssertValid();
			}
		}

		[Description("The algorithm used to lay out the treemap's rectangles.")]
		[Category("Layout")]
		public LayoutAlgorithm LayoutAlgorithm
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.LayoutAlgorithm;
			}
			set
			{
				m_oTreemapGenerator.LayoutAlgorithm = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the padding that is added to the rectangles for top-level
		/// nodes.
		/// </summary>
		///
		/// <value>
		/// The padding that is added to the rectangles for top-level nodes, in
		/// pixels.  The value must be between
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPaddingPx" /> and
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPaddingPx" />.  The default value is 5.
		/// </value>
		///
		/// <remarks>
		/// If the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PaddingDecrementPerLevelPx" /> property is 0,
		/// PaddingPx is the padding that is added to all node rectangles.
		/// Otherwise, <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PaddingDecrementPerLevelPx" /> is subtracted from
		/// the padding at each node level.  Decreasing the padding at lower levels
		/// can improve the general appearance of the treemap.
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PaddingDecrementPerLevelPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPaddingPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPaddingPx" />
		[Category("Node Borders")]
		[Description("The padding that is added to the rectangles for top-level nodes.")]
		public int PaddingPx
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.PaddingPx;
			}
			set
			{
				m_oTreemapGenerator.PaddingPx = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the number of pixels that is subtracted from the padding
		/// at each node level.
		/// </summary>
		///
		/// <value>
		/// The number of pixels that is subtracted from the rectangle padding
		/// at each node level.  The value must be between
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPaddingDecrementPerLevelPx" /> and
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPaddingDecrementPerLevelPx" />.  The
		/// default value is 1.
		/// </value>
		///
		/// <remarks>
		/// The rectangles for top-level nodes are drawn with a padding of
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PaddingPx" />.  Set PaddingDecrementPerLevelPx to a positive
		/// value to force the padding to decrease at each level.  This can improve
		/// the general appearance of the treemap.  A value of 0 causes all
		/// nodes to use <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PaddingPx" />.
		///
		/// <para>
		/// If <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PaddingPx" /> is 5 and PaddingDecrementPerLevelPx is 1,
		/// for example, the padding for the top-level, child, and grandchild nodes
		/// will be 5, 4, and 3 pixels, respectively.  Padding is never less than
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPaddingPx" /> pixels.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PaddingPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPaddingDecrementPerLevelPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPaddingDecrementPerLevelPx" />
		[Description("The number of pixels that is subtracted from the padding at each node level.")]
		[Category("Node Borders")]
		public int PaddingDecrementPerLevelPx
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.PaddingDecrementPerLevelPx;
			}
			set
			{
				m_oTreemapGenerator.PaddingDecrementPerLevelPx = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the pen width that is used to draw the rectangles for the
		/// top-level nodes.
		/// </summary>
		///
		/// <value>
		/// The pen width that is used to draw the rectangles for the top-level
		/// nodes, in pixels.  The value must be between
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPenWidthPx" /> and
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPenWidthPx" />.  The default value is 3.
		/// </value>
		///
		/// <remarks>
		/// If the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PenWidthDecrementPerLevelPx" /> property is 0, all
		/// rectangles are drawn with a pen width of PenWidthPx pixels.  Otherwise,
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PenWidthDecrementPerLevelPx" /> is subtracted from
		/// the pen width at each node level.  Decreasing the pen width at lower
		/// levels can improve the general appearance of the treemap.
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PenWidthDecrementPerLevelPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPenWidthPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPenWidthPx" />
		[Category("Node Borders")]
		[Description("The pen width that is used to draw the rectangles for the top-level nodes.")]
		public int PenWidthPx
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.PenWidthPx;
			}
			set
			{
				m_oTreemapGenerator.PenWidthPx = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the number of pixels that is subtracted from the pen
		/// width at each node level.
		/// </summary>
		///
		/// <value>
		/// The number of pixels that is subtracted from the pen width at each
		/// node level.  The value must be between
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPenWidthDecrementPerLevelPx" />
		/// and <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPenWidthDecrementPerLevelPx" />.
		/// The default value is 1.
		///
		/// </value>
		///
		/// <remarks>
		///
		/// The rectangles for top-level nodes are drawn with a pen width of
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PenWidthPx" /> pixels.  Set PenWidthDecrementPerLevelPx to
		/// a positive value to force the pen width to decrease at each level.
		/// This can improve the general appearance of the treemap.  A value of 0
		/// causes all nodes to use <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PenWidthPx" />.
		///
		/// <para>
		/// If <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PenWidthPx" /> is 4 and PenWidthDecrementPerLevelPx is
		/// 1, for example, the pen width for the top-level nodes will be 4, the
		/// width for the child nodes will be 3, the width for the grandchildren
		/// will be 2, and so on.  The pen width is never less than 1 pixel.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.PenWidthPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPenWidthDecrementPerLevelPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPenWidthDecrementPerLevelPx" />
		[Description("The number of pixels that is subtracted from the pen width at each node level.")]
		[Category("Node Borders")]
		public int PenWidthDecrementPerLevelPx
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.PenWidthDecrementPerLevelPx;
			}
			set
			{
				m_oTreemapGenerator.PenWidthDecrementPerLevelPx = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the treemap's background color.
		/// </summary>
		///
		/// <value>
		/// The treemap's background color, as a <see cref="T:System.Drawing.Color" />.
		/// </value>
		///
		/// <remarks>
		/// The background color is used to paint areas of the treemap that aren't
		/// covered by rectangles.  The default value is SystemColors.Window.
		/// </remarks>
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				Color backColor = m_oTreemapGenerator.BackColor = value;
				base.BackColor = backColor;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the color of rectangle borders.
		/// </summary>
		///
		/// <value>
		/// The color of the rectangle borders, as a <see cref="T:System.Drawing.Color" />.
		/// </value>
		///
		/// <remarks>
		/// The default border color is SystemColors.WindowFrame.
		/// </remarks>
		[Description("The color of rectangle borders.")]
		[Category("Node Borders")]
		public Color BorderColor
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.BorderColor;
			}
			set
			{
				m_oTreemapGenerator.BorderColor = value;
				AssertValid();
			}
		}

		[Category("Node Fill Colors")]
		[Description("The algorithm used to color the treemap's rectangles.")]
		public NodeColorAlgorithm NodeColorAlgorithm
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.NodeColorAlgorithm;
			}
			set
			{
				m_oTreemapGenerator.NodeColorAlgorithm = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the maximum negative fill color.
		/// </summary>
		///
		/// <value>
		/// The fill color for nodes with a
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColorMetric" />
		/// or less, as a <see cref="T:System.Drawing.Color" />.
		/// </value>
		///
		/// <remarks>
		/// See <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> for details on how fill colors for
		/// node rectangles are determined.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.DiscreteNegativeColors" />
		[Category("Node Fill Colors")]
		[Description("The maximum negative fill color.")]
		public Color MinColor
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.MinColor;
			}
			set
			{
				m_oTreemapGenerator.MinColor = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the maximum positive fill color.
		/// </summary>
		///
		/// <value>
		/// The fill color for nodes with a
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColorMetric" />
		/// or greater, as a <see cref="T:System.Drawing.Color" />.
		/// </value>
		///
		/// <remarks>
		/// See <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> for details on how fill colors for
		/// node rectangles are determined.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.DiscretePositiveColors" />
		[Description("The maximum positive fill color.")]
		[Category("Node Fill Colors")]
		public Color MaxColor
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.MaxColor;
			}
			set
			{
				m_oTreemapGenerator.MaxColor = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value to map to
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColor" />.
		/// </summary>
		///
		/// <value>
		/// The <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value to map to
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColor" />.
		/// </value>
		///
		/// <remarks>
		/// See <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> for details on how fill colors for
		/// node rectangles are determined.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColor" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.DiscreteNegativeColors" />
		[Category("Node Fill Colors")]
		[Description("The Node.ColorMetric value to map to MinColor.")]
		public float MinColorMetric
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.MinColorMetric;
			}
			set
			{
				m_oTreemapGenerator.MinColorMetric = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value to map to
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColor" />.
		/// </summary>
		///
		/// <value>
		/// The <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value to map to
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColor" />.
		/// </value>
		///
		/// <remarks>
		/// See <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> for details on how fill colors for
		/// node rectangles are determined.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColor" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.DiscretePositiveColors" />
		[Category("Node Fill Colors")]
		[Description("The Node.ColorMetric value to map to MaxColor.")]
		public float MaxColorMetric
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.MaxColorMetric;
			}
			set
			{
				m_oTreemapGenerator.MaxColorMetric = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the number of discrete fill colors to use in the negative
		/// color range.
		/// </summary>
		///
		/// <value>
		/// The number of discrete fill colors to use in the range between white
		/// and <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColor" />.  Must be between
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinDiscreteColors" /> and
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxDiscreteColors" />.  The default value
		/// is 20.
		/// </value>
		///
		/// <remarks>
		///
		/// When filling rectangles for nodes that have a negative
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value, the
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> splits the negative color range, which
		/// is white to <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColor" />, into a set of discrete colors.
		/// Nodes with a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of 0 are filled with
		/// white, nodes with a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColorMetric" /> or less are filled with
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColor" />, and nodes with a
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> somewhere between are mapped to the
		/// nearest discrete color within the negative color range.
		///
		/// <para>
		/// The default value provides sufficient color granularity for most
		/// applications.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColor" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MinColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		[Category("Node Fill Colors")]
		[Description("The number of discrete fill colors to use in the negative color range.")]
		public int DiscreteNegativeColors
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.DiscreteNegativeColors;
			}
			set
			{
				m_oTreemapGenerator.DiscreteNegativeColors = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the number of discrete fill colors to use in the positive
		/// color range.
		/// </summary>
		///
		/// <value>
		/// The number of discrete fill colors to use in the range between white
		/// and <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColor" />.  Must be between
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinDiscreteColors" /> and
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxDiscreteColors" />.  The default value
		/// is 20.
		/// </value>
		///
		/// <remarks>
		///
		/// When filling rectangles for nodes that have a positive
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value, the
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> splits the positive color range, which
		/// is white to <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColor" />, into a set of discrete colors.
		/// Nodes with a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of 0 are filled with
		/// white, nodes with a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColorMetric" /> or greater are filled with
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColor" />, and nodes with a
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> somewhere between are mapped to the
		/// nearest discrete color within the positive color range.
		///
		/// <para>
		/// The default value provides sufficient color granularity for most
		/// applications.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColor" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MaxColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		[Description("The number of discrete fill colors to use in the positive color range.")]
		[Category("Node Fill Colors")]
		public int DiscretePositiveColors
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.DiscretePositiveColors;
			}
			set
			{
				m_oTreemapGenerator.DiscretePositiveColors = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the font family to use for drawing node text.
		/// </summary>
		///
		/// <value>
		/// The name of the font family to use for drawing node text.  The default
		/// value is Arial.
		/// </value>
		[Description("The font family to use for drawing node text.")]
		[Category("Node Text")]
		public string FontFamily
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.FontFamily;
			}
			set
			{
				m_oTreemapGenerator.FontFamily = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the color to use for node text.
		/// </summary>
		///
		/// <value>
		/// The color to use for node text, as a <see cref="T:System.Drawing.Color" />.
		/// </value>
		///
		/// <remarks>
		/// This must be a solid color, which means its alpha component must be
		/// 255.  Text can be drawn using transparent colors; see
		/// <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SetFontAlphaRange(System.Int32,System.Int32,System.Int32)" /> for details.
		/// </remarks>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SetFontAlphaRange(System.Int32,System.Int32,System.Int32)" />
		[Description("The solid color to use for unselected node text.")]
		[Category("Node Text")]
		public Color FontSolidColor
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.FontSolidColor;
			}
			set
			{
				m_oTreemapGenerator.FontSolidColor = value;
				AssertValid();
			}
		}

		[Category("Node Text")]
		[Description("The color to use for selected node text.")]
		public Color SelectedFontColor
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.SelectedFontColor;
			}
			set
			{
				m_oTreemapGenerator.SelectedFontColor = value;
				AssertValid();
			}
		}

		[Description("The color to use to highlight the selected node.")]
		[Category("Node Text")]
		public Color SelectedBackColor
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.SelectedBackColor;
			}
			set
			{
				m_oTreemapGenerator.SelectedBackColor = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets the node levels to show text for.
		/// </summary>
		///
		/// <value>
		/// A <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.NodeLevelsWithText" /> enumeration that indicates which node levels
		/// should include text.  The default value is All.
		/// </value>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SetNodeLevelsWithTextRange(System.Int32,System.Int32)" />
		[Description("The node levels to show text for.")]
		[Category("Node Text")]
		public NodeLevelsWithText NodeLevelsWithText
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.NodeLevelsWithText;
			}
			set
			{
				m_oTreemapGenerator.NodeLevelsWithText = value;
				AssertValid();
			}
		}

		[Category("Node Text")]
		[Description("The location within a node's rectangle where the node's text is shown.")]
		public TextLocation TextLocation
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.TextLocation;
			}
			set
			{
				m_oTreemapGenerator.TextLocation = value;
				AssertValid();
			}
		}

		[Description("The location within a node's rectangle where the node's empty space is shown.")]
		[Category("Layout")]
		public EmptySpaceLocation EmptySpaceLocation
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.EmptySpaceLocation;
			}
			set
			{
				m_oTreemapGenerator.EmptySpaceLocation = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets the selected <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />, if there is one.
		/// </summary>
		///
		/// <value>
		/// The selected <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />, or null if there is none.
		/// </value>
		[Browsable(false)]
		[ReadOnly(true)]
		public Node SelectedNode
		{
			get
			{
				AssertValid();
				return m_oTreemapGenerator.SelectedNode;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether tooltips should be shown.
		/// </summary>
		///
		/// <value>
		/// true to show tooltips.  The default value is true.
		/// </value>
		///
		/// <remarks>
		/// If ShowToolTips is true and a node has a non-empty
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ToolTip" /> property, a tooltip is shown when the user
		/// hovers the mouse over the node's rectangle .
		/// </remarks>
		[Category("Node Text")]
		[Description("Indicates whether tooltips should be shown.")]
		public bool ShowToolTips
		{
			get
			{
				AssertValid();
				return m_bShowToolTips;
			}
			set
			{
				if (!value)
				{
					m_oToolTipTracker.Reset();
				}
				m_bShowToolTips = value;
				AssertValid();
			}
		}

		[Description("Indicates whether a node can be dragged out of the control.")]
		[Category("Behavior")]
		public bool AllowDrag
		{
			get
			{
				AssertValid();
				return m_bAllowDrag;
			}
			set
			{
				m_bAllowDrag = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the treemap can be zoomed.
		/// </summary>
		///
		/// <value>
		/// true if the treemap can be zoomed.  The default value is false.
		/// </value>
		///
		/// <remarks>
		/// If your application is interactive and you want the user to be able to
		/// zoom in to a specified node or zoom out from the current top-level
		/// node, follow these steps.
		///
		/// <list type="bullet">
		///
		/// <item><description>
		/// Set the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> property to true.
		/// </description></item>
		///
		/// <item><description>
		/// Populate the treemap.
		/// </description></item>
		///
		/// <item><description>
		/// To determine whether a node can be zoomed to, call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanZoomIn(Microsoft.Research.CommunityTechnologies.Treemap.Node)" />.
		/// </description></item>
		///
		/// <item><description>
		/// If <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanZoomIn(Microsoft.Research.CommunityTechnologies.Treemap.Node)" /> returns true, zoom to the node using the
		/// <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.ZoomIn(Microsoft.Research.CommunityTechnologies.Treemap.Node)" /> method.
		/// </description></item>
		///
		/// <item><description>
		/// To determine whether the treemap can be zoomed out one level, call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanZoomOut" />.
		/// </description></item>
		///
		/// <item><description>
		/// If <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanZoomOut" /> returns true, zoom out one level using the
		/// <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.ZoomOut" /> method.
		/// </description></item>
		///
		/// <item><description>
		/// The TreemapControl maintains a history list of zoom states.  A zoom
		/// state item is added to the history list each time <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.ZoomIn(Microsoft.Research.CommunityTechnologies.Treemap.Node)" />
		/// or <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.ZoomOut" /> is called.  To determine whether the treemap
		/// can be moved to the previous or next zoom state, call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanMoveBack" /> and <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanMoveForward" />.
		/// </description></item>
		///
		/// <item><description>
		/// If <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanMoveBack" /> or <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanMoveForward" /> returns
		/// true, move to the previous or next zoom state using the <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MoveBack" /> or <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MoveForward" /> method.
		/// </description></item>
		///
		/// <item><description>
		/// The control fires a <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.ZoomStateChanged" /> event each time
		/// <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.ZoomIn(Microsoft.Research.CommunityTechnologies.Treemap.Node)" />, <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.ZoomOut" />, <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MoveBack" />,
		/// or <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MoveForward" /> is called.  You should handle this event
		/// if there are navigation buttons that need to be selectively enabled
		/// depending on the treemap's zoom state.  The event handler should use
		/// <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanZoomOut" />, <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanMoveBack" />, and <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanMoveForward" /> to determine whether the buttons should be
		/// enabled.
		/// </description></item>
		///
		/// <item><description>
		/// If there is a navigation button for zooming in to the selected node,
		/// you should handle the <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SelectedNodeChanged" /> event, call
		/// <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanZoomIn(Microsoft.Research.CommunityTechnologies.Treemap.Node)" /> from the event handler, and enable the button
		/// if true is returned.
		/// </description></item>
		///
		/// <item><description>
		/// IMPORTANT: Do not add nodes to the treemap after you have zoomed it
		/// without first calling <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Clear" />.  Doing so will lead to
		/// unpredictable results.
		/// </description></item>
		///
		/// <item><description>
		/// Call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Clear" /> to remove all nodes from the treemap and
		/// clear the zoom state history list.  You can then repopulate the treemap
		/// and zoom it again.
		/// </description></item>
		///
		/// </list>
		///
		/// </remarks>
		[Category("Zooming")]
		[Description("Indicates whether the treemap can be zoomed.")]
		public bool IsZoomable
		{
			get
			{
				AssertValid();
				return m_bIsZoomable;
			}
			set
			{
				if (value != m_bIsZoomable)
				{
					m_bIsZoomable = value;
					if (m_bIsZoomable)
					{
						Debug.Assert(m_oZoomActionHistoryList == null);
						m_oZoomActionHistoryList = new ZoomActionHistoryList();
						m_oZoomActionHistoryList.Change += ZoomActionHistoryList_Change;
					}
					else
					{
						Debug.Assert(m_oZoomActionHistoryList != null);
						m_oZoomActionHistoryList.Change -= ZoomActionHistoryList_Change;
						m_oZoomActionHistoryList = null;
					}
				}
				AssertValid();
			}
		}

		/// <summary>
		/// Gets the control's internal <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Bitmap" />.
		/// </summary>
		///
		/// <value>
		/// The control's internal <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Bitmap" /> object.  May be null.
		/// </value>
		///
		/// <remarks>
		/// The <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Draw(System.Boolean)" /> method draws a treemap onto an
		/// internal <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Bitmap" /> object, then transfers the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Bitmap" /> onto the control's surface.  This property allows the
		/// caller to save the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Bitmap" /> to a file or print it.  The
		/// returned object is strictly read-only; it should not be modified in
		/// any way.  If <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.Draw(System.Boolean)" /> hasn't been called yet,
		/// the returned value is null.
		/// </remarks>
		[ReadOnly(true)]
		[Browsable(false)]
		public Bitmap Bitmap
		{
			get
			{
				AssertValid();
				return m_oBitmap;
			}
		}

		/// <summary>
		/// Occurs when the mouse pointer is over the rectangle of a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object and a mouse button is pressed.
		/// </summary>
		///
		/// <remarks>
		/// See <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SelectNode(Microsoft.Research.CommunityTechnologies.Treemap.Node)" /> for details on what happens when a mouse
		/// button is pressed.
		/// </remarks>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SelectNode(Microsoft.Research.CommunityTechnologies.Treemap.Node)" />
		[Category("Mouse")]
		public event NodeMouseEventHandler NodeMouseDown;

		/// <summary>
		/// Occurs when the mouse pointer is over the rectangle of a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object and a mouse button is released.
		/// </summary>
		[Category("Mouse")]
		public event NodeMouseEventHandler NodeMouseUp;

		/// <summary>
		/// Occurs when the mouse pointer hovers over the rectangle of a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object.
		/// </summary>
		[Category("Mouse")]
		public event NodeEventHandler NodeMouseHover;

		/// <summary>
		/// Occurs when the rectangle of a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object is
		/// double-clicked.
		/// </summary>
		[Category("Action")]
		public event NodeEventHandler NodeDoubleClick;

		/// <summary>
		/// Occurs when the treemap's zoom state changes.
		/// </summary>
		///
		/// <remarks>
		/// See the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> property for details on the treemap's
		/// zooming scheme.
		/// </remarks>
		[Category("Action")]
		public event EventHandler ZoomStateChanged;

		/// <summary>
		/// Occurs when the selected node changes.
		/// </summary>
		///
		/// <remarks>
		/// This event is fired when the selected node changes due to either a
		/// programmatic change or a mouse button press.
		///
		/// <para>
		/// See <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SelectNode(Microsoft.Research.CommunityTechnologies.Treemap.Node)" /> for details on what happens when a mouse
		/// button is pressed.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SelectNode(Microsoft.Research.CommunityTechnologies.Treemap.Node)" />
		[Category("Action")]
		public event EventHandler SelectedNodeChanged;

		/// <summary>
		/// Initializes a new instance of the TreemapControl class.
		/// </summary>
		public TreemapControl()
		{
			InitializeComponent();
			base.Controls.Add(picPictureBox);
			base.Controls.Add(pnlToolTip);
			pnlToolTip.BringToFront();
			m_oTreemapGenerator = new TreemapGenerator();
			m_oTreemapGenerator.RedrawRequired += TreemapGenerator_RedrawRequired;
			m_oBitmap = null;
			m_bShowToolTips = true;
			m_bAllowDrag = false;
			m_bIsZoomable = false;
			m_oZoomActionHistoryList = null;
			m_oToolTipTracker = new ToolTipTracker();
			m_oToolTipTracker.ShowToolTip += oToolTipTracker_ShowToolTip;
			m_oToolTipTracker.HideToolTip += oToolTipTracker_HideToolTip;
			m_oLastMouseMovePoint = new Point(-1, -1);
			m_oLastDraggableMouseDownPoint = new Point(-1, -1);
			base.ResizeRedraw = true;
		}

		/// <summary>
		/// Gets the range of node levels for which text is shown.
		/// </summary>
		///
		/// <param name="minLevel">
		/// Minimum node level.  Level 0 is the top level.
		/// </param>
		///
		/// <param name="maxLevel">
		/// Maximum node level.  Level 0 is the top level.
		/// </param>
		///
		/// <remarks>
		/// The range returned by this method is used only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.NodeLevelsWithText" /> is set to Range.
		/// </remarks>
		public void GetNodeLevelsWithTextRange(out int minLevel, out int maxLevel)
		{
			AssertValid();
			m_oTreemapGenerator.GetNodeLevelsWithTextRange(out minLevel, out maxLevel);
		}

		/// <summary>
		/// Sets the range of node levels for which text is displayed.
		/// </summary>
		///
		/// <param name="minLevel">
		/// Minimum node level.  Level 0 is the top level.  Must be greater than or
		/// equal to 0.
		/// </param>
		///
		/// <param name="maxLevel">
		/// Maximum node level.  Level 0 is the top level.  Must be greater than or
		/// equal to zero and greater than or equal to <paramref name="minLevel" />.
		/// </param>
		///
		/// <remarks>
		/// The range specified in this method is used only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.NodeLevelsWithText" /> is set to Range.  The default minimum and
		/// maximum values are 0 and 999.
		/// </remarks>
		public void SetNodeLevelsWithTextRange(int minLevel, int maxLevel)
		{
			m_oTreemapGenerator.SetNodeLevelsWithTextRange(minLevel, maxLevel);
			AssertValid();
		}

		/// <summary>
		/// Gets the range of font sizes used for drawing node text.
		/// </summary>
		///
		/// <param name="minSizePt">
		/// Minimum font size, in points.
		/// </param>
		///
		/// <param name="maxSizePt">
		/// Maximum font size, in points.
		/// </param>
		///
		/// <param name="incrementPt">
		/// Increment between font sizes, in points.
		/// </param>
		///
		/// <remarks>
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> draws node text using a range of
		/// font sizes.  For each node, the largest font in the range that won't
		/// exceed the bounds of the node's rectangle is used.
		///
		/// <para>
		/// By default, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> uses
		/// <paramref name="minSizePt" />=2, <paramref name="maxSizePt" />=100,
		/// and <paramref name="incrementPt" />=2.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// If <paramref name="minSizePt" /> is 2.0, <paramref name="maxSizePt" />
		/// is 10.0, and <paramref name="incrementPt" /> is 2.0, node text will be
		/// drawn with one of the following font sizes: 2.0, 4.0, 6.0, 8.0, 10.0.
		/// </example>
		public void GetFontSizeRange(out float minSizePt, out float maxSizePt, out float incrementPt)
		{
			AssertValid();
			m_oTreemapGenerator.GetFontSizeRange(out minSizePt, out maxSizePt, out incrementPt);
		}

		/// <summary>
		/// Sets the range of font sizes used for drawing node text.
		/// </summary>
		///
		/// <param name="minSizePt">
		/// Minimum font size, in points.
		/// </param>
		///
		/// <param name="maxSizePt">
		/// Maximum font size, in points.
		/// </param>
		///
		/// <param name="incrementPt">
		/// Increment between font sizes, in points.
		/// </param>
		///
		/// <remarks>
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> draws node text using a range of font
		/// sizes.  For each node, the largest font in the range that won't exceed
		/// the bounds of the node's rectangle is used.
		///
		/// <para>
		/// By default, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> uses
		/// <paramref name="minSizePt" />=2, <paramref name="maxSizePt" />=100,
		/// and <paramref name="incrementPt" />=2.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// If <paramref name="minSizePt" /> is 2.0, <paramref name="maxSizePt" />
		/// is 10.0, and <paramref name="incrementPt" /> is 2.0, node text will be
		/// drawn with one of the following font sizes: 2.0, 4.0, 6.0, 8.0, 10.0.
		/// </example>
		public void SetFontSizeRange(float minSizePt, float maxSizePt, float incrementPt)
		{
			m_oTreemapGenerator.SetFontSizeRange(minSizePt, maxSizePt, incrementPt);
		}

		/// <summary>
		/// Gets the range of transparency used for drawing node text.
		/// </summary>
		///
		/// <param name="minAlpha">
		/// Alpha value used for the level with maximum transparency.
		/// </param>
		///
		/// <param name="maxAlpha">
		/// Alpha value used for the level with minimum transparency.
		/// </param>
		///
		/// <param name="incrementPerLevel">
		/// Amount that alpha is incremented from level to level.
		/// </param>
		///
		/// <remarks>
		/// To improve text legibility, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> varies
		/// the transparency of node text.  The text for higher-level nodes is more
		/// transparent than the text for lower-level nodes, so the lower-level
		/// text shows through the higher-level text.
		///
		/// <para>
		/// Alpha values range from 0 (transparent) to 255 (opaque).
		/// </para>
		///
		/// <para>
		/// By default, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> uses
		/// <paramref name="minAlpha" />=105, <paramref name="maxAlpha" />=255,
		/// and <paramref name="incrementPerLevel" />=50.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// If <paramref name="minAlpha" /> is 55, <paramref name="maxAlpha" />
		/// is 255, and <paramref name="incrementPerLevel" /> is 100,
		/// then the text for level-0 (top-level) nodes will be drawn with
		/// alpha=55, the text for level-1 nodes will be drawn with alpha=155, and
		/// the text for level-2 nodes and below will be drawn with alpha=255.
		/// </example>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SetFontAlphaRange(System.Int32,System.Int32,System.Int32)" />
		public void GetFontAlphaRange(out int minAlpha, out int maxAlpha, out int incrementPerLevel)
		{
			AssertValid();
			m_oTreemapGenerator.GetFontAlphaRange(out minAlpha, out maxAlpha, out incrementPerLevel);
		}

		/// <summary>
		/// Sets the range of transparency used for drawing node text.
		/// </summary>
		///
		/// <param name="minAlpha">
		/// Alpha value to use for the level with maximum transparency.  Must be
		/// between 0 and 255.
		/// </param>
		///
		/// <param name="maxAlpha">
		/// Alpha value to use for the level with minimum transparency.  Must be
		/// between 0 and 255.  Must be &gt;= <paramref name="minAlpha" />.
		/// </param>
		///
		/// <param name="incrementPerLevel">
		/// Amount that alpha should be incremented from level to level.  Must
		/// be &gt; 0.
		/// </param>
		///
		/// <remarks>
		/// To improve text legibility, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> varies
		/// the transparency of node text.  The text for higher-level nodes is more
		/// transparent than the text for lower-level nodes, so the lower-level
		/// text shows through the higher-level text.
		///
		/// <para>
		/// Alpha values range from 0 (transparent) to 255 (opaque).
		/// </para>
		///
		/// <para>
		/// By default, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl" /> uses
		/// <paramref name="minAlpha" />=105, <paramref name="maxAlpha" />=255,
		/// and <paramref name="incrementPerLevel" />=50.
		/// </para>
		///
		/// </remarks>
		///
		/// <example>
		/// If <paramref name="minAlpha" /> is 55, <paramref name="maxAlpha" /> is
		/// 255, and <paramref name="incrementPerLevel" /> is 100,
		/// then the text for level-0 (top-level) nodes will be drawn with
		/// alpha=55, the text for level-1 nodes will be drawn with alpha=155, and
		/// the text for level-2 nodes and below will be drawn with alpha=255.
		/// </example>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.GetFontAlphaRange(System.Int32@,System.Int32@,System.Int32@)" />
		public void SetFontAlphaRange(int minAlpha, int maxAlpha, int incrementPerLevel)
		{
			m_oTreemapGenerator.SetFontAlphaRange(minAlpha, maxAlpha, incrementPerLevel);
			AssertValid();
		}

		/// <summary>
		/// Disables any redrawing of the treemap.
		/// </summary>
		///
		/// <remarks>
		/// To improve performance, call BeginUpdate before adding <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects to the treemap.  Call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.EndUpdate" />
		/// when you are done.
		/// </remarks>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.EndUpdate" />
		public void BeginUpdate()
		{
			AssertValid();
			m_oTreemapGenerator.BeginUpdate();
		}

		/// <summary>
		/// Enables the redrawing of the treemap.
		/// </summary>
		///
		/// <remarks>
		/// To improve performance, call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.BeginUpdate" /> before
		/// adding <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects to the treemap.  Call EndUpdate when
		/// you are done.
		/// </remarks>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.BeginUpdate" />
		public void EndUpdate()
		{
			AssertValid();
			m_oTreemapGenerator.EndUpdate();
		}

		/// <summary>
		/// Removes all nodes from the treemap.
		/// </summary>
		public void Clear()
		{
			AssertValid();
			Node selectedNode = SelectedNode;
			m_oTreemapGenerator.Clear();
			if (m_bIsZoomable)
			{
				Debug.Assert(m_oZoomActionHistoryList != null);
				m_oZoomActionHistoryList.Reset();
			}
			if (selectedNode != null)
			{
				FireSelectedNodeChanged();
			}
		}

		/// <summary>
		/// Determines whether a specified node can be zoomed in to.
		/// </summary>
		///
		/// <param name="node">
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> to test.  Must be contained in the treemap.
		/// </param>
		///
		/// <returns>
		/// true if <paramref name="node" /> can be zoomed in to using the <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.ZoomIn(Microsoft.Research.CommunityTechnologies.Treemap.Node)" /> method.
		/// </returns>
		///
		/// <remarks>
		/// This method can be called only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is true.
		/// It throws an exception if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is false.
		///
		/// <para>
		/// See the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> property for details on the treemap's
		/// zooming scheme.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" />
		public bool CanZoomIn(Node node)
		{
			AssertValid();
			VerifyIsZoomable("CanZoomIn");
			Nodes nodes = Nodes;
			switch (nodes.Count)
			{
			case 0:
				return false;
			case 1:
				if (node == nodes[0])
				{
					return false;
				}
				break;
			}
			return true;
		}

		/// <summary>
		/// Determines whether the treemap can be zoomed out one level.
		/// </summary>
		///
		/// <returns>
		/// true if the treemap can be zoomed out one level using the <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.ZoomOut" /> method.
		/// </returns>
		///
		/// <remarks>
		/// This method can be called only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is true.
		/// It throws an exception if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is false.
		///
		/// <para>
		/// See the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> property for details on the treemap's
		/// zooming scheme.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" />
		public bool CanZoomOut()
		{
			AssertValid();
			VerifyIsZoomable("CanZoomOut");
			if (!m_oZoomActionHistoryList.HasCurrentState)
			{
				return false;
			}
			ZoomAction peekCurrentState = m_oZoomActionHistoryList.PeekCurrentState;
			return peekCurrentState.CanZoomOutFromZoomedNode();
		}

		/// <summary>
		/// Determines whether the treemap can be moved back to the previous zoom
		/// state.
		/// </summary>
		///
		/// <returns>
		/// true if the treemap can be moved back to the previous zoom state using
		/// the <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MoveBack" /> method.
		/// </returns>
		///
		/// <remarks>
		/// This method can be called only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is true.
		/// It throws an exception if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is false.
		///
		/// <para>
		/// See the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> property for details on the treemap's
		/// zooming scheme.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" />
		public bool CanMoveBack()
		{
			AssertValid();
			VerifyIsZoomable("CanMoveBack");
			Debug.Assert(m_oZoomActionHistoryList != null);
			return m_oZoomActionHistoryList.HasCurrentState;
		}

		/// <summary>
		/// Determines whether the treemap can be moved forward to the next zoom
		/// state.
		/// </summary>
		///
		/// <returns>
		/// true if the treemap can be moved forward to the next zoom state using
		/// the <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.MoveForward" /> method.
		/// </returns>
		///
		/// <remarks>
		/// This method can be called only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is true.
		/// It throws an exception if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is false.
		///
		/// <para>
		/// See the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> property for details on the treemap's
		/// zooming scheme.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" />
		public bool CanMoveForward()
		{
			AssertValid();
			VerifyIsZoomable("CanMoveForward");
			Debug.Assert(m_oZoomActionHistoryList != null);
			return m_oZoomActionHistoryList.HasNextState;
		}

		/// <summary>
		/// Zooms in to a specified node.
		/// </summary>
		///
		/// <param name="node">
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> to zoom in to.  Must be contained in the treemap.
		/// </param>
		///
		/// <remarks>
		/// This method replaces the current top-level nodes with <paramref name="node" />.  It can be called only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is
		/// true and <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanZoomIn(Microsoft.Research.CommunityTechnologies.Treemap.Node)" /> has returned true for <paramref name="node" />.  It throws an exception otherwise.
		///
		/// <para>
		/// See the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> property for details on the treemap's
		/// zooming scheme.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" />
		public void ZoomIn(Node node)
		{
			Debug.Assert(node != null);
			AssertValid();
			VerifyIsZoomable("ZoomIn");
			if (!CanZoomIn(node))
			{
				throw new InvalidOperationException("TreemapControl.ZoomIn: Can't zoom in to node.  Check the CanZoomIn property first.");
			}
			Nodes nodes = Nodes;
			Debug.Assert(nodes.Count > 0);
			ZoomAction zoomAction;
			if (nodes.Count > 1 || !m_oZoomActionHistoryList.HasCurrentState)
			{
				zoomAction = new ZoomedFromTopLevelAction(m_oZoomActionHistoryList, node, nodes);
			}
			else
			{
				Debug.Assert(nodes.Count == 1);
				Debug.Assert(m_oZoomActionHistoryList.HasCurrentState);
				Node node2 = nodes[0];
				ZoomAction peekCurrentState = m_oZoomActionHistoryList.PeekCurrentState;
				if (peekCurrentState.ParentOfZoomedNode == null)
				{
					zoomAction = ((m_oZoomActionHistoryList.OriginalTopLevelNodes.Length <= 1) ? ((ZoomAction)new ZoomedFromTopLevelAction(m_oZoomActionHistoryList, node, nodes)) : ((ZoomAction)new ZoomedFromOneTopLevelNodeAction(m_oZoomActionHistoryList, node, node2)));
				}
				else
				{
					Debug.Assert(peekCurrentState.ParentOfZoomedNode != null);
					node2.PrivateSetParent(peekCurrentState.ParentOfZoomedNode);
					zoomAction = new ZoomedFromInnerNodeAction(m_oZoomActionHistoryList, node, node2);
				}
			}
			m_oZoomActionHistoryList.InsertState(zoomAction);
			Node selectedNode = SelectedNode;
			m_oTreemapGenerator.Clear();
			nodes.Add(node);
			if (selectedNode != null)
			{
				FireSelectedNodeChanged();
			}
		}

		/// <summary>
		/// Zooms the treemap out one level.
		/// </summary>
		///
		/// <remarks>
		/// This method replaces the current top-level node with either the node's
		/// parent or the treemap's original top-level nodes.  It can be called
		/// only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is true and <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanZoomOut" />
		/// has returned true.  It throws an exception otherwise.
		///
		/// <para>
		/// See the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> property for details on the treemap's
		/// zooming scheme.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" />
		public void ZoomOut()
		{
			AssertValid();
			VerifyIsZoomable("ZoomOut");
			if (!CanZoomOut())
			{
				throw new InvalidOperationException("TreemapControl.ZoomOut: Can't zoom out.  Check the CanZoomOut property first.");
			}
			Nodes nodes = Nodes;
			Debug.Assert(nodes.Count == 1);
			Node node = nodes[0];
			Debug.Assert(m_oZoomActionHistoryList.HasCurrentState);
			ZoomAction peekCurrentState = m_oZoomActionHistoryList.PeekCurrentState;
			ZoomAction zoomAction;
			Node[] array;
			float num;
			if (peekCurrentState.ParentOfZoomedNode == null)
			{
				array = m_oZoomActionHistoryList.OriginalTopLevelNodes;
				num = m_oZoomActionHistoryList.OriginalTopLevelEmptySpaceSizeMetric;
				zoomAction = new ZoomedFromOneTopLevelNodeAction(m_oZoomActionHistoryList, null, node);
			}
			else
			{
				Node parentOfZoomedNode = peekCurrentState.ParentOfZoomedNode;
				array = new Node[1]
				{
					parentOfZoomedNode
				};
				num = 0f;
				Debug.Assert(parentOfZoomedNode != null);
				node.PrivateSetParent(parentOfZoomedNode);
				zoomAction = new ZoomedFromInnerNodeAction(m_oZoomActionHistoryList, parentOfZoomedNode, node);
			}
			m_oZoomActionHistoryList.InsertState(zoomAction);
			Node selectedNode = SelectedNode;
			m_oTreemapGenerator.Clear();
			BeginUpdate();
			Node[] array2 = array;
			foreach (Node node2 in array2)
			{
				nodes.Add(node2);
			}
			nodes.EmptySpace.SizeMetric = num;
			EndUpdate();
			if (selectedNode != null)
			{
				FireSelectedNodeChanged();
			}
		}

		/// <summary>
		/// Moves the treemap back to the previous zoom state.
		/// </summary>
		///
		/// <remarks>
		/// This method moves the treemap back to the previous zoom state in the
		/// zoom state history list.  It can be called only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is true and <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanMoveBack" /> has
		/// returned true.  It throws an exception
		/// otherwise.
		///
		/// <para>
		/// See the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> property for details on the treemap's
		/// zooming scheme.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" />
		public void MoveBack()
		{
			AssertValid();
			VerifyIsZoomable("MoveBack");
			if (!CanMoveBack())
			{
				throw new InvalidOperationException("TreemapControl.MoveBack: Can't move back.  Check the CanMoveBack property first.");
			}
			Debug.Assert(m_oZoomActionHistoryList.HasCurrentState);
			ZoomAction currentState = m_oZoomActionHistoryList.CurrentState;
			Node selectedNode = SelectedNode;
			currentState.Undo(m_oTreemapGenerator);
			if (selectedNode != null)
			{
				FireSelectedNodeChanged();
			}
		}

		/// <summary>
		/// Moves the treemap forward to the next zoom state.
		/// </summary>
		///
		/// <remarks>
		/// This method moves the treemap forward to the next zoom state in the
		/// zoom state history list.  It can be called only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is true and <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.CanMoveForward" /> has
		/// returned true.  It throws an exception otherwise.
		///
		/// <para>
		/// See the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> property for details on the treemap's
		/// zooming scheme.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" />
		public void MoveForward()
		{
			AssertValid();
			VerifyIsZoomable("MoveForward");
			if (!CanMoveForward())
			{
				throw new InvalidOperationException("TreemapControl.MoveForward: Can't move forward.  Check the CanMoveForward property first.");
			}
			Debug.Assert(m_oZoomActionHistoryList.HasNextState);
			ZoomAction zoomAction = (ZoomAction)m_oZoomActionHistoryList.NextState;
			Node selectedNode = SelectedNode;
			zoomAction.Redo(m_oTreemapGenerator);
			if (selectedNode != null)
			{
				FireSelectedNodeChanged();
			}
		}

		/// <summary>
		/// Draws the treemap onto a <see cref="T:System.Drawing.Graphics" /> object.
		/// </summary>
		///
		/// <param name="graphics">
		/// <see cref="T:System.Drawing.Graphics" /> object to draw onto.
		/// </param>
		///
		/// <param name="treemapRectangle">
		/// Rectangle to draw into.
		/// </param>
		///
		/// <remarks>
		/// Draws a treemap onto a <see cref="T:System.Drawing.Graphics" /> object.  The selection
		/// is not drawn.
		///
		/// <para>
		/// This method can be used to print the treemap.
		/// </para>
		///
		/// </remarks>
		public void Draw(Graphics graphics, Rectangle treemapRectangle)
		{
			Debug.Assert(graphics != null);
			Debug.Assert(!treemapRectangle.IsEmpty);
			AssertValid();
			m_oTreemapGenerator.Draw(graphics, treemapRectangle);
			Invalidate();
		}

		/// <summary>
		/// Selects a node.
		/// </summary>
		///
		/// <param name="node">
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> to select.  Specify null to remove the
		/// selection.
		/// </param>
		///
		/// <remarks>
		/// This redraws the specified node's rectangle and text to show it as
		/// selected.  If another node was already selected, it gets redrawn as
		/// unselected.
		///
		/// <para>
		/// A node can also be selected by clicking on it with the mouse.  Clicking
		/// a node results in the following:
		///
		/// <list type="bullet">
		/// <item>
		/// <description>
		/// The specified node is selected and redrawn.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// The <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SelectedNodeChanged" /> event is fired.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// The <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.NodeMouseDown" /> event is fired.
		/// </description>
		/// </item>
		/// </list>
		///
		/// </para>
		///
		/// </remarks>
		public void SelectNode(Node node)
		{
			AssertValid();
			if (node != SelectedNode)
			{
				m_oTreemapGenerator.SelectNode(node, m_oBitmap);
				if (m_oBitmap != null)
				{
					picPictureBox.Image = m_oBitmap;
				}
				FireSelectedNodeChanged();
			}
		}

		/// <summary>
		/// Draw method.
		/// </summary>
		///
		/// <param name="bRetainSelection">
		/// Boolean.  true to reselect the selected node after drawing the treemap.
		/// </param>
		///
		/// <remarks>
		/// Draws a treemap onto the control.
		/// </remarks>
		protected void Draw(bool bRetainSelection)
		{
			AssertValid();
			m_oToolTipTracker.Reset();
			m_oLastMouseMovePoint = new Point(-1, -1);
			Size bitmapSizeToDraw = GetBitmapSizeToDraw();
			if (bitmapSizeToDraw.Width != 0 && bitmapSizeToDraw.Height != 0)
			{
				if (m_oBitmap != null)
				{
					m_oBitmap.Dispose();
				}
				Graphics graphics = CreateGraphics();
				try
				{
					m_oBitmap = new Bitmap(bitmapSizeToDraw.Width, bitmapSizeToDraw.Height, graphics);
				}
				catch (ArgumentException innerException)
				{
					m_oBitmap = null;
					throw new InvalidOperationException("The treemap image could not be created.  It may be too large.  (Its size is " + bitmapSizeToDraw + ".)", innerException);
				}
				finally
				{
					graphics.Dispose();
				}
				m_oTreemapGenerator.Draw(m_oBitmap, bRetainSelection);
				picPictureBox.Size = bitmapSizeToDraw;
				picPictureBox.Location = new Point(0);
				picPictureBox.Image = m_oBitmap;
			}
		}

		/// <summary>
		/// GetClientMousePosition method.
		/// </summary>
		///
		/// <returns>
		/// PointF.  Mouse position in client coordinates.
		/// </returns>
		///
		/// <remarks>
		/// Returns the current mouse position in client coordinates.
		///
		/// <para>
		/// NOTE: The point returned by this method can be outside the client
		/// area.  This can happen if the user is moving the mouse quickly.  The
		/// caller should check for this.
		/// </para>
		///
		/// </remarks>
		protected PointF GetClientMousePosition()
		{
			return ControlUtil.GetClientMousePosition(this);
		}

		/// <summary>
		/// GetBitmapSizeToDraw method.
		/// </summary>
		///
		/// <returns>
		/// Size.  Size to use for the bitmap.
		/// </returns>
		///
		/// <remarks>
		/// Returns the size to use for the treemap bitmap, taking zoom settings
		/// into consideration.
		/// </remarks>
		protected Size GetBitmapSizeToDraw()
		{
			bool autoScroll = AutoScroll;
			AutoScroll = false;
			Size clientSize = base.ClientSize;
			AutoScroll = autoScroll;
			if (!AutoScroll)
			{
				return clientSize;
			}
			return base.AutoScrollMinSize;
		}

		/// <summary>
		/// Handles the PaintBackground event.
		/// </summary>
		///
		/// <param name="oPaintEventArgs">
		/// Standard event arguments.
		/// </param>
		protected override void OnPaintBackground(PaintEventArgs oPaintEventArgs)
		{
		}

		/// <summary>
		/// Handles the Paint event.
		/// </summary>
		///
		/// <param name="oPaintEventArgs">
		/// Standard event arguments.
		/// </param>
		protected override void OnPaint(PaintEventArgs oPaintEventArgs)
		{
			AssertValid();
			Debug.Assert(oPaintEventArgs != null);
			Draw(bRetainSelection: true);
		}

		/// <summary>
		/// ShowToolTip method.
		/// </summary>
		///
		/// <param name="oNode">
		/// Treemap.Node.  Node to show a tooltip for.
		/// </param>
		///
		/// <remarks>
		/// Shows a tooltip for the specified node.  Do not call this if
		/// m_bShowToolTips is false.
		/// </remarks>
		protected void ShowToolTip(Node oNode)
		{
			Debug.Assert(m_bShowToolTips);
			Debug.Assert(oNode != null);
			string toolTip = oNode.ToolTip;
			if (toolTip != null && !(toolTip == ""))
			{
				pnlToolTip.ShowToolTip(toolTip, this);
			}
		}

		/// <summary>
		/// HideToolTip method.
		/// </summary>
		///
		/// <remarks>
		/// Hides the tooltip shown by ShowToolTip().
		/// </remarks>
		protected void HideToolTip()
		{
			pnlToolTip.HideToolTip();
			AssertValid();
		}

		/// <summary>
		/// Throws an exception if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is false.
		/// </summary>
		///
		/// <param name="sMethodName">
		/// Name of the method calling this method.
		/// </param>
		///
		/// <remarks>
		/// This method throws an exception if the user is trying to call a
		/// zooming-related method when <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.IsZoomable" /> is false.
		/// </remarks>
		protected void VerifyIsZoomable(string sMethodName)
		{
			Debug.Assert(sMethodName != null);
			Debug.Assert(sMethodName.Length > 0);
			if (!IsZoomable)
			{
				throw new InvalidOperationException($"TreemapControl.{sMethodName}: This can't be used if the IsZoomable property is false.");
			}
		}

		/// <summary>
		/// Fires the <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.SelectedNodeChanged" /> event.
		/// </summary>
		protected void FireSelectedNodeChanged()
		{
			AssertValid();
			this.SelectedNodeChanged?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Dispose method.
		/// </summary>
		///
		/// <param name="bDisposing">
		/// Boolean.  See Component.Dispose().
		/// </param>
		///
		/// <remarks>
		/// Frees resources.  Call this when you are done with the object.
		/// </remarks>
		protected override void Dispose(bool bDisposing)
		{
			if (bDisposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
				if (m_oBitmap != null)
				{
					m_oBitmap.Dispose();
					m_oBitmap = null;
				}
				if (m_oToolTipTracker != null)
				{
					m_oToolTipTracker.Dispose();
					m_oToolTipTracker = null;
				}
			}
			base.Dispose(bDisposing);
		}

		/// <summary>
		/// picPictureBox_MouseDown method.
		/// </summary>
		///
		/// <param name="oSource">
		/// Object.  Source of the event.
		/// </param>
		///
		/// <param name="oMouseEventArgs">
		/// MouseEventArgs.  Standard mouse event arguments.
		/// </param>
		///
		/// <remarks>
		/// Handles the MouseDown event fired by picPictureBox.
		/// </remarks>
		protected void picPictureBox_MouseDown(object oSource, MouseEventArgs oMouseEventArgs)
		{
			if (!m_oTreemapGenerator.GetNodeFromPoint(oMouseEventArgs.X, oMouseEventArgs.Y, out Node node))
			{
				return;
			}
			SelectNode(node);
			this.NodeMouseDown?.Invoke(this, new NodeMouseEventArgs(oMouseEventArgs, node));
			if (oMouseEventArgs.Clicks == 2)
			{
				this.NodeDoubleClick?.Invoke(this, new NodeEventArgs(node));
			}
			else if (m_bAllowDrag && (oMouseEventArgs.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				m_oLastDraggableMouseDownPoint = new Point(oMouseEventArgs.X, oMouseEventArgs.Y);
			}
		}

		/// <summary>
		/// picPictureBox_MouseUp method.
		/// </summary>
		///
		/// <param name="oSource">
		/// Object.  Source of the event.
		/// </param>
		///
		/// <param name="oMouseEventArgs">
		/// MouseEventArgs.  Standard mouse event arguments.
		/// </param>
		///
		/// <remarks>
		/// Handles the MouseUp event fired by picPictureBox.
		/// </remarks>
		protected void picPictureBox_MouseUp(object oSource, MouseEventArgs oMouseEventArgs)
		{
			if (m_oTreemapGenerator.GetNodeFromPoint(oMouseEventArgs.X, oMouseEventArgs.Y, out Node node) && this.NodeMouseUp != null)
			{
				NodeMouseEventArgs nodeMouseEventArgs = new NodeMouseEventArgs(oMouseEventArgs, node);
				this.NodeMouseUp(this, nodeMouseEventArgs);
			}
			m_oLastDraggableMouseDownPoint = new Point(-1, -1);
		}

		/// <summary>
		/// picPictureBox_MouseMove method.
		/// </summary>
		///
		/// <param name="oSource">
		/// Object.  Source of the event.
		/// </param>
		///
		/// <param name="oMouseEventArgs">
		/// MouseEventArgs.  Standard mouse event arguments.
		/// </param>
		///
		/// <remarks>
		/// Handles the MouseMove event fired by picPictureBox.
		/// </remarks>
		private void picPictureBox_MouseMove(object oSource, MouseEventArgs oMouseEventArgs)
		{
			if (oMouseEventArgs.X == m_oLastMouseMovePoint.X && oMouseEventArgs.Y == m_oLastMouseMovePoint.Y)
			{
				return;
			}
			m_oLastMouseMovePoint = new Point(oMouseEventArgs.X, oMouseEventArgs.Y);
			if (pnlToolTip.Visible && new Rectangle(pnlToolTip.Location, pnlToolTip.Size).Contains(m_oLastMouseMovePoint))
			{
				return;
			}
			m_oTreemapGenerator.GetNodeFromPoint(oMouseEventArgs.X, oMouseEventArgs.Y, out Node node);
			m_oToolTipTracker.OnMouseMoveOverObject(node);
			if (!(m_oLastDraggableMouseDownPoint != new Point(-1, -1)))
			{
				return;
			}
			int x = m_oLastDraggableMouseDownPoint.X;
			int y = m_oLastDraggableMouseDownPoint.Y;
			if (Math.Abs(oMouseEventArgs.X - x) >= SystemInformation.DragSize.Width / 2 || Math.Abs(oMouseEventArgs.Y - y) >= SystemInformation.DragSize.Height / 2)
			{
				bool nodeFromPoint = m_oTreemapGenerator.GetNodeFromPoint(x, y, out node);
				Debug.Assert(nodeFromPoint);
				Debug.Assert(node != null);
				object obj = node.Tag;
				if (obj == null)
				{
					obj = string.Empty;
				}
				DoDragDrop(obj, DragDropEffects.Copy);
			}
		}

		/// <summary>
		/// picPictureBox_MouseLeave method.
		/// </summary>
		///
		/// <param name="oSource">
		/// Object.  Source of the event.
		/// </param>
		///
		/// <param name="oEventArgs">
		/// EventArgs.  Standard event arguments.
		/// </param>
		///
		/// <remarks>
		/// Handles the MouseLeave event fired by picPictureBox.
		/// </remarks>
		private void picPictureBox_MouseLeave(object oSource, EventArgs oEventArgs)
		{
			m_oToolTipTracker.OnMouseMoveOverObject(null);
			m_oLastDraggableMouseDownPoint = new Point(-1, -1);
		}

		/// <summary>
		/// oToolTipTracker_ShowToolTip method.
		/// </summary>
		///
		/// <param name="oSource">
		/// Object.  Source of the event.
		/// </param>
		///
		/// <param name="oToolTipTrackerEventArgs">
		/// ToolTipEventArgs.  Event arguments.
		/// </param>
		///
		/// <remarks>
		/// Handles the ShowToolTip event fired by oToolTipTracker.
		/// </remarks>
		private void oToolTipTracker_ShowToolTip(object oSource, ToolTipTrackerEventArgs oToolTipTrackerEventArgs)
		{
			Node node = (Node)oToolTipTrackerEventArgs.Object;
			Debug.Assert(node != null);
			if (m_bShowToolTips)
			{
				ShowToolTip(node);
			}
			this.NodeMouseHover?.Invoke(this, new NodeEventArgs(node));
		}

		/// <summary>
		/// oToolTipTracker_HideToolTip method.
		/// </summary>
		///
		/// <param name="oSource">
		/// Object.  Source of the event.
		/// </param>
		///
		/// <param name="oToolTipTrackerEventArgs">
		/// ToolTipEventArgs.  Event arguments.
		/// </param>
		///
		/// <remarks>
		/// Handles the HideToolTip event fired by oToolTipTracker.
		/// </remarks>
		private void oToolTipTracker_HideToolTip(object oSource, ToolTipTrackerEventArgs oToolTipTrackerEventArgs)
		{
			if (m_bShowToolTips)
			{
				HideToolTip();
			}
		}

		/// <summary>
		/// Handles the Change event on the m_oZoomActionHistoryList object.
		/// </summary>
		///
		/// <param name="oSender">
		/// Standard event arguments.
		/// </param>
		///
		/// <param name="oEventArgs">
		/// Standard event arguments.
		/// </param>
		private void ZoomActionHistoryList_Change(object oSender, EventArgs oEventArgs)
		{
			this.ZoomStateChanged?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Handles the RedrawRequired event on the m_oTreemapGenerator object.
		/// </summary>
		///
		/// <param name="oSender">
		/// Standard event arguments.
		/// </param>
		///
		/// <param name="oEventArgs">
		/// Standard event arguments.
		/// </param>
		protected void TreemapGenerator_RedrawRequired(object oSender, EventArgs oEventArgs)
		{
			Invalidate();
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(m_oTreemapGenerator != null);
			m_oTreemapGenerator.AssertValid();
			if (m_bIsZoomable)
			{
				Debug.Assert(m_oZoomActionHistoryList != null);
			}
			else
			{
				Debug.Assert(m_oZoomActionHistoryList == null);
			}
			Debug.Assert(m_oToolTipTracker != null);
		}

		/// <summary>
		/// InitializeComponent method.
		/// </summary>
		///
		/// <remarks>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </remarks>
		private void InitializeComponent()
		{
			picPictureBox = new System.Windows.Forms.PictureBox();
			pnlToolTip = new Microsoft.Research.CommunityTechnologies.AppLib.ToolTipPanel();
			picPictureBox.Location = new System.Drawing.Point(126, 17);
			picPictureBox.Name = "picPictureBox";
			picPictureBox.TabIndex = 0;
			picPictureBox.TabStop = false;
			picPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(picPictureBox_MouseUp);
			picPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(picPictureBox_MouseMove);
			picPictureBox.MouseLeave += new System.EventHandler(picPictureBox_MouseLeave);
			picPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(picPictureBox_MouseDown);
			pnlToolTip.BackColor = System.Drawing.SystemColors.Window;
			pnlToolTip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			pnlToolTip.Name = "pnlToolTip";
			pnlToolTip.TabIndex = 0;
		}
	}
}
