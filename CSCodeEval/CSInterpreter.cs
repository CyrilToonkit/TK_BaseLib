﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace TK.BaseLib.CSCodeEval
{
    public class InterpreterResult
    {
        public InterpreterResult()
        {
        }

        public InterpreterResult(string inCode, bool inSuccess, object inOutput)
        {
            _code = inCode;
            _success = inSuccess;
            _output = inOutput;
        }

        string _code = "";
        bool _success = false;
        object _output = null;

        public string Code
        {
            get { return _code; }
        }

        public bool Success
        {
            get{return _success;}
        }

        public object Output
        {
            get{return _output;}
        }
    }

    public static class CSInterpreter
    {

        [System.Diagnostics.DebuggerNonUserCode]
        [System.Diagnostics.DebuggerStepThrough]
        private static object Invoke(MethodInfo method, Object target, params Object[] invokeArgs)
        {
            try
            {
                return method.Invoke(target, invokeArgs);
            }
            catch (TargetInvocationException te)
            {
                if (te.InnerException == null)
                    throw;
                Exception innerException = te.InnerException;

                ThreadStart savestack = Delegate.CreateDelegate(typeof(ThreadStart), innerException, "InternalPreserveStackTrace", false, false) as ThreadStart;
                if (savestack != null) savestack();
                throw innerException;// -- now we can re-throw without trashing the stack
            }
        }

        public static InterpreterResult Eval(string sCSCode, string sSCMethods, string sCustomAssemblies, string sCustomUsings, Dictionary<string, object> arguments, bool safe)
        {
            //validate Custom Assemblies
            string envFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

            List<string> CustomAssembliesPath = new List<string>();
            List<string> CustomAssemblies = new List<string>();

            //Perform several replacements as separators
            sCustomAssemblies = sCustomAssemblies.Replace("\r\n", "|");
            sCustomAssemblies = sCustomAssemblies.Replace("\n", "|");
            sCustomAssemblies = sCustomAssemblies.Replace(",", "|");
            sCustomAssemblies = sCustomAssemblies.Replace(";", "|");

            string[] split = sCustomAssemblies.Split("|".ToCharArray());

            if (split.Length > 0)
            {
                FileInfo file;
                string path;
                foreach (string testAssembly in split)
                {
                    path = envFolder + testAssembly;
                    file = new FileInfo(path);
                    if (file.Exists)
                    {
                        CustomAssembliesPath.Add(path);
                        CustomAssemblies.Add(Path.GetFileNameWithoutExtension(path));
                    }
                }
            }

            //Perform several replacements as separators
            sCustomUsings = sCustomUsings.Replace("\r\n", "|");
            sCustomUsings = sCustomUsings.Replace("\n", "|");
            sCustomUsings = sCustomUsings.Replace(",", "|");

            List<string> CustomUsings = TypesHelper.StringSplit(sCustomUsings, "|", true, true);

            CSharpCodeProvider c = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();

            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;

            cp.ReferencedAssemblies.Add("system.dll");
            cp.ReferencedAssemblies.Add("system.xml.dll");
            cp.ReferencedAssemblies.Add("system.data.dll");
            cp.ReferencedAssemblies.Add("system.windows.forms.dll");
            cp.ReferencedAssemblies.Add("system.drawing.dll");
            cp.IncludeDebugInformation = true;
            cp.GenerateInMemory = true;
            /*cp.GenerateInMemory = false;
            cp.OutputAssembly = "InterpreterAssembly.dll";*/

            //Custom assemblies
            foreach (string customPath in CustomAssembliesPath)
            {
                cp.ReferencedAssemblies.Add(customPath);
            }

            //Generate the code

            StringBuilder sb = new StringBuilder("");
            sb.Append("using System;\n");
            sb.Append("using System.Xml;\n");
            sb.Append("using System.Data;\n");
            sb.Append("using System.Data.SqlClient;\n");
            sb.Append("using System.Windows.Forms;\n");
            sb.Append("using System.Drawing;\n");

            //Custom usings
            foreach (string customPath in CustomUsings)
            {
                sb.Append(customPath + "\n");
            }

            List<string> argNames = new List<string>(arguments.Keys);

            sb.Append("public class CSCodeEvaler{ \n");

            sb.Append(sSCMethods);

            string argsText = "";
            foreach (string argKey in argNames)
            {
                argsText += argKey + ",";
            }

            if (argsText.Length > 1)
                argsText = argsText.Substring(0, argsText.Length - 1);

            sb.Append("public object EvalCode(" + argsText + "){\n");
            sb.Append(sCSCode + "\n");
            if (!sCSCode.Contains("return "))
            {
                sb.Append("return null;");
            }
            sb.Append("} \n");
            sb.Append("} \n");

            string code = sb.ToString();

            CompilerResults cr = c.CompileAssemblyFromSource(cp, code);
            if (cr.Errors.Count > 0)
            {
                return new InterpreterResult(code, false, cr.Errors);
            }

            System.Reflection.Assembly a = cr.CompiledAssembly;
            object o = a.CreateInstance("CSCodeEvaler");

            Type t = o.GetType();
            MethodInfo mi = t.GetMethod("EvalCode");

            object[] parameters = new object[argNames.Count];
            int cnt = 0;
            foreach (string argKey in argNames)
            {
                parameters[cnt] = arguments[argKey];
                cnt++;
            }

            object returnedValue = null;

            try
            {
                returnedValue = CSInterpreter.Invoke(mi, o, parameters);
            }
            catch (Exception e)
            {
                if (!safe)
                {
                    e.HelpLink += sCSCode;
                    throw;
                }

                string msg = e.Message + "\n" + e.StackTrace;
                if (e.InnerException != null)
                {
                    msg = e.InnerException.Message + "\n" + e.InnerException.StackTrace;
                }
                return new InterpreterResult(code, false, msg);
            }
            finally
            {

            }

            return new InterpreterResult(code, true, returnedValue);
        }
    }
}
