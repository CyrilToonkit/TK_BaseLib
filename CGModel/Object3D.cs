using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using TK.BaseLib.Math3D;

namespace TK.BaseLib.CGModel
{
    [Serializable()]
    public class CG_Object3D : CG_SceneElement, ISerializable
    {
        public CG_Object3D()
        {

        }
        
        protected CG_Transform mTrans = new CG_Transform();

        [CategoryAttribute("State")]
        [DescriptionAttribute("Transformation of the rig (the root)")]
        public CG_Transform Trans
        {
            get { return mTrans; }
            set { mTrans = value; }
        }

        #region ISerializable Members

        public CG_Object3D(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            mName = (string)info.GetValue("Name", typeof(string));
            mAccessName = (string)info.GetValue("AccessName", typeof(string));

            mTrans = (CG_Transform)info.GetValue("Trans", typeof(CG_Transform));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", mName);
            info.AddValue("AccessName", mAccessName);

            info.AddValue("Trans", mTrans, typeof(CG_Transform));
        }

        #endregion

        public override CG_SceneElement Copy(bool Resolve)
        {
            CG_Object3D Obj = new CG_Object3D();
            Obj.AccessName = AccessName;
            Obj.Name = Name;
            Obj.Trans = Trans.Copy();

            return Obj;
        }

        public void SetParamterValue(string ParamName, double Value)
        {
            if (ParamName.Contains("pos"))
            {
                if (ParamName.Contains("X"))
                {
                    Trans.Pos.X = Value;
                }
                else if (ParamName.Contains("Y"))
                {
                    Trans.Pos.Y = Value;
                }
                else
                {
                    Trans.Pos.Z = Value;
                }
            }
            else if (ParamName.Contains("rot"))
            {
                if (ParamName.Contains("X"))
                {
                    Trans.Rot.X = Value;
                }
                else if (ParamName.Contains("Y"))
                {
                    Trans.Rot.Y = Value;
                }
                else
                {
                    Trans.Rot.Z = Value;
                }
            }
            else if (ParamName.Contains("scl"))
            {
                if (ParamName.Contains("X"))
                {
                    Trans.Scl.X = Value;
                }
                else if (ParamName.Contains("Y"))
                {
                    Trans.Scl.Y = Value;
                }
                else
                {
                    Trans.Scl.Z = Value;
                }
            }
        }
    }
}
