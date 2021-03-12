using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TK.BaseLib.Processes
{
    public class Tracer
    {
        BenchMarker bm = new BenchMarker();

        private Tracer()
        {

        }

        private static Tracer _instance = null;

        public static Tracer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Tracer();
                }

                return _instance;
            }
        }

        bool _log = false;
        public bool Log
        {
            get { return _log; }
            set { _log = value; }
        }

        bool _active = false;
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;

                if (value)
                {
                    bm.Start();
                }
                else
                {
                    bm.Stop();
                }
            }
        }

        List<TracedCall> _callStack = new List<TracedCall>();
        Dictionary<string, CallData> _calls = new Dictionary<string, CallData>();

        public void BeginCall(string cmdname)
        {
            if (_active)
            {
                _callStack.Add(new TracedCall(cmdname));
            }
        }

        public void BeginCall(object objectKey)
        {
            if (_active)
            {
                string cmdname = objectKey.ToString();
                _callStack.Add(new TracedCall(cmdname));
            }
        }
        
        public void EndCall(string cmdname)
        {
            if (_active)
            {
                //Find call
                for (int i = _callStack.Count - 1; i >= 0; i--)
                {
                    if (_callStack[i].Name == cmdname)
                    {
                        _callStack[i].Stop();
                        break;
                    }
                }
            }
        }

        public void EndCall(string cmdname, object objectResult, string info)
        {
            if (_active)
            {
                //Find call
                for (int i = _callStack.Count - 1; i >= 0; i--)
                {
                    if (_callStack[i].Name == cmdname)
                    {
                        _callStack[i].Stop();
                        _callStack[i].Result = objectResult;
                        _callStack[i].Info = info;
                        break;
                    }
                }
            }
        }
        
        public void EndCall(object objectKey)
        {
            if (_active)
            {
                string cmdname = objectKey.ToString();

                for (int i = _callStack.Count - 1; i >= 0; i--)
                {
                    if (_callStack[i].Name == cmdname)
                    {
                        _callStack[i].Stop();
                        break;
                    }
                }
            }
        }
        
        public void EndCall(object objectKey, object objectResult, string info)
        {
            if (_active)
            {
                string cmdname = objectKey.ToString();

                for (int i = _callStack.Count - 1; i >= 0; i--)
                {
                    if (_callStack[i].Name == cmdname)
                    {
                        _callStack[i].Stop();
                        _callStack[i].Result = objectResult;
                        _callStack[i].Info = info;
                        break;
                    }
                }
            }
        }

        public string GetText()
        {
            //Fill callData
            long totalTicks = 0;
            
            foreach (TracedCall call in _callStack)
            {
                if (!_calls.ContainsKey(call.Name))
                {
                    CallData callD = new CallData(call.Name);
                    _calls.Add(call.Name, callD);
                }
                totalTicks += call.Duration.Ticks;
                _calls[call.Name].Add(call);
            }

            TimeSpan totalDuration = new TimeSpan(totalTicks);

            if (_calls.Count == 0)
            {
                return "No values...\n";
            }

            string text = string.Format("\n *** Commands diagnostic : {0} called in {1:0.0} seconds ({2:0} cmds/s). Total time = {3:0.0} seconds ({4:0.000} seconds processing) ***\n", _callStack.Count, totalDuration.TotalSeconds, _callStack.Count / totalDuration.TotalSeconds, bm.Seconds, bm.Seconds - totalDuration.TotalSeconds);

            foreach (CallData data in _calls.Values)
            {
                double totalSeconds = data.TotalDuration.TotalSeconds;
                text += string.Format("{0,-20} called {1} times, {2:0.000}s avg, {3:0.000}s total={4:0.0}% ({5:0.000}s < {6:0.000}s)\n", data.Name, data.NbCalls, data.AverageDuration.TotalSeconds, totalSeconds, 100 * totalSeconds / totalDuration.TotalSeconds, data.MinimumDuration.TotalSeconds, data.MaximumDuration.TotalSeconds);
            }

            return text;
        }

        public void SaveCsv(string inPath)
        {
            StringBuilder csv = new StringBuilder();
            csv.Append("Cmd;Start;Duration\n");

            foreach (TracedCall call in _callStack)
            {
                csv.Append(string.Format("{0};{1};{2:0.000};{3};{4}\n", call.Name, call.Start, call.Duration.TotalSeconds, TypesHelper.Join(call.Result as object[], "|"), call.Info));
            }

            File.WriteAllText(inPath, csv.ToString());
        }

        public void Clear()
        {
            _callStack.Clear();
            _calls.Clear();
        }

        public bool HaveResults()
        {
            return _calls.Count > 0;
        }
    }
}
