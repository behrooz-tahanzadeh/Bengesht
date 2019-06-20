using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;




namespace Bengesht.CurveCat
{
    public class AssembleCurves : CurveComponent
{
    /// <summary>
    /// Initializes a new instance of the AssembleCurves class.
    /// </summary>
	public AssembleCurves(): base
    (
        "Assemble Curves",
        "AsmblCrvS/E",
        "Assemble curves based on their start/end points."
    )
	{}//eof




    /// <summary>
    /// Help description
    /// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
			"Assemble curves based on their Start/End Points."
	    );}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
		pManager.AddCurveParameter("curves", "Crv", "curves to be asssembled", GH_ParamAccess.list);
	}//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
		pManager.AddCurveParameter("curves", "Crv", "assembleded curves", GH_ParamAccess.list);
	}//eof



    
    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
	protected override void SolveInstance(IGH_DataAccess DA)
	{
		List<Curve> crv = new List<Curve>();
		if (!DA.GetDataList(0, crv)) return;

		List<Curve> alignedCurves = new List<Curve>();
		Curve tCrv = null;
		bool isAnyCrvAligned = true;

		alignedCurves.Add(crv[0]);
		while (isAnyCrvAligned)
		{
			isAnyCrvAligned = false;
			for (int i = 0; i < crv.Count; ++i)
			{
				if (alignedCurves.IndexOf(crv[i]) == -1)
				{
					tCrv = alignCurve(crv[i], alignedCurves);
					if (tCrv != null)
					{
						crv[i] = tCrv;
						alignedCurves.Add(crv[i]);
						isAnyCrvAligned = true;
					}
				}
			}
			if (!isAnyCrvAligned)
			{
				for (int i = 0; i < crv.Count; ++i)
				{
					if (alignedCurves.IndexOf(crv[i]) == -1)
					{
						alignedCurves.Add(crv[i]);
						isAnyCrvAligned = true;
						break;
					}
				}
			}
		}

		DA.SetDataList(0, alignedCurves);
	}//eof




	//<Custom additional code> 
	private Curve alignCurve(Curve crv, List<Curve> aCrv)
	{
		for (int i = 0; i < aCrv.Count; ++i)
		{
			if (areEqualPoints(crv.PointAtStart, aCrv[i].PointAtEnd))
			{
				crv.Translate(0, 0, aCrv[i].PointAtEnd.Z - crv.PointAtStart.Z);
				return crv;
			}
		}

		for (int i = 0; i < aCrv.Count; ++i)
		{
			if (areEqualPoints(crv.PointAtStart, aCrv[i].PointAtStart))
			{
				crv.Translate(0, 0, aCrv[i].PointAtStart.Z - crv.PointAtStart.Z);
				return crv;
			}
		}

		for (int i = 0; i < aCrv.Count; ++i)
		{
			if (areEqualPoints(crv.PointAtEnd, aCrv[i].PointAtEnd))
			{
				crv.Translate(0, 0, aCrv[i].PointAtEnd.Z - crv.PointAtEnd.Z);
				return crv;
			}
		}

		for (int i = 0; i < aCrv.Count; ++i)
		{
			if (areEqualPoints(crv.PointAtEnd, aCrv[i].PointAtStart))
			{
				crv.Translate(0, 0, aCrv[i].PointAtStart.Z - crv.PointAtEnd.Z);
				return crv;
			}
		}
		return null;
	}//eof




	private bool areEqualPoints(Point3d p1, Point3d p2)
	{
		return (Math.Abs(p1.X - p2.X) <= 0.01 && Math.Abs(p1.Y - p2.Y) <= 0.01);
	}//eof
	//</Custom additional code>




    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get { return Resource.curveAssembleCurves; }
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
		get { return new Guid("{c42b26a8-e774-45b7-a13a-4ebc4acc3973}"); }
	}//eof




}//eoc
}//eons