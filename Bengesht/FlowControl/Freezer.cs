using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using WiimoteLib;
using Rhino.Geometry;
using System.Windows.Forms;
using WebSocketSharp;
using System.Threading;



namespace Bengesht.FlowControlCat
{
public class Freezer : FlowControlComponent
{
    public Freezer()
        : base
    (
        "Freezer",
        "FRZ",
        "Freeze the grasshopper solution for given amount of secounds (Developer tool)"
    )
    {}//eof



	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
            "Freeze the grasshopper solution for given amount of secounds (Developer tool)"
        );}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
        pManager.AddNumberParameter("seconds", "Sec", "Seconds", GH_ParamAccess.item);
        pManager.AddGenericParameter("trigger", "TRG", "Trigger the component", GH_ParamAccess.tree);
    }//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
    }//eof




    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    protected override void SolveInstance(IGH_DataAccess DA)
	{
        double seconds = 0;

        if (!DA.GetData(0, ref seconds)) return;

        Thread.Sleep((Int32)seconds * 1000);
    }



    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.expireDam;}
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
        get { return new Guid("BF75C756-4031-4F7F-8C30-17414BDA06C6"); }
    }
}//eoc
}//eons
