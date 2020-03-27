using Microsoft.Research.CommunityTechnologies.Treemap;
using System.Diagnostics;
using System.Drawing;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class SquarifiedLayoutEngine : LayoutEngineBase
	{
		protected bool m_bBottomWeighted;

		protected internal SquarifiedLayoutEngine(bool bBottomWeighted)
		{
			m_bBottomWeighted = bBottomWeighted;
		}

		public override void CalculateNodeRectangles(Nodes oNodes, RectangleF oParentRectangle, Node oParentNode, EmptySpaceLocation eEmptySpaceLocation)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(oParentRectangle.Width > 0f);
			Debug.Assert(oParentRectangle.Height > 0f);
			if (oNodes.Count == 0)
			{
				return;
			}
			Node[] array = oNodes.ToArraySortedBySizeMetric();
			double areaPerSizeMetric = GetAreaPerSizeMetric(oNodes, oParentRectangle);
			if (areaPerSizeMetric == 0.0)
			{
				SetNodeRectanglesToEmpty(oNodes, bRecursive: true);
				return;
			}
			if (eEmptySpaceLocation == EmptySpaceLocation.Top && oNodes.EmptySpace.SizeMetric > 0f)
			{
				double num = areaPerSizeMetric * (double)oNodes.EmptySpace.SizeMetric;
				Debug.Assert(oParentRectangle.Width > 0f);
				double num2 = num / (double)oParentRectangle.Width;
				oParentRectangle = RectangleF.FromLTRB(oParentRectangle.Left, oParentRectangle.Top + (float)num2, oParentRectangle.Right, oParentRectangle.Bottom);
				if (oParentRectangle.Height <= 0f)
				{
					SetNodeRectanglesToEmpty(oNodes, bRecursive: true);
					return;
				}
			}
			CalculateSquarifiedNodeRectangles(array, oParentRectangle, areaPerSizeMetric);
			Node[] array2 = array;
			foreach (Node node in array2)
			{
				RectangleF rectangle = node.Rectangle;
				Debug.Assert(rectangle.Width >= 0f);
				Debug.Assert(rectangle.Height >= 0f);
			}
		}

		protected void CalculateSquarifiedNodeRectangles(Node[] aoSortedNodes, RectangleF oParentRectangle, double dAreaPerSizeMetric)
		{
			Debug.Assert(aoSortedNodes != null);
			Debug.Assert(oParentRectangle.Width > 0f);
			Debug.Assert(oParentRectangle.Height > 0f);
			Debug.Assert(dAreaPerSizeMetric >= 0.0);
			int num = aoSortedNodes.Length;
			int num2 = -1;
			int num3 = -1;
			int num4 = 0;
			double num5 = 0.0;
			double num6 = double.MaxValue;
			while (true)
			{
				if (num4 >= num)
				{
					return;
				}
				if (oParentRectangle.IsEmpty)
				{
					break;
				}
				Node node = aoSortedNodes[num4];
				double num7 = node.SizeMetric;
				if (num7 == 0.0)
				{
					SetNodeRectanglesToEmpty(aoSortedNodes, num4, num4);
					num4++;
					continue;
				}
				bool flag = num2 != -1;
				if (flag)
				{
					SaveInsertedRectangles(aoSortedNodes, num2, num3);
				}
				num5 += num7;
				InsertNodesInRectangle(aoSortedNodes, oParentRectangle, flag ? num2 : num4, num4, num5, dAreaPerSizeMetric);
				double aspectRatio = node.AspectRatio;
				if (aspectRatio <= num6)
				{
					if (flag)
					{
						num3++;
					}
					else
					{
						num2 = (num3 = num4);
					}
					num4++;
					num6 = aspectRatio;
					continue;
				}
				if (flag)
				{
					RestoreInsertedRectangles(aoSortedNodes, num2, num3);
				}
				oParentRectangle = GetRemainingEmptySpace(aoSortedNodes, oParentRectangle, num2, num3);
				num2 = -1;
				num3 = -1;
				num5 = 0.0;
				num6 = double.MaxValue;
			}
			SetNodeRectanglesToEmpty(aoSortedNodes, num4, num - 1);
		}

		protected void InsertNodesInRectangle(Node[] aoSortedNodes, RectangleF oParentRectangle, int iIndexOfFirstNodeToInsert, int iIndexOfLastNodeToInsert, double dSizeMetricSum, double dAreaPerSizeMetric)
		{
			Debug.Assert(aoSortedNodes != null);
			Debug.Assert(oParentRectangle.Width > 0f);
			Debug.Assert(oParentRectangle.Height > 0f);
			Debug.Assert(iIndexOfFirstNodeToInsert >= 0);
			Debug.Assert(iIndexOfLastNodeToInsert >= 0);
			Debug.Assert(iIndexOfLastNodeToInsert >= iIndexOfFirstNodeToInsert);
			Debug.Assert(iIndexOfLastNodeToInsert < aoSortedNodes.Length);
			Debug.Assert(dSizeMetricSum > 0.0);
			Debug.Assert(dAreaPerSizeMetric >= 0.0);
			bool flag = oParentRectangle.Width >= oParentRectangle.Height;
			double num = flag ? oParentRectangle.Height : oParentRectangle.Width;
			Debug.Assert(num != 0.0);
			double num2 = dAreaPerSizeMetric * dSizeMetricSum;
			double num3 = num2 / num;
			double num4;
			double num5;
			double num6;
			double num7;
			if (flag)
			{
				num4 = oParentRectangle.Left;
				num5 = num4 + num3;
				num6 = (num7 = (m_bBottomWeighted ? oParentRectangle.Bottom : oParentRectangle.Top));
			}
			else
			{
				if (m_bBottomWeighted)
				{
					num7 = oParentRectangle.Bottom;
					num6 = num7 - num3;
				}
				else
				{
					num6 = oParentRectangle.Top;
					num7 = num6 + num3;
				}
				num4 = (num5 = oParentRectangle.Left);
			}
			for (int i = iIndexOfFirstNodeToInsert; i <= iIndexOfLastNodeToInsert; i++)
			{
				Node node = aoSortedNodes[i];
				Debug.Assert(dSizeMetricSum != 0.0);
				double num8 = num * ((double)node.SizeMetric / dSizeMetricSum);
				if (flag)
				{
					if (m_bBottomWeighted)
					{
						num6 = num7 - num8;
					}
					else
					{
						num7 = num6 + num8;
					}
				}
				else
				{
					num5 = num4 + num8;
				}
				node.Rectangle = RectangleF.FromLTRB((float)num4, (float)num6, (float)num5, (float)num7);
				Debug.Assert(node.Rectangle.Width >= 0f);
				Debug.Assert(node.Rectangle.Height >= 0f);
				if (flag)
				{
					if (m_bBottomWeighted)
					{
						num7 = num6;
					}
					else
					{
						num6 = num7;
					}
				}
				else
				{
					num4 = num5;
				}
			}
		}

		protected void SaveInsertedRectangles(Node[] aoNodes, int iIndexOfFirstInsertedNode, int iIndexOfLastInsertedNode)
		{
			Debug.Assert(aoNodes != null);
			Debug.Assert(iIndexOfFirstInsertedNode >= 0);
			Debug.Assert(iIndexOfLastInsertedNode >= 0);
			Debug.Assert(iIndexOfLastInsertedNode >= iIndexOfFirstInsertedNode);
			Debug.Assert(iIndexOfLastInsertedNode < aoNodes.Length);
			for (int i = iIndexOfFirstInsertedNode; i <= iIndexOfLastInsertedNode; i++)
			{
				aoNodes[i].SaveRectangle();
			}
		}

		protected void RestoreInsertedRectangles(Node[] aoNodes, int iIndexOfFirstInsertedNode, int iIndexOfLastInsertedNode)
		{
			Debug.Assert(aoNodes != null);
			Debug.Assert(iIndexOfFirstInsertedNode >= 0);
			Debug.Assert(iIndexOfLastInsertedNode >= 0);
			Debug.Assert(iIndexOfLastInsertedNode >= iIndexOfFirstInsertedNode);
			Debug.Assert(iIndexOfLastInsertedNode < aoNodes.Length);
			for (int i = iIndexOfFirstInsertedNode; i <= iIndexOfLastInsertedNode; i++)
			{
				aoNodes[i].RestoreRectangle();
			}
		}

		protected double GetAreaPerSizeMetric(Nodes oNodes, RectangleF oParentRectangle)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(oParentRectangle.Width > 0f);
			Debug.Assert(oParentRectangle.Height > 0f);
			double num = oParentRectangle.Width * oParentRectangle.Height;
			double num2 = 0.0;
			foreach (Node oNode in oNodes)
			{
				num2 += (double)oNode.SizeMetric;
			}
			num2 += (double)oNodes.EmptySpace.SizeMetric;
			if (num2 == 0.0)
			{
				return 0.0;
			}
			Debug.Assert(num2 != 0.0);
			return num / num2;
		}

		protected RectangleF GetRemainingEmptySpace(Node[] aoNodes, RectangleF oParentRectangle, int iIndexOfFirstInsertedNode, int iIndexOfLastInsertedNode)
		{
			Debug.Assert(aoNodes != null);
			Debug.Assert(oParentRectangle.Width > 0f);
			Debug.Assert(oParentRectangle.Height > 0f);
			Debug.Assert(iIndexOfFirstInsertedNode >= 0);
			Debug.Assert(iIndexOfLastInsertedNode >= 0);
			Debug.Assert(iIndexOfLastInsertedNode >= iIndexOfFirstInsertedNode);
			Debug.Assert(iIndexOfLastInsertedNode < aoNodes.Length);
			RectangleF rectangle = aoNodes[iIndexOfLastInsertedNode].Rectangle;
			RectangleF result = (oParentRectangle.Width >= oParentRectangle.Height) ? RectangleF.FromLTRB(rectangle.Right, oParentRectangle.Top, oParentRectangle.Right, oParentRectangle.Bottom) : ((!m_bBottomWeighted) ? RectangleF.FromLTRB(oParentRectangle.Left, rectangle.Bottom, oParentRectangle.Right, oParentRectangle.Bottom) : RectangleF.FromLTRB(oParentRectangle.Left, oParentRectangle.Top, oParentRectangle.Right, rectangle.Top));
			if (result.IsEmpty)
			{
				result = RectangleF.FromLTRB(0f, 0f, 0f, 0f);
			}
			return result;
		}
	}
}
