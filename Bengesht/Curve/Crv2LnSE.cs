using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;




namespace Bengesht.CurveCat
{
public class Crv2LnSE : CurveComponent
{
    /// <summary>
    /// Initializes a new instance of the AssembleCurves class.
    /// </summary>
    public Crv2LnSE() : base
    (
        "Curve To Line Start/End",
        "Crv2LnS/E",
        "Convert curves to lines, based on their start/end points."
    )
	{
		IconDisplayMode = GH_IconDisplayMode.icon;
	}//eof




	/// <summary>
    /// Help description
	/// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
			"Convert curves to lines, based on their start/end points."
		);}
	}//eof




	/// <summary>
    /// Registers all the input parameters for this component.
	/// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
		pManager.AddCurveParameter("Curve", "Crv", "input curves", GH_ParamAccess.item);
	}//eof




	/// <summary>
    /// Registers all the output parameters for this component.
	/// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
		pManager.AddLineParameter("Line", "Ln", "output lines", GH_ParamAccess.item);
	}//eof




	/// <summary>
    /// This is the method that actually does the work.
	/// </summary>
	protected override void SolveInstance(IGH_DataAccess DA)
	{
        Curve Crv = null;

        if (!DA.GetData("Curve", ref Crv)) return;

        Line outputLn = new Line(Crv.PointAtStart, Crv.PointAtEnd);
        
        DA.SetData(0, outputLn);
	}//eof




	/// <summary>
    /// Provides an Icon for the component.
	/// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.curveCrv2LnSE;}
	}//eof




	/// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
	/// </summary>
	public override Guid ComponentGuid
	{
		get { return new Guid("{c0d2c5c6-230d-4371-9304-d462f114484f}"); }
	}//eof




}//eoc
}//eons