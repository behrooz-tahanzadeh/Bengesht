using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;




namespace Bengesht.CurveCat
{
public class StarLn : CurveComponent
{
	// Initializes a new instance of the curveAlignCrvSE class.
	public StarLn() : base
    (
        "Star Lines",
        "StarLn",
        "Create an simple star and give its lines."
    )
	{}//eof




    /// <summary>
    /// Help description
    /// </summary>
	protected override string HelpDescription
	{
        get { return BengeshtInfo.getComponentDescription(
            "Create an simple star and give its lines."
		);}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
		pManager.AddNumberParameter("radius", "R", "radius of star", GH_ParamAccess.item, 10);
		pManager.AddIntegerParameter("segments", "S", "segments of star", GH_ParamAccess.item, 5);
		pManager.AddIntegerParameter("jump number", "J", "jump number between each star's corner", GH_ParamAccess.item, 3);
	}//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
		pManager.AddLineParameter("outlut lines", "Ln", "star lines", GH_ParamAccess.list);
	}//eof




    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
	protected override void SolveInstance(IGH_DataAccess DA)
	{
        //<init>
		double R = 10;
		int S = 5;
		int J = 3;

		DA.GetData(0, ref R);
		DA.GetData(1, ref S);
		DA.GetData(2, ref J);
        //</init>

		Circle c = new Circle(R);
		List<Point3d> ps = new List<Point3d>();

		for (int i = 0; i < S; i++)
		{
			ps.Add(c.PointAt(i * ((2 * Math.PI) / S)));
		}

		List<Line> ls = new List<Line>();
		for (int i = 0; i < S; i++)
		{
			int pi = (J + i) % S;
			ls.Add(new Line(ps[i], ps[pi]));
		}

		DA.SetDataList(0, ls);

	}//eof




    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.curveStarLines;}
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
		get { return new Guid("{7f51c5b0-368d-4e40-8a1e-ce19c021aa5f}"); }
    }//eof




}//eoc
}//eons