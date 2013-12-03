using System;
using System.Collections.Generic;
using System.Text;

//COMMENTS OK
namespace TK.BaseLib.Processes
{
    /// <summary>
    /// Class that encapsulate a Benchmarker with a name to track processes
    /// </summary>
    public class PostponedProcess
    {
        public string Name = "";
        public IPostProcessor Processor;
        public object Data;
        public int Step;

        // == CONSTRUCTORS ================================================================

        public PostponedProcess(IPostProcessor inProcessor, string inName, object inData, int inStep)
        {
            Name = inName;
            Processor = inProcessor;
            Data = inData;
            Step = inStep;
        }
    }
}
