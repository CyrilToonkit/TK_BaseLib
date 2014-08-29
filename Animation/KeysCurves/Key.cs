using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation.KeysCurves
{
    public class Key : IComparable
    {
        public Key()
        {
        }

        public Key(double inTime, object inValue)
        {
            _time = inTime;
            _value = inValue;
        }

        public double Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public Key BTangent
        {
            get { return _bTangent; }
            set { _bTangent = value; }
        }

        public Key ATangent
        {
            get { return _aTangent; }
            set { _aTangent = value; }
        }

        object _value;
        double _time;
        Key _bTangent;
        Key _aTangent;

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            Key otherKey = obj as Key;

            if (otherKey == null)
            {
                return 1;
            }

            return _time.CompareTo(otherKey.Time);
        }

        #endregion
    }
}
