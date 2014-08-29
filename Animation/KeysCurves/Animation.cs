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

 
    }
}
