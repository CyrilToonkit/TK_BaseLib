using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TK.BaseLib
{
    public static class PathHelper
    {
        public static void SearchReplace(string search, string replace, string inPath)
        {
            SearchReplace(search, replace, inPath, false, false);
        }

        public static void SearchReplace(string search, string replace, string inPath, bool inFolders, bool inRecursive)
        {
            string path = inPath;
            string trunk = "";
            string leaf = "";

            if (Path.HasExtension(inPath))//Simple file
            {
                trunk = GetFolderPath(inPath);
                leaf = Path.GetFileName(inPath);

                if (leaf.Contains(search))
                {
                    File.Move(inPath, Path.Combine(trunk, leaf.Replace(search, replace)));
                }
            }
            else//Directory
            {
                //Rename root folder
                if (inFolders)
                {
                    trunk = GetFolderPath(path);
                    leaf = Path.GetFileName(path);

                    if (leaf.Contains(search))
                    {
                        path = Path.Combine(trunk, leaf.Replace(search, replace));
                        Directory.Move(inPath, path);
                    }
                }
                //Rename files
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    SearchReplace(search, replace, file);
                }

                if (inRecursive)//Rename folders
                {
                    string[] directories = Directory.GetDirectories(path);
                    foreach (string dir in directories)
                    {
                        SearchReplace(search, replace, dir, inFolders, true);
                    }
                }
            }
        }

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

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists; if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
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
        public static DirectoryInfo DirectoryInfo(string path)
        {
            return new DirectoryInfo(ExpandedPath(path));
        }

        public static void  AddPathSubstitution(string token, string path)
        {
            if (PathSubstitutions.ContainsKey(token))
            {
                PathSubstitutions[token] = path;
            }
            else
            {
                PathSubstitutions.Add(token, path);
            }
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
                    string tmp = AppDomain.CurrentDomain.BaseDirectory;
                    sPathSubstitutions.Add("%OSCARPATH%", tmp);
                    sPathSubstitutions.Add("%OSCARDATAPATH%", Path.Combine( tmp, "Data"));
                    sPathSubstitutions.Add("$RIGSPATH", Path.Combine(tmp, "Data\\Rigs"));
                }
            }
        }

        private static object lok = new object();
    }
}
