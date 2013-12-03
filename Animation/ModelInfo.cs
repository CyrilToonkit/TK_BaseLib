using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TK.BaseLib.Animation
{
    public class ModelInfo
    {
        public ModelInfo()
        {
        }

        public ModelInfo(string inName)
        {
            name = inName;
        }

        string name = "REF";
        [XmlIgnore]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        List<Rescaler> scalers = new List<Rescaler>();
        public List<Rescaler> Scalers
        {
            get { return scalers; }
            set { scalers = value; }
        }

        public bool HasScaler(Rescaler refScaler)
        {
            foreach (Rescaler scaler in scalers)
            {
                if (scaler.RefObject1 == refScaler.RefObject1 && scaler.RefObject2 == refScaler.RefObject2)
                {
                    return true;
                }
            }

            return false;
        }

        public Rescaler GetScaler(Rescaler refScaler)
        {
            foreach (Rescaler scaler in scalers)
            {
                if (scaler.RefObject1 == refScaler.RefObject1 && scaler.RefObject2 == refScaler.RefObject2)
                {
                    return scaler;
                }
            }

            return null;
        }

        public Rescaler GetScaler(string scaledCtrl, ControlsMap inMap)
        {
            foreach (Rescaler scaler in scalers)
            {
                if (scaler.AffectedControls.Contains(scaledCtrl))
                {
                    return scaler;
                }
                else
                {
                    List<string> remaps = inMap.GetPossibleRemaps(scaledCtrl);
                    foreach (string map in remaps)
                    {
                        if (scaler.AffectedControls.Contains(map))
                        {
                            return scaler;
                        }
                    }
                }
            }

            return null;
        }
    }
}
