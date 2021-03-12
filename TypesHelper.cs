using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using TK.BaseLib.Geometry;
using TK.BaseLib.Math3D;

namespace TK.BaseLib
{
    /// <summary>
    /// Simple static class to manage native types (lists, conversions...)
    /// </summary>
    public static class TypesHelper
    {
        // *** INT ***
        /// <summary>
        /// Join a int List as a string
        /// </summary>
        /// <param name="inList">List of int to be joined</param>
        /// <param name="inSeparator">Separator to use</param>
        /// <returns>the joined string</returns>
        public static string Join(List<int> inList, string inSeparator)
        {
            string pnts = "";
            if (inList.Count > 0)
            {
                foreach (int index in inList)
                {
                    pnts += index.ToString() + inSeparator;
                }

                pnts = pnts.Substring(0, pnts.Length - 1);
            }
            return pnts;
        }

        // *** INT ***
        /// <summary>
        /// Join a double array as a string with "," as separator
        /// </summary>
        /// <param name="inList">List of int to be joined</param>
        /// <returns>the joined string</returns>
        public static string Join(double[] inList)
        {
            return Join(inList, ",");
        }

        // *** INT ***
        /// <summary>
        /// Join a double array as a string
        /// </summary>
        /// <param name="inList">List of int to be joined</param>
        /// <param name="inSeparator">Separator to use</param>
        /// <returns>the joined string</returns>
        public static string Join(double[] inList, string inSeparator)
        {
            string pnts = "";
            if (inList.Length > 0)
            {
                foreach (double value in inList)
                {
                    pnts += value.ToString() + inSeparator;
                }

                pnts = pnts.Substring(0, pnts.Length - 1);
            }
            return pnts;
        }

        /// <summary>
        /// Join a int List as a string
        /// </summary>
        /// <param name="inList">List of int to be joined</param>
        /// <returns>the joined string, comma separated</returns>
        public static string Join(List<int> inList)
        {
            return Join(inList, ",");
        }

        /// <summary>
        /// Split a string as a int List
        /// </summary>
        /// <param name="inJoinedString">The joined string</param>
        /// <returns>The List of ints</returns>
        public static List<int> IntSplit(string inJoinedString)
        {
            return IntSplit(inJoinedString, ",", 0);
        }

        /// <summary>
        /// Split a string as a int List
        /// </summary>
        /// <param name="inJoinedString">The joined string</param>
        /// <param name="inSeparator">The separator used in the joined string</param>
        /// <param name="inDefaultValue">The default value used in case wa cannot parse as int</param>
        /// <returns>The List of ints</returns>
        public static List<int> IntSplit(string inJoinedString, string inSeparator, int inDefaultValue)
        {
            string[] saIndices = inJoinedString.Split(inSeparator.ToCharArray());
            List<int> Indices = new List<int>();

            foreach (string sIndex in saIndices)
            {
                int rslt = inDefaultValue;
                if (int.TryParse(sIndex, out rslt))
                {
                    Indices.Add(rslt);
                }
            }

            return Indices;
        }

        /// <summary>
        /// Remove a int List from another one
        /// </summary>
        /// <param name="inBaseList">The base list</param>
        /// <param name="inRemoveList">The list to be removed</param>
        /// <returns>The filtered list</returns>
        public static List<int> IntRemove(List<int> inBaseList, List<int> inRemoveList)
        {
            List<int> newIndices = new List<int>();

            foreach (int index in inBaseList)
            {
                if (!inRemoveList.Contains(index))
                {
                    newIndices.Add(index);
                }
            }

            return newIndices;
        }

        public static bool IntIsFuzzyEqual(int inValue, int inRefValue, int inTolerance)
        {
            if (inTolerance == 0)
            {
                return inValue == inRefValue;
            }

            return (Math.Abs(inRefValue - inValue) < inTolerance);
        }

        // *** DOUBLE ***

        /// <summary>
        /// Parse a double correctly whatever the decimal separator
        /// </summary>
        /// <param name="inStringDouble">The double value as string</param>
        /// <returns>The parsed double, or 0 if we cannot parse</returns>
        public static double DoubleParse(string inStringDouble)
        {
            return DoubleParse(inStringDouble, 0);
        }

        /// <summary>
        /// Parse a double correctly whatever the decimal separator
        /// </summary>
        /// <param name="inStringDouble">The double value as string</param>
        /// <param name="inDefaultValue">The default value if we cannot parse</param>
        /// <returns>The parsed double, or 'inDefaultValue' if we cannot parse</returns>
        public static double DoubleParse(string inStringDouble, double inDefaultValue)
        {
            double rslt = inDefaultValue;

            if (double.TryParse(inStringDouble, out rslt))
            {
                return rslt;
            }
            else
            {
                inStringDouble = inStringDouble.Replace(",", ".");

                if (double.TryParse(inStringDouble, out rslt))
                {
                    return rslt;
                }
                else
                {
                    inStringDouble = inStringDouble.Replace(".", ",");

                    if (double.TryParse(inStringDouble, out rslt))
                    {
                        return rslt;
                    }
                }
            }

            return rslt;
        }

        /// <summary>
        /// Tells if two doubles are Equal within a tolerance
        /// </summary>
        /// <param name="inValue">First double</param>
        /// <param name="inRefValue">Second double</param>
        /// <param name="inTolerance">Tolerance</param>
        /// <returns>true if doubles are Equal within the tolerance</returns>
        public static bool DoubleIsFuzzyEqual(double inValue, double inRefValue, double inTolerance)
        {
            if (inTolerance == 0)
            {
                return inValue == inRefValue;
            }

            return (Math.Abs(inRefValue - inValue) < inTolerance);
        }

        // *** FLOAT ***

        /// <summary>
        /// Parse a float correctly whatever the decimal separator
        /// </summary>
        /// <param name="inStringDouble">The float value as string</param>
        /// <returns>The parsed float, or 0 if we cannot parse</returns>
        public static float FloatParse(string inStringFloat)
        {
            return FloatParse(inStringFloat, 0);
        }

        /// <summary>
        /// Parse a double correctly whatever the decimal separator
        /// </summary>
        /// <param name="inStringDouble">The float value as string</param>
        /// <param name="inDefaultValue">The default value if we cannot parse</param>
        /// <returns>The parsed float, or 'inDefaultValue' if we cannot parse</returns>
        public static float FloatParse(string inStringFloat, float inDefaultValue)
        {
            float rslt = inDefaultValue;

            if (float.TryParse(inStringFloat, out rslt))
            {
                return rslt;
            }
            else
            {
                inStringFloat = inStringFloat.Replace(",", ".");

                if (float.TryParse(inStringFloat, out rslt))
                {
                    return rslt;
                }
                else
                {
                    inStringFloat = inStringFloat.Replace(".", ",");

                    if (float.TryParse(inStringFloat, out rslt))
                    {
                        return rslt;
                    }
                }
            }

            return rslt;
        }

        /// <summary>
        /// Tells if two floats are Equal within a tolerance
        /// </summary>
        /// <param name="inValue">First double</param>
        /// <param name="inRefValue">Second double</param>
        /// <param name="inTolerance">Tolerance</param>
        /// <returns>true if floats are Equal within the tolerance</returns>
        public static bool FloatIsFuzzyEqual(float inValue, float inRefValue, float inTolerance)
        {
            if (inTolerance == 0)
            {
                return inValue == inRefValue;
            }

            return (Math.Abs(inRefValue - inValue) < inTolerance);
        }

        // *** STRING ***

        /// <summary>
        /// Convert a string to a MemoryStream
        /// </summary>
        /// <param name="inStringValue">The string to convert</param>
        /// <returns>The converted Stream</returns>
        public static MemoryStream StringToStream(string inStringValue)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(inStringValue);
            return new MemoryStream( byteArray );
        }

        /// <summary>
        /// Convert a MemoryStream to a string
        /// </summary>
        /// <param name="inStreamValue">The Stream to convert</param>
        /// <returns>The converted string</returns>
        public static string StringFromStream(MemoryStream inStreamValue)
        {
            inStreamValue.Position = 0;
            StreamReader reader = new StreamReader(inStreamValue);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Replace words in a sentence, considering basic syntaxic breaks as separators : WORD followed by [/*,; )+-] and/or preceded by [(+-], and specified custom separators
        /// </summary>
        /// <param name="fullStr">String with words to replace</param>
        /// <param name="oldStr">String to be searched</param>
        /// <param name="newStr">String that will replace the old one</param>
        /// <returns>The replaced string</returns>
        public static string SmartReplace(string fullStr, string oldStr, string newStr)
        {
            return SmartReplace(fullStr, oldStr, newStr, 100, string.Empty, string.Empty);
        }

        /// <summary>
        /// Replace words in a sentence, considering basic syntaxic breaks as separators : WORD followed by [/*,; )+-] and/or preceded by [(+-], and specified custom separators
        /// </summary>
        /// <param name="fullStr">String with words to replace</param>
        /// <param name="oldStr">String to be searched</param>
        /// <param name="newStr">String that will replace the old one</param>
        /// <param name="maxOccurences">max number of times to loop</param>
        /// <returns>The replaced string</returns>
        public static string SmartReplace(string fullStr, string oldStr, string newStr, int maxOccurences)
        {
            return SmartReplace(fullStr, oldStr, newStr, maxOccurences, string.Empty, string.Empty);
        }

        /// <summary>
        /// Replace words in a sentence, considering basic syntaxic breaks as separators : WORD followed by [/*,; )+-] and/or preceded by [(+-], and specified custom separators
        /// </summary>
        /// <param name="fullStr">String with words to replace</param>
        /// <param name="oldStr">String to be searched</param>
        /// <param name="newStr">String that will replace the old one</param>
        /// <param name="maxOccurences">max number of times to loop</param>
        /// <param name="customBeforeChars">Custom separators before the word</param>
        /// <param name="customAfterChars">Custom separators after the word</param>
        /// <returns>The replaced string</returns>
        public static string SmartReplace(string fullStr, string oldStr, string newStr, int maxOccurences, string customBeforeChars, string customAfterChars)
        {
            int FoundCounter = 1;
            int SecurityCounter = 0;

            while (FoundCounter > 0)
            {
                Regex reg = new Regex("^" + oldStr + "[/*" + customAfterChars + ",; )+-]|[/*=, " + customBeforeChars + "(+\b-]" + oldStr + "[/*" + customAfterChars + ", );+-]", RegexOptions.None);
                Match regMatch = reg.Match(fullStr);
                foreach (Capture capt in regMatch.Captures)
                {
                    string captured = capt.Value.Replace(oldStr, newStr);
                    fullStr = fullStr.Replace(capt.Value, captured);
                }

                FoundCounter = regMatch.Captures.Count;

                //Secutity to get out the while in case we failed renaming
                if (SecurityCounter > maxOccurences)
                {
                    break;
                }

                SecurityCounter++;
            }

            return fullStr;
        }

        /// <summary>
        /// Split a string as a string List
        /// </summary>
        /// <param name="inString">The joined string</param>
        /// <returns>The List of strings</returns>
        public static List<string> StringSplit(string inString)
        {
            return StringSplit(inString, ",", true, true);
        }

        /// <summary>
        /// Split a string as a string List
        /// </summary>
        /// <param name="inString">The joined string</param>
        /// <returns>The List of strings</returns>
        /// <param name="inSeparator">the separator</param>
        /// <returns>The List of strings</returns>
        public static List<string> StringSplit(string inString, string inSeparator, bool inTrim, bool inIgnoreEmpty)
        {
            string[] saIndices = inString.Split(new string[] { inSeparator }, inIgnoreEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
            List<string> Strings = new List<string>();

            foreach (string sIndex in saIndices)
            {
                Strings.Add(inTrim ? sIndex.Trim() : sIndex);
            }

            return Strings;
        }

        /// <summary>
        /// Join a string List as a string
        /// </summary>
        /// <param name="inList">List of string to be joined</param>
        /// <returns>the joined string</returns>
        public static string Join(List<string> inList, string inSeparator)
        {
            string joined = "";
            if (inList.Count > 0)
            {
                foreach (string s in inList)
                {
                    joined += s + inSeparator;
                }

                joined = joined.Substring(0, joined.Length - inSeparator.Length);
            }

            return joined;
        }

        /// <summary>
        /// Join a string List as a string
        /// </summary>
        /// <param name="inList">List of string to be joined</param>
        /// <returns>the joined string, comma separated</returns>
        public static string Join(List<string> inList)
        {
            return Join(inList, ",");
        }

        /// <summary>
        /// Join a string List as a string
        /// </summary>
        /// <param name="inList">List of string to be joined</param>
        /// <returns>the joined string</returns>
        public static string Join(string[] inList, string inSeparator)
        {
            string joined = "";
            if (inList.Length > 0)
            {
                foreach (string s in inList)
                {
                    joined += s + inSeparator;
                }

                joined = joined.Substring(0, joined.Length - 1);
            }

            return joined;
        }

        /// <summary>
        /// Join a string List as a string
        /// </summary>
        /// <param name="inList">List of string to be joined</param>
        /// <returns>the joined string, comma separated</returns>
        public static string Join(string[] inList)
        {
            return Join(inList, ",");
        }

        /// <summary>
        /// Join a string List as a string
        /// </summary>
        /// <param name="inList">List of string to be joined</param>
        /// <returns>the joined string</returns>
        public static string Join(object[] inList, string inSeparator)
        {
            string joined = "";
            if (inList.Length > 0)
            {
                foreach (object s in inList)
                {
                    joined += s.ToString() + inSeparator;
                }

                joined = joined.Substring(0, joined.Length - 1);
            }

            return joined;
        }

        /// <summary>
        /// Join a string List as a string
        /// </summary>
        /// <param name="inList">List of string to be joined</param>
        /// <returns>the joined string, comma separated</returns>
        public static string Join(object[] inList)
        {
            return Join(inList, ",");
        }

        /// <summary>
        /// Copy a string List to a new one
        /// </summary>
        /// <param name="inSource">The source list</param>
        /// <returns>The copy</returns>
        public static List<string> Copy(List<string> inSource)
        {
            List<string> Strings = new List<string>();

            foreach (string s in inSource)
            {
                Strings.Add(s);
            }

            return Strings;
        }

        /// <summary>
        /// Merge a string list into another one
        /// </summary>
        /// <param name="inList">Base list</param>
        /// <param name="inMergedList">List to be merged</param>
        /// <returns>The merged list</returns>
        public static List<string> Merge(List<string> inList, List<string> inMergedList)
        {
            List<string> Strings = new List<string>();

            foreach (string s in inMergedList)
            {
                if (!inList.Contains(s))
                {
                    inList.Add(s);
                }
            }

            return inList;
        }

        public static string Truncate(string inString, int inMaxLen, bool inWarn)
        {
            if (string.IsNullOrEmpty(inString) || inString.Length <= inMaxLen)
                return inString;

            if(!inWarn)
                return inString.Substring(0, inMaxLen);

            return inString.Substring(0, inMaxLen) + "...(Truncated)";
        }

        public static string Truncate(string inString)
        {
            return Truncate(inString, 256, true);
        }

        /// <summary>
        /// Tells if two objects are Equal within a tolerance
        /// </summary>
        /// <param name="inValue">First object</param>
        /// <param name="inRefValue">Second object</param>
        /// <param name="inTolerance">Tolerance</param>
        /// <returns>true if object are Equal within the tolerance, false otherwise (or if types are different or unknown)</returns>
        public static bool ObjectIsFuzzyEqual(object inValue, object inRefValue, double inTolerance)
        {
            if (inTolerance == 0)
            {
                return inValue.Equals(inRefValue);
            }

            if (inValue is double)
            {
                if (!(inRefValue is double))
                {
                    return false;
                }

                return DoubleIsFuzzyEqual((double)inValue, (double)inRefValue, inTolerance);
            }
            else if (inValue is float)
            {
                if (!(inRefValue is float))
                {
                    return false;
                }

                return FloatIsFuzzyEqual((float)inValue, (float)inRefValue, (float)inTolerance);
            }
            else if (inValue is Int16 || inValue is Int32 || inValue is Int64)
            {
                if (!(inRefValue is Int16 || inRefValue is Int32 || inRefValue is Int64))
                {
                    return false;
                }

                return IntIsFuzzyEqual((int)inValue, (int)inRefValue, (int)inTolerance);
            }
            else if (inValue is bool)
            {
                if (!(inRefValue is bool))
                {
                    return false;
                }

                return (bool)inValue == (bool)inRefValue;
            }

            return false;
        }

        // *** DICT ***

        public static Dictionary<string, string> GetReversedDictionary(Dictionary<string, string> inDict)
        {
            Dictionary<string, string> reversedDict = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> entry in inDict)
            {
                if (!reversedDict.ContainsKey(entry.Value))
                    reversedDict.Add(entry.Value, entry.Key);
            }

            return reversedDict;
        }

        // *** CUSTOM TYPES ***
        public static List<CG_Vector3> GeometryToListVector(CG_Geometry inGeo)
        {
            List<CG_Vector3> positions = new List<CG_Vector3>();

            double[] pointPos = inGeo.PointPositions;
            int count = inGeo.PointsCount;

            for (int i = 0; i < count; i++)
            {
                positions.Add(new CG_Vector3(pointPos[i*3], pointPos[i*3 + 1], pointPos[i*3 + 2]));
            }

            return positions;
        }

        public static CG_Curve ListVectorToGeometry(List<CG_Vector3> inPoints)
        {
            CG_Curve crv = new CG_Curve();

            double[] pointPos = new double[3 * inPoints.Count];

            int i = 0;
            foreach (CG_Vector3 vec in inPoints)
            {
                pointPos[i * 3] = vec.X;
                pointPos[i * 3 + 1] = vec.Y;
                pointPos[i * 3 + 2] = vec.Z;
                i++;
            }

            crv.PointPositions = pointPos;

            return crv;
        }

        public static string NiceStackTrace(System.Diagnostics.StackTrace inStack, int inDepth, int skipTop)
        {
            StringBuilder sb = new StringBuilder();

            int length = inDepth > 0 ? Math.Min(inDepth, inStack.FrameCount) : inStack.FrameCount;

            skipTop = Math.Max(skipTop, 0);
            skipTop = Math.Min(skipTop, length - 1);

            for (int i = skipTop; i < length; i++)
            {
                if (i > skipTop)
                    sb.Append(" > ");

                System.Diagnostics.StackFrame frame = inStack.GetFrame(i);

                sb.Append(frame.GetMethod());
            }

            return sb.ToString();
        }

        public static string NiceStackTrace(System.Diagnostics.StackTrace inStack, int skipTop)
        {
            return NiceStackTrace(inStack, -1, skipTop);
        }
    }
}
