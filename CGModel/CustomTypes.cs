using System;

namespace TK.BaseLib.CGModel
{
    public static class CustomTypes
    {
        static string _elementInfoName = "TK_OSCAR_ElementInfo";
        static string[] _elementInfoTypes = new string[] { "Model", "Root", "Controller", "Deformer", "PlaceHolder", "Output", "Input", "Null" };
        static string[] _guideRules = new string[] { "None", "AsItIs", "Oriented", "Delegate", "OrientedDelegate" };

        public static string ElementInfoName
        {
            get {return _elementInfoName;}
        }

        public static string ParamTypeName
        {
            get { return "tk_type"; }
        }
        public static string ParamRuleName
        {
            get { return "tk_guideRule"; }
        }
        public static string ParamPlaceHolderName
        {
            get { return "tk_placeHolder"; }
        }
        public static string ParamUpVName
        {
            get { return "tk_upVReference"; }
        }
        public static string ParamTargetName
        {
            get { return "tk_target"; }
        }
        public static string ParamInvertName
        {
            get { return "tk_invert"; }
        }
        public static string ParamInvertUpName
        {
            get { return "tk_invertUp"; }
        }
        public static string ParamGuideParentName
        {
            get { return "tk_guideParent"; }
        }

        public static string[] ElementInfoTypes
        {
            get { return _elementInfoTypes; }
        }

        public static string[] GuideRules
        {
            get { return _guideRules; }
        }

        public static int GuideRule(string inGuideRule)
        {
            int rule = 0;
            foreach(string strRule in _guideRules)
            {
                if(strRule == inGuideRule)
                {
                    return rule;
                }
                rule++;
            }
            return 0;
        }
    }

    public enum AffectType
    { NoEffect, True, False }

    public enum BugFixStatusType
    { Waiting, Running, Fixed }

    public enum ApparatusState
    { Guide, Rig, Mixed }

    public enum ApparatusType
    { OneToOne, CurveInterpolated, Custom }

    public enum ConstraintType
    { Position, Orientation, Direction, Pose, Scaling, Pin }
}
