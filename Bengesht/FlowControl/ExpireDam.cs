using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using WiimoteLib;
using Rhino.Geometry;
using System.Windows.Forms;
using WebSocketSharp;



namespace Bengesht.FlowControlCat
{
public class ExpireDam : FlowControlComponent
{
    private bool isExpiring;

    public ExpireDam()
        : base
    (
        "Expire Dam",
        "<>",
        "Expire down stream object only if the gate is open"
    )
    {}//eof



	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
            "Expire down stream object only if the gate is open"
        );}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
        pManager.AddBooleanParameter("expire", "Exp", "Expire", GH_ParamAccess.item);
        pManager.AddGenericParameter("input value", "Val", "InputValue", GH_ParamAccess.item);
    }//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
        pManager.AddGenericParameter("Output value", "Val", "OutputValue", GH_ParamAccess.item);
    }//eof




    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    protected override void SolveInstance(IGH_DataAccess DA)
	{
        object value = null;
        bool expire = false;

        if (!DA.GetData(0, ref expire)) return;
        if (!DA.GetData(1, ref value)) return;
        

        this.isExpiring = expire;

        DA.SetData(0, value);
    }


    protected override void ExpireDownStreamObjects()
    {
        if(this.isExpiring)
            base.ExpireDownStreamObjects();
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
        get { return new Guid("16880C27-8F0A-4F97-9B17-8BCCDC11F5FB"); }
    }
}//eoc
}//eons
