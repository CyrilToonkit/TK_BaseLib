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

        public static string OscarPath
        {
            get
            {
                return PathSubstitutions["%OSCARPATH%"];
            }
        }

        public static string DefaultDataPath
        {
            get
            {
                return PathSubstitutions["%OSCARDATAPATH%"];
            }
        }        

        public static string ExpandedPath(string path)
        {
            //ret = Environment.ExpandEnvironmentVariables(ret);
            string ret = path;
            foreach (KeyValuePair<string, string> kvp in PathSubstitutions)
            {
                ret = ret.Replace(kvp.Key, kvp.Value);
            }            
            return ret;
        }

        public static string ContractedPath(string path)
        {
            string ret = path;
            foreach (KeyValuePair<string, string> kvp in PathSubstitutions)
            {                
                ret = ret.Replace(kvp.Value, kvp.Key);
            }
            return ret;
        }


        public static FileInfo FileInfo(string path)
        {
            return new FileInfo(ExpandedPath(path));
        }

        public static void  AddPathSubstitution(string token, string path)
        {
            PathSubstitutions.Add(token, path);
        }

        private static Dictionary<string, string> sPathSubstitutions = null;
        private static Dictionary<string, string> PathSubstitutions
        {
            get
            {
                if (sPathSubstitutions == null)
                    InitPathSubstitutions();
                return sPathSubstitutions;
            }
        }
        private static void InitPathSubstitutions()
        {
            lock (lok)
            {
                if (sPathSubstitutions == null)
                {
                    sPathSubstitutions = new Dictionary<string, string>();
                    string tmp = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                    sPathSubstitutions.Add("%OSCARPATH%", tmp);
                    sPathSubstitutions.Add("%OSCARDATAPATH%", tmp + @"\Data");
                }
            }
        }
        private static object lok = new object();
    }
}
