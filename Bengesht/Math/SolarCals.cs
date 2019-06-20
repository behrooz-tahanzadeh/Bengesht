using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Bengesht.MathCat
{
public class SolarCals : MathComponent
{
    /// <summary>
    /// Initializes a new instance of the AlignCrvSE class.
    /// </summary>
	public SolarCals() : base
    (
        "Solar Calculations",
        "SolarCals",
        "All of the calculations to get sun properties at specific location, in a specific time."
    )
	{}//eof




    /// <summary>
    /// Help description
    /// </summary>
    protected override string HelpDescription
    {
        get { return BengeshtInfo.getComponentDescription(
            "All of the calculations to get sun properties at specific location, in a specific time."
        );}
    }//eof




    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
		pManager.AddNumberParameter("Latitude", "L", "Latitde of place", GH_ParamAccess.item, 32.24);
		pManager.AddIntegerParameter("Month", "M", "Month between 1 to 12", GH_ParamAccess.item, 3);
		pManager.AddIntegerParameter("Day", "D", "Day number between 1 to 31", GH_ParamAccess.item, 26);
		pManager.AddNumberParameter("Hour", "H", "Hour", GH_ParamAccess.item, 7);
        pManager.AddVectorParameter("North Direction", "N", "North direction", GH_ParamAccess.item, Vector3d.YAxis);
	}//eof




    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
		pManager.AddTextParameter("Month", "MN", "month name", GH_ParamAccess.item);
		pManager.AddIntegerParameter("NYD", "NYD", "number of day of the year", GH_ParamAccess.item);
		pManager.AddNumberParameter("DEC", "DEC", "solar declination", GH_ParamAccess.item);
		pManager.AddNumberParameter("HRA", "HRA", "hour-angle", GH_ParamAccess.item);
		pManager.AddNumberParameter("ALT", "ALT", "solar altitude angle", GH_ParamAccess.item);
		pManager.AddNumberParameter("AZI", "AZI", "solar azimuth angle", GH_ParamAccess.item);
		pManager.AddNumberParameter("SRH", "SRH", "sunrise hour-anglee", GH_ParamAccess.item);
		pManager.AddNumberParameter("SRT", "SRT", "sunrise time", GH_ParamAccess.item);
        pManager.AddVectorParameter("Sun Vector", "SUN", "Sun Vector", GH_ParamAccess.item);
	}//eof




    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
	protected override void SolveInstance(IGH_DataAccess DA)
	{
        //<init>
        SolarCalsCore core = new SolarCalsCore();

		if (!DA.GetData(0, ref core.L)) return;
        if (!DA.GetData(1, ref core.M)) return;
        if (!DA.GetData(2, ref core.D)) return;
        if (!DA.GetData(3, ref core.H)) return;
        if (!DA.GetData(4, ref core.N)) return;
        //</init>

		DA.SetData(0, core.MN);
        DA.SetData(1, core.NYD);
        DA.SetData(2, core.DEC);
        DA.SetData(3, core.HRA);
        DA.SetData(4, core.ALT);
        DA.SetData(5, core.AZI);
        DA.SetData(6, core.SRH);
        DA.SetData(7, core.SRT);
        DA.SetData(8, core.SUN);
	}//eof



    
    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
	protected override System.Drawing.Bitmap Icon
	{
		get{return Resource.mathSolarCals;}
	}//eof




    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
	public override Guid ComponentGuid
	{
		get { return new Guid("{8c9eeb9d-5cf8-4e99-b004-9186588edbf3}"); }
    }//eof




}//eoc




class SolarCalsCore
{
    public double L = 32.24;
    public int M = 3;
    public int D = 26;
    public double H = 7;
    public Vector3d N = Vector3d.YAxis;




    public string MN
    {
        get
        {
            string[] months = { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            return months[M - 1];
        }
    }//eof




    public int NYD
    {
        get
        {
            int[] dayOfMonths = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            return dayOfMonths[M - 1] + D;
        }
    }//eof




    public double DEC
    {
        get
        {
            return 23.45 * Math.Sin((0.986 * (284 + NYD)) * (Math.PI / 180));
        }
    }//eof




    public double HRA
    {
        get
        {
            return (15 * (H - 12));
        }
    }//eof




    //ALT = arcsin(sinDEC * sinLAT + cosDEC * cosLAT * cos HRA)
    public double ALT
    {
        get
        {
            double sinDEC = Math.Sin((Math.PI / 180) * DEC);
            double cosDEC = Math.Cos((Math.PI / 180) * DEC);
            double sinLAT = Math.Sin((Math.PI / 180) * L);
            double cosLAT = Math.Cos((Math.PI / 180) * L);
            double cosHRA = Math.Cos((Math.PI / 180) * HRA);

            return (180 / Math.PI) * (Math.Asin(sinDEC * sinLAT + cosDEC * cosLAT * cosHRA));
        }
    }//eof




    //AZI = arccos[(cosLAT*sinDEC-cosDEC*sinLAT*cosHRA)/cosALT]
    public double AZI
    {
        get
        {
            double sinDEC = Math.Sin((Math.PI / 180) * DEC);
            double cosDEC = Math.Cos((Math.PI / 180) * DEC);
            double sinLAT = Math.Sin((Math.PI / 180) * L);
            double cosLAT = Math.Cos((Math.PI / 180) * L);
            double cosHRA = Math.Cos((Math.PI / 180) * HRA);
            double cosALT = Math.Cos((Math.PI / 180) * ALT);

            double result = (180 / Math.PI) * (Math.Acos((cosLAT * sinDEC - cosDEC * sinLAT * cosHRA) / cosALT));

            return Double.IsNaN(result) ? 180 : result;
        }
    }//eof




    //SRH = arcos(-tanDEC * tanLAT)
    public double SRH
    {
        get
        {
            double tanDEC = Math.Tan((Math.PI / 180) * DEC);
            double tanLAT = Math.Tan((Math.PI / 180) * L);
            return (180 / Math.PI) * (Math.Acos(-1 * tanDEC * tanLAT));
        }
    }//eof




    //SRT = 12-[SRH/15]
    public double SRT
    {
        get
        {
            return 12 - (SRH / 15);
        }
    }//eof




    public Vector3d SUN
    {
        get
        {
            Vector3d v = new Vector3d(N);

            double az = (H > 12 ? 1 : -1) * AZI * (Math.PI / 180);

            v.Rotate(az, Vector3d.ZAxis);

            Vector3d av = new Vector3d(N);
            av.Rotate(Math.PI / 2, Vector3d.ZAxis);

            double al = ALT * (Math.PI / 180);

            v.Rotate(al, av);

            return v;
        }
    }




}//eoc

}//eons