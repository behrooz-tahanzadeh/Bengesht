using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Bengesht.MathCat
{
abstract public class MathComponent : GH_Component
{
    public MathComponent(string name, string nickname, string description)
        : base(name, nickname, description, "Bengesht", "Math")
    {}
}//eoc
}//eons
