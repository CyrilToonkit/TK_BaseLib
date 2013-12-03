using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TK.BaseLib.Time;
using TK.BaseLib.Animation.KeysCurves;

namespace BaseLib_Tester
{
    static class Test_AnimCurve
    {
        public static void Execute()
        {
            AnimCurve curve = new AnimCurve();

            curve.AddKey(0.0, 0.0);
        }
    }
}
