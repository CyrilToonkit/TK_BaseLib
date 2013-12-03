using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Processes
{
    public interface IPostProcessor
    {
        bool PostProcess(string processName, object processData);
    }
}
