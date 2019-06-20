using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using System.Net;

namespace Bengesht.HttpCat
{
public class HttpIn : HttpComponent
{
    private bool onChangedTriggered;
    private HttpHandler httpHandler;
    private string prevAddress;
    private bool documentEventRegistered;



    /// <summary>
    /// Initializes a new instance of the AssembleCurves class.
    /// </summary>
	public HttpIn() : base
    (
        "Http Input",
        "HttpIn",
        "Start create an http server, listening to an address."
    )
	{
        this.onChangedTriggered = false;
        this.httpHandler = new HttpHandler();
        this.prevAddress = "";
        this.httpHandler.changed += this.httpHandlerOnChanged;
        this.documentEventRegistered = false;
    }//eof




    /// <summary>
    /// Help description
    /// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
			"Create a webserver and start listening to an address. It's a start point for each request which must be ended by and HttpOut component."
		);}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
        pManager.AddBooleanParameter("start", "Start", "It turns on/off webserver", GH_ParamAccess.item,false);
        pManager.AddTextParameter("prefix", "Prefix", "address to be listened", GH_ParamAccess.item, "http://localhost:8080/bengesht/");
	}//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
        pManager.AddTextParameter("keys", "Keys", "All query parameter keys", GH_ParamAccess.list);
        pManager.AddTextParameter("values", "Values", "All query parameter values", GH_ParamAccess.list);
        pManager.AddGenericParameter("listener object", "Listener", "it should be passed to httpOut component", GH_ParamAccess.item);
	}//eof




    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
	protected override void SolveInstance(IGH_DataAccess DA)
	{
        this.subscribeOnDocumentObjectsDeleted();

        if (!System.Net.HttpListener.IsSupported)
        {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            return;
        }

        if(!this.onChangedTriggered)
        {
            bool start = false;
            string prefix = null;

            if (!DA.GetData(0, ref start)) return;
            if (!DA.GetData(1, ref prefix)) return;

            if (start)
            {
                if (this.prevAddress != prefix || !this.httpHandler.isRunning)
                {
                    this.httpHandler.stop();
                    this.httpHandler.init(prefix).start();
                    this.prevAddress = prefix;

                    if (this.httpHandler.canNotStartListener)
                    {
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, this.httpHandler.canNotStartListenerMessage);
                        this.httpHandler.canNotStartListener = false;
                    }
                }
            }
            else
            {
                this.httpHandler.stop();
            }
        }

        DA.SetDataList(0, this.httpHandler.keyList);
        DA.SetDataList(1, this.httpHandler.valueList);
        DA.SetData(2, this.httpHandler);

        this.onChangedTriggered = false;
	}



    private void httpHandlerOnChanged(object sender, EventArgs e)
    {
        this.onChangedTriggered = true;
        this.ExpireSolution(true);
    }//eof



    private void DocumentObjectsDeleted(object sender, GH_DocObjectEventArgs e)
    {
        if (e.Objects.Contains(this))
        {
            e.Document.ObjectsDeleted -= DocumentObjectsDeleted;
            this.documentEventRegistered = false;
            this.httpHandler.stop();
        }
    }



    private void subscribeOnDocumentObjectsDeleted()
    {
        if (!documentEventRegistered)
        {
            GH_Document doc = OnPingDocument();

            if (doc != null)
            {
                documentEventRegistered = true;
                doc.ObjectsDeleted += DocumentObjectsDeleted;
            }
        }
    }



    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.httpIn;}
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
        get { return new Guid("{982093F5-A33D-469B-B13F-D7E1B54E945A}"); }
	}
}//eoc



}//eons