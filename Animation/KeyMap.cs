using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation
{
    public class KeyMap : IComparable<KeyMap>
    {
        public KeyMap(double inRealKey, double inWarpKey)
        {
            m_realKey = inRealKey;
            m_warpKey = inWarpKey;
        }

        double m_realKey;
        public double RealKey
        {
            get { return m_realKey; }
            set { m_realKey = value; }
        }

        double m_warpKey;
        public double WarpKey
        {
            get { return m_warpKey; }
            set { m_warpKey = value; }
        }

        int m_index = 0;
        public int Index
        {
            get { return m_index; }
            set { m_index = value; }
        }


        #region IComparable<KeyMap> Members

        public int CompareTo(KeyMap other)
        {
            return Index.CompareTo(other.Index);
        }

        #endregion
    }
}
