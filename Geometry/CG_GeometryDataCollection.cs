using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TK.BaseLib.Geometry
{
    public class CG_GeometryDataCollection
    {
        public const string EnvelopeType = "Envelope";

        List<CG_GeometryData> mGeometriesData = new List<CG_GeometryData>();
        public List<CG_GeometryData> GeometriesData
        {
            get { return mGeometriesData; }
            set { mGeometriesData = value; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return mGeometriesData.Count; }
        }

        [XmlIgnore]
        public int Compression
        {
            set
            {
                foreach(CG_GeometryData geoData in mGeometriesData)
                {
                    foreach(CG_PointData pointData in geoData.Data)
                    {
                        pointData.Compression = value;
                    }
                }
            }
        }

        public void Add(CG_GeometryData data)
        {
            mGeometriesData.Add(data);
            mGeosDic.Add(data.Geometry.AccessName, data);
            if (!mGeosDic.ContainsKey(data.Geometry.Name))
            {
                mGeosDic.Add(data.Geometry.Name, data);
            }
        }

        Dictionary<string, CG_GeometryData> mGeosDic = new Dictionary<string,CG_GeometryData>();

        public void UpdateDic()
        {
            mGeosDic = new Dictionary<string, CG_GeometryData>();

            foreach (CG_GeometryData data in GeometriesData)
            {
                mGeosDic.Add(data.Geometry.AccessName, data);
            }
        }

        public CG_GeometryData GetObject(string AccessName, string Name)
        {
            //AccessName pass
            if (mGeosDic.ContainsKey(AccessName))
            {
               return mGeosDic[AccessName];
            }

            //Name pass
            foreach (CG_GeometryData data in GeometriesData)
            {
                if (data.Geometry.Name == Name)
                {
                    return data;
                }
            }

            return null;
        }

        public CG_PointData GetObjectData(string AccessName, string Name, int NbPoints)
        {
            CG_GeometryData data = GetObject(AccessName, Name);

            if (data == null)
            {
                data = GetFirstValidObject(NbPoints);
            }

            if (data != null && data.Data.Count > 0)
            {
                return data.Data[0];
            }

            return null;
        }

        public void SetObjectData(string AccessName, string Name, CG_PointData pointData)
        {
            foreach (CG_GeometryData geo in GeometriesData)
            {
                if (geo.Geometry.Name == Name)
                {
                    geo.Data.Clear();
                    geo.Data.Add(pointData);
                    break;
                }
            }
        }

        public CG_GeometryData GetFirstValidObject(int NbPoints)
        {
            foreach (CG_GeometryData data in mGeometriesData)
            {
                if (data.Data.Count > 0 && data.Data[0].PointCount == NbPoints)
                {
                    return data;
                }
            }

            return null;
        }

        public CG_PointData GetObjectData(string AccessName, string Name, string type)
        {
            CG_GeometryData data = GetObject(AccessName, Name);

            if (data != null)
            {
                foreach (CG_PointData pointData in data.Data)
                {
                    if (pointData.Type == type)
                    {
                        return pointData;
                    }
                }
            }

            return null;
        }

        public CG_PointData GetObjectData(string AccessName, string Name)
        {
            return GetObjectData(AccessName, Name, EnvelopeType);
        }

        public bool HasObject(string AccessName)
        {
            return mGeosDic.ContainsKey(AccessName);
        }

        public void RenameDeformers(string ObjectName, string Name, string value)
        {
            foreach (CG_GeometryData geo in GeometriesData)
            {
                if (geo.Geometry.Name == ObjectName)
                {
                    foreach(CG_PointData pData in geo.Data)
                    {
                        for (int i = 0; i < pData.Fields.Count; i++ )
                        {
                            pData.Fields[i] = pData.Fields[i].Replace(Name, value);
                        }
                    }
                    break;
                }
            }
        }

        public bool Save(string inPath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(CG_GeometryDataCollection));
            FileInfo file = new FileInfo(inPath);
            Stream fileStream = null;

            bool success = false;

            try
            {
                if (!file.Directory.Exists)
                {
                    file.Directory.Create();
                }
                else
                {
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }

                fileStream = file.OpenWrite();
                ser.Serialize(fileStream, this);
                success = true;
            }
            catch (Exception) {}

            if (fileStream != null)
            {
                fileStream.Close();
            }

            return success;
        }

        public static CG_GeometryDataCollection Load(string inPath)
        {
            CG_GeometryDataCollection loaded = null;

            XmlSerializer ser = new XmlSerializer(typeof(CG_GeometryDataCollection));
            FileInfo file = new FileInfo(inPath);
            Stream fileStream = null;

            if (file.Exists)
            {
                try
                {
                    fileStream = file.OpenRead();
                    loaded = (CG_GeometryDataCollection)ser.Deserialize(fileStream);
                }
                catch (Exception) {}

                if (fileStream != null)
                {
                    fileStream.Close();
                }

                return loaded;
            }

            return null;
        }
    }
}
