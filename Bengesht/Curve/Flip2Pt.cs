using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;




namespace Bengesht.CurveCat
{
    public class Flip2Pt : CurveComponent
{
	// Initializes a new instance of the curveFlip2Pt class.
	public Flip2Pt() : base
    (
        "Flip curve toward a point",
        "Flip2Pt",
        "Flip a curve toward a point."
    )
	{}//eof




    /// <summary>
    /// Help description
    /// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
		    "Flip a curve toward a point."
        );}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
        pManager.AddBooleanParameter("inverse", "Inv", "invert curves", GH_ParamAccess.item, false);
		pManager.AddPointParameter("Point", "Pt", "input point", GH_ParamAccess.item, Point3d.Origin);
		pManager.AddCurveParameter("Curve", "Crv", "input curve", GH_ParamAccess.item);
	}//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
		pManager.AddCurveParameter("Curve", "Crv", "output curve", GH_ParamAccess.item);
	}//eof




    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
	protected override void SolveInstance(IGH_DataAccess DA)
	{
        //<init>
        Boolean Inv = false;
		Point3d Pt = Point3d.Origin;
		Curve Crv = null;

        if (!DA.GetData(0, ref Inv)) return;
		if (!DA.GetData(1, ref Pt)) return;
        if (!DA.GetData(2, ref Crv)) return;
        //</init>

        if (Crv.PointAtEnd.DistanceTo(Pt) > Crv.PointAtStart.DistanceTo(Pt))
            Crv.Reverse();

        if (Inv)
            Crv.Reverse();

		DA.SetData(0, Crv);
	}//eof




    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.curveFlipToPoint;}
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
		get { return new Guid("{e5665b15-3a64-40d7-b0d9-79a420888dee}"); }
    }//eof




}//eoc
}//eons