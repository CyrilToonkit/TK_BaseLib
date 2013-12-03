using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Geometry
{
    public class CG_Curve : CG_Geometry
    {
        bool mClosed = false;
        public bool Closed
        {
            get { return mClosed; }
            set { mClosed = value; }
        }

        int mDegree = 3;
        public int Degree
        {
            get { return mDegree; }
            set { mDegree = value; }
        }

        public override bool IsValid()
        {
            return (PointPositions.Length > 5 && PointPositions.Length % 3 == 0);
        }
    }
}
