using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation
{
    public class Rescaler
    {
        public Rescaler()
        {
        }

        public Rescaler(string inName, string inRefObject1, string inRefObject2, double inDist)
        {
            name = inName;
            refObject1 = inRefObject1;
            refObject2 = inRefObject2;
            distance = inDist;
        }

        string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string refObject1 = "";
        public string RefObject1
        {
            get { return refObject1; }
            set { refObject1 = value; }
        }

        string refObject2 = "";
        public string RefObject2
        {
            get { return refObject2; }
            set { refObject2 = value; }
        }

        double distance = 0.0;
        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        List<string> affectedControls = new List<string>();
        public List<string> AffectedControls
        {
            get { return affectedControls; }
            set { affectedControls = value; }
        }

        public Rescaler Copy(double newValue)
        {
            Rescaler newRescaler = new Rescaler(name, refObject1, refObject2, newValue);
            foreach (string ctrl in AffectedControls)
            {
                newRescaler.AffectedControls.Add(ctrl);
            }

            return newRescaler;
        }
    }
}
