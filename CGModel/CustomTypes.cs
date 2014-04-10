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

        public static string[] ElementInfoTypes
        {
            get { return _elementInfoTypes; }
        }

        public static string[] GuideRules
        {
            get { return _guideRules; }
        }
    }

    public enum RigObjectType
    { Model, Root, Input, Output, Null, Control, Deform, PlaceHolder }

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
