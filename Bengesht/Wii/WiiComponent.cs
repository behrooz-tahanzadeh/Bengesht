using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Bengesht.WiiCat
{
abstract public class WiiComponent : GH_Component
{
    public WiiComponent(string name, string nickname, string description)
        : base(name, nickname, description, "Bengesht", "Wii")
    {
    }
}//eoc
}//eons
