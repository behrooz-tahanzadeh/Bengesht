using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Net;
using WebSocketSharp;
using System.Windows.Forms;
using Bengesht.WsClient;
using Grasshopper;



namespace Bengesht.WsClientCat
{
public class WsClientStart : WsClientComponent
{
	private WsObject wscObj;
	private bool isSubscribedToEvents;
	private GH_Document ghDocument;
	private WsAddress wsAddress;



	public WsClientStart():base
	(
		"Websocket Client Start",
		"WS*",
		"Start a new connection to a Websocket server."
	)
	{
		this.isSubscribedToEvents = false;
		this.wsAddress = new WsAddress("");
	}//eof




	~WsClientStart()
	{
		this.disconnect();
	}




	protected override string HelpDescription
	{
		get
		{
			return BengeshtInfo.getComponentDescription
			(
				"Start a new connection to a Websocket server. Read more about <a href=\"\">websocket connection<a>."
			);
		}
	}//eof



	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
		pManager.AddTextParameter("address", "URL", "Websocket server address. Scheme (ws://) should be included. For example <b>ws://echo.websocket.org</b>", GH_ParamAccess.item);
		pManager.AddTextParameter("inital message", "Msg", "initial message", GH_ParamAccess.item, "Hello World");
		pManager.AddBooleanParameter("reset", "Rst", "Restart the connection.", GH_ParamAccess.item, false);
	}//eof




	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
		pManager.AddGenericParameter("Websocket Objects", "WSC", "This object provides access to the connection. Connect this output to WS input websocket Send/Recv components.", GH_ParamAccess.item);
	}//eof




	/// <summary>
	/// Disconnect from websocket server.
	/// This function needs to be run on events such as delete the component.
	/// </summary>
	private void disconnect()
	{
		if(this.wscObj != null)
		{
			try { this.wscObj.disconnect(); }
			catch { }
			this.wscObj.changed -= this.wsObjectOnChange;
			this.wscObj = null;
			this.wsAddress.setAddress(null);
		}
	}




	/// <summary>
	/// Detecting the deletation of this component and run disconnect function.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void documentOnObjectsDeleted(object sender, GH_DocObjectEventArgs e)
	{
		if (e.Objects.Contains(this))
		{
			e.Document.ObjectsDeleted -= documentOnObjectsDeleted;
			this.disconnect();
		}
	}




	private void documentServerOnDocumentClosed(GH_DocumentServer sender, GH_Document doc)
	{
		if (this.ghDocument != null && doc.DocumentID == this.ghDocument.DocumentID)
		{
			this.disconnect();
		}
	}




	void onObjectChanged(IGH_DocumentObject sender, GH_ObjectChangedEventArgs e)
	{
		if (this.Locked)
			this.disconnect();
	}




	private void subscribeToEvents()
	{
		if (!this.isSubscribedToEvents)
		{
			this.ghDocument = OnPingDocument();

			if (this.ghDocument != null)
			{
				this.ghDocument.ObjectsDeleted += documentOnObjectsDeleted;
				GH_InstanceServer.DocumentServer.DocumentRemoved += documentServerOnDocumentClosed;
			}

			this.ObjectChanged += this.onObjectChanged;
			this.isSubscribedToEvents = true;
		}
	}



	protected override void SolveInstance(IGH_DataAccess DA)
	{
		this.subscribeToEvents();

		string address = null;
		string initMsg = "Hello World";
		bool reset = false;

		DA.GetData(0, ref address);
		if (!DA.GetData(1, ref initMsg)) return;
		if (!DA.GetData(2, ref reset)) return;

		if(!this.wsAddress.isSameAs(address) || reset)
		{
			this.disconnect();

			this.wsAddress.setAddress(address);

			if (this.wsAddress.isValid())
			{
				this.wscObj = new WsObject().init(address, initMsg);
				this.Message = "Connecting";
				this.wscObj.changed += this.wsObjectOnChange;
			}
			else
			{
				this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid address");
			}
		}

		DA.SetData(0, this.wscObj);
	}




	private void wsObjectOnChange(object sender, EventArgs e)
	{
		this.Message = WsObjectStatus.GetStatusName(this.wscObj.status);
	}




	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.WscStart;}
	}//eof




	public override Guid ComponentGuid
	{
		get { return new Guid("D842FB9F-8FE5-488F-ADBA-6A39FFE2BFB1"); }
	}
}//eoc

}//eons