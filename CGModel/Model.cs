using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using TK.BaseLib.Math3D;

namespace TK.BaseLib.CGModel
{
    [Serializable]
    public class CG_Model : CG_Object3D, ISerializable
    {
        protected string mModelPath = string.Empty;
        [CategoryAttribute("Info")]
        [ReadOnlyAttribute(true)]
        [DescriptionAttribute("Paths (for explicit rig)")]
        public string ModelPath
        {
            get { return mModelPath; }
            set { mModelPath = value; }
        }
        
        protected List<CG_Group> mGroups = new List<CG_Group>();
        [BrowsableAttribute(false)]
        public List<CG_Group> Groups
        {
            get { return mGroups; }
            set { mGroups = value; }
        }

        public CG_Model()
        {
        }

        public CG_Model(String inName)
        {
            this.Name = inName;
            this.AccessName = inName;
            this.mGroups = new List<CG_Group>();
        }

        
        #region ISerializable Members

        public CG_Model(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            mName = (string)info.GetValue("Name", typeof(string));
            mAccessName = (string)info.GetValue("AccessName", typeof(string));

            mTrans = (CG_Transform)info.GetValue("Trans", typeof(CG_Transform));

            mModelPath = (string)info.GetValue("ModelPath", typeof(string));
            mGroups = (List<CG_Group>)info.GetValue("Groups", typeof(CG_Group));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", mName);
            info.AddValue("AccessName", mAccessName);

            info.AddValue("Trans", mTrans, typeof(CG_Transform));

            info.AddValue("ModelPath", mModelPath);
            info.AddValue("Groups", mGroups, typeof(List<CG_Group>));
        }

        #endregion
    }
}
