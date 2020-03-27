using System;
using System.Diagnostics;
using System.Xml;

namespace Microsoft.Research.CommunityTechnologies.XmlLib
{
	/// <summary>
	/// XML utility methods.
	/// </summary>
	///
	/// <remarks>
	/// This class contains utility methods for dealing with XML.  All methods are
	/// static.
	/// </remarks>
	internal class XmlUtil
	{
		/// <summary>
		/// Initializes a new instance of the XmlUtil class.
		/// </summary>
		///
		/// <remarks>
		/// Do not instantiate an XmlUtil object.  All XmlUtil methods are static.
		/// </remarks>
		private XmlUtil()
		{
		}

		/// <overloads>
		/// Creates a new node and appends it to a parent node.
		/// </overloads>
		///
		/// <summary>
		/// Creates a new node and appends it to a parent node.
		/// </summary>
		///
		/// <param name="oParentNode">
		/// Node to append the new node to.
		/// </param>
		///
		/// <param name="sChildName">
		/// Name of the new node.
		/// </param>
		///
		/// <returns>
		/// The new node.
		/// </returns>
		public static XmlNode AppendNewNode(XmlNode oParentNode, string sChildName)
		{
			Debug.Assert(oParentNode != null);
			Debug.Assert(sChildName != "");
			XmlDocument xmlDocument = oParentNode.OwnerDocument;
			if (xmlDocument == null)
			{
				xmlDocument = (XmlDocument)oParentNode;
			}
			return oParentNode.AppendChild(xmlDocument.CreateElement(sChildName));
		}

		/// <summary>
		/// Creates a new node, sets its inner text, and appends the new node to a
		/// parent node.
		/// </summary>
		///
		/// <param name="oParentNode">
		/// Node to append a new node to.
		/// </param>
		///
		/// <param name="sChildName">
		/// Name of the new node.
		/// </param>
		///
		/// <param name="sInnerText">
		/// Inner text of the new node.
		/// </param>
		///
		/// <returns>
		/// The new node.
		/// </returns>
		public static XmlNode AppendNewNode(XmlNode oParentNode, string sChildName, string sInnerText)
		{
			Debug.Assert(oParentNode != null);
			Debug.Assert(sChildName != "");
			Debug.Assert(sInnerText != null);
			XmlNode xmlNode = AppendNewNode(oParentNode, sChildName);
			xmlNode.InnerText = sInnerText;
			return xmlNode;
		}

		/// <summary>
		/// Selects a single node that must exist.  If it doesn't exist, an
		/// exception is thrown.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node to select from.
		/// </param>
		///
		/// <param name="sXPath">
		/// XPath expression.
		/// </param>
		///
		/// <returns>
		/// Selected node.
		/// </returns>
		public static XmlNode SelectRequiredSingleNode(XmlNode oNode, string sXPath)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sXPath != "");
			XmlNode xmlNode = oNode.SelectSingleNode(sXPath);
			if (xmlNode == null)
			{
				throw new InvalidOperationException("XmlUtil.SelectRequiredSingleNode: A " + oNode.Name + " node is missing a required descendent node.  The XPath is \"" + sXPath + "\".");
			}
			return xmlNode;
		}

		/// <summary>
		/// Asserts if a node doesn't have the expected name.  Debug-only.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node to check.
		/// </param>
		///
		/// <param name="sExpectedName">
		/// Expected name.
		/// </param>
		[Conditional("DEBUG")]
		public static void CheckNodeName(XmlNode oNode, string sExpectedName)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(oNode.Name == sExpectedName);
		}

		/// <summary>
		/// Reads the inner text from an XmlNode.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node to read inner text from.
		/// </param>
		///
		/// <param name="bRequired">
		/// true if the inner text is required.
		/// </param>
		///
		/// <param name="sInnerText">
		/// Where the inner text gets stored.
		/// </param>
		///
		/// <returns>
		/// true if the inner text was found and stored in sInnerText.
		/// </returns>
		///
		/// <remarks>
		/// If <paramref name="bRequired" /> is true and the inner text is missing,
		/// an exception is thrown.  If <paramref name="bRequired" /> is false and
		/// the inner text is missing, an empty string is stored in <paramref name="sInnerText" /> and false is returned.
		/// </remarks>
		public static bool GetInnerText(XmlNode oNode, bool bRequired, out string sInnerText)
		{
			Debug.Assert(oNode != null);
			sInnerText = oNode.InnerText;
			if (sInnerText == null || sInnerText.Trim().Length == 0)
			{
				if (bRequired)
				{
					throw new InvalidOperationException("A " + oNode.Name + " node is missing required inner text.");
				}
				return false;
			}
			return true;
		}

		/// <summary>
		/// Reads an XML attribute from an XmlNode.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node to read attribute from.
		/// </param>
		///
		/// <param name="sName">
		/// Attribute to read.
		/// </param>
		///
		/// <param name="bRequired">
		/// true if the attribute is required.
		/// </param>
		///
		/// <param name="sValue">
		/// Where the attribute value gets stored.
		/// </param>
		///
		/// <returns>
		/// true if the attribute was found and stored in sValue.
		/// </returns>
		///
		/// <remarks>
		/// If <paramref name="bRequired" /> is true and the attribute is missing,
		/// an exception is thrown.  If <paramref name="bRequired" /> is false and
		/// the attribute is missing, an empty string is stored in <paramref name="sValue" /> and false is returned.
		/// </remarks>
		public static bool GetAttribute(XmlNode oNode, string sName, bool bRequired, out string sValue)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			sValue = ((XmlElement)oNode).GetAttribute(sName);
			if (sValue == "")
			{
				if (bRequired)
				{
					throw new InvalidOperationException("A " + oNode.Name + " node is missing a required " + sName + " attribute.");
				}
				return false;
			}
			return true;
		}

		/// <summary>
		/// Reads an XML attribute that contains an Int32 string and converts it to
		/// an Int32.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node to read attribute from.
		/// </param>
		///
		/// <param name="sName">
		/// Attribute to read.
		/// </param>
		///
		/// <param name="bRequired">
		/// true if the attribute is required.
		/// </param>
		///
		/// <param name="iValue">
		/// Where the converted attribute gets stored.
		/// </param>
		///
		/// <returns>
		/// true if the attribute was found and stored in iValue.
		/// </returns>
		///
		/// <remarks>
		/// If <paramref name="bRequired" /> is true and the attribute is missing,
		/// an exception is thrown.  If <paramref name="bRequired" /> is false and
		/// the attribute is missing, Int32.MinValue is stored in iValue and false
		/// is returned.
		/// </remarks>
		public static bool GetInt32Attribute(XmlNode oNode, string sName, bool bRequired, out int iValue)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			if (!GetAttribute(oNode, sName, bRequired, out string sValue))
			{
				iValue = int.MinValue;
				return false;
			}
			try
			{
				iValue = int.Parse(sValue);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("XmlUtil.GetInt32Attribute: Can't convert " + sValue + " from String to Int32.", innerException);
			}
			return true;
		}

		/// <summary>
		/// Reads an XML attribute that contains an Int64 string and converts it to
		/// an Int64.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node to read attribute from.
		/// </param>
		///
		/// <param name="sName">
		/// Attribute to read.
		/// </param>
		///
		/// <param name="bRequired">
		/// true if the attribute is required.
		/// </param>
		///
		/// <param name="i64Value">
		/// Where the converted attribute gets stored.
		/// </param>
		///
		/// <returns>
		/// true if the attribute was found and stored in i64Value.
		/// </returns>
		///
		/// <remarks>
		/// If <paramref name="bRequired" /> is true and the attribute is missing,
		/// an exception is thrown.  If <paramref name="bRequired" /> is false and
		/// the attribute is missing, Int64.MinValue is stored in i64Value and
		/// false is returned.
		/// </remarks>
		public static bool GetInt64Attribute(XmlNode oNode, string sName, bool bRequired, out long i64Value)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			if (!GetAttribute(oNode, sName, bRequired, out string sValue))
			{
				i64Value = long.MinValue;
				return false;
			}
			try
			{
				i64Value = long.Parse(sValue);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("XmlUtil.GetInt64Attribute: Can't convert " + sValue + " from String to Int64.", innerException);
			}
			return true;
		}

		/// <summary>
		/// Reads an XML attribute that contains a Single string and converts it to
		/// a Single.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node to read attribute from.
		/// </param>
		///
		/// <param name="sName">
		/// Attribute to read.
		/// </param>
		///
		/// <param name="bRequired">
		/// true if the attribute is required.
		/// </param>
		///
		/// <param name="fValue">
		/// Where the converted attribute gets stored.
		/// </param>
		///
		/// <returns>
		/// true if the attribute was found and stored in fValue.
		/// </returns>
		///
		/// <remarks>
		/// If <paramref name="bRequired" /> is true and the attribute is missing,
		/// an exception is thrown.  If <paramref name="bRequired" /> is false and
		/// the attribute is missing, Single.MinValue is stored in fValue and false
		/// is returned.
		/// </remarks>
		public static bool GetSingleAttribute(XmlNode oNode, string sName, bool bRequired, out float fValue)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			if (!GetAttribute(oNode, sName, bRequired, out string sValue))
			{
				fValue = float.MinValue;
				return false;
			}
			try
			{
				fValue = float.Parse(sValue);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("XmlUtil.GetSingleAttribute: Can't convert " + sValue + " from String to Single.", innerException);
			}
			return true;
		}

		/// <summary>
		/// Reads an XML attribute that contains a string that is either "0" or "1"
		/// and converts it to a Boolean.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node to read attribute from.
		/// </param>
		///
		/// <param name="sName">
		/// Attribute to read.
		/// </param>
		///
		/// <param name="bRequired">
		/// true if the attribute is required.
		/// </param>
		///
		/// <param name="bValue">
		/// Where the converted attribute gets stored.
		/// </param>
		///
		/// <returns>
		/// true if the attribute was found and stored in bValue.
		/// </returns>
		///
		/// <remarks>
		/// If <paramref name="bRequired" /> is true and the attribute is missing,
		/// an exception is thrown.  If <paramref name="bRequired" /> is false and
		/// the attribute is missing, false is stored in bValue and false is
		/// returned.
		/// </remarks>
		public static bool GetBooleanAttribute(XmlNode oNode, string sName, bool bRequired, out bool bValue)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			if (!GetAttribute(oNode, sName, bRequired, out string sValue))
			{
				bValue = false;
				return false;
			}
			bValue = sValue switch
			{
				"0" => false,
				"1" => true,
				_ => throw new InvalidOperationException("XmlUtil.GetBooleanAttribute: A " + oNode.Name + " node has a " + sName + " attribute that is not 0 or 1."),
			};
			return true;
		}

		/// <summary>
		/// Reads an XML attribute that contains a date/time string and converts it
		/// to a DateTime.
		/// </summary>
		///
		/// <param name="oNode">
		/// Node to read attribute from.
		/// </param>
		///
		/// <param name="sName">
		/// Attribute to read.
		/// </param>
		///
		/// <param name="bRequired">
		/// true if the attribute is required.
		/// </param>
		///
		/// <param name="oValue">
		/// Where the converted attribute gets stored.
		/// </param>
		///
		/// <returns>
		/// true if the attribute was found and stored in oValue.
		/// </returns>
		///
		/// <remarks>
		/// If <paramref name="bRequired" /> is true and the attribute is missing,
		/// an exception is thrown.  If <paramref name="bRequired" /> is false and
		/// the attribute is missing, DateTime.MinValue is stored in oValue and
		/// false is returned.
		/// </remarks>
		public static bool GetDateTimeAttribute(XmlNode oNode, string sName, bool bRequired, out DateTime oValue)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			if (!GetAttribute(oNode, sName, bRequired, out string sValue))
			{
				oValue = DateTime.MinValue;
				return false;
			}
			try
			{
				oValue = DateTime.Parse(sValue);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("XmlUtil.GetDateTimeAttribute: Can't convert " + sValue + " from String to DateTime.", innerException);
			}
			return true;
		}

		/// <summary>
		/// Sets multiple attributes on an XML node.
		/// </summary>
		///
		/// <param name="oNode">
		/// XmlNode.  Node to set attributes on.
		/// </param>
		///
		/// <param name="asNameValuePairs">
		/// String[].  One or more pairs of strings.  The first string in each
		/// pair is an attribute name and the second is the attribute value.
		/// </param>
		///
		/// <remarks>
		/// This sets multiple attributes on an XML node in one call.  It's an
		/// alternative to calling <see cref="M:System.Xml.XmlElement.SetAttribute(System.String,System.String)" /> repeatedly.
		/// </remarks>
		public static void SetAttributes(XmlNode oNode, params string[] asNameValuePairs)
		{
			int num = asNameValuePairs.Length;
			if (num % 2 != 0)
			{
				throw new ArgumentException("XmlUtil.SetAttributes: asNameValuePairs must contain an even number of strings.");
			}
			XmlElement xmlElement = (XmlElement)oNode;
			for (int i = 0; i < num; i += 2)
			{
				string name = asNameValuePairs[i];
				string value = asNameValuePairs[i + 1];
				xmlElement.SetAttribute(name, value);
			}
		}
	}
}
