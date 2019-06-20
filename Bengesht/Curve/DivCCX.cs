using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;




namespace Bengesht.CurveCat
{
public class DivCCX : CurveComponent
{
    /// <summary>
    /// Initializes a new instance of the AlignCrvSE class.
    /// </summary>
	public DivCCX(): base
    (
        "Divide Curves on Intersects",
        "DivCCX",
        "Divide curves on all of their intersects."
    )
	{}//eof




    /// <summary>
    /// Help description
    /// </summary>
	protected override string HelpDescription
	{
		get	{return BengeshtInfo.getComponentDescription(
            "Divide curves on all of their intersects."
		);}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
		pManager.AddCurveParameter("curves", "Crv", "curves to be divided", GH_ParamAccess.list);
        pManager.AddNumberParameter("Tolerance", "Tlr", "ZeroTolerance", GH_ParamAccess.item, Rhino.RhinoMath.ZeroTolerance);
	}//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
		pManager.AddCurveParameter("curves", "Crv", "aligned curves", GH_ParamAccess.list);
	}//eof




    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
	protected override void SolveInstance(IGH_DataAccess DA)
	{
		List<Curve> curves = new List<Curve>();
        double tlr = 0;

		if (!DA.GetDataList(0, curves)) return;
        DA.GetData(1, ref tlr);

		List<List<double>> intersectPoints = new List<List<double>>();
		for (int i = 0; i < curves.Count; ++i)
		{
			intersectPoints.Add(new List<double>());
		}
		List<Curve> outputCurves = new List<Curve>();

		for (int i = 0; i < curves.Count - 1; ++i)
		{
			for (int j = i + 1; j < curves.Count; ++j)
			{
				CurveIntersections cis = Intersection.CurveCurve(curves[i], curves[j], tlr, 0);
				for (int k = 0; k < cis.Count; ++k)
				{
					intersectPoints[i].Add(cis[k].ParameterA);
					intersectPoints[j].Add(cis[k].ParameterB);
				}
			}
		}

		for (int i = 0; i < curves.Count; ++i)
		{
			Curve[] crvI = curves[i].DuplicateCurve().Split(intersectPoints[i]);
			for (int j = 0; j < crvI.Length; ++j)
			{
				outputCurves.Add(crvI[j]);
			}
		}

		DA.SetDataList(0, outputCurves);
	}//eof




    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.curveDivCCX;}
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
		get { return new Guid("{4c0d75e1-4266-45b8-b5b4-826c9ad51ace}"); }
	}//eof




}//eoc
}//eons
