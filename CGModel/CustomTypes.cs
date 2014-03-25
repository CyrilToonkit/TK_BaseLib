using System;

namespace TK.BaseLib.CGModel
{
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
