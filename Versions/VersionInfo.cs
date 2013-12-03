using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//COMMENTS OK
namespace TK.BaseLib.Versions
{
    /// <summary>
    /// Helps in keeping track of version changes in "SetupManager" and "RigElements" (XSI Specific)
    /// the principle is to read an embedded xml (ressources) to retrieve "Version" objects.
    /// </summary>
    public class VersionsInfo
    {
        // == MEMBERS =====================================================================

        /// <summary>
        /// The list of versions
        /// </summary>
        public List<Version> Versions = new List<Version>();

        /// <summary>
        /// Quick accessor for the formatted latest version name
        /// </summary>
        public string CurrentVersion
        {
            get { return Versions[0].GetVersion(); }
        }

        /// <summary>
        /// Quick accessor for the formatted latest version name, including the build
        /// </summary>
        public string CurrentFullVersion
        {
            get { return CurrentVersion + "." + Versions[0].Build; }
        }

        /// <summary>
        /// Quick accessor for the formatted Date of the latest version
        /// </summary>
        public string CurrentDate
        {
            get { return Versions[0].GetDate(); }
        }

        // == METHODS =====================================================================

        /// <summary>
        /// To get the versions text to be displayed in the About Form (SetupManager)
        /// </summary>
        /// <returns>The versions text with a bit of formatting</returns>
        public string GetVersionTracks()
        {
            string VersionTracks = "";
            int CurrentMajor = -1;
            int CurrentMinor = -1;

            foreach (Version version in Versions)
            {
                if (version.BuildComment != "")
                {
                    if (version.Major != CurrentMajor)
                    {
                        VersionTracks += ":: " + version.GetVersion() + " ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::\n";
                        CurrentMajor = version.Major;
                        CurrentMinor = -1;
                        VersionTracks += "\n";
                    }

                    if (version.Minor != CurrentMinor)
                    {
                        VersionTracks += "  -- " + version.Minor.ToString() + " ------------------------------------------------------------\n";
                        CurrentMinor = version.Minor;
                        VersionTracks += "\n";
                    }

                    string[] comments = version.BuildComment.Split("|".ToCharArray());

                    VersionTracks += "   R" + version.Build.ToString() + " : " + version.BuildDate.ToShortDateString() + "\n";

                    for (int i = 0; i < comments.Length; i++)
                    {
                        VersionTracks += "      " + comments[i] + "\n";
                    }
                }

                VersionTracks += "\n";
            }

            return VersionTracks;
        }

        /// <summary>
        /// Load the Versions List
        /// </summary>
        /// <param name="versionInfoXml">The serialized VersionInfo</param>
        public void Load(string versionInfoXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Version));

            Versions.Clear();
            Version ver;

            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(versionInfoXml));
            XmlReader reader = XmlReader.Create(stream);

            reader.MoveToContent();
            reader.Read();

            while (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Version")
            {
                ver = (Version)serializer.Deserialize(reader);
                Versions.Add(ver);
            }

            reader.Close();
        }
    }
}