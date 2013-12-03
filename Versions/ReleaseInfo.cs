using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TK.BaseLib.Versions
{
    public class ReleaseInfo
    {
        string mCompany = "Toonkit";
        public string Company
        {
            get { return mCompany; }
            set { mCompany = value; }
        }

        string msBuildDate = "2012-01-27";
        public string sBuildDate
        {
            get { return msBuildDate; }
            set { msBuildDate = value; }
        }

        [XmlIgnore]
        public DateTime BuildDate
        {
            get { return DateTime.ParseExact(msBuildDate, "yyyy-MM-dd", null); }
        }

        string mVersion = "1.0.26";
        public string Version
        {
            get { return mVersion; }
            set { mVersion = value; }
        }

        string mHost = "Softimage 2012 SP1 (64-bit)";
        public string Host
        {
            get { return mHost; }
            set { mHost = value; }
        }

        int mLength = 60;
        public int Length
        {
            get { return mLength; }
            set { mLength = value; }
        }

        /// <summary>
        /// Load the release Info
        /// </summary>
        /// <param name="versionInfoXml">The serialized releaseInfo</param>
        public void Load(string releaseInfoXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ReleaseInfo));

            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(releaseInfoXml));
            XmlReader reader = XmlReader.Create(stream);

            ReleaseInfo newInfo = (ReleaseInfo)serializer.Deserialize(reader);

            reader.Close();

            mCompany = newInfo.Company;
            msBuildDate = newInfo.sBuildDate;
            mVersion = newInfo.Version;
            mHost = newInfo.Host;
            mLength = newInfo.Length;
        }

        /// <summary>
        /// Load the release Info From File
        /// </summary>
        /// <param name="versionInfoXml">The serialized releaseInfo path</param>
        public void LoadFromFile(string releaseInfoXmlPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ReleaseInfo));

            XmlReader reader = XmlReader.Create(releaseInfoXmlPath);

            ReleaseInfo newInfo = (ReleaseInfo)serializer.Deserialize(reader);

            reader.Close();

            mCompany = newInfo.Company;
            msBuildDate = newInfo.sBuildDate;
            mVersion = newInfo.Version;
            mHost = newInfo.Host;
            mLength = newInfo.Length;
        }

        /// <summary>
        /// Load the release Info
        /// </summary>
        /// <param name="versionInfoXml">The serialized releaseInfo path</param>
        public void Save(string releaseInfoXmlPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ReleaseInfo));

            StreamWriter writer = new StreamWriter(releaseInfoXmlPath, false);

            serializer.Serialize(writer, this);

            writer.Close();
        }
    }
}
