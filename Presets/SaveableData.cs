using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;

//COMMENTS OK
namespace TK.BaseLib
{
    /// <summary>
    /// Precursor for Preferences classes
    /// Simply define a Data which can be Saved, Loaded and Cloned. Automate the process of Serializing
    /// to file and Deserializing from file classes that can use default serialization
    /// Override Clone : set every properties to the SaveableData in argument
    /// Override ToDefault : set every properties to default values
    /// </summary>
    public class SaveableData
    {
        // == METHODS =====================================================================

        /// <summary>
        /// Serialize the Data to a file, Always overwriting
        /// </summary>
        /// <param name="inPath">Path to save the file to</param>
        /// <returns>True in case of Success</returns>
        public bool Save(string inPath)
        {
            return Save(inPath, true);
        }

        /// <summary>
        /// Serialize the Data to a file
        /// </summary>
        /// <param name="inPath">Path to save the file to</param>
        /// <param name="inOverWrite">OverWrite if exists</param>
        /// <returns>True in case of success</returns>
        public bool Save(string inPath, bool inOverWrite)
        {
            FileInfo ThisFileInfo = new FileInfo(inPath);

            if (inOverWrite || !(ThisFileInfo.Exists))
            {
                XmlSerializer thisSaveable = new XmlSerializer(GetType());
                DirectoryInfo parentDir = ThisFileInfo.Directory;

                if (!parentDir.Exists)
                {
                    parentDir.Create();
                }

                if (ThisFileInfo.Exists)
                {
                    ThisFileInfo.Delete();
                }

                FileStream stream = ThisFileInfo.OpenWrite();

                try
                {
                    thisSaveable.Serialize(stream, this);
                    stream.Close();
                    return true;
                }
                catch (Exception)
                {
                    if(stream != null)
                    {
                        stream.Close();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Deserialize the Data to a file, Always overwriting
        /// </summary>
        /// <param name="inPath">Path to load the file from</param>
        /// <returns>True in case of Success</returns>
        public bool Load(string inPath)
        {
            FileInfo ThisFileInfo = new FileInfo(inPath);
            
            if (ThisFileInfo.Exists)
            {
                XmlSerializer thisSaveable = new XmlSerializer(GetType());
                FileStream stream = ThisFileInfo.OpenRead();

                try
                {
                    SaveableData loaded = (SaveableData)thisSaveable.Deserialize(stream);
                    Clone(loaded);
                    stream.Close();
                    return true;
                }
                catch (Exception)
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Clone the values from an instance to another
        /// Classes that inherits from SaveAbleData must override this and read values from 'Data'
        /// </summary>
        /// <param name="Data">Data to get the values from</param>
        public virtual void Clone(SaveableData Data)
        {

        }

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
                inFont.Name, inFont.Size, inFont.Style);
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

            return new Font(pieces[0], float.Parse(pieces[1]), style);

        }
    }
}
