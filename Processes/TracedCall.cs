using System;
using System.Collections.Generic;
using System.Text;
using TK.BaseLib.Processes;

namespace TK.BaseLib.Processes
{
    public class TracedCall
    {
        string _name = string.Empty;
        DateTime _start = DateTime.Now;
        DateTime _end = DateTime.Now;
        object _result = null;
        string _info = string.Empty;


        public TracedCall(string cmdname)
        {
            _name = cmdname;
        }

        public TracedCall(string cmdname, object result)
        {
            _name = cmdname;
            _result = result;
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

        public object Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public string Info
        {
            get
            {
                return _info;
            }
            set
            {
                _info = value;
            }
        }

        public TimeSpan Duration
        {
            get { return _end - _start; }
        }
    }
}
