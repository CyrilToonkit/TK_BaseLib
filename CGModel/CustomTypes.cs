using System;

namespace TK.BaseLib.CGModel
{
    public enum AffectType
    { NoEffect, True, False }

    public enum BugFixStatusType
    { Waiting, Running, Fixed }

    public enum ApparatusState
    { Guide, Rig, Mixed }

    public enum ApparatusType
    { OneToOne, CurveInterpolated, Custom }
}
