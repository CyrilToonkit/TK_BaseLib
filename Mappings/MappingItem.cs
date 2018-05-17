using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TK.BaseLib
{
    public class MappingItem
    {
        public MappingItem(Mapping inMapping, string inBaseValue, string inOtherValue)
        {
            _mapping = inMapping;
            _baseValue = inBaseValue;
            _otherValue = inOtherValue;
        }

        Mapping _mapping;
        string _baseValue;
        [TypeConverter(typeof(PossibleMappingsConverter))]
        public string BaseValue
        {
            get { return _baseValue; }
            set { _baseValue = value; }
        }

        string _otherValue;
        [TypeConverter(typeof(PossibleMappingsConverter))]
        public string OtherValue
        {
            get { return _otherValue; }
            set { _otherValue = value; }
        }

    }
}
