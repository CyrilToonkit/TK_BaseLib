using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Geometry
{
    public class CG_PolyMesh : CG_Geometry
    {
        int[] mPolygonData = new int[0];
        /// <summary>
        /// An ordered array of polygon definitions, each polygon is defined by a list of elements, the first element of a polygon definition must be set with the number of indices for that polygon. The ordering of vertices must respect a ccw ordering to get out going normals (right-hand rule). E.g. array of polygons with 4 indices each e.g. {4,0,1,4,3,4,1,2,5,4... }
        /// </summary>
        public int[] PolygonData
        {
            get { return mPolygonData; }
            set { mPolygonData = value; }
        }

        public override bool IsValid()
        {
            return (PointPositions.Length > 5 && PointPositions.Length % 3 == 0 && mPolygonData.Length > 3);
        }
    }
}
