using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
	public class SerializationUtil
	{
		private SerializationUtil()
		{
		}

		public static void Serialize(object oObject, Stream oStream)
		{
			Debug.Assert(oObject != null);
			Debug.Assert(oStream != null);
			XmlSerializer xmlSerializer = CreateXmlSerializer(oObject);
			xmlSerializer.Serialize(oStream, oObject);
		}

		public static void Serialize(object oObject, StreamWriter oStreamWriter)
		{
			Debug.Assert(oObject != null);
			Debug.Assert(oStreamWriter != null);
			XmlSerializer xmlSerializer = CreateXmlSerializer(oObject);
			xmlSerializer.Serialize(oStreamWriter, oObject);
		}

		public static object Deserialize(Type oType, Stream oStream)
		{
			Debug.Assert(oType is object);
			Debug.Assert(oStream != null);
			XmlSerializer xmlSerializer = CreateXmlSerializer(oType);
			object obj = xmlSerializer.Deserialize(oStream);
			Debug.Assert(obj != null);
			if (obj is IDeserializationCallback)
			{
				((IDeserializationCallback)obj).OnDeserialization(null);
			}
			return obj;
		}

		public static object Deserialize(Type oType, FileInfo oFileInfo)
		{
			Debug.Assert(oType is object);
			Debug.Assert(oFileInfo != null);
			Debug.Assert(oFileInfo.Exists);
			FileStream fileStream = oFileInfo.OpenRead();
			object obj = null;
			try
			{
				obj = Deserialize(oType, fileStream);
			}
			catch
			{
				throw;
			}
			finally
			{
				fileStream.Close();
			}
			Debug.Assert(obj != null);
			return obj;
		}

		public static object Clone(object oObject)
		{
			Debug.Assert(oObject != null);
			MemoryStream memoryStream = new MemoryStream();
			Serialize(oObject, memoryStream);
			memoryStream.Position = 0L;
			object result = Deserialize(oObject.GetType(), memoryStream);
			memoryStream.Close();
			return result;
		}

		public static void SerializeStringAttributes(XmlTextWriter oXmlTextWriter, params string[] asNameValuePairs)
		{
			Debug.Assert(oXmlTextWriter != null);
			Debug.Assert(asNameValuePairs != null);
			int num = asNameValuePairs.Length;
			if (num % 2 != 0)
			{
				throw new ApplicationException("SerializationUtil.SerializeStringAttributes: asNameValuePairs must contain an even number of elements.");
			}
			int num2 = 0;
			while (true)
			{
				if (num2 < num)
				{
					string text = asNameValuePairs[num2];
					string value = asNameValuePairs[num2 + 1];
					if (StringUtil.IsEmpty(text))
					{
						break;
					}
					oXmlTextWriter.WriteAttributeString(text, value);
					num2 += 2;
					continue;
				}
				return;
			}
			throw new ApplicationException($"SerializationUtil.SerializeStringAttributes: asNameValuePairs[{num2}] is empty or null.");
		}

		public static string DeserializeRequiredStringAttribute(XmlTextReader oXmlTextReader, string sElementName, string sAttributeName)
		{
			Debug.Assert(oXmlTextReader != null);
			Debug.Assert(oXmlTextReader.NodeType == XmlNodeType.Element);
			Debug.Assert(oXmlTextReader.Name == sElementName);
			StringUtil.AssertNotEmpty(sElementName);
			StringUtil.AssertNotEmpty(sAttributeName);
			string attribute = oXmlTextReader.GetAttribute(sAttributeName);
			if (attribute == null)
			{
				throw new ApplicationException($"SerializationUtil.DeserializeRequiredStringAttribute: A {sElementName} XML element is missing a required {sAttributeName} attribute.");
			}
			return attribute;
		}

		public static int DeserializeRequiredInt32Attribute(XmlTextReader oXmlTextReader, string sElementName, string sAttributeName)
		{
			Debug.Assert(oXmlTextReader != null);
			Debug.Assert(oXmlTextReader.NodeType == XmlNodeType.Element);
			Debug.Assert(oXmlTextReader.Name == sElementName);
			StringUtil.AssertNotEmpty(sElementName);
			StringUtil.AssertNotEmpty(sAttributeName);
			string s = DeserializeRequiredStringAttribute(oXmlTextReader, sElementName, sAttributeName);
			try
			{
				return int.Parse(s);
			}
			catch (FormatException innerException)
			{
				throw new ApplicationException($"SerializationUtil.DeserializeRequiredInt32Attribute: A {sElementName} XML element has a {sAttributeName} attribute that must be an Int32 but is not in Int32 format.", innerException);
			}
		}

		public static float DeserializeRequiredSingleAttribute(XmlTextReader oXmlTextReader, string sElementName, string sAttributeName)
		{
			Debug.Assert(oXmlTextReader != null);
			Debug.Assert(oXmlTextReader.NodeType == XmlNodeType.Element);
			Debug.Assert(oXmlTextReader.Name == sElementName);
			StringUtil.AssertNotEmpty(sElementName);
			StringUtil.AssertNotEmpty(sAttributeName);
			string s = DeserializeRequiredStringAttribute(oXmlTextReader, sElementName, sAttributeName);
			try
			{
				return float.Parse(s);
			}
			catch (FormatException innerException)
			{
				throw new ApplicationException($"SerializationUtil.DeserializeRequiredSingleAttribute: A {sElementName} XML element has a {sAttributeName} attribute that must be a Single but is not in Single format.", innerException);
			}
		}

		protected static XmlSerializer CreateXmlSerializer(object oObject)
		{
			Debug.Assert(oObject != null);
			return CreateXmlSerializer(oObject.GetType());
		}

		protected static XmlSerializer CreateXmlSerializer(Type oType)
		{
			Debug.Assert(oType is object);
			return new XmlSerializer(oType);
		}
	}
}
