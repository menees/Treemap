using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.Research.CommunityTechnologies.TreemapNoDoc;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	/// <summary>
	/// Treemap drawing engine.
	/// </summary>
	///
	/// <remarks>
	/// TreemapGenerator is one of two components that render a hierarchical data
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
	/// <term>TreemapGenerator</term>
	/// <term>
	/// Any application that wants to draw a treemap onto a <see cref="T:System.Drawing.Bitmap" />
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
	/// TreemapGenerator is a drawing engine without its own user interface.  It
	/// draws on a <see cref="T:System.Drawing.Bitmap" /> object provided by the caller.
	/// This allows it to be used in a variety of environments, including Web
	/// applications that generate images on the server for downloading to client
	/// browsers.
	/// </para>
	///
	/// <para>
	/// Using the TreemapGenerator in an application involves three steps:
	/// <list type="bullet">
	/// <item>
	/// <description>Populate the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Nodes" /> collection
	/// </description>
	/// </item>
	/// <item>
	/// <description>
	/// Set properties that determine how the treemap is drawn
	/// </description>
	/// </item>
	/// <item>
	/// <description>
	/// Draw the treemap using one of the <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Draw(System.Drawing.Bitmap,System.Boolean)" />
	/// methods
	/// </description>
	/// </item>
	/// </list>
	/// </para>
	///
	/// <para>
	/// The <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Nodes" /> property on TreemapGenerator
	/// returns a collection of top-level <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.  Each
	/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object in turn has a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.Nodes" /> property
	/// that returns a collection of child <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.  These
	/// nested collections are directly analogous to the Nodes collections in the
	/// standard .NET TreeView control.
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
	/// Several overloaded <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Draw(System.Drawing.Bitmap,System.Boolean)" /> methods are
	/// provided.  You can draw onto an entire <see cref="T:System.Drawing.Bitmap" />, part of a
	/// <see cref="T:System.Drawing.Bitmap" />, or a <see cref="T:System.Drawing.Graphics" /> object.  You can
	/// also implement your own drawing code by calling <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Draw(System.Drawing.Rectangle)" /> and handling the <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DrawItem" />
	/// event.
	/// </para>
	///
	/// <para>
	/// To improve performance, call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.BeginUpdate" /> before populating
	/// the treemap with nodes.  This prevents the chart from being immediately
	/// updated.  Call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.EndUpdate" /> when you are done.
	/// </para>
	///
	/// </remarks>
	///
	/// <example>
	/// Here is sample C# code that populates a TreemapGenerator
	/// with two top-level nodes, each of which has two child nodes.  A few
	/// properties that determine how the treemap is drawn are set, and the treemap
	/// is drawn to a bitmap object.
	///
	/// <code>
	///
	/// protected void
	/// MakeTreemapBitmap()
	/// {
	///     // Create a TreemapGenerator object.
	///
	///     TreemapGenerator oTreemapGenerator = new TreemapGenerator();
	///
	///     // Improve performance by turning off updating while the chart is
	///     // being populated.
	///
	///     oTreemapGenerator.BeginUpdate();
	///     PopulateTreemap(oTreemapGenerator);
	///     oTreemapGenerator.EndUpdate();
	///
	///     // Set some properties on the treemap and draw it onto a bitmap.
	///
	///     SetTreemapProperties(oTreemapGenerator);
	///     DrawTreemap(oTreemapGenerator);
	/// }
	///
	/// protected void
	/// PopulateTreemap(TreemapGenerator oTreemapGenerator)
	/// {
	///     Nodes oNodes;
	///     Node oNode;
	///     Nodes oChildNodes;
	///     Node oChildNode;
	///
	///     // Get the collection of top-level nodes.
	///
	///     oNodes = oTreemapGenerator.Nodes;
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
	///     // method, the component can be populated via an XML string
	///     // passed to the TreemapGenerator.NodesXml property.)
	/// }
	///
	/// protected void
	/// SetTreemapProperties(TreemapGenerator oTreemapGenerator)
	/// {
	///     // All TreemapGenerator properties have default values that yield
	///     // reasonable results in many cases.  We want to change the
	///     // range of colors for this example.
	///
	///     // Make Node.ColorMetric values of -200 to 200 map to a color
	///     // range between blue and yellow.
	///
	///     oTreemapGenerator.MinColorMetric = -200F;
	///     oTreemapGenerator.MaxColorMetric = 200F;
	///
	///     oTreemapGenerator.MinColor = Color.Blue;
	///     oTreemapGenerator.MaxColor = Color.Yellow;
	///
	///     // (If desired, set other properties that determine border widths,
	///     // spacing between rectangles, fonts, etc.)
	/// }
	///
	/// protected void
	/// DrawTreemap(TreemapGenerator oTreemapGenerator)
	/// {
	///     // Create a bitmap.
	///
	///     Bitmap oBitmap = new Bitmap(200, 200);
	///
	///     // Draw the treemap onto the bitmap.
	///
	///     oTreemapGenerator.Draw(oBitmap, false);
	///
	///     // (Do something with the bitmap...)
	/// }
	///
	/// </code>
	///
	/// </example>
	public class TreemapGenerator : ITreemapComponent
	{
		/// <summary>
		/// Represents the method that will handle the <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DrawItem" />
		/// event of a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> object.
		/// </summary>
		///
		/// <param name="sender">
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> that fired the event.
		/// </param>
		///
		/// <param name="treemapDrawItemEventArgs">
		/// Provides information that can be used to draw the item.
		/// </param>
		public delegate void TreemapDrawItemEventHandler(object sender, TreemapDrawItemEventArgs treemapDrawItemEventArgs);

		/// <summary>
		/// Minimum value allowed for the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingPx" /> property.
		/// </summary>
		public const int MinPaddingPx = 1;

		/// <summary>
		/// Maximum value allowed for the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingPx" /> property.
		/// </summary>
		public const int MaxPaddingPx = 100;

		/// <summary>
		/// Minimum value allowed for the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingDecrementPerLevelPx" /> property.
		/// </summary>
		public const int MinPaddingDecrementPerLevelPx = 0;

		/// <summary>
		/// Maximum value allowed for the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingDecrementPerLevelPx" /> property.
		/// </summary>
		public const int MaxPaddingDecrementPerLevelPx = 99;

		/// <summary>
		/// Minimum value allowed for the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthPx" /> property.
		/// </summary>
		public const int MinPenWidthPx = 1;

		/// <summary>
		/// Maximum value allowed for the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthPx" /> property.
		/// </summary>
		public const int MaxPenWidthPx = 100;

		/// <summary>
		/// Minimum value allowed for the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthDecrementPerLevelPx" /> property.
		/// </summary>
		public const int MinPenWidthDecrementPerLevelPx = 0;

		/// <summary>
		/// Maximum value allowed for the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthDecrementPerLevelPx" /> property.
		/// </summary>
		public const int MaxPenWidthDecrementPerLevelPx = 99;

		/// <summary>
		/// Minimum value allowed for the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DiscreteNegativeColors" />
		/// and <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DiscretePositiveColors" /> properties.
		/// </summary>
		public const int MinDiscreteColors = 2;

		/// <summary>
		/// Maximum value allowed for the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DiscreteNegativeColors" />
		/// and <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DiscretePositiveColors" /> properties.
		/// </summary>
		public const int MaxDiscreteColors = 50;

		/// Minimum width or height that a node rectangle can have.  Because of
		/// the way GDI draws rectangles, this results in a 2x2 rectangle on the
		/// screen.
		protected const float MinRectangleWidthOrHeightPx = 1f;

		/// Collection of top-level Node objects.
		protected Nodes m_oNodes;

		/// Padding to draw around the rectangles for the top-level nodes, in
		/// pixels.  Must be between MinPaddingPx and MaxPaddingPx.
		protected int m_iPaddingPx;

		/// Number of pixels to subtract from m_iPaddingPx at each node level.  Must
		/// be between MinPaddingDecrementPerLevelPx and
		/// MaxPaddingDecrementPerLevelPx.
		protected int m_iPaddingDecrementPerLevelPx;

		/// Pen width to use for the top-level nodes, in pixels.  Must be between
		/// MinPenWidthPx and MaxPenWidthPx.
		protected int m_iPenWidthPx;

		/// Number of pixels to subtract from m_iPenWidthPx at each node level.
		/// Must be between MinPenWidthDecrementPerLevelPx and
		/// MaxPenWidthDecrementPerLevelPx.
		protected int m_iPenWidthDecrementPerLevelPx;

		/// Color to use for the treemap's background, which shows through where
		/// there are no node rectangles.
		protected Color m_oBackColor;

		/// Color to use for the rectangle borders.
		protected Color m_oBorderColor;

		protected NodeColorAlgorithm m_eNodeColorAlgorithm;

		/// Color used to fill rectangles for which Node.ColorMetric is less
		/// than or equal to m_fMinColorMetric.
		protected Color m_oMinColor;

		/// Color used to fill rectangles for which ColorMetric is greater than or
		/// equal to m_fMaxColorMetric.
		protected Color m_oMaxColor;

		/// Nodes with a ColorMetric value of m_fMinColorMetric or less get filled
		/// with m_oMinColor.  Must be negative.
		protected float m_fMinColorMetric;

		/// Nodes with a ColorMetric value of m_fMaxColorMetric or greater get
		/// filled with m_oMaxColor.
		protected float m_fMaxColorMetric;

		/// Number of discrete colors in the color gradient between white and
		/// m_oMaxColor.
		protected int m_iDiscretePositiveColors;

		/// Number of discrete colors in the color gradient between white and
		/// m_oMinColor.
		protected int m_iDiscreteNegativeColors;

		/// Font family to use.
		protected string m_sFontFamily;

		/// Minimum font size to use.
		protected float m_fFontMinSizePt;

		/// Maximum font size to use.
		protected float m_fFontMaxSizePt;

		/// Increment between font sizes.
		protected float m_fFontIncrementPt;

		/// Color to use for text.
		protected Color m_oFontSolidColor;

		/// Minimum alpha value to use for text.
		protected int m_iFontMinAlpha;

		/// Maximum alpha value to use for text.
		protected int m_iFontMaxAlpha;

		/// Increment between alpha values.
		protected int m_iFontAlphaIncrementPerLevel;

		protected Color m_oSelectedFontColor;

		protected Color m_oSelectedBackColor;

		/// Indicates which node levels should include text.
		protected NodeLevelsWithText m_iNodeLevelsWithText;

		/// Minimum node level that should include text if m_iNodeLevelsWithText
		/// is set to Range.
		protected int m_iMinNodeLevelWithText;

		/// Maximum node level that should include text if m_iNodeLevelsWithText
		/// is set to Range.
		protected int m_iMaxNodeLevelWithText;

		protected TextLocation m_eTextLocation;

		protected EmptySpaceLocation m_eEmptySpaceLocation;

		/// Currently selected node, or null if there is none.
		protected Node m_oSelectedNode;

		/// Bitmap of selected node before it was repainted to show it as selected.
		protected Bitmap m_oSavedSelectedNodeBitmap;

		/// Gets set to true by BeginUpdate() and false by EndUpdate().
		protected bool m_bInBeginUpdate;

		protected LayoutAlgorithm m_eLayoutAlgorithm;

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
		[Browsable(false)]
		public Nodes Nodes
		{
			get
			{
				AssertValid();
				return m_oNodes;
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
		/// persistence mechanism.  Once you populate the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Nodes" />
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
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.NodesXmlAttributeOverrides" /> property.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Nodes" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.NodesXmlAttributeOverrides" />
		[Browsable(false)]
		public string NodesXml
		{
			get
			{
				AssertValid();
				NodesXmlSerializer nodesXmlSerializer = new NodesXmlSerializer();
				return nodesXmlSerializer.SerializeToString(m_oNodes, this);
			}
			set
			{
				CancelSelectedNode();
				NodesXmlSerializer nodesXmlSerializer = new NodesXmlSerializer();
				m_oNodes = nodesXmlSerializer.DeserializeFromString(value, this);
				m_oNodes.TreemapGenerator = this;
				FireRedrawRequired();
				AssertValid();
			}
		}

		public LayoutAlgorithm LayoutAlgorithm
		{
			get
			{
				AssertValid();
				return m_eLayoutAlgorithm;
			}
			set
			{
				if (m_eLayoutAlgorithm != value)
				{
					m_eLayoutAlgorithm = value;
					FireRedrawRequired();
				}
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
		/// If the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingDecrementPerLevelPx" /> property is 0,
		/// PaddingPx is the padding that is added to all node rectangles.
		/// Otherwise, <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingDecrementPerLevelPx" /> is subtracted from
		/// the padding at each node level.  Decreasing the padding at lower levels
		/// can improve the general appearance of the treemap.
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingDecrementPerLevelPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPaddingPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPaddingPx" />
		public int PaddingPx
		{
			get
			{
				AssertValid();
				return m_iPaddingPx;
			}
			set
			{
				if (value < 1 || value > 100)
				{
					throw new ArgumentOutOfRangeException("PaddingPx", value, "TreemapGenerator.PaddingPx: Must be between " + 1 + " and " + 100 + ".");
				}
				if (m_iPaddingPx != value)
				{
					m_iPaddingPx = value;
					FireRedrawRequired();
				}
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
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingPx" />.  Set PaddingDecrementPerLevelPx to a positive
		/// value to force the padding to decrease at each level.  This can improve
		/// the general appearance of the treemap.  A value of 0 causes all
		/// nodes to use <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingPx" />.
		///
		/// <para>
		/// If <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingPx" /> is 5 and PaddingDecrementPerLevelPx is 1,
		/// for example, the padding for the top-level, child, and grandchild nodes
		/// will be 5, 4, and 3 pixels, respectively.  Padding is never less than
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPaddingPx" /> pixels.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPaddingDecrementPerLevelPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPaddingDecrementPerLevelPx" />
		public int PaddingDecrementPerLevelPx
		{
			get
			{
				AssertValid();
				return m_iPaddingDecrementPerLevelPx;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("PaddingDecrementPerLevelPx", value, "TreemapGenerator.PaddingDecrementPerLevelPx: Must be between " + 0 + " and " + 99 + ".");
				}
				if (m_iPaddingDecrementPerLevelPx != value)
				{
					m_iPaddingDecrementPerLevelPx = value;
					FireRedrawRequired();
				}
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
		/// If the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthDecrementPerLevelPx" /> property is 0, all
		/// rectangles are drawn with a pen width of PenWidthPx pixels.  Otherwise,
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthDecrementPerLevelPx" /> is subtracted from
		/// the pen width at each node level.  Decreasing the pen width at lower
		/// levels can improve the general appearance of the treemap.
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthDecrementPerLevelPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPenWidthPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPenWidthPx" />
		public int PenWidthPx
		{
			get
			{
				AssertValid();
				return m_iPenWidthPx;
			}
			set
			{
				if (value < 1 || value > 100)
				{
					throw new ArgumentOutOfRangeException("PenWidthPx", value, "TreemapGenerator.PenWidthPx: Must be between " + 1 + " and " + 100 + ".");
				}
				if (m_iPenWidthPx != value)
				{
					m_iPenWidthPx = value;
					FireRedrawRequired();
				}
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
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthPx" /> pixels.  Set PenWidthDecrementPerLevelPx to
		/// a positive value to force the pen width to decrease at each level.
		/// This can improve the general appearance of the treemap.  A value of 0
		/// causes all nodes to use <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthPx" />.
		///
		/// <para>
		/// If <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthPx" /> is 4 and PenWidthDecrementPerLevelPx is
		/// 1, for example, the pen width for the top-level nodes will be 4, the
		/// width for the child nodes will be 3, the width for the grandchildren
		/// will be 2, and so on.  The pen width is never less than 1 pixel.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinPenWidthDecrementPerLevelPx" />
		/// <seealso cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxPenWidthDecrementPerLevelPx" />
		public int PenWidthDecrementPerLevelPx
		{
			get
			{
				AssertValid();
				return m_iPenWidthDecrementPerLevelPx;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("PenWidthDecrementPerLevelPx", value, "TreemapGenerator.PenWidthDecrementPerLevelPx: Must be between " + 0 + " and " + 99 + ".");
				}
				if (m_iPenWidthDecrementPerLevelPx != value)
				{
					m_iPenWidthDecrementPerLevelPx = value;
					FireRedrawRequired();
				}
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
		public Color BackColor
		{
			get
			{
				AssertValid();
				return m_oBackColor;
			}
			set
			{
				if (m_oBackColor != value)
				{
					m_oBackColor = value;
					FireRedrawRequired();
				}
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
		public Color BorderColor
		{
			get
			{
				AssertValid();
				return m_oBorderColor;
			}
			set
			{
				if (m_oBorderColor != value)
				{
					m_oBorderColor = value;
					FireRedrawRequired();
				}
			}
		}

		public NodeColorAlgorithm NodeColorAlgorithm
		{
			get
			{
				AssertValid();
				return m_eNodeColorAlgorithm;
			}
			set
			{
				if (m_eNodeColorAlgorithm != value)
				{
					m_eNodeColorAlgorithm = value;
					FireRedrawRequired();
				}
			}
		}

		/// <summary>
		/// Gets or sets the maximum negative fill color.
		/// </summary>
		///
		/// <value>
		/// The fill color for nodes with a
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColorMetric" />
		/// or less, as a <see cref="T:System.Drawing.Color" />.
		/// </value>
		///
		/// <remarks>
		/// See <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> for details on how fill colors for
		/// node rectangles are determined.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DiscreteNegativeColors" />
		public Color MinColor
		{
			get
			{
				AssertValid();
				return m_oMinColor;
			}
			set
			{
				if (m_oMinColor != value)
				{
					m_oMinColor = value;
					FireRedrawRequired();
				}
			}
		}

		/// <summary>
		/// Gets or sets the maximum positive fill color.
		/// </summary>
		///
		/// <value>
		/// The fill color for nodes with a
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColorMetric" />
		/// or greater, as a <see cref="T:System.Drawing.Color" />.
		/// </value>
		///
		/// <remarks>
		/// See <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> for details on how fill colors for
		/// node rectangles are determined.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DiscretePositiveColors" />
		public Color MaxColor
		{
			get
			{
				AssertValid();
				return m_oMaxColor;
			}
			set
			{
				if (m_oMaxColor != value)
				{
					m_oMaxColor = value;
					FireRedrawRequired();
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value to map to 
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColor" />.
		/// </summary>
		///
		/// <value>
		/// The <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value to map to
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColor" />.
		/// </value>
		///
		/// <remarks>
		/// See <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> for details on how fill colors for
		/// node rectangles are determined.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColor" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DiscreteNegativeColors" />
		public float MinColorMetric
		{
			get
			{
				AssertValid();
				return m_fMinColorMetric;
			}
			set
			{
				if (value >= 0f)
				{
					throw new ArgumentOutOfRangeException("MinColorMetric", value, "TreemapGenerator.MinColorMetric: Must be negative.");
				}
				if (m_fMinColorMetric != value)
				{
					m_fMinColorMetric = value;
					FireRedrawRequired();
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value to map to 
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColor" />.
		/// </summary>
		///
		/// <value>
		/// The <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value to map to
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColor" />.
		/// </value>
		///
		/// <remarks>
		/// See <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> for details on how fill colors for
		/// node rectangles are determined.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColor" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DiscretePositiveColors" />
		public float MaxColorMetric
		{
			get
			{
				AssertValid();
				return m_fMaxColorMetric;
			}
			set
			{
				if (value <= 0f)
				{
					throw new ArgumentOutOfRangeException("MaxColorMetric", value, "TreemapGenerator.MaxColorMetric: Must be positive.");
				}
				if (m_fMaxColorMetric != value)
				{
					m_fMaxColorMetric = value;
					FireRedrawRequired();
				}
			}
		}

		/// <summary>
		/// Gets or sets the number of discrete fill colors to use in the negative
		/// color range.
		/// </summary>
		///
		/// <value>
		/// The number of discrete fill colors to use in the range between white
		/// and <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColor" />.  Must be between
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinDiscreteColors" /> and
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxDiscreteColors" />.  The default value
		/// is 20.
		/// </value>
		///
		/// <remarks>
		///
		/// When filling rectangles for nodes that have a negative
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value, the
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> splits the negative color range, which
		/// is white to <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColor" />, into a set of discrete colors.
		/// Nodes with a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of 0 are filled with
		/// white, nodes with a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColorMetric" /> or less are filled with
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColor" />, and nodes with a
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
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColor" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		public int DiscreteNegativeColors
		{
			get
			{
				AssertValid();
				return m_iDiscreteNegativeColors;
			}
			set
			{
				if (value < 2 || value > 50)
				{
					throw new ArgumentOutOfRangeException("DiscreteNegativeColors", value, "TreemapGenerator.DiscreteNegativeColors: Must be between " + 2 + " and " + 50 + ".");
				}
				if (m_iDiscreteNegativeColors != value)
				{
					m_iDiscreteNegativeColors = value;
					FireRedrawRequired();
				}
			}
		}

		/// <summary>
		/// Gets or sets the number of discrete fill colors to use in the positive
		/// color range.
		/// </summary>
		///
		/// <value>
		/// The number of discrete fill colors to use in the range between white
		/// and <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColor" />.  Must be between
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinDiscreteColors" /> and
		/// <see cref="F:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxDiscreteColors" />.  The default value
		/// is 20.
		/// </value>
		///
		/// <remarks>
		///
		/// When filling rectangles for nodes that have a positive
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value, the
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> splits the positive color range, which
		/// is white to <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColor" />, into a set of discrete colors.
		/// Nodes with a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of 0 are filled with
		/// white, nodes with a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value of
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColorMetric" /> or greater are filled with
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColor" />, and nodes with a
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
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColor" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColorMetric" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" />
		public int DiscretePositiveColors
		{
			get
			{
				AssertValid();
				return m_iDiscretePositiveColors;
			}
			set
			{
				if (value < 2 || value > 50)
				{
					throw new ArgumentOutOfRangeException("DiscretePositiveColors", value, "TreemapGenerator.DiscretePositiveColors: Must be between " + 2 + " and " + 50 + ".");
				}
				if (m_iDiscretePositiveColors != value)
				{
					m_iDiscretePositiveColors = value;
					FireRedrawRequired();
				}
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
		public string FontFamily
		{
			get
			{
				AssertValid();
				return m_sFontFamily;
			}
			set
			{
				Font font = new Font(value, 8f);
				string name = font.FontFamily.Name;
				font.Dispose();
				if (name.ToLower() != value.ToLower())
				{
					throw new ArgumentOutOfRangeException("FontFamily", value, "TreemapGenerator.FontFamily: No such font.");
				}
				if (m_sFontFamily != value)
				{
					m_sFontFamily = value;
					FireRedrawRequired();
				}
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
		/// <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.SetFontAlphaRange(System.Int32,System.Int32,System.Int32)" /> for details.
		/// </remarks>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.SetFontAlphaRange(System.Int32,System.Int32,System.Int32)" />
		public Color FontSolidColor
		{
			get
			{
				AssertValid();
				return m_oFontSolidColor;
			}
			set
			{
				if (value.A != byte.MaxValue)
				{
					throw new ArgumentOutOfRangeException("FontSolidColor", value, "TreemapGenerator.FontSolidColor: Must not be transparent.");
				}
				if (m_oFontSolidColor != value)
				{
					m_oFontSolidColor = value;
					FireRedrawRequired();
				}
			}
		}

		public Color SelectedFontColor
		{
			get
			{
				AssertValid();
				return m_oSelectedFontColor;
			}
			set
			{
				if (m_oSelectedFontColor != value)
				{
					m_oSelectedFontColor = value;
					FireRedrawRequired();
				}
			}
		}

		public Color SelectedBackColor
		{
			get
			{
				AssertValid();
				return m_oSelectedBackColor;
			}
			set
			{
				if (m_oSelectedBackColor != value)
				{
					m_oSelectedBackColor = value;
					FireRedrawRequired();
				}
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
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.SetNodeLevelsWithTextRange(System.Int32,System.Int32)" />
		public NodeLevelsWithText NodeLevelsWithText
		{
			get
			{
				AssertValid();
				return m_iNodeLevelsWithText;
			}
			set
			{
				if (m_iNodeLevelsWithText != value)
				{
					m_iNodeLevelsWithText = value;
					FireRedrawRequired();
				}
			}
		}

		public TextLocation TextLocation
		{
			get
			{
				AssertValid();
				return m_eTextLocation;
			}
			set
			{
				if (m_eTextLocation != value)
				{
					m_eTextLocation = value;
					FireRedrawRequired();
				}
			}
		}

		public EmptySpaceLocation EmptySpaceLocation
		{
			get
			{
				AssertValid();
				return m_eEmptySpaceLocation;
			}
			set
			{
				if (m_eEmptySpaceLocation != value)
				{
					m_eEmptySpaceLocation = value;
					FireRedrawRequired();
				}
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
		public Node SelectedNode
		{
			get
			{
				AssertValid();
				return m_oSelectedNode;
			}
		}

		/// <summary>
		/// Occurs during owner drawing.
		/// </summary>
		///
		/// <remarks>
		/// Owner drawing is initiated by calling the Draw(rectangle) method.
		/// This computes the rectangle for each of the TreemapGenerator's
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects, then fires the DrawItem event for each
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />.  Information needed to draw the
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> can be obtained from the event arguments.
		/// </remarks>
		///
		/// <example>
		/// Here is sample C# code that populates a TreemapGenerator with two
		/// top-level nodes, each of which has two child nodes.  It implements a
		/// DrawItem handler to do owner drawing and calls the Draw(Rectangle)
		/// method to initiate drawing.
		///
		/// <code>
		///
		/// protected void
		/// OwnerDrawTreemap()
		/// {
		///     // Create a TreemapGenerator object.
		///
		///     TreemapGenerator oTreemapGenerator = new TreemapGenerator();
		///
		///     // We want to do owner drawing, so handle the DrawItem event.
		///
		///     oTreemapGenerator.DrawItem +=
		///         new TreemapGenerator.TreemapDrawItemEventHandler(DrawItem);
		///
		///     // Populate the Nodes collection.
		///
		///     PopulateTreemap(oTreemapGenerator);
		///
		///     // Create a rectangle to draw within.
		///
		///     Rectangle oRectangle = new Rectangle(0, 0, 500, 400);
		///
		///     // Draw the treemap.  This causes the TreemapGenerator to compute
		///     // a rectangle for each Node object, then call DrawItem for each
		///     // Node.
		///
		///     oTreemapGenerator.Draw(oRectangle);
		/// }
		///
		/// protected void
		/// PopulateTreemap(TreemapGenerator oTreemapGenerator)
		/// {
		///     Nodes oNodes;
		///     Node oNode;
		///     Nodes oChildNodes;
		///     Node oChildNode;
		///
		///     // Get the collection of top-level nodes.
		///
		///     oNodes = oTreemapGenerator.Nodes;
		///
		///     // Add a top-level node to the collection.  The node's Tag property,
		///     // which can hold anything, is set to an arbitrary string here.
		///
		///     oNode = oNodes.Add("Top Level 1", 25F, 100F,
		///         "TagForTopLevel1");
		///
		///     // Add child nodes to the top-level node.
		///
		///     oChildNodes = oNode.Nodes;
		///     oChildNode = oChildNodes.Add("Child 1-1", 90F, 2.5F,
		///         "TagForChild11");
		///     oChildNode = oChildNodes.Add("Child 1-2", 10F, -34.5F,
		///         "TagForChild12");
		///
		///     // Add another top-level node.
		///
		///     oNode = oNodes.Add("Top Level 2", 50F, -40.1F,
		///         "TagForTopLevel2");
		///
		///     // Add child nodes to the second top-level node.
		///
		///     oChildNodes = oNode.Nodes;
		///     oChildNode = oChildNodes.Add("Child 2-1", 61F, 0F,
		///         "TagForChild21");
		///     oChildNode = oChildNodes.Add("Child 2-2", 100F, 200F,
		///         "TagForChild22");
		/// }
		///
		/// protected void
		/// DrawItem(Object sender, TreemapDrawItemEventArgs e)
		/// {
		///     // Get the Node object that needs to be drawn.
		///
		///     Node oNode = e.Node;
		///
		///     // The owner-draw code can do anything with the node -- create SVG,
		///     // write to a PDF file, save to some private binary format, or
		///     // whatever it wants.  In this example, we'll just write
		///     // information about the node to a log file.
		///
		///     WriteToLog("Text: " + oNode.Text);
		///     WriteToLog("Tag: " + oNode.Tag);
		///     WriteToLog("ColorMetric: " + oNode.ColorMetric);
		///     WriteToLog("Bounds: " + e.Bounds);
		///     WriteToLog("Pen width: " + e.PenWidthPx);
		///     WriteToLog("--------------------");
		/// }
		///
		/// </code>
		///
		/// <para>
		/// Running this code produces the following log file:
		///
		/// <code>
		///
		/// Text: Top Level 1
		/// Tag: TagForTopLevel1
		/// ColorMetric: 100
		/// Bounds: {X=335,Y=5,Width=161,Height=391}
		/// Pen width: 3
		/// --------------------
		/// Text: Child 1-1
		/// Tag: TagForChild11
		/// ColorMetric: 2.5
		/// Bounds: {X=342,Y=52,Width=147,Height=337}
		/// Pen width: 2
		/// --------------------
		/// Text: Child 1-2
		/// Tag: TagForChild12
		/// ColorMetric: -34.5
		/// Bounds: {X=342,Y=12,Width=147,Height=36}
		/// Pen width: 2
		/// --------------------
		/// Text: Top Level 2
		/// Tag: TagForTopLevel2
		/// ColorMetric: -40.1
		/// Bounds: {X=5,Y=5,Width=325,Height=391}
		/// Pen width: 3
		/// --------------------
		/// Text: Child 2-1
		/// Tag: TagForChild21
		/// ColorMetric: 0
		/// Bounds: {X=12,Y=12,Width=311,Height=141}
		/// Pen width: 2
		/// --------------------
		/// Text: Child 2-2
		/// Tag: TagForChild22
		/// ColorMetric: 200
		/// Bounds: {X=12,Y=157,Width=311,Height=232}
		/// Pen width: 2
		/// --------------------
		///
		/// </code>
		///
		/// </para>
		///
		/// </example>
		public event TreemapDrawItemEventHandler DrawItem;

		/// <summary>
		/// Occurs when the treemap needs to be redrawn.
		/// </summary>
		///
		/// <remarks>
		/// This event occurs when a property that affects the treemap's appearance
		/// is changed, and when a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object is added to the
		/// treemap.  It does not occur between calls to <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.BeginUpdate" />
		/// and <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.EndUpdate" />.
		/// </remarks>
		public event EventHandler RedrawRequired;

		/// <summary>
		/// Initializes a new instance of the TreemapGenerator class.
		/// </summary>
		public TreemapGenerator()
		{
			m_oNodes = new Nodes(null);
			m_oNodes.TreemapGenerator = this;
			m_iPaddingPx = 5;
			m_iPaddingDecrementPerLevelPx = 1;
			m_iPenWidthPx = 3;
			m_iPenWidthDecrementPerLevelPx = 1;
			m_oBackColor = SystemColors.Window;
			m_oBorderColor = SystemColors.WindowFrame;
			m_eNodeColorAlgorithm = NodeColorAlgorithm.UseColorMetric;
			m_oMinColor = Color.Red;
			m_oMaxColor = Color.Green;
			m_fMinColorMetric = -100f;
			m_fMaxColorMetric = 100f;
			m_iDiscretePositiveColors = 20;
			m_iDiscreteNegativeColors = 20;
			m_sFontFamily = "Arial";
			m_fFontMinSizePt = 8f;
			m_fFontMaxSizePt = 100f;
			m_fFontIncrementPt = 2f;
			m_oFontSolidColor = SystemColors.WindowText;
			m_iFontMinAlpha = 105;
			m_iFontMaxAlpha = 255;
			m_iFontAlphaIncrementPerLevel = 50;
			m_oSelectedFontColor = SystemColors.HighlightText;
			m_oSelectedBackColor = SystemColors.Highlight;
			m_iNodeLevelsWithText = NodeLevelsWithText.All;
			m_iMinNodeLevelWithText = 0;
			m_iMaxNodeLevelWithText = 999;
			m_eTextLocation = TextLocation.Top;
			m_eEmptySpaceLocation = EmptySpaceLocation.DeterminedByLayoutAlgorithm;
			m_oSelectedNode = null;
			m_oSavedSelectedNodeBitmap = null;
			m_bInBeginUpdate = false;
			m_eLayoutAlgorithm = LayoutAlgorithm.BottomWeightedSquarified;
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
		/// The range returned by this method is used only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.NodeLevelsWithText" /> is set to Range.
		/// </remarks>
		public void GetNodeLevelsWithTextRange(out int minLevel, out int maxLevel)
		{
			AssertValid();
			minLevel = m_iMinNodeLevelWithText;
			maxLevel = m_iMaxNodeLevelWithText;
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
		/// The range specified in this method is used only if <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.NodeLevelsWithText" /> is set to Range.  The default minimum and
		/// maximum values are 0 and 999.
		/// </remarks>
		public void SetNodeLevelsWithTextRange(int minLevel, int maxLevel)
		{
			if (minLevel < 0)
			{
				throw new ArgumentOutOfRangeException("minLevel", minLevel, "TreemapGenerator.SetNodeLevelsWithTextRange: Must be >= 0.");
			}
			if (maxLevel < 0)
			{
				throw new ArgumentOutOfRangeException("maxLevel", maxLevel, "TreemapGenerator.SetNodeLevelsWithTextRange: Must be >= 0.");
			}
			if (maxLevel < minLevel)
			{
				throw new ArgumentOutOfRangeException("maxLevel", maxLevel, "TreemapGenerator.SetNodeLevelsWithTextRange: Must be >= minLevel.");
			}
			m_iMinNodeLevelWithText = minLevel;
			m_iMaxNodeLevelWithText = maxLevel;
			FireRedrawRequired();
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
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> draws node text using a range of
		/// font sizes.  For each node, the largest font in the range that won't
		/// exceed the bounds of the node's rectangle is used.
		///
		/// <para>
		/// By default, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> uses
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
			minSizePt = m_fFontMinSizePt;
			maxSizePt = m_fFontMaxSizePt;
			incrementPt = m_fFontIncrementPt;
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
		/// The TreemapGenerator draws node text using a range of font sizes.  For
		/// each node, the largest font in the range that won't exceed the bounds
		/// of the node's rectangle is used.
		///
		/// <para>
		/// By default, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> uses
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
			MaximizingFontMapper.ValidateSizeRange(minSizePt, maxSizePt, incrementPt, "TreemapGenerator.SetFontSizeRange()");
			m_fFontMinSizePt = minSizePt;
			m_fFontMaxSizePt = maxSizePt;
			m_fFontIncrementPt = incrementPt;
			FireRedrawRequired();
			AssertValid();
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
		/// To improve text legibility, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> varies
		/// the transparency of node text.  The text for higher-level nodes is more
		/// transparent than the text for lower-level nodes, so the lower-level
		/// text shows through the higher-level text.
		///
		/// <para>
		/// Alpha values range from 0 (transparent) to 255 (opaque).
		/// </para>
		///
		/// <para>
		/// By default, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> uses
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
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.SetFontAlphaRange(System.Int32,System.Int32,System.Int32)" />
		public void GetFontAlphaRange(out int minAlpha, out int maxAlpha, out int incrementPerLevel)
		{
			AssertValid();
			minAlpha = m_iFontMinAlpha;
			maxAlpha = m_iFontMaxAlpha;
			incrementPerLevel = m_iFontAlphaIncrementPerLevel;
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
		/// To improve text legibility, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> varies
		/// the transparency of node text.  The text for higher-level nodes is more
		/// transparent than the text for lower-level nodes, so the lower-level
		/// text shows through the higher-level text.
		///
		/// <para>
		/// Alpha values range from 0 (transparent) to 255 (opaque).
		/// </para>
		///
		/// <para>
		/// By default, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator" /> uses
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
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.GetFontAlphaRange(System.Int32@,System.Int32@,System.Int32@)" />
		public void SetFontAlphaRange(int minAlpha, int maxAlpha, int incrementPerLevel)
		{
			TransparentBrushMapper.ValidateAlphaRange(minAlpha, maxAlpha, incrementPerLevel, "TreemapGenerator.SetFontAlphaRange");
			m_iFontMinAlpha = minAlpha;
			m_iFontMaxAlpha = maxAlpha;
			m_iFontAlphaIncrementPerLevel = incrementPerLevel;
			FireRedrawRequired();
			AssertValid();
		}

		/// <summary>
		/// Disables any redrawing of the treemap.
		/// </summary>
		///
		/// <remarks>
		/// To improve performance, call BeginUpdate before adding <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects to the treemap.  Call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.EndUpdate" />
		/// when you are done.
		/// </remarks>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.EndUpdate" />
		public void BeginUpdate()
		{
			AssertValid();
			m_bInBeginUpdate = true;
		}

		/// <summary>
		/// Enables the redrawing of the treemap.
		/// </summary>
		///
		/// <remarks>
		/// To improve performance, call <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.BeginUpdate" /> before
		/// adding <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects to the treemap.  Call EndUpdate when
		/// you are done.
		/// </remarks>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.BeginUpdate" />
		public void EndUpdate()
		{
			AssertValid();
			m_bInBeginUpdate = false;
			FireRedrawRequired();
		}

		/// <summary>
		/// Removes all nodes from the treemap.
		/// </summary>
		public void Clear()
		{
			m_oNodes.Clear();
			CancelSelectedNode();
			FireRedrawRequired();
			AssertValid();
		}

		/// <overloads>
		/// Draws the treemap.
		/// </overloads>
		///
		/// <summary>
		/// Draws the treemap onto the entire rectangle of a <see cref="T:System.Drawing.Bitmap" />.
		/// </summary>
		///
		/// <param name="bitmap">
		/// <see cref="T:System.Drawing.Bitmap" /> to draw onto.
		/// </param>
		///
		/// <param name="drawSelection">
		/// If true and there is a selected node, that node is drawn as selected.
		/// </param>
		///
		/// <remarks>
		/// To print the treemap by drawing onto a <see cref="T:System.Drawing.Graphics" /> object,
		/// use Draw(Graphics, Rectangle) instead.
		/// </remarks>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.SelectNode(Microsoft.Research.CommunityTechnologies.Treemap.Node,System.Drawing.Bitmap)" />
		public void Draw(Bitmap bitmap, bool drawSelection)
		{
			Debug.Assert(bitmap != null);
			AssertValid();
			Rectangle treemapRectangle = Rectangle.FromLTRB(0, 0, bitmap.Width, bitmap.Height);
			Draw(bitmap, drawSelection, treemapRectangle);
		}

		/// <summary>
		/// Draws the treemap onto a specified rectangle of a
		/// <see cref="T:System.Drawing.Bitmap" />.
		/// </summary>
		///
		/// <param name="bitmap">
		/// <see cref="T:System.Drawing.Bitmap" /> to draw onto.
		/// </param>
		///
		/// <param name="drawSelection">
		/// If true and there is a selected node, that node is drawn as selected.
		/// </param>
		///
		/// <param name="treemapRectangle">
		/// Rectangle to draw onto.  This must be contained within the bitmap
		/// rectangle.
		/// </param>
		///
		/// <remarks>
		/// To print the treemap by drawing onto a <see cref="T:System.Drawing.Graphics" /> object,
		/// use Draw(Graphics, Rectangle) instead.
		/// </remarks>
		///
		/// <seealso cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.SelectNode(Microsoft.Research.CommunityTechnologies.Treemap.Node,System.Drawing.Bitmap)" />
		public void Draw(Bitmap bitmap, bool drawSelection, Rectangle treemapRectangle)
		{
			Debug.Assert(bitmap != null);
			AssertValid();
			if (!Rectangle.FromLTRB(0, 0, bitmap.Width, bitmap.Height).Contains(treemapRectangle))
			{
				throw new ArgumentException("TreemapGenerator.Draw(): treemapRectangle is not contained within the bitmap.");
			}
			Node oSelectedNode = m_oSelectedNode;
			CancelSelectedNode();
			Graphics graphics = Graphics.FromImage(bitmap);
			Draw(graphics, treemapRectangle);
			graphics.Dispose();
			if (drawSelection && oSelectedNode != null)
			{
				SelectNode(oSelectedNode, bitmap);
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
		/// This method can be used to print the treemap.  To draw the treemap onto
		/// a <see cref="T:System.Drawing.Bitmap" /> instead, use Draw(Bitmap, Boolean).
		/// </para>
		///
		/// </remarks>
		public void Draw(Graphics graphics, Rectangle treemapRectangle)
		{
			Debug.Assert(graphics != null);
			Debug.Assert(!treemapRectangle.IsEmpty);
			AssertValid();
			CalculateAndDrawRectangles(graphics, treemapRectangle, m_oNodes, null);
			if (m_iNodeLevelsWithText != NodeLevelsWithText.None)
			{
				DrawText(graphics, treemapRectangle, m_oNodes);
			}
		}

		/// <summary>
		/// Draws the treemap using owner-implemented code.
		/// </summary>
		///
		/// <param name="treemapRectangle">
		/// Rectangle to draw onto.
		/// </param>
		///
		/// <remarks>
		/// If you want to draw the treemap yourself, call this method and handle
		/// the <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DrawItem" /> event.  <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DrawItem" /> will get
		/// called once for each of the TreemapGenerator's <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		/// objects.  The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.TreemapDrawItemEventArgs" /> object passed to
		/// the event handler includes a reference to the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		/// object to draw, the rectangle to draw the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> within,
		/// and other drawing information.
		///
		/// <para>
		/// This method does not use a <see cref="T:System.Drawing.Bitmap" /> or
		/// <see cref="T:System.Drawing.Graphics" /> object, and the <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DrawItem" /> handler
		/// does not have to draw anything in graphical sense.  It can create XML,
		/// write to a binary file, or do anything else with the drawing
		/// information passed to it.
		/// </para>
		///
		/// <para>
		/// See <see cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DrawItem" /> for an example of owner-draw code.
		/// </para>
		///
		/// </remarks>
		///
		/// <seealso cref="E:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DrawItem" />
		public void Draw(Rectangle treemapRectangle)
		{
			AssertValid();
			if (this.DrawItem == null)
			{
				throw new InvalidOperationException("TreemapGenerator.Draw: The Draw(Rectangle) method initiates owner draw, which requires that the DrawItem event be handled.  The DrawItem event is not being handled.");
			}
			ILayoutEngine oLayoutEngine = CreateLayoutEngine();
			CalculateRectangles(m_oNodes, treemapRectangle, null, m_iPaddingPx, m_iPaddingPx, m_iPenWidthPx, oLayoutEngine);
			DrawNodesByOwnerDraw(m_oNodes);
		}

		/// <overloads>
		/// Gets the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object containing a specified point.
		/// </overloads>
		///
		/// <summary>
		/// Gets the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object containing a specified PointF.
		/// </summary>
		///
		/// <param name="pointF">
		/// Point to get a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object for.
		/// </param>
		///
		/// <param name="node">
		/// Where the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object gets stored.
		/// </param>
		///
		/// <returns>
		/// true if a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object was found, false if not.
		/// </returns>
		///
		/// <remarks>
		/// Looks for the innermost node whose rectangle contains the specified
		/// point.  If found, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object is stored in
		/// <paramref name="node" /> and true is returned.  false is returned
		/// otherwise.
		///
		/// <para>
		/// <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Draw(System.Drawing.Bitmap,System.Boolean)" /> should be called before this
		/// method is used.  If it hasn't been called yet, false is returned.
		/// </para>
		///
		/// </remarks>
		public bool GetNodeFromPoint(PointF pointF, out Node node)
		{
			return m_oNodes.GetNodeFromPoint(pointF, out node);
		}

		/// <summary>
		/// Gets the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object containing a specified coordinate
		/// pair.
		/// </summary>
		///
		/// <param name="x">
		/// X-coordinate of the point to get a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object for.
		/// </param>
		///
		/// <param name="y">
		/// Y-coordinate of the point to get a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object for.
		/// </param>
		///
		/// <param name="node">
		/// Where the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object gets stored.
		/// </param>
		///
		/// <returns>
		/// true if a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object was found, false if not.
		/// </returns>
		///
		/// <remarks>
		/// Looks for the innermost node whose rectangle contains the specified
		/// coordinates.  If found, the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object is stored in
		/// <paramref name="node" /> and true is returned.  false is returned
		/// otherwise.
		///
		/// <para>
		/// <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Draw(System.Drawing.Bitmap,System.Boolean)" /> should be called before this
		/// method is used.  If it hasn't been called yet, false is returned.
		/// </para>
		///
		/// </remarks>
		public bool GetNodeFromPoint(int x, int y, out Node node)
		{
			return GetNodeFromPoint(new PointF(x, y), out node);
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
		/// <param name="bitmap">
		/// If <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Draw(System.Drawing.Bitmap,System.Boolean)" /> has already been called, this
		/// should be the same Bitmap that was passed to <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Draw(System.Drawing.Bitmap,System.Boolean)" />.  Otherwise, specify null.  <paramref name="node" /> will then be drawn as selected the first time <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Draw(System.Drawing.Bitmap,System.Boolean)" /> is called.
		/// </param>
		///
		/// <remarks>
		/// This redraws the specified node's rectangle and text to show it as
		/// selected.  If another node was already selected, it gets redrawn as
		/// unselected.
		/// </remarks>
		public void SelectNode(Node node, Bitmap bitmap)
		{
			if (node != null)
			{
				node.AssertValid();
				if (node == m_oSelectedNode)
				{
					return;
				}
			}
			if (bitmap != null)
			{
				Graphics graphics = Graphics.FromImage(bitmap);
				if (m_oSelectedNode != null && m_oSavedSelectedNodeBitmap != null)
				{
					m_oSelectedNode.AssertValid();
					Debug.Assert(!m_oSelectedNode.Rectangle.IsEmpty);
					Debug.Assert(m_oSavedSelectedNodeBitmap != null);
					int penWidthPx = SetNodePenWidthForSelection(m_oSelectedNode);
					Rectangle rectangleToDraw = m_oSelectedNode.RectangleToDraw;
					graphics.DrawImage(m_oSavedSelectedNodeBitmap, rectangleToDraw.X, rectangleToDraw.Y);
					m_oSelectedNode.PenWidthPx = penWidthPx;
					CancelSelectedNode();
				}
				if (node != null && node.HasBeenDrawn)
				{
					Rectangle rectangleToDraw2 = node.RectangleToDraw;
					m_oSavedSelectedNodeBitmap = bitmap.Clone(Rectangle.FromLTRB(rectangleToDraw2.Left, rectangleToDraw2.Top, Math.Min(rectangleToDraw2.Right + 1, bitmap.Width), Math.Min(rectangleToDraw2.Bottom + 1, bitmap.Height)), bitmap.PixelFormat);
					DrawNodeAsSelected(node, graphics);
				}
				graphics.Dispose();
			}
			m_oSelectedNode = node;
		}

		/// <summary>
		/// CalculateAndDrawRectangles method.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// Graphics.  Object to draw onto.
		/// </param>
		///
		/// <param name="oTreemapRectangle">
		/// System.Drawing.RectangleF.  Outer rectangle for the entire treemap.
		/// </param>
		///
		/// <param name="oNodes">
		/// Nodes.  Collection of Node objects.
		/// </param>
		///
		/// <param name="oParentNode">
		/// Node.  Parent of oNodes, or null if oNodes are top-level.
		/// </param>
		///
		/// <remarks>
		/// This method calculates and draws the rectangles for the nodes in
		/// oNodes and all their descendents.
		/// </remarks>
		protected void CalculateAndDrawRectangles(Graphics oGraphics, RectangleF oTreemapRectangle, Nodes oNodes, Node oParentNode)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oNodes != null);
			AssertValid();
			Brush brush = new SolidBrush(m_oBackColor);
			oGraphics.FillRectangle(brush, oTreemapRectangle);
			brush.Dispose();
			ILayoutEngine oLayoutEngine = CreateLayoutEngine();
			CalculateRectangles(oNodes, oTreemapRectangle, oParentNode, GetTopLevelTopPaddingPx(oGraphics), m_iPaddingPx, m_iPenWidthPx, oLayoutEngine);
			ColorGradientMapper colorGradientMapper = null;
			ColorGradientMapper colorGradientMapper2 = null;
			if (m_eNodeColorAlgorithm == NodeColorAlgorithm.UseColorMetric)
			{
				colorGradientMapper = new ColorGradientMapper();
				Debug.Assert(m_fMaxColorMetric > 0f);
				colorGradientMapper.Initialize(oGraphics, 0f, m_fMaxColorMetric, Color.White, m_oMaxColor, m_iDiscretePositiveColors, bCreateBrushes: true);
				colorGradientMapper2 = new ColorGradientMapper();
				Debug.Assert(m_fMinColorMetric < 0f);
				colorGradientMapper2.Initialize(oGraphics, 0f, 0f - m_fMinColorMetric, Color.White, m_oMinColor, m_iDiscreteNegativeColors, bCreateBrushes: true);
			}
			PenCache penCache = new PenCache();
			penCache.Initialize(m_oBorderColor);
			DrawRectangles(oNodes, 0, oGraphics, colorGradientMapper2, colorGradientMapper, penCache);
			colorGradientMapper2?.Dispose();
			colorGradientMapper?.Dispose();
			penCache.Dispose();
		}

		protected void CalculateRectangles(Nodes oNodes, RectangleF oParentRectangle, Node oParentNode, int iTopPaddingPx, int iLeftRightBottomPaddingPx, int iPenWidthPx, ILayoutEngine oLayoutEngine)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(iTopPaddingPx > 0);
			Debug.Assert(iLeftRightBottomPaddingPx > 0);
			Debug.Assert(iPenWidthPx > 0);
			Debug.Assert(oLayoutEngine != null);
			AssertValid();
			int num = iTopPaddingPx;
			if (oParentNode == null)
			{
				iTopPaddingPx = iLeftRightBottomPaddingPx;
			}
			if (!AddPaddingToParentRectangle(ref oParentRectangle, ref iTopPaddingPx, ref iLeftRightBottomPaddingPx))
			{
				oLayoutEngine.SetNodeRectanglesToEmpty(oNodes, bRecursive: true);
				return;
			}
			if (oParentNode == null)
			{
				iTopPaddingPx = num;
			}
			oLayoutEngine.CalculateNodeRectangles(oNodes, oParentRectangle, oParentNode, m_eEmptySpaceLocation);
			int num2 = DecrementPadding(iLeftRightBottomPaddingPx);
			int iPenWidthPx2 = DecrementPenWidth(iPenWidthPx);
			int iTopPaddingPx2 = 0;
			switch (m_eTextLocation)
			{
			case TextLocation.CenterCenter:
				iTopPaddingPx2 = num2;
				break;
			case TextLocation.Top:
				iTopPaddingPx2 = iTopPaddingPx;
				break;
			default:
				Debug.Assert(condition: false);
				break;
			}
			foreach (Node oNode in oNodes)
			{
				if (!oNode.Rectangle.IsEmpty)
				{
					RectangleF oChildRectangle = oNode.Rectangle;
					if (!AddPaddingToChildRectangle(ref oChildRectangle, oParentRectangle, iLeftRightBottomPaddingPx))
					{
						oNode.Rectangle = FixSmallRectangle(oNode.Rectangle);
						oNode.PenWidthPx = 1;
						oLayoutEngine.SetNodeRectanglesToEmpty(oNode.Nodes, bRecursive: true);
					}
					else
					{
						oNode.Rectangle = oChildRectangle;
						oNode.PenWidthPx = iPenWidthPx;
						RectangleF oParentRectangle2 = RectangleF.Inflate(oChildRectangle, -iPenWidthPx, -iPenWidthPx);
						CalculateRectangles(oNode.Nodes, oParentRectangle2, oNode, iTopPaddingPx2, num2, iPenWidthPx2, oLayoutEngine);
					}
				}
			}
		}

		/// <summary>
		/// DrawRectangles method.
		/// </summary>
		///
		/// <param name="oNodes">
		/// Nodes.  Collection of Node objects.
		/// </param>
		///
		/// <param name="iNodeLevel">
		/// Int32.  Level of the nodes in oNodes.  Top-level nodes are level 0.
		/// </param>
		///
		/// <param name="oGraphics">
		/// System.Drawing.Graphics.  Object to draw onto.
		/// </param>
		///
		/// <param name="oNegativeColorGradientMapper">
		/// ColorGradientMapper.  Object that knows how to map negative color
		/// metric values to colors.
		/// </param>
		///
		/// <param name="oPositiveColorGradientMapper">
		/// ColorGradientMapper.  Object that knows how to map positive color
		/// metric values to colors.
		/// </param>
		///
		/// <param name="oPenCache">
		/// PenCache.  Object that creates and caches pens.
		/// </param>
		///
		/// <remarks>
		/// This method draws the rectangles for the nodes in oNodes and all their
		/// descendents.  It's assumed that the Rectangle and PenWidthPx properties
		/// have already been set on each Node object.
		/// </remarks>
		protected void DrawRectangles(Nodes oNodes, int iNodeLevel, Graphics oGraphics, ColorGradientMapper oNegativeColorGradientMapper, ColorGradientMapper oPositiveColorGradientMapper, PenCache oPenCache)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(iNodeLevel >= 0);
			Debug.Assert(oGraphics != null);
			Debug.Assert(m_eNodeColorAlgorithm != 0 || oNegativeColorGradientMapper != null);
			Debug.Assert(m_eNodeColorAlgorithm != 0 || oPositiveColorGradientMapper != null);
			Debug.Assert(oPenCache != null);
			foreach (Node oNode in oNodes)
			{
				if (!oNode.Rectangle.IsEmpty)
				{
					Pen pen = oPenCache.GetPen(oNode.PenWidthPx);
					Brush brush = null;
					bool flag = false;
					switch (m_eNodeColorAlgorithm)
					{
					case NodeColorAlgorithm.UseColorMetric:
					{
						Debug.Assert(oNegativeColorGradientMapper != null);
						Debug.Assert(oPositiveColorGradientMapper != null);
						float colorMetric = oNode.ColorMetric;
						brush = ((!(colorMetric <= 0f)) ? oPositiveColorGradientMapper.ColorMetricToBrush(colorMetric) : oNegativeColorGradientMapper.ColorMetricToBrush(0f - colorMetric));
						break;
					}
					case NodeColorAlgorithm.UseAbsoluteColor:
						brush = new SolidBrush(oNode.AbsoluteColor);
						flag = true;
						break;
					default:
						Debug.Assert(condition: false);
						break;
					}
					Debug.Assert(brush != null);
					Rectangle rectangleToDraw = oNode.RectangleToDraw;
					oGraphics.FillRectangle(brush, rectangleToDraw);
					oGraphics.DrawRectangle(pen, rectangleToDraw);
					if (flag)
					{
						brush.Dispose();
					}
					DrawRectangles(oNode.Nodes, iNodeLevel + 1, oGraphics, oNegativeColorGradientMapper, oPositiveColorGradientMapper, oPenCache);
				}
			}
		}

		/// <summary>
		/// DrawText method.
		/// </summary>
		///
		/// <param name="oGraphics">
		/// System.Drawing.Graphics.  Object to draw onto.
		/// </param>
		///
		/// <param name="oTreemapRectangle">
		/// System.Drawing.Rectangle.  Outer rectangle for the entire treemap.
		/// </param>
		///
		/// <param name="oNodes">
		/// Nodes.  Collection of Node objects.
		/// </param>
		///
		/// <remarks>
		/// This method draws the text for the nodes in oNodes and all their
		/// descendents.
		/// </remarks>
		protected void DrawText(Graphics oGraphics, Rectangle oTreemapRectangle, Nodes oNodes)
		{
			AssertValid();
			ITextDrawer textDrawer = CreateTextDrawer();
			textDrawer.DrawTextForAllNodes(oGraphics, oTreemapRectangle, oNodes);
		}

		/// <summary>
		/// DrawNodesByOwnerDraw method.
		/// </summary>
		///
		/// <param name="oNodes">
		/// Nodes.  Collection of Node objects.
		/// </param>
		///
		/// <remarks>
		/// This method draws the nodes in oNodes and all their descendents by
		/// firing the DrawItem event.  It's assumed that the Rectangle and
		/// PenWidthPx properties have already been set on each Node object.
		/// Also, the DrawItem delegate must not be null.
		/// </remarks>
		protected void DrawNodesByOwnerDraw(Nodes oNodes)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(this.DrawItem != null);
			AssertValid();
			foreach (Node oNode in oNodes)
			{
				if (!oNode.Rectangle.IsEmpty)
				{
					TreemapDrawItemEventArgs treemapDrawItemEventArgs = new TreemapDrawItemEventArgs(oNode);
					this.DrawItem(this, treemapDrawItemEventArgs);
					DrawNodesByOwnerDraw(oNode.Nodes);
				}
			}
		}

		/// <summary>
		/// DrawNodeAsSelected method.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node.  The node to draw as selected.
		/// </param>
		///
		/// <param name="oGraphics">
		/// Graphics.  Object to draw onto.
		/// </param>
		///
		/// 
		///
		/// <remarks>
		/// Redraws a node's rectangle and text to show it as selected.
		/// </remarks>
		protected void DrawNodeAsSelected(Node oNode, Graphics oGraphics)
		{
			Debug.Assert(oNode != null);
			oNode.AssertValid();
			Debug.Assert(oGraphics != null);
			int penWidthPx = SetNodePenWidthForSelection(oNode);
			Brush brush = new SolidBrush(m_oSelectedBackColor);
			Pen pen = new Pen(brush, oNode.PenWidthPx);
			pen.Alignment = PenAlignment.Inset;
			oGraphics.DrawRectangle(pen, oNode.RectangleToDraw);
			pen.Dispose();
			brush.Dispose();
			oNode.PenWidthPx = penWidthPx;
			ITextDrawer textDrawer = CreateTextDrawer();
			textDrawer.DrawTextForSelectedNode(oGraphics, oNode);
		}

		/// <summary>
		/// Fires the RedrawRequired event if appropriate.
		/// </summary>
		///
		/// <remarks>
		/// This should be called when something occurs that affects the treemap's
		/// appearance.
		/// </remarks>
		protected internal void FireRedrawRequired()
		{
			if (this.RedrawRequired != null && !m_bInBeginUpdate)
			{
				this.RedrawRequired(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// CancelSelectedNode method.
		/// </summary>
		///
		/// <remarks>
		/// Cancels the selected node.  The treemap is not redrawn.
		/// </remarks>
		protected void CancelSelectedNode()
		{
			m_oSelectedNode = null;
			if (m_oSavedSelectedNodeBitmap != null)
			{
				m_oSavedSelectedNodeBitmap.Dispose();
				m_oSavedSelectedNodeBitmap = null;
			}
		}

		protected ILayoutEngine CreateLayoutEngine()
		{
			switch (m_eLayoutAlgorithm)
			{
			case LayoutAlgorithm.BottomWeightedSquarified:
				return new BottomWeightedSquarifiedLayoutEngine();
			case LayoutAlgorithm.TopWeightedSquarified:
				return new TopWeightedSquarifiedLayoutEngine();
			default:
				Debug.Assert(condition: false);
				return null;
			}
		}

		protected ITextDrawer CreateTextDrawer()
		{
			AssertValid();
			switch (m_eTextLocation)
			{
			case TextLocation.CenterCenter:
				return new CenterCenterTextDrawer(m_iNodeLevelsWithText, m_iMinNodeLevelWithText, m_iMaxNodeLevelWithText, m_sFontFamily, m_fFontMinSizePt, m_fFontMaxSizePt, m_fFontIncrementPt, m_oFontSolidColor, m_iFontMinAlpha, m_iFontMaxAlpha, m_iFontAlphaIncrementPerLevel, m_oSelectedFontColor);
			case TextLocation.Top:
				return new TopTextDrawer(m_iNodeLevelsWithText, m_iMinNodeLevelWithText, m_iMaxNodeLevelWithText, m_sFontFamily, m_fFontMinSizePt, GetTopMinimumTextHeight(), m_oFontSolidColor, m_oSelectedFontColor, m_oSelectedBackColor);
			default:
				Debug.Assert(condition: false);
				return null;
			}
		}

		protected bool AddPaddingToParentRectangle(ref RectangleF oParentRectangle, ref int iTopPaddingPx, ref int iLeftRightBottomPaddingPx)
		{
			Debug.Assert(iTopPaddingPx >= 0);
			Debug.Assert(iLeftRightBottomPaddingPx >= 0);
			int num = iTopPaddingPx;
			int num2 = iLeftRightBottomPaddingPx;
			RectangleF rectangleF = AddPaddingToRectangle(oParentRectangle, num, num2);
			if (RectangleIsSmallerThanMin(rectangleF))
			{
				return false;
			}
			oParentRectangle = rectangleF;
			iTopPaddingPx = num;
			iLeftRightBottomPaddingPx = num2;
			return true;
		}

		protected RectangleF AddPaddingToRectangle(RectangleF oRectangle, int iTopPaddingPx, int iLeftRightBottomPaddingPx)
		{
			Debug.Assert(iTopPaddingPx >= 0);
			Debug.Assert(iLeftRightBottomPaddingPx >= 0);
			return RectangleF.FromLTRB(oRectangle.Left + (float)iLeftRightBottomPaddingPx, oRectangle.Top + (float)iTopPaddingPx, oRectangle.Right - (float)iLeftRightBottomPaddingPx, oRectangle.Bottom - (float)iLeftRightBottomPaddingPx);
		}

		protected bool AddPaddingToChildRectangle(ref RectangleF oChildRectangle, RectangleF oParentRectangle, int iPaddingPx)
		{
			RectangleF rectangleF = AddPaddingToChildRectangle(oChildRectangle, oParentRectangle, iPaddingPx);
			if (RectangleIsSmallerThanMin(rectangleF))
			{
				if (iPaddingPx > 1)
				{
					rectangleF = AddPaddingToChildRectangle(oChildRectangle, oParentRectangle, 1);
				}
				if (RectangleIsSmallerThanMin(rectangleF))
				{
					return false;
				}
			}
			oChildRectangle = rectangleF;
			return true;
		}

		protected RectangleF AddPaddingToChildRectangle(RectangleF oChildRectangle, RectangleF oParentRectangle, int iPaddingPx)
		{
			float num = oChildRectangle.Left;
			float num2 = oChildRectangle.Top;
			float num3 = oChildRectangle.Right;
			float num4 = oChildRectangle.Bottom;
			float num5 = (float)(iPaddingPx + 1) / 2f;
			if (Math.Round(oChildRectangle.Left) != Math.Round(oParentRectangle.Left))
			{
				num += num5;
			}
			if (Math.Round(oChildRectangle.Top) != Math.Round(oParentRectangle.Top))
			{
				num2 += num5;
			}
			if (Math.Round(oChildRectangle.Right) != Math.Round(oParentRectangle.Right))
			{
				num3 -= num5;
			}
			if (Math.Round(oChildRectangle.Bottom) != Math.Round(oParentRectangle.Bottom))
			{
				num4 -= num5;
			}
			return RectangleF.FromLTRB(num, num2, num3, num4);
		}

		protected int GetTopLevelTopPaddingPx(Graphics oGraphics)
		{
			AssertValid();
			Debug.Assert(oGraphics != null);
			switch (m_eTextLocation)
			{
			case TextLocation.CenterCenter:
				return m_iPaddingPx;
			case TextLocation.Top:
				return TopTextDrawer.GetTextHeight(oGraphics, m_sFontFamily, m_fFontMinSizePt, GetTopMinimumTextHeight());
			default:
				Debug.Assert(condition: false);
				return -1;
			}
		}

		protected int GetTopMinimumTextHeight()
		{
			AssertValid();
			return DecrementPadding(m_iPaddingPx);
		}

		/// <summary>
		/// DecrementPadding method.
		/// </summary>
		///
		/// <param name="iPaddingPx">
		/// Int32.  Padding to decrement, in pixels.
		/// </param>
		///
		/// <returns>
		/// Int32.  Decremented padding, in pixels.
		/// </returns>
		///
		/// <remarks>
		/// This method decrements the padding that gets added to the rectangles
		/// for the Node objects in a Nodes collection.  The padding can be made to
		/// decrease at each node level by setting m_iPaddingDecrementPerLevelPx to
		/// a positive value.
		/// </remarks>
		protected int DecrementPadding(int iPaddingPx)
		{
			return Math.Max(iPaddingPx - m_iPaddingDecrementPerLevelPx, 1);
		}

		/// <summary>
		/// DecrementPenWidth method.
		/// </summary>
		///
		/// <param name="iPenWidthPx">
		/// Int32.  Pen width to decrement, in pixels.
		/// </param>
		///
		/// <returns>
		/// Int32.  Decremented pen width, in pixels.
		/// </returns>
		///
		/// <remarks>
		/// This method decrements the pen width that is used to draw the
		/// rectangles for the Node objects in a Nodes collection.  The padding can
		/// be made to decrease at each node level by setting
		/// m_iPenWidthDecrementPerLevelPx to a positive value.
		/// </remarks>
		protected int DecrementPenWidth(int iPenWidthPx)
		{
			return Math.Max(iPenWidthPx - m_iPenWidthDecrementPerLevelPx, 1);
		}

		/// <summary>
		/// RectangleIsSmallerThanMin method.
		/// </summary>
		///
		/// <param name="oRectangle">
		/// RectangleF.  Rectangle to test.
		/// </param>
		///
		/// <returns>
		/// Boolean.  true if the rectangle is smaller than the minimum.
		/// </returns>
		///
		/// <remarks>
		/// This method returns true if the rectangle's height or width is smaller
		/// than a specified minimum.
		/// </remarks>
		protected bool RectangleIsSmallerThanMin(RectangleF oRectangle)
		{
			return oRectangle.Width < 1f || oRectangle.Height < 1f;
		}

		/// <summary>
		/// FixSmallRectangle method.
		/// </summary>
		///
		/// <param name="oUnpaddedNodeRectangle">
		/// RectangleF.  Node's rectangle, without padding.
		/// </param>
		///
		/// <returns>
		/// Modified copy of oUnpaddedNodeRectangle.
		/// </returns>
		///
		/// <remarks>
		/// This method is called when it's determined that a node's rectangle with
		/// padding is too small to display as a rectangle.  It returns a modified
		/// rectangle to use for the node.
		/// </remarks>
		protected RectangleF FixSmallRectangle(RectangleF oUnpaddedNodeRectangle)
		{
			float num = oUnpaddedNodeRectangle.Left;
			float num2 = oUnpaddedNodeRectangle.Top;
			float num3 = oUnpaddedNodeRectangle.Right;
			float num4 = oUnpaddedNodeRectangle.Bottom;
			float width = oUnpaddedNodeRectangle.Width;
			float height = oUnpaddedNodeRectangle.Height;
			float num5 = 0.5f;
			if (height < 1f)
			{
				num2 = (float)((double)(num2 + num4) / 2.0 - (double)num5);
				num4 = num2 + 1f;
			}
			if (width < 1f)
			{
				num = (num + num3) / 2f - num5;
				num3 = num + 1f;
			}
			RectangleF result = RectangleF.FromLTRB(num, num2, num3, num4);
			if (height < 1f)
			{
				Debug.Assert(Math.Round(result.Height) == 1.0);
			}
			if (width < 1f)
			{
				Debug.Assert(Math.Round(result.Width) == 1.0);
			}
			return result;
		}

		/// <summary>
		/// SetNodePenWidthForSelection method.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node.  Node whose pen width needs to be adjusted.
		/// </param>
		///
		/// <returns>
		/// Previous pen width, in pixels.
		/// </returns>
		///
		/// <remarks>
		/// This method sets the pen width of a Node object to a width appropriate
		/// for displaying the node's rectangle as selected.
		/// </remarks>
		protected int SetNodePenWidthForSelection(Node oNode)
		{
			Debug.Assert(oNode != null);
			AssertValid();
			int penWidthPx = oNode.PenWidthPx;
			int num = m_iPaddingPx;
			int level = oNode.Level;
			for (int i = 0; i < level + 1; i++)
			{
				num = DecrementPadding(num);
			}
			int num2 = penWidthPx + num;
			switch (m_eTextLocation)
			{
			case TextLocation.CenterCenter:
				num2 = Math.Max(num2, 4);
				break;
			default:
				Debug.Assert(condition: false);
				break;
			case TextLocation.Top:
				break;
			}
			oNode.PenWidthPx = num2;
			return penWidthPx;
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(m_oNodes != null);
			m_oNodes.AssertValid();
			Debug.Assert(m_iPaddingPx >= 1);
			Debug.Assert(m_iPaddingPx <= 100);
			Debug.Assert(m_iPaddingDecrementPerLevelPx >= 0);
			Debug.Assert(m_iPaddingDecrementPerLevelPx <= 99);
			Debug.Assert(m_iPenWidthPx >= 1);
			Debug.Assert(m_iPenWidthPx <= 100);
			Debug.Assert(m_iPenWidthDecrementPerLevelPx >= 0);
			Debug.Assert(m_iPenWidthDecrementPerLevelPx <= 99);
			Debug.Assert(Enum.IsDefined(typeof(NodeColorAlgorithm), m_eNodeColorAlgorithm));
			Debug.Assert(m_fMinColorMetric < 0f);
			Debug.Assert(m_fMaxColorMetric > 0f);
			Debug.Assert(m_iDiscretePositiveColors >= 2);
			Debug.Assert(m_iDiscretePositiveColors <= 50);
			Debug.Assert(m_iDiscreteNegativeColors >= 2);
			Debug.Assert(m_iDiscreteNegativeColors <= 50);
			StringUtil.AssertNotEmpty(m_sFontFamily);
			Debug.Assert(m_fFontMinSizePt > 0f);
			Debug.Assert(m_fFontMaxSizePt > 0f);
			Debug.Assert(m_fFontMaxSizePt >= m_fFontMinSizePt);
			Debug.Assert(m_fFontIncrementPt > 0f);
			Debug.Assert(m_oFontSolidColor.A == byte.MaxValue);
			Debug.Assert(m_iFontMinAlpha >= 0 && m_iFontMinAlpha <= 255);
			Debug.Assert(m_iFontMaxAlpha >= 0 && m_iFontMaxAlpha <= 255);
			Debug.Assert(m_iFontMaxAlpha >= m_iFontMinAlpha);
			Debug.Assert(m_iFontAlphaIncrementPerLevel > 0);
			Debug.Assert(Enum.IsDefined(typeof(NodeLevelsWithText), m_iNodeLevelsWithText));
			Debug.Assert(m_iMinNodeLevelWithText >= 0);
			Debug.Assert(m_iMaxNodeLevelWithText >= 0);
			Debug.Assert(m_iMaxNodeLevelWithText >= m_iMinNodeLevelWithText);
			Debug.Assert(Enum.IsDefined(typeof(TextLocation), m_eTextLocation));
			Debug.Assert(Enum.IsDefined(typeof(EmptySpaceLocation), m_eEmptySpaceLocation));
			if (m_oSelectedNode != null)
			{
				m_oSelectedNode.AssertValid();
			}
			Debug.Assert(Enum.IsDefined(typeof(LayoutAlgorithm), m_eLayoutAlgorithm));
		}
	}
}
