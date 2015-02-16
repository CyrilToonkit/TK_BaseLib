using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Time
{
    public class TimeCode
    {
        public TimeCode()
        {

        }

        public TimeCode(float inFps, float inSeconds)
        {
            _fps = inFps;
            TotalSeconds = inSeconds;
        }

        public TimeCode(float inFps, int inFrames)
        {
            _fps = inFps;
            AddFrames(inFrames);
        }

        long _milliseconds = 0;

        float _fps = 25;

        public float Fps
        {
            get { return _fps; }
            set { _fps = value; }
        }

        public float TotalSeconds
        {
            get { return (float)_milliseconds / 1000f; }
            set { _milliseconds = (long)Math.Round((value * 1000f));}
        }

        public float TotalFrames
        {
            get { return (float)TotalSeconds * _fps; }
            set { TotalSeconds = value / _fps; }
        }

        public int Hours
        {
            get { return (int)Math.Floor((TotalSeconds / 3600f)); }
            set
            {
                TotalSeconds -= (float)(Hours * 3600f);
                TotalSeconds += (float)(value * 3600f);
            }
        }

        public int Minutes
        {
            get { return (int)Math.Floor((TotalSeconds / 60) % 60); }
            set
            {
                TotalSeconds -= (float)(Minutes * 60f);
                TotalSeconds += (float)(value * 60f);
            }
        }

        public int Seconds
        {
            get { return (int)Math.Floor(TotalSeconds % 60); }
            set
            {
                TotalSeconds -= (float)Seconds;
                TotalSeconds += (float)value;
            }
        }

        public int Frames
        {
            get { return (int)Math.Floor(((TotalSeconds * _fps) % _fps)); }
            set
            {
                TotalSeconds -= (float)Frames / _fps;
                TotalSeconds += (float)value / _fps;
            }
        }

        #region OPERATORS

        // Returns a new timeCode with the contents
        // multiplied together.
        public static TimeCode operator *(TimeCode tc1, float scal2)
        {
            if (tc1 == null)
                throw new ArgumentNullException("tc1");
            return new TimeCode(tc1.Fps, tc1.TotalSeconds * scal2);
        }

        // Returns a new timeCode with the contents
        // divided together.
        public static TimeCode operator /(TimeCode tc1, float scal2)
        {
            if (tc1 == null)
                throw new ArgumentNullException("tc1");
            return new TimeCode(tc1.Fps, tc1.TotalSeconds / scal2);
        }

        // Returns a new timeCode with the contents
        // added together.
        public static TimeCode operator +(TimeCode tc1, float scal2)
        {
            if (tc1 == null)
                throw new ArgumentNullException("tc1");
            return new TimeCode(tc1.Fps, tc1.TotalSeconds + scal2);
        }

        // Returns a new timeCode with the contents
        // substracted together.
        public static TimeCode operator -(TimeCode tc1, float scal2)
        {
            if (tc1 == null)
                throw new ArgumentNullException("tc1");
            return new TimeCode(tc1.Fps, tc1.TotalSeconds - scal2);
        }

        // Returns a new timeCode with the contents
        // added together.
        public static TimeCode operator +(TimeCode tc1, TimeCode tc2)
        {
            if (tc1 == null)
                throw new ArgumentNullException("tc1");
            if (tc1 == null)
                throw new ArgumentNullException("tc2");
            return new TimeCode(tc1.Fps, tc1.TotalSeconds + tc2.TotalSeconds);
        }

        // Returns a new timeCode with the contents
        // substracted together.
        public static TimeCode operator -(TimeCode tc1, TimeCode tc2)
        {
            if (tc1 == null)
                throw new ArgumentNullException("tc1");
            if (tc1 == null)
                throw new ArgumentNullException("tc2");
            return new TimeCode(tc1.Fps, tc1.TotalSeconds - tc2.TotalSeconds);
        }

        public static bool operator >(TimeCode tc1, TimeCode tc2)
        {
            if (tc1 == null)
                throw new ArgumentNullException("tc1");
            if (tc1 == null)
                throw new ArgumentNullException("tc2");
            return tc1.TotalSeconds > tc2.TotalSeconds;
        }

        public static bool operator <(TimeCode tc1, TimeCode tc2)
        {
            if (tc1 == null)
                throw new ArgumentNullException("tc1");
            if (tc1 == null)
                throw new ArgumentNullException("tc2");
            return tc1.TotalSeconds < tc2.TotalSeconds;
        }

        public static bool operator >=(TimeCode tc1, TimeCode tc2)
        {
            if (tc1 == null)
                throw new ArgumentNullException("tc1");
            if (tc1 == null)
                throw new ArgumentNullException("tc2");
            return tc1.TotalSeconds >= tc2.TotalSeconds;
        }

        public static bool operator <=(TimeCode tc1, TimeCode tc2)
        {
            if (tc1 == null)
                throw new ArgumentNullException("tc1");
            if (tc1 == null)
                throw new ArgumentNullException("tc2");
            return tc1.TotalSeconds <= tc2.TotalSeconds;
        }

        #endregion

        public void AddHours(float inHours)
        {
            float toAdd = inHours * 3600f;
            if (toAdd > 0 && Math.Abs(toAdd) < 0.001)
            {
                Console.Error.WriteLine("Value added two small, inferior to 1 millisecond !");
            }
            TotalSeconds += toAdd;
        }

        public void AddMinutes(float inMinutes)
        {
            float toAdd = inMinutes * 60f;
            if (toAdd > 0 && Math.Abs(toAdd) < 0.001)
            {
                Console.Error.WriteLine("Value added two small, inferior to 1 millisecond !");
            }
            TotalSeconds += toAdd;
        }

        public void AddSeconds(float inSeconds)
        {
            float toAdd = inSeconds;
            if (toAdd > 0 && Math.Abs(toAdd) < 0.001)
            {
                Console.Error.WriteLine("Value added two small, inferior to 1 millisecond !");
            }
            TotalSeconds += toAdd;
        }

        public void AddFrames(float inFrames)
        {
            float toAdd = inFrames / _fps;
            if (toAdd > 0 && Math.Abs(toAdd) < 0.001)
            {
                Console.Error.WriteLine("Value added two small, inferior to 1 millisecond !");
            }
            TotalSeconds += toAdd;
        }

        public void Convert(float inFps)
        {
            TotalSeconds = TotalSeconds * (_fps / inFps);
            _fps = inFps;
        }

        public static TimeCode Parse(string inStrTimeCode)
        {
            return TimeCode.Parse(inStrTimeCode, 25);
        }

        public static TimeCode Parse(string inStrTimeCode, float inFps)
        {
            TimeCode tc = new TimeCode(inFps, 0);
            string[] parts = inStrTimeCode.Split(':');

            if (parts.Length > 0)
            {
                string frames = parts[parts.Length - 1].Trim();
                tc.AddFrames(TypesHelper.FloatParse(frames));

                if (parts.Length > 1)
                {
                    string seconds = parts[parts.Length - 2].Trim();
                    tc.AddSeconds(TypesHelper.FloatParse(seconds));

                    if (parts.Length > 2)
                    {
                        string minutes = parts[parts.Length - 3].Trim();
                        tc.AddMinutes(TypesHelper.FloatParse(minutes));

                        if (parts.Length > 3)
                        {
                            string hours = parts[parts.Length - 4].Trim();
                            tc.AddHours(TypesHelper.FloatParse(hours));
                        }
                    }
                }
            }

            return tc;
        }

        public static bool TryParse(string inStrTimeCode, out TimeCode outTimeCode)
        {
            try
            {
                outTimeCode = TimeCode.Parse(inStrTimeCode);
                return true;
            }
            catch (Exception) { outTimeCode = new TimeCode(); return false; }
        }

        public static bool TryParse(string inStrTimeCode, float inFps, out TimeCode outTimeCode)
        {
            try
            {
                outTimeCode = TimeCode.Parse(inStrTimeCode, inFps);
                return true;
            }
            catch (Exception) { outTimeCode = new TimeCode(); return false; }
        }

        public string ToLongString()
        {
            string value = "";
            if (Hours != 0)
            {
                value += (string.IsNullOrEmpty(value) ? "" : ", ") + string.Format("{0} Hour{1}", Hours, Math.Abs(Hours) > 1 ? "s" : "");
            }
            if (Minutes != 0)
            {
                value += (string.IsNullOrEmpty(value) ? "" : ", ") + string.Format("{0} Minute{1}", Minutes, Math.Abs(Minutes) > 1 ? "s" : "");
            }
            if (Seconds != 0)
            {
                value += (string.IsNullOrEmpty(value) ? "" : ", ") + string.Format("{0} Second{1}", Seconds, Math.Abs(Seconds) > 1 ? "s" : "");
            }

            if (Frames != 0 || value == "")
            {
                value += (string.IsNullOrEmpty(value) ? "" : ", ") + string.Format("{0} Frame{1}", Frames, Math.Abs(Frames) > 1 ? "s" : "");
            }

            value += string.Format(" (at {0} fps)", _fps);

            return value;
        }

        public override string ToString()
        {
            return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", Hours, Minutes, Seconds, Frames);
        }
    }
}
