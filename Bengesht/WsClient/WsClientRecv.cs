using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using WiimoteLib;
using Rhino.Geometry;
using System.Windows.Forms;
using WebSocketSharp;
using Bengesht.WsClient;
using Grasshopper;



namespace Bengesht.WsClientCat
{
public class WsClientRecv : WsClientComponent
{
    private WsObject wscObj;
    private bool onMessageTriggered;
    private GH_Document ghDocument;
    private bool isAutoUpdate;
    private bool isAskingNewSolution;
    private List<string> buffer = new List<string>();




    /// <summary>
    /// Initializes a new instance of the AssembleCurves class.
    /// </summary>
    public WsClientRecv(): base
    (
        "Websocket Client Receiver",
        "WS>>",
        "Read data from websocket"
    )
    {
        onMessageTriggered = false;
        this.isAutoUpdate = true;
        this.isAskingNewSolution = false;
    }//eof
    



    /// <summary>
    /// Help description
    /// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
            "Read data from websocket."
        );}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
        pManager.AddGenericParameter("Websocket Objects", "WSC", "websocket objects", GH_ParamAccess.item);
        pManager.AddBooleanParameter("Auto Update", "Upd", "update solution on new message, not recommended for high frequency inputs", GH_ParamAccess.item, true);
    }//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
        pManager.AddGenericParameter("Message", "Msg", "message", GH_ParamAccess.item);
        pManager.AddGenericParameter("Status", "Sts", "status", GH_ParamAccess.item);
    }//eof




    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    protected override void SolveInstance(IGH_DataAccess DA)
	{
        DA.GetData(1, ref this.isAutoUpdate);

        if(this.ghDocument == null)
        {
            this.ghDocument = OnPingDocument();
            if (this.ghDocument == null) return;

            GH_Document.SolutionEndEventHandler handle = delegate(Object sender, GH_SolutionEventArgs e)
            {
                
            };

            ghDocument.SolutionEnd += handle;
        }

        if(!this.onMessageTriggered)
        {
            WsObject wscObj = new WsObject();

            if (DA.GetData(0, ref wscObj))
            {
                if (this.wscObj != wscObj)
                {
                    this.unsubscribeEventHandlers();
                    this.wscObj = wscObj;
                    this.subscribeEventHandlers();
                }
            }
            else
            {
                this.unsubscribeEventHandlers();
                this.wscObj = null;
                this.onMessageTriggered = false;
                return;
            }
        }

        DA.SetData(0, this.wscObj.message);
        DA.SetData(1, WsObjectStatus.GetStatusName(this.wscObj.status));
        this.onMessageTriggered = false;
    }




    private void unsubscribeEventHandlers()
    {
        try { this.wscObj.changed -= this.wscObjOnChanged; }
        catch {}
    }//eof




    private void subscribeEventHandlers()
    {
        this.wscObj.changed += this.wscObjOnChanged;
    }




    private void wscObjOnChanged(object sender, EventArgs e)
    {
        /*
        ghDocument.ScheduleSolution(0, doc =>
        {
            this.onMessageTriggered = true;
            this.ExpireSolution(true);
        });
        */

        if (this.isAutoUpdate && ghDocument.SolutionState != GH_ProcessStep.Process && wscObj != null && !isAskingNewSolution)
        {
            
            GH_InstanceServer.DocumentEditor.BeginInvoke((Action)delegate()
            {
                if (ghDocument.SolutionState != GH_ProcessStep.Process)
                {
                    isAskingNewSolution = true;
                    this.onMessageTriggered = true; 
                    this.ExpireSolution(true);
                    isAskingNewSolution = false;
                }
            });
        }
    }//eof



    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.WscRecv;}
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
        get { return new Guid("7A10D835-7BB5-4476-B46D-415DA87D9981"); }
    }
}//eoc
}//eons
