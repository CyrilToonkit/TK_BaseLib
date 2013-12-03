using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace TK.BaseLib.Math3D
{
    [Serializable()]
    public class CG_Position : CG_Vector3
    {
		public CG_Position(double inx, double iny, double inz)	: base(inx, iny, inz)
        {
        }

		public CG_Position(double[] inArray): base(inArray)
        {
        }

        #region ISerializable Members

        /*public CG_Position(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            mx = (double)info.GetValue("X", typeof(double));
            my = (double)info.GetValue("Y", typeof(double));
            mz = (double)info.GetValue("Z", typeof(double));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }*/

        #endregion
    }

        
}
