using Grasshopper.Kernel;
using System.Reflection;

namespace Bengesht
{
public class BengeshtInfo : GH_AssemblyInfo
{




	public static string getComponentDescription(string str)
	{
		string output = str +
			"<span style='color:#7c7;'>■</span>" +
			"<br><br><br><hr>"+
			"<a href='http://b-tz.com' target='_blank'>"+
			"<i>Developer Contact</i>"+
			"</a>"+
			"<br/>" +
            "<a href='http://app.b-tz.com/bengesht/rpc.php?f=check-for-update&v=3.2.0.0' >" +
			"<i>check for update</i>" +
			"</a>";

		return output;
	}//eof




}//eoc
}//eons