using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

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

        string _paramHolder = string.Empty;
        public string ParamHolder
        {
            get { return _paramHolder; }
            set { _paramHolder = value == null ? string.Empty : value; }
        }

        string _category = "No category";
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        List<string> _tags = new List<string>();
        public List<string> Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }

        int _priority = 0;
        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        bool _ignorePriorities = false;
        public bool IgnorePriorities
        {
            get { return _ignorePriorities; }
            set { _ignorePriorities = value; }
        }

        bool _split = true;
        public bool Split
        {
            get { return _split; }
            set { _split = value; }
        }

        bool _embed = true;
        public bool Embed
        {
            get { return _embed; }
            set { _embed = value; }
        }

        [XmlIgnore]
        public bool Embeddable
        {
            get { return _embed && ParamHolder != string.Empty; }
        }

        [XmlIgnore]
        public bool ManualConnections
        {
            get { return Embeddable && ParamHolder.Split(',')[0].Contains(".");}
        }

        [XmlIgnore]
        public string OutputAttributes
        {
            get { return TypesHelper.Join(GetAttributes()); }
        }

        string _customName = string.Empty;
        public string CustomName
        {
            get { return _customName; }
            set { _customName = value; }
        }

        [XmlIgnore]
        public string ExposedName
        {
            get { return string.IsNullOrEmpty(_customName) ? _name : _customName; }
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
                _paramHolder = anim.ParamHolder;
                _customName = anim.CustomName;
                _priority = anim.Priority;
                _ignorePriorities = anim.IgnorePriorities;
                _split = anim.Split;
                _embed = anim.Embed;
                _category = anim.Category;
                _tags = new List<string>(anim.Tags);
            }
        }

        public double GetFirstFrame()
        {
            double first = double.MaxValue;
            foreach (AnimCurve curve in _curves)
            {
                if(curve.Keys.Count > 0 && curve.Keys[0].Time < first)
                {
                    first = curve.Keys[0].Time;
                }
            }

            if (first == double.MaxValue)
            {
                return 0.0;
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

        public void Scale(double inScale, bool inTranslationOnly)
        {
            foreach (AnimCurve curve in _curves)
            {
                if (!inTranslationOnly || curve.ChannelName.Contains(".tx") || curve.ChannelName.Contains(".ty") || curve.ChannelName.Contains(".tz") || curve.ChannelName.Contains(".Pos"))
                {
                    curve.Scale(inScale, 0);
                }
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

        public List<string> GetAttributes()
        {
            List<string> attrs = new List<string>();
            List<string> paramHolders = new List<string>(ParamHolder.Split(','));

            for (int i = 0; i < paramHolders.Count; i++)
            {
                paramHolders[i] = paramHolders[i].Trim();
            }

            if (Split)
            {
                if(paramHolders.Count < 2)
                {
                    paramHolders.Add(paramHolders[0]);
                }

                if (paramHolders[0].Contains("."))
                {
                    attrs.Add(paramHolders[0].Replace("$NAME", ExposedName).Replace("$SIDE", "Left"));
                }
                else
                {
                    attrs.Add(string.Format("{0}.Left_{1}", paramHolders[0].Replace("$NAME", ExposedName).Replace("$SIDE", "Left"), ExposedName));
                }

                if (paramHolders[1].Contains("."))
                {
                    attrs.Add(paramHolders[1].Replace("$NAME", ExposedName).Replace("$SIDE", "Right"));
                }
                else
                {
                    attrs.Add(string.Format("{0}.Right_{1}", paramHolders[1].Replace("$NAME", ExposedName).Replace("$SIDE", "Right"), ExposedName));
                }
            }
            else
            {
                if (paramHolders[0].Contains("."))
                {
                    attrs.Add(paramHolders[0].Replace("$NAME", ExposedName).Replace("$SIDE_", string.Empty).Replace("$SIDE", string.Empty));
                }
                else
                {
                    attrs.Add(string.Format("{0}.{1}", paramHolders[0].Replace("$NAME", ExposedName).Replace("$SIDE_", string.Empty).Replace("$SIDE", string.Empty), ExposedName));
                }
            }

            return attrs;
        }
    }
}
