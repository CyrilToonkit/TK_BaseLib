using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Checking
{
    public class CheckLogic
    {
        public CheckLogic(){}

        public CheckLogic(string inName, params CheckParameter[] inParameters)
        {
            _logicName = inName;
            _parameters = new List<CheckParameter>(inParameters);
        }

        string _logicName = "";
        public string LogicName
        {
            get { return _logicName; }
            set { _logicName = value; }
        }

        List<CheckParameter> _parameters = new List<CheckParameter>();
        public List<CheckParameter> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public override string ToString()
        {
            string parametersStr = "";
            foreach (CheckParameter param in Parameters)
            {
                parametersStr += (string.IsNullOrEmpty(parametersStr) ? "" : ",") + param.StringValue;
            }

            return string.Format("{0}({1})", _logicName, parametersStr);
        }

        internal void Copy(CheckLogic inRefLogic)
        {
            _logicName = inRefLogic.LogicName;
            _parameters.Clear();

            foreach (CheckParameter param in inRefLogic.Parameters)
            {
                CheckParameter newParam = new CheckParameter();
                newParam.Copy(param);
                _parameters.Add(param);
            }
        }
    }
}
