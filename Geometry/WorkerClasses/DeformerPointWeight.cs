using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Geometry
{
    public class DeformerPointWeight : IComparable
    {
        public DeformerPointWeight(int inDeformerIndex,int inPointIndex, double inWeight)
        {
            mDeformerIndex = inDeformerIndex;
            mPointIndex = inPointIndex;
            mWeight = inWeight;
        }

        int mDeformerIndex = -1;
        public int DeformerIndex
        {
            get { return mDeformerIndex; }
            set { mDeformerIndex = value; }
        }

        int mPointIndex = -1;
        public int PointIndex
        {
            get { return mPointIndex; }
            set { mPointIndex = value; }
        }

        double mWeight = -1;
        public double Weight
        {
            get { return mWeight; }
            set { mWeight = value; }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            DeformerPointWeight otherDPW = obj as DeformerPointWeight;
            if (otherDPW == null)
            {
                return 0;
            }

            return Weight.CompareTo(otherDPW.Weight);
        }

        #endregion
    }
}
