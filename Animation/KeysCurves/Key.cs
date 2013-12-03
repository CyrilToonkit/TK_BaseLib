using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation.KeysCurves
{
    public class Key : IComparable
    {
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

        object _value;
        double _time;

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
