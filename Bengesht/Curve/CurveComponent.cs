using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Bengesht.CurveCat
{
abstract public class CurveComponent : GH_Component
{
    public CurveComponent(string name, string nickname, string description)
        : base(name, nickname, description, "Bengesht", "Curve")
    {}
}//eoc
}//eons
