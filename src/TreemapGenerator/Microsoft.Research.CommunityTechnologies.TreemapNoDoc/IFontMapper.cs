using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// Interface implemented by classes that can provide a Font object to use for
	/// drawing a node's text.
	/// </summary>
	public interface IFontMapper : IDisposable
	{
		/// <summary>
		/// Returns a Font object to use for drawing a node's text.
		/// </summary>
		///
		/// <param name="oNode">
		/// The node that needs to be drawn.
		/// </param>
		///
		/// <param name="iNodeLevel">
		/// Node level.  Top-level nodes are at level 0.
		/// </param>
		///
		/// <param name="oGraphics">
		/// Object the caller will use to draw the node's text.
		/// </param>
		///
		/// <param name="oFont">
		/// Where the font gets stored.  The font is owned by the object
		/// implementing this interface.  Do not call the font's Dispose() method.
		/// </param>
		///
		/// <param name="sTextToDraw">
		/// Where the text to draw gets stored.  This is either the node's text or
		/// an abbreviated form of the node's text.
		/// </param>
		///
		/// <returns>
		/// true if an appropriate font was found, false if not.
		/// </returns>
		///
		/// <remarks>
		/// If a Font object suitable for drawing the text for <paramref name="oNode" /> is available, the font is stored at <paramref name="oFont" />, the text to draw is stored at <paramref name="sTextToDraw" />, and true is returned.  false if returned
		/// otherwise.
		/// </remarks>
		bool NodeToFont(Node oNode, int iNodeLevel, Graphics oGraphics, out Font oFont, out string sTextToDraw);
	}
}
