using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using WiimoteLib;
using Rhino.Geometry;



namespace Bengesht.WiiCat
{
public class WiiIn : WiiComponent
{

    /// <summary>
    /// Initializes a new instance of the AssembleCurves class.
    /// </summary>
    public WiiIn() : base
    (
        "WiiIn",
        "Wii>>",
        "Read data from wii controller"
    )
    {}//eof
    



    /// <summary>
    /// Help description
    /// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
            "Read data from available wii controllers.<br>Use a timer component to refresh the value."
        );}
	}//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
        pManager.AddGenericParameter("WiiObjects", "Wii", "Wii objects (it should be connected to a WiiStart component)", GH_ParamAccess.item);
    }//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
        pManager.AddPointParameter("IRSensor", "IRS", "List of detected ir sensor data. Possible values:\nNull: if could not find the ir light\nPoint3d: x∈[0,1023] y∈[0,767] z=0", GH_ParamAccess.list);
        pManager.AddVectorParameter("Accel", "Acl", "Acceleration vector", GH_ParamAccess.item);
        pManager.AddBooleanParameter("Buttons", "Btn", "Buttons state in following order:\n0: A\n1: B\n2: Minus\n3: Home\n4: Plus\n5: One\n6: Two\n7: Up\n8: Down\n9: Left\n10: Right", GH_ParamAccess.list);
    }//eof




    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    protected override void SolveInstance(IGH_DataAccess DA)
	{
        WiiObj wiiObj = new WiiObj();
        if(!DA.GetData(0, ref wiiObj)) return;

        List<object> irsernsorP2 = new List<object>();
        List<bool> buttons = new List<bool>();

        for (int i = 0; i < wiiObj.state.IRState.IRSensors.Length; ++i)
        {
            if (wiiObj.state.IRState.IRSensors[i].Found)
            {
                float x = wiiObj.state.IRState.IRSensors[i].RawPosition.X;
                float y = wiiObj.state.IRState.IRSensors[i].RawPosition.Y;

                irsernsorP2.Add(new Point2d(x,y));
            }
            else
            {
                irsernsorP2.Add(null);
            }
        }

        Vector3d accelVt = new Vector3d(wiiObj.state.AccelState.Values.X, wiiObj.state.AccelState.Values.Y, wiiObj.state.AccelState.Values.Z);

        buttons.Add(wiiObj.state.ButtonState.A);
        buttons.Add(wiiObj.state.ButtonState.B);
        buttons.Add(wiiObj.state.ButtonState.Minus);
        buttons.Add(wiiObj.state.ButtonState.Home);
        buttons.Add(wiiObj.state.ButtonState.Plus);
        buttons.Add(wiiObj.state.ButtonState.One);
        buttons.Add(wiiObj.state.ButtonState.Two);
        buttons.Add(wiiObj.state.ButtonState.Up);
        buttons.Add(wiiObj.state.ButtonState.Down);
        buttons.Add(wiiObj.state.ButtonState.Left);
        buttons.Add(wiiObj.state.ButtonState.Right);

        DA.SetDataList(0, irsernsorP2);
        DA.SetData(1, accelVt);
        DA.SetDataList(2, buttons);
    }//eof




    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.WiiIn;}
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
        get { return new Guid("C5EA94B2-1F70-4B84-B9C0-D56E85E37B38"); }
    }
}//eoc
}//eons