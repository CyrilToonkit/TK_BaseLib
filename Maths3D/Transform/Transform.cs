using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace TK.BaseLib.Math3D
{
    [Serializable()]
    [TypeConverterAttribute(typeof(TransInspector)),
DescriptionAttribute("Expand to see Transformation.")]
    public class CG_Transform : ISerializable
    {
		public CG_Transform()
		{
            mPos = new CG_Vector3();
            mRot = new CG_Vector3();
            mScl = new CG_Vector3(1,1,1);
		}
		
        private CG_Vector3 mPos;
        public CG_Vector3 Pos
        {
            get { return mPos; }
            set { mPos = value; }
        }

        private CG_Vector3 mRot;
        public CG_Vector3 Rot
        {
            get { return mRot; }
            set { mRot = value; }
        }

        private CG_Vector3 mScl;
        public CG_Vector3 Scl
        {
            get { return mScl; }
            set { mScl = value; }
        }

        [XmlIgnore]
        public bool Identity
        {
            get { return (Pos.Null && Rot.Null && Scl.Identity); }
        }

        public bool FuzzyEquals(CG_Transform trans)
        {
            return Pos.FuzzyEquals(trans.Pos) && Rot.FuzzyEquals(trans.Rot) && Scl.FuzzyEquals(trans.Scl);
        }

        public CG_Transform Copy()
        {
            CG_Transform Trans = new CG_Transform();

            Trans.Scl.X = Scl.X;
            Trans.Scl.Y = Scl.Y;
            Trans.Scl.Z = Scl.Z;

            Trans.Pos.X = Pos.X;
            Trans.Pos.Y = Pos.Y;
            Trans.Pos.Z = Pos.Z;

            Trans.Rot.X = Rot.X;
            Trans.Rot.Y = Rot.Y;
            Trans.Rot.Z = Rot.Z;

            return Trans;
        }

        public static string ToCleanString(double p)
        {
            return p.ToString("0.##").Replace(",", ".");
        }

        public override string ToString()
        {
            return ToCleanString(Scl.X) + "," + ToCleanString(Scl.Y) + "," + ToCleanString(Scl.Z) + ";" +
                       ToCleanString(Pos.X) + "," + ToCleanString(Pos.Y) + "," + ToCleanString(Pos.Z) + ";" +
                       ToCleanString(Rot.X) + "," + ToCleanString(Rot.Y) + "," + ToCleanString(Rot.Z);
        }

        // Returns a new transform with the contents
        // added together.
        public static CG_Transform operator +(CG_Transform trans1, CG_Transform trans2)
        {
            if (trans1 == null)
                throw new ArgumentNullException("trans1");
            if (trans2 == null)
                throw new ArgumentNullException("trans2");
            CG_Transform addedTrans = new CG_Transform();
            addedTrans.Pos = trans1.Pos + trans2.Pos;
            addedTrans.Rot = trans1.Rot + trans2.Rot;
            addedTrans.Scl = trans1.Scl * trans2.Scl;
            return addedTrans;
        }

        // Returns a new transform with the contents
        // substracted together.
        public static CG_Transform operator -(CG_Transform trans1, CG_Transform trans2)
        {
            if (trans1 == null)
                throw new ArgumentNullException("trans1");
            if (trans2 == null)
                throw new ArgumentNullException("trans2");
            CG_Transform addedTrans = new CG_Transform();
            addedTrans.Pos = trans1.Pos - trans2.Pos;
            addedTrans.Rot = trans1.Rot - trans2.Rot;
            addedTrans.Scl = trans1.Scl / trans2.Scl;
            return addedTrans;
        }

        // Returns a new transform with the contents
        // multiplied by scalar together.
        public static CG_Transform operator *(CG_Transform trans1, double scalar)
        {
            if (trans1 == null)
                throw new ArgumentNullException("trans1");
            CG_Transform addedTrans = new CG_Transform();
            addedTrans.Pos = trans1.Pos * scalar;
            addedTrans.Rot = trans1.Rot * scalar;
            addedTrans.Scl = trans1.Scl * scalar;
            return addedTrans;
        }

        #region ISerializable Members

        public CG_Transform(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            mPos = (CG_Vector3)info.GetValue("Pos", typeof(CG_Vector3));
            mRot = (CG_Vector3)info.GetValue("Rot", typeof(CG_Vector3));
            mScl = (CG_Vector3)info.GetValue("Scl", typeof(CG_Vector3));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Pos", mPos, typeof(CG_Vector3));
            info.AddValue("Rot", mRot, typeof(CG_Vector3));
            info.AddValue("Scl", mScl, typeof(CG_Vector3));
        }

        #endregion
    }

    public class TransInspector : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                      System.Type destinationType)
        {
            if (destinationType == typeof(CG_Transform))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                               CultureInfo culture,
                               object value,
                               System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is CG_Transform)
            {

                CG_Transform Trans = (CG_Transform)value;

                return ToCleanString(Trans.Scl.X) + "," + ToCleanString(Trans.Scl.Y) + "," + ToCleanString(Trans.Scl.Z) + ";" +
                       ToCleanString(Trans.Pos.X) + "," + ToCleanString(Trans.Pos.Y) + "," + ToCleanString(Trans.Pos.Z) + ";" +
                       ToCleanString(Trans.Rot.X) + "," + ToCleanString(Trans.Rot.Y) + "," + ToCleanString(Trans.Rot.Z);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        private string ToCleanString(double p)
        {
            return p.ToString("0.##").Replace(",", ".");
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
                              CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;

                    string[] split = s.Split(";".ToCharArray());

                    if (split.Length == 3)
                    {
                        string[] Scl = split[0].Split(",".ToCharArray());
                        string[] Pos = split[1].Split(",".ToCharArray());
                        string[] Rot = split[2].Split(",".ToCharArray());

                        if (Scl.Length == 3 && Pos.Length == 3 && Rot.Length == 3)
                        {
                            CG_Transform Trans = new CG_Transform();

                            Trans.Scl.X = TypesHelper.DoubleParse(Scl[0]);
                            Trans.Scl.Y = TypesHelper.DoubleParse(Scl[1]);
                            Trans.Scl.Z = TypesHelper.DoubleParse(Scl[2]);

                            Trans.Pos.X = TypesHelper.DoubleParse(Pos[0]);
                            Trans.Pos.Y = TypesHelper.DoubleParse(Pos[1]);
                            Trans.Pos.Z = TypesHelper.DoubleParse(Pos[2]);

                            Trans.Rot.X = TypesHelper.DoubleParse(Rot[0]);
                            Trans.Rot.Y = TypesHelper.DoubleParse(Rot[1]);
                            Trans.Rot.Z = TypesHelper.DoubleParse(Rot[2]);

                            return Trans;
                        }
                    }
                }
                catch
                {
                    throw new ArgumentException(
                        "Can not convert '" + (string)value +
                                           "' to type SpellingOptions");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
