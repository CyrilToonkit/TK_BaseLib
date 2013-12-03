using System;
using System.Collections.Generic;
using System.Text;

//COMMENTS OK
namespace TK.BaseLib.Processes
{
    /// <summary>
    /// Class that encapsulate a Benchmarker with a name to track processes
    /// </summary>
    public class TrackedProcess
    {
        // == CONSTRUCTORS ================================================================

        public TrackedProcess(string inName, int inLevel)
        {
            Name = inName;
            Level = inLevel;
            bench = new BenchMarker();
        }

        // == MEMBERS =====================================================================

        /// <summary>
        /// Name of the process
        /// </summary>
        public string Name;

        /// <summary>
        /// Stack level of the process
        /// </summary>
        public int Level = 0;

        /// <summary>
        /// private benchmarker
        /// </summary>
        BenchMarker bench;

        // == METHODS =====================================================================

        /// <summary>
        /// Starts the process
        /// </summary>
        /// <returns>The formatted string of the process name</returns>
        public string Start()
        {
            bench.Start();
            return Pad("== START " + Name + "  ==========================================================");
        }

        /// <summary>
        /// Ends the process
        /// </summary>
        /// <returns>The formatted string of the process name and duration</returns>
        public string End()
        {
            bench.Stop();
            return Pad("== END " + Name + ", duration " + bench.GetDurationMessage() + "  ==============================");
        }

        /// <summary>
        /// Add spaces in front of the string depending on the Level
        /// </summary>
        /// <param name="inRawStr">raww string</param>
        /// <returns>the padded string</returns>
        public string Pad(string inRawStr)
        {
            for(int i = 0; i < Level; i++)
            {
                inRawStr = " " + inRawStr;
            }

            return inRawStr;
        }
    }
}
