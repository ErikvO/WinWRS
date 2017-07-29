using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using ErikvO.WinWRS.Business;
using ErikvO.WinWRS.Models;
using LiteDB;

namespace ErikvO.WinWRS.Controllers
{
	public class WinWRSApiController : ApiController
	{
		private string _liteDbPath => HttpContext.Current.Server.MapPath("~/App_Data/ErikvO.WinWRS.db");

		public IEnumerable<Computer> Get()
		{
			using (var db = new LiteDatabase(_liteDbPath))
			{
				return db
					.GetCollection<Computer>("Computers")
					.FindAll();
			}
		}

		public Computer Add(Computer computer)
		{
			using (var db = new LiteDatabase(_liteDbPath))
			{
				var computers = db.GetCollection<Computer>("Computers");
				computers.Insert(computer);
			}
			return computer;
		}

		public bool Update(Computer computerToUpdate)
		{
			using (var db = new LiteDatabase(_liteDbPath))
			{
				var computers = db.GetCollection<Computer>("Computers");
				computers.EnsureIndex(computer => computer.Id, true);
				return computers.Upsert(computerToUpdate);
			}
		}

		public bool Remove(Computer computerToRemove)
		{
			using (var db = new LiteDatabase(_liteDbPath))
			{
				var computers = db.GetCollection<Computer>("Computers");
				computers.EnsureIndex(computer => computer.MAC, true);
				int deleteCount = computers.Delete(wi => wi.MAC == computerToRemove.MAC);
				return deleteCount > 0;
			}
		}

		public void Wake(Computer computer)
		{
			new ComputerActions(computer).Wake();
		}

		public int Reboot(Computer computer)
		{
			return new ComputerActions(computer).Reboot();
		}

		public int Shutdown(Computer computer)
		{
			return new ComputerActions(computer).Shutdown();
		}

		[AcceptVerbs("POST")]
		public Computer FillByName(Computer computer)
		{
			return new ComputerActions(computer).FillByName();
		}

		[AcceptVerbs("POST")]
		public Computer FillByMac(Computer computer)
		{
			return new ComputerActions(computer).FillByMac();
		}

		public Computer FillByIp(Computer computer)
		{
			return new ComputerActions(computer).FillByIp();
		}
	}
}