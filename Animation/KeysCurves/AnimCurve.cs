using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation.KeysCurves
{
    public class AnimCurve
    {
        string _channelName = "unkwnown";
        public string ChannelName
        {
            get { return _channelName; }
            set { _channelName = value; }
        }
        string _channelAccessName = "unkwnown";
        public string ChannelAccessName
        {
            get { return _channelAccessName; }
            set { _channelAccessName = value; }
        }
        string _channelParent = "unkwnown";
        public string ChannelParent
        {
            get { return _channelParent; }
            set { _channelParent = value; }
        }

        

        List<Key> _keys = new List<Key>();
        public List<Key> Keys
        {
            get { return _keys; }
            set
            {
                _keys = value;
                _keys.Sort();
            }
        }

        public void AddKey(double inTime, object inValue)
        {
            Keys.Add(new Key(inTime, inValue));
        }

        public object[] GetObjectArray()
        {
            object[] keyValues = new object[_keys.Count * 2];

            for (int counter = 0; counter < _keys.Count; counter++)
            {
                keyValues[counter * 2] = _keys[counter].Time;
                keyValues[counter * 2 + 1] = _keys[counter].Value;
            }

            return keyValues;
        }

        public void Offset(double inTimeOffset)
        {
            foreach (Key key in _keys)
            {
                key.Time += inTimeOffset;
            }
        }

        public void Retime(double inRetime, double inRef)
        {
            foreach (Key key in _keys)
            {
                key.Time = (key.Time - inRef) / inRetime + inRef;
            }
        }
    }
}
