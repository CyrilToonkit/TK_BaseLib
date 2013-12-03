using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace TK.BaseLib.CGModel
{
    [Serializable()]
    public class CG_Parameter : CG_SceneElement, ISerializable
    {
        public CG_Parameter(String inName, String inScriptName, Type inType, Object inValue)
        {
            this.Name = inName;
            this.AccessName = inScriptName;
            this.mParamType = inType;
            this.mParamValue = inValue;
        }

        public CG_Parameter(String inScriptName, Object inValue)
        {
            this.Name = inScriptName;
            this.AccessName = inScriptName;
            this.mParamValue = inValue;
        }

        private Object mParamValue;
        public Object Value
        {
            get { return mParamValue; }
            set { mParamValue = value; }
        }
        
        private Type mParamType;
        public Type Type
        {
            get { return mParamType; }
            set { mParamType = value; }
        }

        private string mDesc;
        public string Desc
        {
            get { return mDesc; }
            set { mDesc = value; }
        }

        #region ISerializable Members

        public CG_Parameter(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            mName = (string)info.GetValue("Name", typeof(string));
            mAccessName = (string)info.GetValue("AccessName", typeof(string));

            mParamType = (Type)info.GetValue("ParamType", typeof(Type));
            mParamValue = info.GetValue("ParamValue", typeof(object));
            mDesc = (string)info.GetValue("Desc", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", mName);
            info.AddValue("AccessName", mAccessName);

            info.AddValue("ParamType", mParamType, typeof(Type));
            info.AddValue("ParamValue", mParamValue, typeof(object));
            info.AddValue("Desc", mDesc, typeof(string));
        }

        #endregion
    }
}
