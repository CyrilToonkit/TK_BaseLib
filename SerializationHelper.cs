﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TK.BaseLib
{
    public static class SerializationHelper
    {
        /// <summary>
        /// Helper for Color Serialization
        /// </summary>
        /// <param name="color">The Color</param>
        /// <returns>The serialized color</returns>
        public static string SerializeColor(Color color)
        {
            if (color.IsNamedColor)
                return string.Format("{0}:{1}",
                    ColorFormat.NamedColor, color.Name);
            else
                return string.Format("{0}:{1}:{2}:{3}:{4}",
                    ColorFormat.ARGBColor,
                    color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Helper for Color Deserialization
        /// </summary>
        /// <param name="color">The serialized color</param>
        /// <returns>The Color</returns>
        public static Color DeserializeColor(string color)
        {
            byte a, r, g, b;

            string[] pieces = color.Split(new char[] { ':' });

            ColorFormat colorType = (ColorFormat)
                Enum.Parse(typeof(ColorFormat), pieces[0], true);

            switch (colorType)
            {
                case ColorFormat.NamedColor:
                    return Color.FromName(pieces[1]);

                case ColorFormat.ARGBColor:
                    a = byte.Parse(pieces[1]);
                    r = byte.Parse(pieces[2]);
                    g = byte.Parse(pieces[3]);
                    b = byte.Parse(pieces[4]);

                    return Color.FromArgb(a, r, g, b);
            }
            return Color.Empty;
        }

        /// <summary>
        /// Helper for Font Serialization
        /// </summary>
        /// <param name="color">The Font</param>
        /// <returns>The serialized font</returns>
        public static string SerializeFont(Font inFont)
        {
            if (inFont == null)
            {
                return string.Empty;
            }

            return string.Format("{0}:{1}:{2}",
                inFont.Name, inFont.Size.ToString().Replace(",","."), inFont.Style);
        }

        /// <summary>
        /// Helper for Font Deserialization
        /// </summary>
        /// <param name="color">The serialized font</param>
        /// <returns>The Font</returns>
        public static Font DeserializeFont(string font)
        {
            Font def = new Font("Arial", 9, FontStyle.Regular);

            if (string.IsNullOrEmpty(font))
            {
                return def;
            }

            string[] pieces = font.Split(new char[] { ':' });
            FontStyle style = (FontStyle)
                Enum.Parse(typeof(FontStyle), pieces[2], true);

            return new Font(pieces[0], TypesHelper.FloatParse(pieces[1]), style);
        }

        /// <summary>
        /// Helper for Dictionary Serialization
        /// </summary>
        /// <param name="inDic">The Dictionary</param>
        /// <returns>The serialized Dictionary</returns>
        public static string SerializeDictionary(Dictionary<string,string> inDic)
        {
            List<string> serializedItems = new List<string>();

            foreach (string key in inDic.Keys)
            {
                serializedItems.Add(string.Format("{0}:DicValue:{1}", key, inDic[key]));
            }

            return TypesHelper.Join(serializedItems, "|DicItem|");
        }

        /// <summary>
        /// Helper for Dictionary Serialization
        /// </summary>
        /// <param name="inDic">The Dictionary</param>
        /// <returns>The serialized Dictionary</returns>
        public static string SerializeDictionary(SortedDictionary<string, string> inDic)
        {
            List<string> serializedItems = new List<string>();

            foreach (string key in inDic.Keys)
            {
                serializedItems.Add(string.Format("{0}:DicValue:{1}", key, inDic[key]));
            }

            return TypesHelper.Join(serializedItems, "|DicItem|");
        }

        /// <summary>
        /// Helper for Dictionary Deserialization
        /// </summary>
        /// <param name="inDic">The serialized Dictionary</param>
        /// <returns>The Dictionary</returns>
        public static Dictionary<string, string> DeserializeDictionary(string inDic)
        {
            Dictionary<string,string> dic = new Dictionary<string,string>();

            if (string.IsNullOrEmpty(inDic))
            {
                return dic;
            }

            List<string> stringSplit = TypesHelper.StringSplit(inDic, "|DicItem|", false, true);

            foreach (string stringItem in stringSplit)
            {
                List<string> itemSplit = TypesHelper.StringSplit(stringItem, ":DicValue:", false, false);

                if (itemSplit.Count == 2)
                {
                    dic.Add(itemSplit[0], itemSplit[1]);
                }
            }

            return dic;
        }

        /// <summary>
        /// Helper for Dictionary Deserialization
        /// </summary>
        /// <param name="inDic">The serialized Dictionary</param>
        /// <returns>The Dictionary</returns>
        public static SortedDictionary<string, string> DeserializeSortedDictionary(string inDic)
        {
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            if (string.IsNullOrEmpty(inDic))
            {
                return dic;
            }

            List<string> stringSplit = TypesHelper.StringSplit(inDic, "|DicItem|", false, true);

            foreach (string stringItem in stringSplit)
            {
                List<string> itemSplit = TypesHelper.StringSplit(stringItem, ":DicValue:", false, false);

                if (itemSplit.Count == 2)
                {
                    dic.Add(itemSplit[0], itemSplit[1]);
                }
            }

            return dic;
        }

        public static string ToCsv(OrderedDictionary<string, string> mapping)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in mapping.Keys)
            {
                sb.AppendFormat("{0},{1}\n", key, mapping[key]);
            }

            return sb.ToString();
        }

        public static OrderedDictionary<string, string> SortedDictionaryFromCsv(string inCsv)
        {
            OrderedDictionary<string, string> dic = new OrderedDictionary<string, string>();

            foreach (string line in inCsv.Split("\n".ToCharArray()))
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                string cleanLine = line;
                if (cleanLine.EndsWith("\r"))
                {
                    cleanLine = cleanLine.Substring(0, line.Length - 1);
                }
                string[] lineSplit = cleanLine.Split(",".ToCharArray());
                dic.Add(lineSplit[0], lineSplit[1]);
            }

            return dic;
        }
    }
}
