using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	/// <summary>
	/// Object that knows how to serialize and deserialize a Nodes object.
	/// </summary>
	internal class NodesXmlSerializer
	{
		protected const string NodesElementName = "Nodes";

		protected const string NodeElementName = "Node";

		protected const string EmptySpaceSizeMetricAttributeName = "EmptySizeMetric";

		protected const string TextAttributeName = "Text";

		protected const string SizeMetricAttributeName = "SizeMetric";

		protected const string ColorMetricAttributeName = "ColorMetric";

		protected const string AbsoluteColorAttributeName = "AbsoluteColor";

		protected const string ToolTipAttributeName = "ToolTip";

		protected const string TagAttributeName = "Tag";

		public NodesXmlSerializer()
		{
			AssertValid();
		}

		public string SerializeToString(Nodes oNodes, ITreemapComponent oTreemapComponent)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(oTreemapComponent != null);
			AssertValid();
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			SerializeNodes(oNodes, xmlTextWriter);
			xmlTextWriter.Close();
			return stringWriter.ToString();
		}

		public Nodes DeserializeFromString(string sNodesXml, ITreemapComponent oTreemapComponent)
		{
			Debug.Assert(sNodesXml != null);
			Debug.Assert(oTreemapComponent != null);
			AssertValid();
			StringReader input = new StringReader(sNodesXml);
			XmlTextReader xmlTextReader = new XmlTextReader(input);
			xmlTextReader.WhitespaceHandling = WhitespaceHandling.Significant;
			Nodes nodes = null;
			try
			{
				xmlTextReader.IsStartElement("Nodes");
				bool isEmptyElement = xmlTextReader.IsEmptyElement;
				nodes = new Nodes(null);
				DeserializeNodes(xmlTextReader, oTreemapComponent, nodes);
				if ((!isEmptyElement && xmlTextReader.NodeType != XmlNodeType.EndElement) || xmlTextReader.Name != "Nodes")
				{
					throw new ApplicationException(string.Format("The top-level {0} XML element is missing a closing element.", "Nodes"));
				}
			}
			catch (Exception innerException)
			{
				throw new ApplicationException("ITreemapComponent.NodesXml: The XML string is not in the expected format.", innerException);
			}
			finally
			{
				xmlTextReader.Close();
			}
			return nodes;
		}

		protected void SerializeNodes(Nodes oNodes, XmlTextWriter oXmlTextWriter)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(oXmlTextWriter != null);
			AssertValid();
			oXmlTextWriter.WriteStartElement("Nodes");
			oXmlTextWriter.WriteAttributeString("EmptySizeMetric", oNodes.EmptySpace.SizeMetric.ToString());
			foreach (Node oNode in oNodes)
			{
				SerializeNode(oNode, oXmlTextWriter);
			}
			oXmlTextWriter.WriteEndElement();
		}

		protected void SerializeNode(Node oNode, XmlTextWriter oXmlTextWriter)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(oXmlTextWriter != null);
			AssertValid();
			oXmlTextWriter.WriteStartElement("Node");
			SerializationUtil.SerializeStringAttributes(oXmlTextWriter, "Text", oNode.Text, "SizeMetric", oNode.SizeMetric.ToString(), "ColorMetric", oNode.ColorMetric.ToString(), "AbsoluteColor", oNode.AbsoluteColor.ToArgb().ToString(), "ToolTip", oNode.ToolTip);
			if (oNode.Tag != null)
			{
				oXmlTextWriter.WriteAttributeString("Tag", oNode.Tag.ToString());
			}
			SerializeNodes(oNode.Nodes, oXmlTextWriter);
			oXmlTextWriter.WriteEndElement();
		}

		protected void DeserializeNodes(XmlTextReader oXmlTextReader, ITreemapComponent oTreemapComponent, Nodes oNodes)
		{
			Debug.Assert(oXmlTextReader != null);
			Debug.Assert(oXmlTextReader.NodeType == XmlNodeType.Element);
			Debug.Assert(oXmlTextReader.Name == "Nodes");
			Debug.Assert(oTreemapComponent != null);
			Debug.Assert(oNodes != null);
			AssertValid();
			oNodes.EmptySpace.SizeMetric = SerializationUtil.DeserializeRequiredSingleAttribute(oXmlTextReader, "Nodes", "EmptySizeMetric");
			if (oXmlTextReader.IsEmptyElement)
			{
				return;
			}
			while (oXmlTextReader.Read())
			{
				switch (oXmlTextReader.NodeType)
				{
				case XmlNodeType.Element:
					if (oXmlTextReader.Name != "Node")
					{
						throw new ApplicationException(string.Format("A {0} XML element has an unexpected child node named {1}.  Only {2} child nodes are allowed.", "Nodes", oXmlTextReader.Name, "Node"));
					}
					oNodes.Add(DeserializeNode(oXmlTextReader, oTreemapComponent));
					if (oXmlTextReader.NodeType != XmlNodeType.EndElement || oXmlTextReader.Name != "Node")
					{
						throw new ApplicationException(string.Format("A {0} XML element is missing a closing element.", "Node"));
					}
					break;
				case XmlNodeType.EndElement:
					if (oXmlTextReader.Name != "Nodes")
					{
						throw new ApplicationException(string.Format("A {0} XML element is missing a closing element.", "Nodes"));
					}
					return;
				default:
					throw new ApplicationException(string.Format("A {0} XML element has an unexpected child node of type {1}.  Only {2} child nodes are allowed.", "Nodes", oXmlTextReader.NodeType, "Node"));
				}
			}
		}

		protected Node DeserializeNode(XmlTextReader oXmlTextReader, ITreemapComponent oTreemapComponent)
		{
			Debug.Assert(oXmlTextReader != null);
			Debug.Assert(oXmlTextReader.NodeType == XmlNodeType.Element);
			Debug.Assert(oXmlTextReader.Name == "Node");
			Debug.Assert(oTreemapComponent != null);
			AssertValid();
			string text = SerializationUtil.DeserializeRequiredStringAttribute(oXmlTextReader, "Node", "Text");
			float sizeMetric = SerializationUtil.DeserializeRequiredSingleAttribute(oXmlTextReader, "Node", "SizeMetric");
			float colorMetric = SerializationUtil.DeserializeRequiredSingleAttribute(oXmlTextReader, "Node", "ColorMetric");
			Color absoluteColor = Color.FromArgb(SerializationUtil.DeserializeRequiredInt32Attribute(oXmlTextReader, "Node", "AbsoluteColor"));
			string toolTip = SerializationUtil.DeserializeRequiredStringAttribute(oXmlTextReader, "Node", "ToolTip");
			string attribute = oXmlTextReader.GetAttribute("Tag");
			Node node = new Node(text, sizeMetric, colorMetric, attribute, toolTip);
			switch (oTreemapComponent.NodeColorAlgorithm)
			{
			case NodeColorAlgorithm.UseAbsoluteColor:
				node.AbsoluteColor = absoluteColor;
				break;
			default:
				Debug.Assert(condition: false);
				break;
			case NodeColorAlgorithm.UseColorMetric:
				break;
			}
			if (!oXmlTextReader.Read() || oXmlTextReader.NodeType != XmlNodeType.Element || oXmlTextReader.Name != "Nodes")
			{
				throw new ApplicationException(string.Format("A {0} XML element is missing a required {1} child.", "Node", "Nodes"));
			}
			DeserializeNodes(oXmlTextReader, oTreemapComponent, node.Nodes);
			if (!oXmlTextReader.Read() || oXmlTextReader.NodeType != XmlNodeType.EndElement || oXmlTextReader.Name != "Node")
			{
				throw new ApplicationException(string.Format("A {0} XML element is missing a closing element.", "Node"));
			}
			return node;
		}

		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		[Conditional("DEBUG")]
		public void AssertValid()
		{
		}
	}
}
