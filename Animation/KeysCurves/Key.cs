using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation.KeysCurves
{
    public enum Interpolations
    {
        Linear, Cubic, Unknown
    }

    public class Key : IComparable
    {
        public Key()
        {
        }

        public Key(double inTime, object inValue, double inbTangentTime, object inbTangentValue, double inaTangentTime, object inaTangentValue, Interpolations inInterpolation)
        {
            _time = inTime;
            _value = inValue;

            _interpolation = inInterpolation;

            _bTangentTime = inbTangentTime;
            _bTangentValue = inbTangentValue;
            _aTangentTime = inaTangentTime;
            _aTangentValue = inaTangentValue;
        }

        public Key(double inTime, object inValue, double inbTangentTime, object inbTangentValue, double inaTangentTime, object inaTangentValue)
            : this(inTime, inValue, inbTangentTime, inbTangentValue, inaTangentTime, inaTangentValue, Interpolations.Cubic)
        {

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

        public Interpolations Interpolation
        {
            get { return _interpolation; }
            set { _interpolation = value; }
        }

        public double BTangentTime
        {
            get { return _bTangentTime; }
            set { _bTangentTime = value; }
        }

        public double ATangentTime
        {
            get { return _aTangentTime; }
            set { _aTangentTime = value; }
        }

        public object BTangentValue
        {
            get { return _bTangentValue; }
            set { _bTangentValue = value; }
        }

        public object ATangentValue
        {
            get { return _aTangentValue; }
            set { _aTangentValue = value; }
        }

        object _value;
        double _time;
        Interpolations _interpolation = Interpolations.Cubic;

        double _bTangentTime;
        object _bTangentValue;
        double _aTangentTime;
        object _aTangentValue;

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

        internal void Offset(double inTimeOffset)
        {
            _time += inTimeOffset;
        }

        internal void Retime(double inRetime, double inRef)
        {
            _time = (_time - inRef) / inRetime + inRef;
            _bTangentTime /= inRetime;
            _aTangentTime /= inRetime;
        }

        internal void Scale(double inScale, double inRef)
        {
            if (_value is double)
            {
                double doubleValue = (double)_value;
                double doublebValue = (double)_bTangentValue;
                double doubleaValue = (double)_aTangentValue;

                _value = (doubleValue - inRef) * inScale + inRef;
                _bTangentValue = doublebValue * inScale;
                _aTangentValue = doubleaValue * inScale;
            }
        }
    }
}
