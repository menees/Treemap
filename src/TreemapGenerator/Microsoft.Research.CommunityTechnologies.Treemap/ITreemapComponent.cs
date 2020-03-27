using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	/// <summary>
	/// Public properties and methods shared by the TreemapGenerator and
	/// TreemapControl components.
	/// </summary>
	///
	/// <remarks>
	/// The public interfaces for the TreemapGenerator and TreemapControl
	/// components are nearly identical.  To make it easy for developers to write
	/// code that can work with either component, the components' shared properties
	/// and methods are defined in the ITreemapComponent interface.  Both
	/// components implement this interface.  Application code that needs to work
	/// with either component can then manipulate the ITreemapComponent interface,
	/// which can be obtained from either TreemapGenerator or TreemapControl.
	///
	/// <para>
	/// Application code that is meant to work with only one of the components does
	/// not need to concern itself with this interface.
	/// </para>
	///
	/// <para>
	/// The properties and methods in this interface are documented within the
	/// TreemapGenerator and TreemapControl components.  The complete documentation
	/// is not repeated here.
	/// </para>
	///
	/// </remarks>
	public interface ITreemapComponent
	{
		/// <summary>
		/// Gets the collection of top-level <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Nodes" />, the
		/// TreemapGenerator implementation of this property.  The property is also
		/// implemented by TreemapControl.
		/// </value>
		Nodes Nodes
		{
			get;
		}

		/// <summary>
		/// Gets or sets the entire node hierarchy as an XML string.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.NodesXml" />, the
		/// TreemapGenerator implementation of this property.  The property is also
		/// implemented by TreemapControl.
		/// </value>
		string NodesXml
		{
			get;
			set;
		}

		LayoutAlgorithm LayoutAlgorithm
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the padding that is added to the rectangles for top-level
		/// nodes.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingPx" />,
		/// the TreemapGenerator implementation of this property.  The property is
		/// also implemented by TreemapControl.
		/// </value>
		int PaddingPx
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the number of pixels that is subtracted from the padding
		/// at each node level.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PaddingDecrementPerLevelPx" />, the
		/// TreemapGenerator implementation of this property.  The property is also
		/// implemented by TreemapControl.
		/// </value>
		int PaddingDecrementPerLevelPx
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the pen width that is used to draw the rectangles for the
		/// top-level nodes.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthPx" />,
		/// the TreemapGenerator implementation of this property.  The property is
		/// also implemented by TreemapControl.
		/// </value>
		int PenWidthPx
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the number of pixels that is subtracted from the pen
		/// width at each node level.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.PenWidthDecrementPerLevelPx" />, the
		/// TreemapGenerator implementation of this property.  The property is also
		/// implemented by TreemapControl.
		/// </value>
		int PenWidthDecrementPerLevelPx
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the treemap's background color.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.BackColor" />,
		/// the TreemapGenerator implementation of this property.  The property is
		/// also implemented by TreemapControl.
		/// </value>
		Color BackColor
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the color of rectangle borders.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.BorderColor" />,
		/// the TreemapGenerator implementation of this property.  The property is
		/// also implemented by TreemapControl.
		/// </value>
		Color BorderColor
		{
			get;
			set;
		}

		NodeColorAlgorithm NodeColorAlgorithm
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the maximum negative fill color.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColor" />, the
		/// TreemapGenerator implementation of this property.  The property is also
		/// implemented by TreemapControl.
		/// </value>
		Color MinColor
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the maximum positive fill color.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColor" />, the
		/// TreemapGenerator implementation of this property.  The property is also
		/// implemented by TreemapControl.
		/// </value>
		Color MaxColor
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value to map to 
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent.MinColor" />.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MinColorMetric" />, the TreemapGenerator
		/// implementation of this property.  The property is also implemented by
		/// TreemapControl.
		/// </value>
		float MinColorMetric
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.ColorMetric" /> value to map to 
		/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent.MaxColor" />.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.MaxColorMetric" />, the TreemapGenerator
		/// implementation of this property.  The property is also implemented by
		/// TreemapControl.
		/// </value>
		float MaxColorMetric
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the number of discrete fill colors to use in the negative
		/// color range.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DiscreteNegativeColors" />, the TreemapGenerator
		/// implementation of this property.  The property is also implemented by
		/// TreemapControl.
		/// </value>
		int DiscreteNegativeColors
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the number of discrete fill colors to use in the positive
		/// color range.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.DiscretePositiveColors" />, the TreemapGenerator
		/// implementation of this property.  The property is also implemented by
		/// TreemapControl.
		/// </value>
		int DiscretePositiveColors
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the font family to use for drawing node text.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.FontFamily" />,
		/// the TreemapGenerator implementation of this property.  The property is
		/// also implemented by TreemapControl.
		/// </value>
		string FontFamily
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the color to use for node text.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.FontSolidColor" />, the TreemapGenerator
		/// implementation of this property.  The property is also implemented by
		/// TreemapControl.
		/// </value>
		Color FontSolidColor
		{
			get;
			set;
		}

		Color SelectedFontColor
		{
			get;
			set;
		}

		Color SelectedBackColor
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the node levels to show text for.
		/// </summary>
		///
		/// <value>
		/// For more information, see <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.NodeLevelsWithText" />, the TreemapGenerator
		/// implementation of this property.  The property is also implemented by
		/// TreemapControl.
		/// </value>
		NodeLevelsWithText NodeLevelsWithText
		{
			get;
			set;
		}

		TextLocation TextLocation
		{
			get;
			set;
		}

		EmptySpaceLocation EmptySpaceLocation
		{
			get;
			set;
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
		/// For more information, see <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.GetNodeLevelsWithTextRange(System.Int32@,System.Int32@)" />, the
		/// TreemapGenerator implementation of this method.  The method is also
		/// implemented by TreemapControl.
		/// </remarks>
		void GetNodeLevelsWithTextRange(out int minLevel, out int maxLevel);

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
		/// For more information, see <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.SetNodeLevelsWithTextRange(System.Int32,System.Int32)" />, the
		/// TreemapGenerator implementation of this method.  The method is also
		/// implemented by TreemapControl.
		/// </remarks>
		void SetNodeLevelsWithTextRange(int minLevel, int maxLevel);

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
		/// For more information, see <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.GetFontSizeRange(System.Single@,System.Single@,System.Single@)" />, the TreemapGenerator
		/// implementation of this method.  The method is also implemented by
		/// TreemapControl.
		/// </remarks>
		void GetFontSizeRange(out float minSizePt, out float maxSizePt, out float incrementPt);

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
		/// For more information, see <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.SetFontSizeRange(System.Single,System.Single,System.Single)" />, the TreemapGenerator
		/// implementation of this method.  The method is also implemented by
		/// TreemapControl.
		/// </remarks>
		void SetFontSizeRange(float minSizePt, float maxSizePt, float incrementPt);

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
		/// For more information, see <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.GetFontAlphaRange(System.Int32@,System.Int32@,System.Int32@)" />, the TreemapGenerator
		/// implementation of this method.  The method is also implemented by
		/// TreemapControl.
		/// </remarks>
		void GetFontAlphaRange(out int minAlpha, out int maxAlpha, out int incrementPerLevel);

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
		/// For more information, see <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.SetFontAlphaRange(System.Int32,System.Int32,System.Int32)" />, the TreemapGenerator
		/// implementation of this method.  The method is also implemented by
		/// TreemapControl.
		/// </remarks>
		void SetFontAlphaRange(int minAlpha, int maxAlpha, int incrementPerLevel);

		/// <summary>
		/// Disables any redrawing of the treemap.
		/// </summary>
		///
		/// <remarks>
		/// For more information, see <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.BeginUpdate" />, the TreemapGenerator
		/// implementation of this method.  The method is also implemented by
		/// TreemapControl.
		/// </remarks>
		void BeginUpdate();

		/// <summary>
		/// Enables the redrawing of the treemap.
		/// </summary>
		///
		/// <remarks>
		/// For more information, see <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.EndUpdate" />, the TreemapGenerator
		/// implementation of this method.  The method is also implemented by
		/// TreemapControl.
		/// </remarks>
		void EndUpdate();

		/// <summary>
		/// Removes all nodes from the treemap.
		/// </summary>
		///
		/// <remarks>
		/// For more information, see <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.TreemapGenerator.Clear" />, the
		/// TreemapGenerator implementation of this method.  The method is also
		/// implemented by TreemapControl.
		/// </remarks>
		void Clear();
	}
}
