"use strict";
/// <reference path="TypeScriptDefinitions/jquery.d.ts"/>
/// <reference path="TypeScriptDefinitions/jsviews.d.ts"/>
module ErikvO {
	export module WinWRS {
		export class ViewModel {
			public Computers: Array<Computer> = new Array<Computer>();

			public NewName: string = "";
			public NewMAC: string = "";
			public NewIP: string = "";
			public NewUserName: string = "";
			public NewPassword: string = "";

			constructor() {
				Ajax.Send({
					method: "GET",
					url: "./api/WinWRSApi",
					success: (data: Array<IComputer>) => {
						data.forEach((computer) => {
							$.observable(this.Computers).insert(new Computer(this, computer));
						});
					}
				});
			}

			public Add = () => {
				if (this.NewName != "" && this.NewMAC != "" && this.NewIP != "" && this.NewUserName != "" && this.NewPassword != "") {
					let computer = new Computer(this, { Name: this.NewName, MAC: this.NewMAC, IP: this.NewIP, UserName: this.NewUserName, Password: this.NewPassword })

					Ajax.Send({
						url: "./api/WinWRSApi/Add",
						data: computer,
						success: (data: IComputer) => {
							$.observable(this.Computers).insert(new Computer(this, data));
							$.observable(this).setProperty("NewName", "");
							$.observable(this).setProperty("NewMAC", "");
							$.observable(this).setProperty("NewIP", "");
							$.observable(this).setProperty("NewUserName", "");
							$.observable(this).setProperty("NewPassword", "");
						}
					});
				}
			}

			public Remove(computer: Computer) {
				$.observable(this.Computers).remove(this.Computers.indexOf(computer));
			}

			public FillByName() {
				let computer = new Computer(this, { Name: this.NewName })

				Ajax.Send({
					url: "./api/WinWRSApi/FillByName",
					data: computer,
					success: (data: IComputer) => {
						$.observable(this).setProperty("NewMAC", data.MAC);
						$.observable(this).setProperty("NewIP", data.IP);
					}
				});
			}

			public FillByMac() {
				let computer = new Computer(this, { MAC: this.NewMAC })

				Ajax.Send({
					url: "./api/WinWRSApi/FillByMac",
					data: computer,
					success: (data: IComputer) => {
						$.observable(this).setProperty("NewName", data.Name);
						$.observable(this).setProperty("NewIP", data.IP);
					}
				});
			}

			public FillByIp() {
				let computer = new Computer(this, { IP: this.NewIP })

				Ajax.Send({
					url: "./api/WinWRSApi/FillByIp",
					data: computer,
					success: (data: IComputer) => {
						$.observable(this).setProperty("NewName", data.Name);
						$.observable(this).setProperty("NewMAC", data.MAC);
					}
				});
			}
		}
	}
}