using System;

namespace TK.BaseLib.CGModel
{
    public static class CustomTypes
    {
        static string _elementInfoName = "TK_OSCAR_ElementInfo";
        static string _initValuesName = "TK_OSCAR_InitValues";
        static string _guideElementName = "TK_OSCAR_GuideElement";
        static string[] _elementInfoTypes = new string[] { "Model", "Root", "Input", "Output","Null", "Controller", "Deformer", "PlaceHolder"};
        static string[] _guideRules = new string[] { "None", "AsItIs", "Oriented", "Delegate", "OrientedDelegate" };

        static string[] _kinematics = new string[] { "t", "r", "s"};
        static string[] _axis = new string[] { "x", "y", "z" };

        public static string ElementInfoName
        {
            get {return _elementInfoName;}
        }

        public static string InitValuesName
        {
            get { return _initValuesName; }
        }
        public static string GuideElementName
        {
            get { return _guideElementName; }
        }
        public static string ParamInitValuesName
        {
            get { return "initValues"; }
        }
        public static string ParamPreScriptName
        {
            get { return "preScript"; }
        }
        public static string ParamPostScriptName
        {
            get { return "postScript"; }
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

        public static string GeometriesName
        {
            get { return "Geometries"; }
        }

        public static string[] ElementInfoTypes
        {
            get { return _elementInfoTypes; }
        }

        public static string[] GuideRules
        {
            get { return _guideRules; }
        }

        public static string[] Kinematics
        {
            get { return _kinematics; }
        }

        public static string[] Axis
        {
            get { return _axis; }
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

        public static string GuideHelpers
        {
            get { return "guideHelpers"; }
        }
    }

    public enum Kinematics
    { Translation, Rotation, Scaling }

    public enum Axis
    { X, Y, Z }

    public struct Channel
    {
        public Channel(Kinematics inKine, Axis inAxis)
        {
            this.kinematic = inKine;
            this.axis = inAxis;
        }

        public override string ToString()
        {
            return String.Format("{0}{1}", CustomTypes.Kinematics[(int)kinematic], CustomTypes.Axis[(int)axis]);
        }

        public Kinematics kinematic;
        public Axis axis;
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
    { Position, Orientation, Direction, Pose, Scaling, Pin, Surface }
}
