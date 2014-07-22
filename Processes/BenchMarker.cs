using System;
using System.Collections.Generic;
using System.Text;

//COMMENTS OK
namespace TK.BaseLib.Processes
{
    /// <summary>
    /// Class to manage timeSpans, basically starts and stops a timer and have convenience methods to log duration
    /// </summary>
    public class BenchMarker
    {
        // == MEMBERS =====================================================================

        /// <summary>
        /// Save Before ticks
        /// </summary>
        long Before;

        /// <summary>
        /// Save After Ticks
        /// </summary>
        long After = 0;

        /// <summary>
        /// TimeSpan for the process
        /// </summary>
        TimeSpan span;
        public TimeSpan TimeSpan
        {
            get
            {
                return span;
            }
        }
        // == METHODS =====================================================================

        /// <summary>
        /// Start the timer
        /// </summary>
        public DateTime Start()
        {
            DateTime now = DateTime.Now;
            Before = now.Ticks;

            return now;
        }

        /// <summary>
        /// End the timer
        /// </summary>
        public void Stop()
        {
            After = DateTime.Now.Ticks;
            span = new TimeSpan(After - Before);
        }

        /// <summary>
        /// Get number of seconds in the span
        /// </summary>
        public double Seconds
        {
            get { return (After != 0 ? span.TotalSeconds : 0); }
        }

        /// <summary>
        /// Get number of milliseconds in the span
        /// </summary>
        public double Milliseconds
        {
            get { return (After != 0 ? span.TotalMilliseconds : 0); }
        }

        /// <summary>
        /// Get the formatted duration message (in seconds or milliseconds)
        /// </summary>
        /// <returns>The formatted duration message</returns>
        public string GetDurationMessage()
        {
            double seconds = Math.Floor(Seconds);
            if (seconds > 0)
            {
                return seconds + " s. " + (Milliseconds - seconds * 1000).ToString() + " ms.";
            }
            else
            {
                return Milliseconds.ToString() + " ms.";
            }
        }
    }
}
