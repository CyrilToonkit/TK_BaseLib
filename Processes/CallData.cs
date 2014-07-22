using System;
using System.Collections.Generic;
using System.Text;
using TK.BaseLib.Processes;

namespace TK.BaseLib.Processes
{
    public class CallData
    {
        string _name = "VOID";

        List<TimeSpan> _durations = new List<TimeSpan>();

        public CallData(string cmdname)
        {
            _name = cmdname;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public int NbCalls
        {
            get
            {
                return _durations.Count;
            }
        }

        public TimeSpan MeanDuration
        {
            get
            {
                long ticks = 0;

                foreach (TimeSpan ts in _durations)
                {
                    ticks += ts.Ticks;
                }

                ticks = ticks / _durations.Count;

                return new TimeSpan(ticks);
            }
        }

        public TimeSpan MinimumDuration
        {
            get
            {
                TimeSpan minTs = new TimeSpan();
                long ticks = long.MaxValue;

                foreach (TimeSpan ts in _durations)
                {
                    if (ts.Ticks < ticks)
                    {
                        minTs = ts;
                        ticks = ts.Ticks;
                    }
                }

                return minTs;
            }   
        }

        public TimeSpan MaximumDuration
        {
            get
            {
                TimeSpan maxTs = new TimeSpan();
                long ticks = 0;

                foreach (TimeSpan ts in _durations)
                {
                    if (ts.Ticks > ticks)
                    {
                        maxTs = ts;
                        ticks = ts.Ticks;
                    }
                }

                return maxTs;
            }
        }

        internal void Add(TracedCall call)
        {
            _durations.Add(call.Duration);
        }
    }
}
