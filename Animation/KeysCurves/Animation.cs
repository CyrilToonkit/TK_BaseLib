using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation.KeysCurves
{
    public class Animation : SaveableData
    {
        string _name = "unkwnown";
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        ActionTypes _type = ActionTypes.Anim;
        public ActionTypes Type
        {
            get { return _type; }
            set { _type = value; }
        }

        List<AnimCurve> _curves = new List<AnimCurve>();
        public List<AnimCurve> Curves
        {
            get { return _curves; }
            set { _curves = value; }
        }

        public override void Clone(SaveableData Data)
        {
            Animation anim = Data as Animation;

            if (anim != null)
            {
                _name = anim.Name;
                _curves = anim.Curves;
                _type = anim.Type;
            }
        }

        public double GetFirstFrame()
        {
            double first = double.MaxValue;
            foreach (AnimCurve curve in _curves)
            {
                if (curve.Keys[0].Time < first)
                {
                    first = curve.Keys[0].Time;
                }
            }

            return first;
        }

        public void SetFirstFrame(double inFrame)
        {
            double first = GetFirstFrame();

            if (first != inFrame)
            {
                double delta = inFrame - first;
                Offset(delta);
            }
        }

        public void Offset(double inTimeOffset)
        {
            foreach (AnimCurve curve in _curves)
            {
                curve.Offset(inTimeOffset);
            }
        }

        public void Retime(double retime, double inRef)
        {
            foreach (AnimCurve curve in _curves)
            {
                curve.Retime(retime, inRef);
            }
        }

        public void Retime(double retime)
        {
            Retime(retime, GetFirstFrame());
        }
    }
}
