using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Rhino;




namespace Bengesht.CurveCat
{
    public class ClassifyCrvSE : CurveComponent
{
	/// <summary>
    /// Initializes a new instance of the curveClassifyCrvSE class.
	/// </summary>
	public ClassifyCrvSE(): base
    (
        "Classify Curves",
        "ClCrvS/E",
        "Classify curves based on their start/end points."
    )
	{}//eof




	/// <summary>
    /// Help description
	/// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
		    "Classify curves based on their start/end points."
		);}
	}//eof




	/// <summary>
    /// Registers all the input parameters for this component.
	/// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
		pManager.AddCurveParameter("curves", "Crv", "curves to be classified", GH_ParamAccess.list);
	}//eof




	/// <summary>
    /// Registers all the output parameters for this component.
	/// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
		pManager.AddCurveParameter("curves", "Crv", "classified curves", GH_ParamAccess.tree);
	}//eof




	/// <summary>
    /// This is the method that actually does the work.
	/// </summary>
	protected override void SolveInstance(IGH_DataAccess DA)
	{
		List<Curve> crv = new List<Curve>();
		if (!DA.GetDataList(0, crv)) return;

		DataTree<Curve> r = new DataTree<Curve>();

		for (int i = 0; i < crv.Count; ++i)
		{
			if (!isChildOfCrv(crv[i], crv))
			{
				GH_Path p = new GH_Path(0);
				r.Add(crv[i], p);
			}
		}

		bool isAnyCrvAligned = true;

		while (isAnyCrvAligned)
		{
			isAnyCrvAligned = false;
			int BC = r.BranchCount;
			for (int i = 0; i < BC; ++i)
			{
				for (int j = 0; j < crv.Count; ++j)
				{
					if (r.AllData().IndexOf(crv[j]) == -1)
					{
						GH_Path p = new GH_Path(i + 1);
						if (isChildOfCrv(crv[j], r.Branch(i)))
						{
							r.Add(crv[j], p);
							isAnyCrvAligned = true;
						}
					}
				}
			}
		}

		DA.SetDataTree(0, r);
	}//eof




	//<Custom additional code> 
	bool isChildOfCrv(Curve input, List<Curve> crv)
	{
		for (int i = 0; i < crv.Count; ++i)
		{
			if (crv[i].PointAtEnd.DistanceTo(input.PointAtStart) <= RhinoMath.ZeroTolerance) { return true; }
		}
		return false;
	}//eof
	//</Custom additional code> 




	/// <summary>
    /// Provides an Icon for the component.
	/// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.curveClassifyCrvSE;}
	}//eof




	/// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
	/// </summary>
	public override Guid ComponentGuid
	{
		get { return new Guid("{07d4aaaf-06f0-40fc-b879-f60a75a18da7}"); }
	}//eof




}//eoc
}//eons