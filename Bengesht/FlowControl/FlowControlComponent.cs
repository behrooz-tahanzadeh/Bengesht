using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Bengesht.FlowControlCat
{
abstract public class FlowControlComponent : GH_Component
{
    public FlowControlComponent(string name, string nickname, string description)
        : base(name, nickname, description, "Bengesht", "FlowControl")
    {
    }
}//eoc
}//eons
