using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Geometry
{
    public class CG_GeometryData
    {
        CG_Geometry mGeometry;
        public CG_Geometry Geometry
        {
            get { return mGeometry; }
            set { mGeometry = value; }
        }

        List<CG_PointData> mData = new List<CG_PointData>();
        public List<CG_PointData> Data
        {
            get { return mData; }
            set { mData = value; }
        }
    }
}
