using System;
using System.Runtime.Serialization;

namespace TK.BaseLib.CGModel
{
    [Serializable()]
    public class CG_Group : CG_SceneElement, ISerializable
    {
        private string mDesc;
        public string Desc
        {
            get { return mDesc; }
            set { mDesc = value; }
        }

        private bool mContainer;
        public bool Container
        {
            get { return mContainer; }
            set { mContainer = value; }
        }

        private bool mSubdivision;
        public bool Subdivision
        {
            get { return mSubdivision; }
            set { mSubdivision = value; }
        }

        private AffectType mVisibility;
        public AffectType Visibility
        {
            get { return mVisibility; }
            set { mVisibility = value; }
        }

        private AffectType mSelectability;
        public AffectType Selectability
        {
            get { return mSelectability; }
            set { mSelectability = value; }
        }

        public CG_Group()
        {
            
        }

        public CG_Group(String inName,Boolean inContainer,Boolean inSubdivision,int inVisibility, int inSelectability)
        {
            this.Name = inName;
            this.Container = inContainer;
            this.Subdivision = inSubdivision;
            this.Visibility = ToAffectType(inVisibility);
            this.Selectability = ToAffectType(inVisibility);
        }

        public AffectType ToAffectType(int AffectString)
        {
            AffectType AffectRslt = new AffectType();

            switch (AffectString)
            {
                case 1:
                    AffectRslt = AffectType.False;
                    break;
                case 2:
                    AffectRslt = AffectType.True;
                    break;
                default:
                    AffectRslt = AffectType.NoEffect;
                    break;
            }

            return AffectRslt;
        }

        #region ISerializable Members

        public CG_Group(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            mName = (string)info.GetValue("Name", typeof(string));
            mAccessName = (string)info.GetValue("AccessName", typeof(string));

            mDesc = (string)info.GetValue("Desc", typeof(string));
            mContainer = (bool)info.GetValue("Container", typeof(bool));
            mSubdivision = (bool)info.GetValue("Subdivision", typeof(bool));
            mVisibility = (AffectType)info.GetValue("Visibility", typeof(AffectType));
            mSelectability = (AffectType)info.GetValue("Selectability", typeof(AffectType));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", mName);
            info.AddValue("AccessName", mAccessName);

            info.AddValue("Desc", mDesc);
            info.AddValue("Container", mContainer);
            info.AddValue("Subdivision", mSubdivision);
            info.AddValue("Visibility", mVisibility, typeof(AffectType));
            info.AddValue("Selectability", mSelectability, typeof(AffectType));
        }

        #endregion
    }
}