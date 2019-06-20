using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Bengesht.WsClientCat
{
abstract public class WsClientComponent : GH_Component
{
    public WsClientComponent(string name, string nickname, string description)
        : base(name, nickname, description, "Bengesht", "WebSocket")
    {
    }
}//eoc
}//eons
