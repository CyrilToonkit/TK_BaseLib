using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TK.BaseLib.CustomData
{
    public class PresetCollection
    {
        public PresetCollection(string inName, string inPath)
        {
            mName = inName;
            mPath = inPath;

            LoadPresets();
        }

        public void LoadPresets()
        {
            Clear();
            DirectoryInfo dir = new DirectoryInfo(mPath + "\\" + Name);

            if (dir.Exists)
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Extension == ".xml")
                    {
                        FileStream stream = null;
                        try
                        {
                            stream = file.OpenRead();
                            KeyValuePreset preset = (KeyValuePreset)serializer.Deserialize(stream);
                            preset.SyncDic();
                            PushPreset(preset.Name, preset);
                            stream.Close();
                        }
                        catch (Exception) { if (stream != null) { stream.Close(); } }
                    }
                }
            }
        }

        public void SavePreset(string inName, bool inOverWrite)
        {
            KeyValuePreset preset = GetPreset(inName);

            if(preset != null)
            {
                DirectoryInfo dir = new DirectoryInfo(mPath + "\\" + Name);

                if (!dir.Exists)
                {
                    dir.Create();
                }

                FileInfo curInfo = new FileInfo(dir.FullName + "\\" + inName + ".xml");
                if (!curInfo.Exists || inOverWrite)
                {
                    FileStream stream = null;
                    try
                    {
                        stream = curInfo.Open(FileMode.Create);
                        serializer.Serialize(stream, preset);
                        stream.Close();
                    }
                    catch (Exception) { if (stream != null) { stream.Close(); } }
                }
            }
        }

        public void Clear()
        {
            Presets.Clear();
            Keys.Clear();
            presets.Clear();
        }

        string mPath = "";

        string mName = "";
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        Dictionary<string, KeyValuePreset> presets = new Dictionary<string, KeyValuePreset>();

        List<string> mKeys = new List<string>();
        public List<string> Keys
        {
            get { return mKeys; }
            set { mKeys = value; }
        }

        List<KeyValuePreset> mPresets = new List<KeyValuePreset>();
        public List<KeyValuePreset> Presets
        {
            get { return mPresets; }
            set { mPresets = value; }
        }

        XmlSerializer serializer = new XmlSerializer(typeof(KeyValuePreset));

        public void PushPreset(string inKey, KeyValuePreset inValue)
        {
            inValue.Name = inKey;

            if (presets.ContainsKey(inKey))
            {
                presets[inKey] = inValue;
            }
            else
            {
                Keys.Add(inKey);
                Presets.Add(inValue);
                presets.Add(inKey, inValue);
            }

            presets[inKey].Collection = this;
        }

        public KeyValuePreset GetPreset(string inKey)
        {
            if (presets.ContainsKey(inKey))
            {
                return presets[inKey];
            }

            return null;
        }

        public bool HasPreset(string inKey)
        {
            return presets.ContainsKey(inKey);
        }

        public void DeletePreset(KeyValuePreset preset)
        {
            if (Presets.Contains(preset))
            {
                Presets.Remove(preset);
                Keys.Remove(preset.Name);
                presets.Remove(preset.Name);
            }

            FileInfo curInfo = new FileInfo(mPath + "\\" + Name + "\\" + preset.Name + ".xml");
            if (curInfo.Exists)
            {
                curInfo.Delete();
            }
        }
    }
}
