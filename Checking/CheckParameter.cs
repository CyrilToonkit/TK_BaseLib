using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Checking
{
    public enum ParameterTypes
    {
        String, Bool, Int, Double
    }

    public class CheckParameter
    {
        ParameterTypes _parameterType = ParameterTypes.String;
        public ParameterTypes ParameterType
        {
            get { return _parameterType; }
            set
            {
                //@todo type conversions
                _parameterType = value;
            }
        }

        object _value = "";

        public string StringValue
        {
            get { return _value.ToString(); }
            set
            {
                switch (_parameterType)
                {
                    case ParameterTypes.Int:
                        int intValue = 0;
                        if (Int32.TryParse(value, out intValue))
                        {
                            _value = intValue;
                        }
                        else
                        {
                            _value = 0;
                        }
                        break;
                    case ParameterTypes.Bool:
                        _value = Boolean.Parse(value);
                        break;
                    case ParameterTypes.Double:
                        _value = TypesHelper.DoubleParse(value);
                        break;
                    default :
                        _value = value;
                        break;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("\"{0}\" ({1})", StringValue, _parameterType);
        }

        internal void Copy(CheckParameter param)
        {
            _parameterType = param.ParameterType;
            StringValue = param.StringValue;
        }
    }
}
