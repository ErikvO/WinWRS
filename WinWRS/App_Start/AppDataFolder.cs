using System.IO;
using System.Web;
using System.Web.Http;

namespace ErikvO.WinWRS
{
	public static class AppDataFolder
	{
		public static void CreateIfNeeded(HttpConfiguration config)
		{
			var appDataFolder = HttpContext.Current.Server.MapPath("~/App_Data/");
			Directory.CreateDirectory(appDataFolder);
		}
	}
}