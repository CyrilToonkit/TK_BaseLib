using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Checking
{
    public class CheckList
    {
        public CheckList(){}

        public CheckList(string inName)
        {
            _name = inName;
        }

        string _name = "New CheckList";
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        List<Check> _checks = new List<Check>();
        public List<Check> Checks
        {
            get { return _checks; }
        }
    }
}
