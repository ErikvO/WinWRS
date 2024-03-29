﻿using System.Web.Http;

namespace ErikvO.WinWRS
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(AppDataFolder.CreateIfNeeded);
			GlobalConfiguration.Configure(WebApiConfig.Register);
			GlobalConfiguration.Configuration.EnsureInitialized();
		}
	}
}
