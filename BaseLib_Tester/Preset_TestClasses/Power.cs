using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TK.BaseLib;

namespace BaseLib_Tester.Preset_TestClasses
{
    public class Power : SaveablePreset
    {
        public Power()
            : base()
        {

        }

        public Power(string inName, PresetTypes inType)
            : base(inName, inType)
        {

        }

        int _value = 0;

                       
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override void Clone(SaveableData Data)
        {
            base.Clone(Data);
            Power preset = Data as Power;

            if (preset != null)
            {
                _value = preset.Value;
            }
        }
    }
}
