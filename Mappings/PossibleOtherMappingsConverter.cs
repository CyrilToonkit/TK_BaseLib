using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TK.BaseLib
{
    public class PossibleOtherMappingsConverter : TypeConverter
    {
        public static List<string> values = new List<string>();

        public override bool
        GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true; // display drop
        }
        public override bool
        GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true; // drop-down vs combo
        }
        public override StandardValuesCollection
        GetStandardValues(ITypeDescriptorContext context)
        {
            // note you can also look at context etc to build list
            return new StandardValuesCollection(values);
        }
    }
}
