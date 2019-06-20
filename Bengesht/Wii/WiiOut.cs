using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using WiimoteLib;
using Rhino.Geometry;



namespace Bengesht.WiiCat
{
public class WiiOut : WiiComponent
{

    /// <summary>
    /// Initializes a new instance of the AssembleCurves class.
    /// </summary>
    public WiiOut():base
    (
        "WiiOut",
        "Wii<<",
        "Send data to wii controller"
    )
    {}//eof
    



    /// <summary>
    /// Help description
    /// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
            "Send data to available wii controllers..."
        );}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
        pManager.AddGenericParameter("WiiObjects", "Wii", "wii objects", GH_ParamAccess.item);
        pManager.AddBooleanParameter("LED 1", "L1", "LED 1", GH_ParamAccess.item, false);
        pManager.AddBooleanParameter("LED 2", "L2", "LED 2", GH_ParamAccess.item, false);
        pManager.AddBooleanParameter("LED 3", "L3", "LED 3", GH_ParamAccess.item, false);
        pManager.AddBooleanParameter("LED 4", "L4", "LED 4", GH_ParamAccess.item, false);
        pManager.AddBooleanParameter("Rumble", "Rmb", "Rumble", GH_ParamAccess.item, false);
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
        WiiObj wiiObj = new WiiObj();
        Boolean led1 = false;
        Boolean led2 = false;
        Boolean led3 = false;
        Boolean led4 = false;
        Boolean rumble = false;

        if (!DA.GetData(0, ref wiiObj)) return;
        if (!DA.GetData(1, ref led1)) return;
        if (!DA.GetData(2, ref led2)) return;
        if (!DA.GetData(3, ref led3)) return;
        if (!DA.GetData(4, ref led4)) return;
        if (!DA.GetData(5, ref rumble)) return;

        wiiObj.wiimote.SetLEDs(led1, led2, led3, led4);
        wiiObj.wiimote.SetRumble(rumble);
    }//eof




    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.WiiOut;}
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
        get { return new Guid("5E5FF99D-3EDD-41A0-A333-72B48FDD608E"); }

    }
}//eoc
}//eons