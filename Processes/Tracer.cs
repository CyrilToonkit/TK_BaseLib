using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TK.BaseLib.Processes
{
    public class Tracer
    {
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

        bool _active = false;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
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

        public string GetText()
        {
            //Fill callData
            foreach (TracedCall call in _callStack)
            {
                if (!_calls.ContainsKey(call.Name))
                {
                    CallData callD = new CallData(call.Name);
                    _calls.Add(call.Name, callD);
                }

                _calls[call.Name].Add(call);
            }

            if (_calls.Count == 0)
            {
                return "No values...\n";
            }

            string text = "";

            foreach (CallData data in _calls.Values)
            {
                text += string.Format("{0} called {1} times, {2:0.000}s mean ({3:0.000}s < {4:0.000}s)\n", data.Name, data.NbCalls, data.MeanDuration.TotalSeconds, data.MinimumDuration.TotalSeconds, data.MaximumDuration.TotalSeconds);
            }

            return text;
        }

        public void SaveCsv(string inPath)
        {
            var csv = new StringBuilder();
            csv.Append("Cmd,Start,Duration\n");

            foreach (TracedCall call in _callStack)
            {
                csv.Append(string.Format("{0},{1},{2}\n", call.Name, call.Start, call.Duration));
            }

            File.WriteAllText(inPath, csv.ToString());
        }
    }
}
