using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace TK.BaseLib.Math3D
{
    [Serializable]
    public class CG_Scale : CG_Vector3, ISerializable
    {
		public CG_Scale(double inx, double iny, double inz) : base(inx, iny, inz)
        {
        }

		public CG_Scale(double[] inArray) : base(inArray)
        {
        }

        #region ISerializable

        public CG_Scale(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            mx = (double)info.GetValue("X", typeof(double));
            my = (double)info.GetValue("Y", typeof(double));
            mz = (double)info.GetValue("Z", typeof(double));
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", mx);
            info.AddValue("Y", my);
            info.AddValue("Z", mz);
        }

        #endregion
    }
}
