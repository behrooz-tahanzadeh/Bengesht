using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using System.Net;

namespace Bengesht.HttpCat
{
    public class HttpOut : HttpComponent
{
    /// <summary>
    /// Initializes a new instance of the AssembleCurves class.
    /// </summary>
	public HttpOut() : base
    (
        "Http Output",
        "HttpOut",
        "Send response to client using and close connection."
    )
    {}//eof




    /// <summary>
    /// Help description
    /// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
			"Send response to client and close http connection."
		);}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
        pManager.AddTextParameter("response", "Response", "response to request", GH_ParamAccess.item,"");
        pManager.AddGenericParameter("HttpListener", "Listener", "respective listener", GH_ParamAccess.item);
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
        
        string responseString = "";
        HttpHandler httpHandler = new HttpHandler();

		if (!DA.GetData(0, ref responseString)) return;
		if (!DA.GetData(1, ref httpHandler)) return;

        if (httpHandler.isRunning)
        {
            if (responseString == null)
                httpHandler.response("");
            else
                httpHandler.response(responseString);
        }
        else
        {
            if (responseString != null)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "There's no request for this reponse!");
        }
	}//eof




    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
        get { return Resource.httpOut; }
	}//eof




    /// <summary>
    /// Gets the unique ID for this co
	public override Guid ComponentGuid
	{
        get { return new Guid("{B84402F3-870D-47DD-8FED-5C7BD21185C0}"); }
	}




}//eoc
}//eons