using System;
using System.Collections.Generic;
using System.Text;
using TK.BaseLib.Processes;

namespace TK.BaseLib.Processes
{
    public class TracedCall
    {
        string _name = "VOID";
        DateTime _start = DateTime.Now;
        DateTime _end = DateTime.Now;

        public TracedCall(string cmdname)
        {
            _name = cmdname;
        }

        internal void Stop()
        {
            _end = DateTime.Now;
        }

        public DateTime Start
        {
            get
            {
                return _start;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public TimeSpan Duration
        {
            get { return _end - _start; }
        }
    }
}
