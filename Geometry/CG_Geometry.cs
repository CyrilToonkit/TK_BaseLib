using System;
using System.Collections.Generic;
using System.Text;
using TK.BaseLib.Math3D;
using TK.BaseLib.CGModel;

namespace TK.BaseLib.Geometry
{
    public class CG_Geometry : CG_Object3D
    {
        double[] mPointPositions = new double[0];
        public double[] PointPositions
        {
            get { return mPointPositions; }
            set { mPointPositions = value; }
        }

        public int PointsCount
        {
            get { return (mPointPositions.Length / 3); }
        }

        public virtual bool IsValid()
        {
            return false;
        }

        public Array GetAsArray4()
        {
            return GetAsArray4(null, 1);
        }

        public Array GetAsArray4(CG_Transform Offset)
        {
            return GetAsArray4(Offset, 1);
        }

        public Array GetAsArray4(CG_Transform Offset, double scale)
        {
            if (Offset == null)
            {
                Offset = new CG_Transform();
            }

            Array a = Array.CreateInstance(typeof(object), 4 * PointsCount);
            int arrayOffset = 0;

            for (int i = 0; i < PointPositions.Length; i++)
            {
                int compIndex = i % 3;
                double posOffset = 0;
                double sclOffset = scale;

                switch (compIndex)
                {
                    case 0:
                        posOffset = Offset.Pos.X;
                        sclOffset *= Offset.Scl.X;
                        break;
                    case 1:
                        posOffset = Offset.Pos.Y;
                        sclOffset *= Offset.Scl.Y;
                        break;
                    case 2:
                        posOffset = Offset.Pos.Z;
                        sclOffset *= Offset.Scl.Z;
                        break;
                }

                a.SetValue(PointPositions[i] * sclOffset + posOffset, i + arrayOffset);

                //Add fourth dimension
                if (compIndex == 2)
                {
                    arrayOffset += 1;
                    a.SetValue(1, i + arrayOffset);
                }
            }

            return a;
        }

        public static List<CG_Vector3> Transform(List<CG_Vector3> points, CG_Transform cG_Transform, double scale)
        {
            List<CG_Vector3> transformedPoints = new List<CG_Vector3>();

            foreach (CG_Vector3 point in points)
            {
                transformedPoints.Add(new CG_Vector3(point.X * cG_Transform.Scl.X * scale + cG_Transform.Pos.X,
                                                    point.Y * cG_Transform.Scl.Y * scale + cG_Transform.Pos.Y,
                                                    point.Z * cG_Transform.Scl.Z * scale + cG_Transform.Pos.Z));
            }

            return transformedPoints;
        }
    }
}
