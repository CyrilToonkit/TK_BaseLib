using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TK.BaseLib.CGModel
{
    [Serializable()]
    public class CG_Property : CG_SceneElement
    {
        public CG_Property(String inScriptName)
        {
            this.Name = inScriptName;
            this.AccessName = inScriptName;
        }

        private string mPreset;
        public string Preset
        {
            get { return mPreset; }
            set { mPreset = value; }
        }

        private Dictionary<string, CG_Parameter> mParameters = new Dictionary<string,CG_Parameter>();
        [XmlIgnore]
        public Dictionary<string, CG_Parameter> Parameters
        {
            get { return mParameters; }
            set { mParameters = value; }
        }

        [XmlElement("Parameters")]
        public List<CG_Parameter> XmlParameters
        {
            get
            {
                List<CG_Parameter> Params = new List<CG_Parameter>();
                foreach(CG_Parameter param in Parameters.Values)
                {
                    Params.Add(param);
                }
                return Params;
            }
            set
            {
                Parameters = new Dictionary<string,CG_Parameter>();
                foreach(CG_Parameter param in value)
                {
                    Parameters.Add(param.AccessName, param);
                }
            }
        }

        public void AddParameter(String inScriptName, Object inValue)
        {
            mParameters.Add(inScriptName, new CG_Parameter(inScriptName, inValue));
        }
    }
}
