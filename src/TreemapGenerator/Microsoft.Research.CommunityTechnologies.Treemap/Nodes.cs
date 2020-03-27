using Microsoft.Research.CommunityTechnologies.TreemapNoDoc;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	/// <summary>
	/// Collection of <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.
	/// </summary>
	///
	/// <remarks>
	/// The <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent.Nodes" /> property on <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent" /> returns a Nodes collection containing the
	/// top-level <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.  Also, each <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
	/// object has a <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.Nodes" /> property that returns a Nodes
	/// collection containing the node's child <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.  A leaf
	/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object has an empty Nodes collection.
	/// </remarks>
	///
	/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
	/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.ITreemapComponent.Nodes" />
	public class Nodes : IEnumerable
	{
		/// Owner of this object, or null if the TreemapGenerator property hasn't
		/// been set yet.
		protected TreemapGenerator m_oTreemapGenerator;

		/// Node that this collection belongs to, or null if this is the top-level
		/// nodes collection.
		protected Node m_oParentNode;

		/// Collection of TreeMapGenerator.Node objects.
		protected ArrayList m_oNodes;

		/// Object that represents the empty space within the rectangle drawn for
		/// the Node object that owns this Nodes collection.
		protected EmptySpace m_oEmptySpace;

		/// <summary>
		/// Gets the number of objects in the collection.
		/// </summary>
		///
		/// <value>
		/// The number of <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects in the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
		/// collection.
		/// </value>
		///
		/// <remarks>
		/// Use <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Nodes.RecursiveCount" /> to get a recursive count.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Nodes.RecursiveCount" />
		/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		public int Count
		{
			get
			{
				AssertValid();
				return m_oNodes.Count;
			}
		}

		/// <summary>
		/// Gets the number of objects in the collection, including all descendant
		/// objects.
		/// </summary>
		///
		/// <value>
		/// The number of <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects in the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
		/// collection, including all descendant <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects.
		/// </value>
		///
		/// <remarks>
		/// Use <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Nodes.Count" /> to get a non-recursive count.
		/// </remarks>
		///
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Nodes.Count" />
		/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		public int RecursiveCount
		{
			get
			{
				int num = 0;
				foreach (Node oNode in m_oNodes)
				{
					num += 1 + oNode.Nodes.RecursiveCount;
				}
				return num;
			}
		}

		/// <summary>
		/// Gets the object at the specified index.
		/// </summary>
		///
		/// <value>
		/// The <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object at the specified index.
		/// </value>
		///
		/// <param name="zeroBasedIndex">
		/// Zero-based index of the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> to get.
		/// </param>
		///
		/// <example>
		/// Node oFirstTopLevelNode = oTreemapGenerator.Nodes[0];
		/// </example>
		public Node this[int zeroBasedIndex]
		{
			get
			{
				AssertValid();
				int count = m_oNodes.Count;
				if (count == 0)
				{
					throw new InvalidOperationException("Nodes[]: There are no nodes in the collection.");
				}
				if (zeroBasedIndex < 0 || zeroBasedIndex >= count)
				{
					throw new ArgumentOutOfRangeException("zeroBasedIndex", zeroBasedIndex, "Nodes[]: zeroBasedIndex must be between 0 and Nodes.Count-1.");
				}
				return (Node)m_oNodes[zeroBasedIndex];
			}
		}

		/// <summary>
		/// Gets the object that represents empty space in the parent rectangle.
		/// </summary>
		///
		/// <value>
		/// An <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.EmptySpace" />
		/// object that represents empty space in the parent rectangle.
		/// </value>
		///
		/// <remarks>
		/// See <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" /> for details on how the size of each
		/// node rectangle is computed and how EmptySpace is involved in the
		/// computations.
		/// </remarks>
		///
		/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.EmptySpace" />
		/// <seealso cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Node.SizeMetric" />
		public EmptySpace EmptySpace
		{
			get
			{
				AssertValid();
				return m_oEmptySpace;
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
				foreach (Node oNode in m_oNodes)
				{
					oNode.TreemapGenerator = value;
				}
				m_oEmptySpace.TreemapGenerator = value;
				AssertValid();
			}
		}

		/// <summary>
		/// Initializes a new instance of the Node class.
		/// </summary>
		///
		/// <param name="oParentNode">
		/// Node that this collection belongs to, or null if this is the top-level
		/// nodes collection.
		/// </param>
		protected internal Nodes(Node oParentNode)
		{
			Initialize(oParentNode);
		}

		/// <overloads>
		/// Adds a node to a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" /> collection.
		/// </overloads>
		///
		/// <summary>
		/// Creates a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object with the specified text, size
		/// metric, and color metric, and adds it to the end of a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" /> collection.
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
		/// <returns>
		/// The new <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object.
		/// </returns>
		///
		/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		public Node Add(string text, float sizeMetric, float colorMetric)
		{
			Node.ValidateSizeMetric(sizeMetric, "Nodes.Add()");
			Node.ValidateColorMetric(colorMetric, "Nodes.Add()");
			return Add(new Node(text, sizeMetric, colorMetric));
		}

		public Node Add(string text, float sizeMetric, Color absoluteColor)
		{
			Node.ValidateSizeMetric(sizeMetric, "Nodes.Add()");
			return Add(new Node(text, sizeMetric, absoluteColor));
		}

		/// <summary>
		/// Creates a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object with the specified text, size
		/// metric, color metric, and tag, and adds it to the end of a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" /> collection.
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
		/// <returns>
		/// The new <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object.
		/// </returns>
		///
		/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		public Node Add(string text, float sizeMetric, float colorMetric, object tag)
		{
			Node node = Add(text, sizeMetric, colorMetric);
			node.Tag = tag;
			AssertValid();
			return node;
		}

		/// <summary>
		/// Creates a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object with the specified text, size
		/// metric, color metric, tag, and tooltip, and adds it to the end of a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" /> collection.
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
		/// is not used by the <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Nodes.TreemapGenerator" />.
		/// </param>
		///
		/// <returns>
		/// The new <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object.
		/// </returns>
		///
		/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		public Node Add(string text, float sizeMetric, float colorMetric, object tag, string toolTip)
		{
			Node node = Add(text, sizeMetric, colorMetric);
			node.Tag = tag;
			node.ToolTip = toolTip;
			AssertValid();
			return node;
		}

		/// <summary>
		/// Adds an existing <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object to the end of a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" /> collection.
		/// </summary>
		///
		/// <param name="node">
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object to add.
		/// </param>
		///
		/// <returns>
		/// The <paramref name="node" /> object.
		/// </returns>
		///
		/// <remarks>
		/// Do not add the same <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> object to the node hierarchy
		/// more than once.
		/// </remarks>
		///
		/// <seealso cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" />
		public Node Add(Node node)
		{
			m_oNodes.Add(node);
			node.SetParent(m_oParentNode);
			if (m_oTreemapGenerator != null)
			{
				node.TreemapGenerator = m_oTreemapGenerator;
			}
			FireRedrawRequired();
			AssertValid();
			return node;
		}

		/// <summary>
		/// Returns an array of the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects in the collection.
		/// </summary>
		///
		/// <returns>
		/// Array of references to the <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Node" /> objects in the
		/// collection.  (The objects in the collection are not copied.)  If the
		/// collection is empty, an empty array is returned.
		/// </returns>
		public Node[] ToArray()
		{
			Node[] array = new Node[m_oNodes.Count];
			m_oNodes.CopyTo(array);
			return array;
		}

		/// <summary>
		/// Gets a type-safe enumerator that can iterate through a
		/// <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" /> collection.
		/// </summary>
		///
		/// <returns>
		/// Type-safe enumerator object.
		/// </returns>
		public NodesEnumerator GetEnumerator()
		{
			AssertValid();
			return new NodesEnumerator(this);
		}

		/// <summary>
		/// Gets an enumerator that can iterate through a <see cref="T:Microsoft.Research.CommunityTechnologies.Treemap.Nodes" />
		/// collection.
		/// </summary>
		///
		/// <returns>
		/// Enumerator object.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			AssertValid();
			return new NodesEnumerator(this);
		}

		/// <summary>
		/// Initializes the object.
		/// </summary>
		///
		/// <param name="oParentNode">
		/// Node that this collection belongs to, or null if this is the top-level
		/// nodes collection.
		/// </param>
		protected void Initialize(Node oParentNode)
		{
			m_oTreemapGenerator = null;
			m_oParentNode = oParentNode;
			m_oNodes = new ArrayList();
			m_oEmptySpace = new EmptySpace();
		}

		/// <summary>
		/// Removes all nodes from the collection and sets the SizeMetric property
		/// on <see cref="P:Microsoft.Research.CommunityTechnologies.Treemap.Nodes.EmptySpace" /> to zero.
		/// </summary>
		protected internal void Clear()
		{
			AssertValid();
			m_oNodes.Clear();
			m_oEmptySpace = new EmptySpace();
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
		/// Node.  Where the Node object gets stored.  null is stored if a Node
		/// object was not found.
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
			foreach (Node oNode2 in m_oNodes)
			{
				if (oNode2.GetNodeFromPoint(oPointF, out oNode))
				{
					return true;
				}
			}
			oNode = null;
			return false;
		}

		/// <summary>
		/// ToArraySortedBySizeMetric() method.
		/// </summary>
		///
		/// <remarks>
		/// Returns an array of Node objects sorted by Node.SizeMetric.
		/// </remarks>
		///
		/// <returns>
		/// Array of Node objects sorted by Node.SizeMetric in descending order.
		/// If this Nodes collection is empty, an empty array is returned.
		/// </returns>
		protected internal Node[] ToArraySortedBySizeMetric()
		{
			Node[] array = new Node[m_oNodes.Count];
			m_oNodes.CopyTo(array);
			Array.Sort((Array)array);
			return array;
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
			Debug.Assert(m_oNodes != null);
			Debug.Assert(m_oEmptySpace != null);
			m_oEmptySpace.AssertValid();
		}
	}
}
