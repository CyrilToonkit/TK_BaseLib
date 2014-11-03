using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation.KeysCurves
{
    public enum Extrapolations
    {
        Constant, Linear, Cycle, RelativeCycle
    }

    public class AnimCurve
    {
        public AnimCurve()
        {
        }

        public AnimCurve(string inChannelName, string inChannelAccessName, string inChannelParent)
        {
            _channelName = inChannelName;
            _channelAccessName = inChannelAccessName;
            _channelParent = inChannelParent;
        }

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

        object _staticValue = null;
        public object StaticValue
        {
            get { return _staticValue; }
            set { _staticValue = value; }
        }

        Extrapolations _bExtrapolation = Extrapolations.Constant;
        public Extrapolations BExtrapolation
        {
            get { return _bExtrapolation; }
            set { _bExtrapolation = value; }
        }

        Extrapolations _aExtrapolation = Extrapolations.Constant;
        public Extrapolations AExtrapolation
        {
            get { return _aExtrapolation; }
            set { _aExtrapolation = value; }
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

        public void AddKey(double inTime, object inValue, double inbTangentTime, object inbTangentValue, double inaTangentTime, object inaTangentValue, Interpolations inInterpolation)
        {
            Keys.Add(new Key(inTime, inValue, inbTangentTime, inbTangentValue, inaTangentTime, inaTangentValue, inInterpolation));
        }

        public void AddKey(double inTime, object inValue, double inbTangentTime, object inbTangentValue, double inaTangentTime, object inaTangentValue)
        {
            Keys.Add(new Key(inTime, inValue, inbTangentTime, inbTangentValue, inaTangentTime, inaTangentValue));
        }

        public void AddKey(double inTime, object inValue)
        {
            Keys.Add(new Key(inTime, inValue, 0.0, 0.0, 0.0, 0.0));
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
                key.Offset(inTimeOffset);
            }
        }

        public void Retime(double inRetime, double inRef)
        {
            foreach (Key key in _keys)
            {
                key.Retime(inRetime, inRef);
            }
        }

        public void Scale(double inScale, double inRef)
        {
            foreach (Key key in _keys)
            {
                key.Scale(inScale, inRef);
            }
        }
    }
}
