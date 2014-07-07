using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Globalization;

namespace TK.BaseLib.Math3D
{
    [Serializable()]
    [TypeConverterAttribute(typeof(TransVector3)),
DescriptionAttribute("Expand to see Vector's X Y Z.")]
    public class CG_Vector3 : ISerializable
    {
        public CG_Vector3()
        {
            this.mx = 0;
            this.my = 0;
            this.mz = 0;
        }

		public CG_Vector3(double inx, double iny, double inz)
        {
            this.mx = inx;
            this.my = iny;
            this.mz = inz;
        }

		public CG_Vector3(double[] inArray)
        {
            this.mx = inArray[0];
            this.my = inArray[1];
            this.mz = inArray[2];
        }

		protected double mx;
		public double X
        {
            get { return mx; }
            set { mx = value; }
        }

        protected double my;
		public double Y
        {
            get { return my; }
            set { my = value; }
        }

        protected double mz;
		public double Z
        {
            get { return mz; }
            set { mz = value; }
        }

        public bool Null
        {
            get { return (X == 0 && Y == 0 && Z == 0); }
        }

        public bool Identity
        {
            get { return (X == 1 && Y == 1 && Z == 1); }
        }

        // Returns the dot product of the current vector and
        // the given one
		public double DotProduct(CG_Vector3 vec2)
        {
            return (X * vec2.X) + (Y * vec2.Y) + (Z * vec2.Z);
        }

        // Returns a vector representing the cross product
        // of the current vector and the given one
        public CG_Vector3 CrossProduct(CG_Vector3 vec2)
        {
            return new CG_Vector3(
                (Y * vec2.Z) - (vec2.Y * Z),
                (Z * vec2.X) - (vec2.Z * X),
                (X * vec2.Y) - (vec2.X * Y));
        }

        // Returns a new vector with the contents
        // multiplied together.
        public static CG_Vector3 operator *(CG_Vector3 vec1, CG_Vector3 vec2)
        {
            if (vec1 == null)
                throw new ArgumentNullException("vec1");
            if (vec2 == null)
                throw new ArgumentNullException("vec2");
            return new CG_Vector3(vec1.X * vec2.X, vec1.Y * vec2.Y, vec1.Z * vec2.Z);
        }

        #region ISerializable Members

        public CG_Vector3(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            mx = (double)info.GetValue("X", typeof(double));
            my = (double)info.GetValue("Y", typeof(double));
            mz = (double)info.GetValue("Z", typeof(double));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", mx);
            info.AddValue("Y", my);
            info.AddValue("Z", mz);
        }

        #endregion

        public void Negate()
        {
            mx *= -1;
            my *= -1;
            mz *= -1;
        }
    }

    public class TransVector3 : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                      System.Type destinationType)
        {
            if (destinationType == typeof(CG_Vector3))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                               CultureInfo culture,
                               object value,
                               System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is CG_Vector3)
            {

                CG_Vector3 Trans = (CG_Vector3)value;

                return ToCleanString(Trans.X) + "," + ToCleanString(Trans.Y) + "," + ToCleanString(Trans.Z);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
                              CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;

                    string[] split = s.Split(",".ToCharArray());

                    if (split.Length == 3)
                    {
                        CG_Vector3 Trans = new CG_Vector3(TypesHelper.DoubleParse(split[0]), TypesHelper.DoubleParse(split[1]), TypesHelper.DoubleParse(split[2]));
                        return Trans;
                    }
                }
                catch
                {
                    throw new ArgumentException(
                        "Can not convert '" + (string)value +
                                           "' to type CG_Vector3");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        private string ToCleanString(double p)
        {
            return p.ToString("0.##").Replace(",", ".");
        }

    }
}
