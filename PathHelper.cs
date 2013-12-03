using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TK.BaseLib
{
    public static class PathHelper
    {
        public static string GetFolderPath(string inPath)
        {
            return inPath == "" ? "" : inPath.Substring(0, inPath.Length - Path.GetFileName(inPath).Length - 1);
        }
    }
}
