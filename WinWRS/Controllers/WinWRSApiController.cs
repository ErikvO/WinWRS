using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ErikvO.WinWRS.ExtensionMethods;
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
					.FindAll()
					.ToArray();
			}
		}

		public Computer Add(Computer computer)
		{
			using (var db = new LiteDatabase(_liteDbPath))
			{
				var computers = db.GetCollection<Computer>("Computers");
				computers.Insert(computer);
				computer.Password = "";
			}
			return computer;
		}

		public bool Update(Computer computerToUpdate)
		{
			using (var db = new LiteDatabase(_liteDbPath))
			{
				var computers = db.GetCollection<Computer>("Computers");
				computers.EnsureIndex(computer => computer.Id, true);

				if (computerToUpdate.Password == null || computerToUpdate.Password == "")
					computerToUpdate.EncryptedPassword = computers.FindById(computerToUpdate.Id).EncryptedPassword;

				return computers.Upsert(computerToUpdate);
			}
		}

		public bool Remove(Computer computerToRemove)
		{
			using (var db = new LiteDatabase(_liteDbPath))
			{
				var computers = db.GetCollection<Computer>("Computers");
				computers.EnsureIndex(computer => computer.MAC, true);
				int deleteCount = computers.DeleteMany(wi => wi.MAC == computerToRemove.MAC);
				return deleteCount > 0;
			}
		}

		public void Wake(Computer computer)
		{
			computer.Wake();
		}

		public string Reboot(Computer computerToReboot)
		{
			using (var db = new LiteDatabase(_liteDbPath))
			{
				var computers = db.GetCollection<Computer>("Computers");
				computers.EnsureIndex(computer => computer.Id, true);

				return computers.FindById(computerToReboot.Id).Reboot();
			}
		}

		public string Shutdown(Computer computerToShutdown)
		{
			using (var db = new LiteDatabase(_liteDbPath))
			{
				var computers = db.GetCollection<Computer>("Computers");
				computers.EnsureIndex(computer => computer.Id, true);

				return computers.FindById(computerToShutdown.Id).Shutdown();
			}
		}

		[AcceptVerbs("POST")]
		public Computer FillByName(Computer computer)
		{
			return computer.FillByName();
		}

		[AcceptVerbs("POST")]
		public Computer FillByMac(Computer computer)
		{
			return computer.FillByMac();
		}

		public Computer FillByIp(Computer computer)
		{
			return computer.FillByIp();
		}
	}
}