using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Net;
using WiimoteLib;



namespace Bengesht.WiiCat
{
public class WiiStart : WiiComponent
{
    private bool startedBefore;
    private WiimoteCollection wiiCollection;
    private static List<WiiObj> wiiObjList;

        /// <summary>
        /// Initializes a new instance of the AssembleCurves class.
        /// </summary>
        public WiiStart():base
    (
        "Wii Start",
        "Wii*",
        "Connect to available wii controllers..."
    )
	{
        startedBefore = false;
    }//eof


    ~WiiStart()
    {
        if(this.startedBefore)
            foreach (WiiObj o in wiiObjList)
                o.disconnect();
    }
    



    /// <summary>
    /// Help description
    /// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
            "Connect to available wii controllers and read data..."
        );}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
        pManager.AddBooleanParameter("start", "Start", "It turns on/off wii connection", GH_ParamAccess.item,false);
	}//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
        pManager.AddGenericParameter("WiiObjects", "Wii", "connect in to wii in or out components", GH_ParamAccess.list);
        pManager.AddGenericParameter("Messages", "Msg", "messages", GH_ParamAccess.item);
	}//eof




    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
	protected override void SolveInstance(IGH_DataAccess DA)
	{
        string message = "";
        bool start = false;
        if (!DA.GetData(0, ref start)) return;

        if (start)
        {
            if (!startedBefore)
            {
                //start wii connection
                wiiCollection = new WiimoteCollection();
                wiiObjList = new List<WiiObj>();

                try
                {
                    wiiCollection.FindAllWiimotes();
                    startedBefore = true;

                    foreach (Wiimote wiimote in wiiCollection)
                    {
                        wiiObjList.Add(new WiiObj(wiimote));
                    }
                }
                catch (WiimoteNotFoundException ex)
                {
                    System.Console.Write("Wiimote not found error");
                    message = "Error: " + ex.Message;
                }
                catch (WiimoteException ex)
                {
                    System.Console.Write("Wiimote error");
                    message = "Error: " + ex.Message;
                }
                catch (Exception ex)
                {
                    System.Console.Write("Unknown error");
                    message = "Error: " + ex.Message;
                }
            }
        }
        else
        {
            if(startedBefore)
            {
                //disconnect from wii
                startedBefore = false;

                foreach (WiiObj o in wiiObjList)
                    o.disconnect();
            }
        }


        //put latest status in output
        if(startedBefore)
        {
            DA.SetDataList(0, wiiObjList);
            message = "Connected!";
        }

        DA.SetData(1, message);
	}//eof




    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.WiiStart;}
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
        get { return new Guid("997A2029-46BB-47BE-808F-5B2BA349DFB4"); }
    }
}//eoc




class WiiObj
{
    public Wiimote wiimote;
    public WiimoteState state;
    
    public WiiObj(){}//eof

    public WiiObj(Wiimote wiimote)
    {
        this.wiimote = wiimote;
        this.wiimote.WiimoteChanged += this.wiimoteChanged;
        this.wiimote.WiimoteExtensionChanged += this.wiimoteExtensionChanged;
        this.wiimote.Connect();
        this.wiimote.SetReportType(InputReport.IRAccel, true);
    }//eof



    private void wiimoteChanged(object sender, WiimoteChangedEventArgs args)
    {
        this.state = args.WiimoteState;
    }//eof

    private void wiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs args)
    {
        
    }//eof
    

    public void disconnect()
    {
        wiimote.Disconnect();
    }//eof

}//eoc




}//eons