using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace TK.BaseLib.Checking
{
    public enum Statuses
    {
        Unchecked, Ok, SolvedAutomatically, Solved, Error
    }

    public class Check
    {
        public Check(){}

        public Check(string inName, string inDescription)
        {
            _name = inName;
            _description = inDescription;
        }

        string _name = "New Check";
        [CategoryAttribute("Basic")]
        [DescriptionAttribute("The short name of the check")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _description = "New Check";
        [CategoryAttribute("Basic")]
        [DescriptionAttribute("The description of the check")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        string _category = "Basic";
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        string _message = "";
        [Browsable(false)]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        bool _active = true;
        [CategoryAttribute("Basic")]
        [DescriptionAttribute("Whether the check is active in this Checklist")]
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        Statuses _status = Statuses.Unchecked;
        [Browsable(false)]
        public Statuses Status
        {
            get { return _status; }
            set { _status = value; }
        }

        CheckLogic _checkLogic = new CheckLogic();
        [CategoryAttribute("AutoCheck")]
        [DescriptionAttribute("The logic to be called")]
        public string CheckLogic
        {
            get { return (_checkLogic == null ? "" : _checkLogic.LogicName); }
            set
            {
                if (_checkLogic == null)
                {
                    _checkLogic = new CheckLogic();
                }
                _checkLogic.LogicName = value;
            }
        }
        [CategoryAttribute("AutoCheck")]
        [DescriptionAttribute("The parameters for the logic to be called")]
        public List<CheckParameter> CheckParameters
        {
            get { return (_checkLogic == null ? new List<CheckParameter>() : _checkLogic.Parameters); }
            set
            {
                if (_checkLogic == null)
                {
                    _checkLogic = new CheckLogic();
                }
                _checkLogic.Parameters = value;
            }
        }
        CheckLogic _solveLogic = new CheckLogic();
        [CategoryAttribute("AutoSolve")]
        [DescriptionAttribute("The logic to be called")]
        public string SolveLogic
        {
            get { return (_solveLogic == null ? "" : _solveLogic.LogicName); }
            set
            {
                if (_solveLogic == null)
                {
                    _solveLogic = new CheckLogic();
                }
                _solveLogic.LogicName = value;
            }
        }
        [CategoryAttribute("AutoSolve")]
        [DescriptionAttribute("The parameters for the logic to be called")]
        public List<CheckParameter> SolveParameters
        {
            get { return (_solveLogic == null ? new List<CheckParameter>() : _solveLogic.Parameters); }
            set
            {
                if (_solveLogic == null)
                {
                    _solveLogic = new CheckLogic();
                }
                _solveLogic.Parameters = value;
            }
        }

        [Browsable(false)]
        public bool AutoCheckable
        {
            get { return _checkLogic != null && !String.IsNullOrEmpty(_checkLogic.LogicName); }
        }

        [Browsable(false)]
        public bool AutoSolvable
        {
            get { return _solveLogic != null && !String.IsNullOrEmpty(_solveLogic.LogicName); }
        }

        public void Copy(Check inRefCheck)
        {
            _name = inRefCheck.Name;
            _description = inRefCheck.Description;
            _category = inRefCheck.Category;
            _message = inRefCheck.Message;
            _active = inRefCheck.Active;
            _status = inRefCheck.Status;

            _checkLogic.Copy(inRefCheck.GetCheckLogic());
            _solveLogic.Copy(inRefCheck.GetSolveLogic());
        }

        private CheckLogic GetSolveLogic()
        {
            return _solveLogic;
        }

        private CheckLogic GetCheckLogic()
        {
            return _solveLogic;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} ({2})", _name, _description, _category);
        }
    }
}
