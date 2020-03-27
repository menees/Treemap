namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	/// <summary>
	/// Specifies values that can be used for the
	/// <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent.NodeLevelsWithText" /> property.  This
	/// determines the node levels to show text for.
	/// </summary>
	///
	/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent.NodeLevelsWithText" />
	public enum NodeLevelsWithText
	{
		/// <summary>
		/// Show text for all node levels.
		/// </summary>
		All,
		/// <summary>
		/// Don't show any text.
		/// </summary>
		None,
		/// <summary>
		/// Show text for leaf nodes only.
		/// </summary>
		Leaves,
		/// <summary>
		/// Show text for the levels specified with <see cref="M:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent.SetNodeLevelsWithTextRange(System.Int32,System.Int32)" />.
		/// </summary>
		Range
	}
}
