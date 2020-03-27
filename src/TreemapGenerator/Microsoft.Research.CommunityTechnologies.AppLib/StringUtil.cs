using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
	/// <summary>
	/// String utility methods.
	/// </summary>
	///
	/// <remarks>
	/// This class contains utility methods for dealing with String objects.  All
	/// methods are static.
	/// </remarks>
	public class StringUtil
	{
		/// <summary>
		/// Do not use this constructor.
		/// </summary>
		///
		/// <remarks>
		/// Do not use this constructor.  All StringUtil methods are static.
		/// </remarks>
		private StringUtil()
		{
		}

		/// <summary>
		/// Returns true if a String is null or has a length of 0.
		/// </summary>
		///
		/// <param name="sString">
		/// String to test.
		/// </param>
		///
		/// <returns>
		/// true if the string is null or has a length of 0.
		/// </returns>
		public static bool IsEmpty(string sString)
		{
			return sString == null || sString.Length == 0;
		}

		/// <summary>
		/// Asserts if a String is null or has a length of 0.  Debug-only.
		/// </summary>
		///
		/// <param name="sString">
		/// String to test.
		/// </param>
		[Conditional("DEBUG")]
		public static void AssertNotEmpty(string sString)
		{
			Debug.Assert(!IsEmpty(sString));
		}

		/// <summary>
		/// Creates a deep copy of an array of strings.
		/// </summary>
		///
		/// <param name="asArrayToCopy">
		/// Array of zero or more strings to copy.
		/// </param>
		///
		/// <returns>
		/// A new array containing copies of the strings in <paramref name="asArrayToCopy" />.
		/// </returns>
		public static string[] CopyStringArray(string[] asArrayToCopy)
		{
			Debug.Assert(asArrayToCopy != null);
			int num = asArrayToCopy.Length;
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = asArrayToCopy[i];
			}
			return array;
		}

		/// <summary>
		/// Creates an array of empty strings.
		/// </summary>
		///
		/// <param name="iEmptyStrings">
		/// Number of empty strings to store in the array.
		/// </param>
		///
		/// <returns>
		/// An array of <paramref name="iEmptyStrings" /> elements, each of which
		/// contains String.Empty.
		/// </returns>
		public static string[] CreateArrayOfEmptyStrings(int iEmptyStrings)
		{
			Debug.Assert(iEmptyStrings >= 0);
			string[] array = new string[iEmptyStrings];
			for (int i = 0; i < iEmptyStrings; i++)
			{
				array[i] = string.Empty;
			}
			return array;
		}

		/// <summary>
		/// Converts an array of bytes to a printable ASCII string.
		/// </summary>
		///
		/// <param name="abtBytes">
		/// Array of bytes to convert.  Can't be null.
		/// </param>
		///
		/// <param name="cReplacementCharacter">
		/// Character to replace non-printable characters with.  Must have a value
		/// less than or equal to 127.
		/// </param>
		///
		/// <remarks>
		/// This method replaces any bytes greater than 127 with <paramref name="cReplacementCharacter" />, then replaces any non-printable ASCII
		/// characters with the same replacement character.
		/// </remarks>
		public static string BytesToPrintableAscii(byte[] abtBytes, char cReplacementCharacter)
		{
			Debug.Assert(abtBytes != null);
			Debug.Assert(cReplacementCharacter < '\u0080');
			byte[] array = (byte[])abtBytes.Clone();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] > 127)
				{
					array[i] = (byte)cReplacementCharacter;
				}
			}
			string @string = Encoding.ASCII.GetString(array);
			return ReplaceNonPrintableAsciiCharacters(@string, cReplacementCharacter);
		}

		/// <summary>
		/// Replaces non-printable ASCII characters with a specified character.
		/// </summary>
		///
		/// <param name="sString">
		/// String that may include non-printable ASCII characters.  Can't be null.
		/// </param>
		///
		/// <param name="cReplacementCharacter">
		/// Character to replace them with.
		/// </param>
		///
		/// <returns>
		/// <paramref name="sString" /> with non-printable characters replaced with
		/// <paramref name="cReplacementCharacter" />.
		/// </returns>
		public static string ReplaceNonPrintableAsciiCharacters(string sString, char cReplacementCharacter)
		{
			Debug.Assert(sString != null);
			Regex regex = new Regex("[^\\x09\\x0A\\x0D\\x20-\\x7E]");
			return regex.Replace(sString, new string(cReplacementCharacter, 1));
		}
	}
}
