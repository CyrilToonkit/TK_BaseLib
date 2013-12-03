using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation.KeysCurves
{
    public class AnimCurve
    {
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
    }
}
