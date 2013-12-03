using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml.Serialization;

namespace TK.BaseLib.CGModel
{
    [Serializable()]
    public class CG_SceneElement 
    {
        public CG_SceneElement()
        {
        }

        #region Fields

        public string mName = string.Empty;
        [CategoryAttribute("Basic")]
        [DescriptionAttribute("Name of the rig")]
        public string Name
        {
            get { return mName; }
            set
            {
                mName = value;
            }
        }

        protected virtual void UpdateName(string value)
        {
 
        }

        protected string mAccessName = string.Empty;
        [BrowsableAttribute(false)]
        public string AccessName
        {
            get { return mAccessName; }
            set { mAccessName = value; }
        }
		
        #endregion

        #region ISerializable Members

        public CG_SceneElement(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            mName = (string)info.GetValue("Name", typeof(string));
            mAccessName = (string)info.GetValue("AccessName", typeof(string));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", mName);
            info.AddValue("AccessName", mAccessName);
        }

        #endregion

        public virtual CG_SceneElement Copy(bool Resolve)
        {
            CG_SceneElement copy = new CG_SceneElement();
            copy.mAccessName = mAccessName;
            copy.mName = mName;

            return copy;
        }
    }
}