using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Bengesht.HttpCat
{
abstract public class HttpComponent : GH_Component
{
    public HttpComponent(string name, string nickname, string description)
        : base(name, nickname, description, "Bengesht", "HTTP")
    {
    }
}//eoc
}//eons
