using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TK.BaseLib.CustomData
{
    public class KeyValuePreset
    {
        PresetCollection mCollection = null;
        [XmlIgnore]
        public PresetCollection Collection
        {
            get { return mCollection; }
            set { mCollection = value; }
        }

        string mName = "";
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        bool mDefault = false;
        public bool Default
        {
            get { return mDefault; }
            set { mDefault = value; }
        }

        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        List<string> mKeys = new List<string>();
        public List<string> Keys
        {
            get { return mKeys; }
            set { mKeys = value; }
        }

        List<object> mValues = new List<object>();
        public List<object> Values
        {
            get { return mValues; }
            set { mValues = value; }
        }

        public void Clear()
        {
            dictionary.Clear();
            Keys.Clear();
            Values.Clear();
        }

        public void PushItem(string inKey, object inValue)
        {
            if (dictionary.ContainsKey(inKey))
            {
                dictionary[inKey] = inValue;
            }
            else
            {
                Keys.Add(inKey);
                Values.Add(inValue);
                dictionary.Add(inKey, inValue);
            }
        }

        public object GetValue(string inKey)
        {
            if (dictionary.ContainsKey(inKey))
            {
                return dictionary[inKey];
            }

            return null;
        }

        public void SyncDic()
        {
            dictionary.Clear();

            int counter = 0;

            foreach (string presetName in Keys)
            {
                dictionary.Add(presetName, Values[counter]);
                counter++;
            }
        }
    }
}
