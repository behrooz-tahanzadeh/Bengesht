using Grasshopper.Kernel;
using Rhino.Geometry;
using System;




namespace Bengesht.CurveCat
{
public class AlignCrvSE : CurveComponent
{
	/// <summary>
    /// Initializes a new instance of the AlignCrvSE class.
	/// </summary>
	public AlignCrvSE() : base
    (
        "Align Curve Start/End",
        "AlignCrvS/E",
        "Align curve on a line based on its start/end points."
    )
	{}//eof




	/// <summary>
    /// Help description
	/// </summary>
	protected override string HelpDescription
	{
		get{return BengeshtInfo.getComponentDescription(
            "<img src=\"http://b-tz.com/wp-content/uploads/2014/08/blink-animated-gif.gif\">" +
			"Align Curve on lines based on its start/end points."
		);}
	}//eof




	/// <summary>
    /// Registers all the input parameters for this component.
	/// </summary>
	protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
	{
		pManager.AddCurveParameter("Curve", "Crv", "curve to be aligned", GH_ParamAccess.item);
		pManager.AddLineParameter("Line", "Ln", "destination lines", GH_ParamAccess.item);
		pManager.AddBooleanParameter("Inverse", "Inv", "invert curves", GH_ParamAccess.item,false);
		pManager.AddNumberParameter("Height", "H", "height", GH_ParamAccess.item, 10);
	}//eof




	/// <summary>
    /// Registers all the output parameters for this component.
	/// </summary>
	protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
	{
		pManager.AddCurveParameter("Aligned Curve", "Crv", "aligned curve", GH_ParamAccess.item);
	}//eof




	/// <summary>
    /// This is the method that actually does the work.
	/// </summary>
	protected override void SolveInstance(IGH_DataAccess DA)
	{
        //<init>
        AlignCrvSECore core = new AlignCrvSECore();

		if (!DA.GetData(0, ref core.curve)) return;
        if (!DA.GetData(1, ref core.line)) return;
		if (!DA.GetData(2, ref core.inverse)) return;
		if (!DA.GetData(3, ref core.height)) return;
        //</init>

        core.run();
        
        DA.SetData(0, core.curve);
	}//eof




	/// <summary>
    /// Provides an Icon for the component.
	/// </summary>
	protected override System.Drawing.Bitmap Icon
	{
        get { return Resource.curveAlignCrvSE; }
	}//eof




	/// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
	/// </summary>
	public override Guid ComponentGuid
	{
		get { return new Guid("{4c0d75e1-4266-45b8-b5b4-826c9ad51acd}"); }
	}//eof




}//eoc



class AlignCrvSECore
{
    public Curve curve = null;
    public Line line = new Line();
    public Boolean inverse = false;
    public Double height = 10;



    public void run()
    {
        rotateCurve().scaleCurve().translateCurve();
    }//eof




    private AlignCrvSECore rotateCurve()
    {
        if (inverse)
            curve.Reverse();

        Point3d curveStart = curve.PointAtStart;
        Point3d curveEnd = curve.PointAtEnd;

        Vector3d curveVector = new Vector3d(curveEnd.X - curveStart.X, curveEnd.Y - curveStart.Y, curveEnd.Z - curveStart.Z);
        Vector3d lineVector = new Vector3d(line.ToX - line.FromX, line.ToY - line.FromY, line.ToZ - line.FromZ);

        double vectorsXYAngle = Vector3d.VectorAngle(curveVector, lineVector, Plane.WorldXY);
        curve.Rotate(vectorsXYAngle, Vector3d.ZAxis, curveStart);

        return this;
    }//eof




    private AlignCrvSECore scaleCurve()
    {
        Point3d curveStart = curve.PointAtStart;
        Point3d curveEnd = curve.PointAtEnd;

        Point3d lineStart = line.From;
        Point3d lineEnd = line.To;


        Transform t = new Transform(1);

        if (Math.Abs(curveEnd.X - curveStart.X) > Rhino.RhinoMath.ZeroTolerance)
            t.M00 = (lineEnd.X - lineStart.X) / (curveEnd.X - curveStart.X);

        if (Math.Abs(curveEnd.Y - curveStart.Y) > Rhino.RhinoMath.ZeroTolerance)
            t.M11 = (lineEnd.Y - lineStart.Y) / (curveEnd.Y - curveStart.Y);

        t.M22 = Math.Abs(height / (curveEnd.Z - curveStart.Z));

        curve.Transform(t);

        return this;
    }//eof




    private AlignCrvSECore translateCurve()
    {
        Point3d curveStart = curve.PointAtStart;
        Point3d lineStart = line.From;

        curve.Translate(lineStart.X - curveStart.X, lineStart.Y - curveStart.Y, lineStart.Z - curveStart.Z);

        return this;
    }//eof
}//eoc
}//eons