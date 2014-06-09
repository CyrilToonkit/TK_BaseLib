using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace TK.BaseLib.Checking
{
    public static class CheckHelper
    {
        static XmlSerializer serializer = null;

        /// <summary>
        /// Serialize the Data to a file
        /// </summary>
        /// <param name="inPath">Path to save the file to</param>
        /// <param name="inOverWrite">OverWrite if exists</param>
        /// <returns>True in case of success</returns>
        public static bool Save(CheckList inCheckList, string inPath, bool inOverWrite)
        {
            FileInfo ThisFileInfo = new FileInfo(inPath);

            if (inOverWrite || !(ThisFileInfo.Exists))
            {
                if (serializer == null)
                {
                    serializer = new XmlSerializer(typeof(CheckList));
                }

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
                    serializer.Serialize(stream, inCheckList);
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
        /// Deserialize the Data to a file, Always overwriting
        /// </summary>
        /// <param name="inPath">Path to load the file from</param>
        /// <returns>True in case of Success</returns>
        public static CheckList Load(string inPath)
        {
            FileInfo ThisFileInfo = new FileInfo(inPath);

            if (ThisFileInfo.Exists)
            {
                if (serializer == null)
                {
                    serializer = new XmlSerializer(typeof(CheckList));
                }

                FileStream stream = ThisFileInfo.OpenRead();

                try
                {
                    CheckList loaded = (CheckList)serializer.Deserialize(stream);
                    stream.Close();
                    return loaded;
                }
                catch (Exception)
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }

            return null;
        }
    }
}
