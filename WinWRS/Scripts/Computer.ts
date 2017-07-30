"use strict";
module ErikvO {
	export module WinWRS {
		export class Computer {
			private _parentViewModel: ViewModel;

			public _actionExecuting: boolean = false;

			public Id: number;
			public Name: string;
			public MAC: string;
			public IP: string;
			public UserName: string;
			public Password: string;
			public ShutdownType: ShutdownType;

			constructor(parentViewModel: ViewModel, data: IComputer) {
				this._parentViewModel = parentViewModel;
				this.Id = data.Id || 0;
				this.Name = data.Name || "";
				this.MAC = data.MAC || "";
				this.IP = data.IP || "";
				this.UserName = data.UserName || "";
				this.Password = data.Password || "";
				this.ShutdownType = data.ShutdownType || ShutdownType.WindowsSMB;
			}

			private get _canSave(): boolean {
				return this.Name != "" &&
					this.MAC != "" &&
					this.IP != "" &&
					this.UserName != "" &&
					(this.Id > 0 || this.Password != "");
			}

			private Clear(): void {
				$.observable(this).setProperty("Id", 0);
				$.observable(this).setProperty("Name", "");
				$.observable(this).setProperty("MAC", "");
				$.observable(this).setProperty("IP", "");
				$.observable(this).setProperty("UserName", "");
				$.observable(this).setProperty("Password", "");
				$.observable(this).setProperty("ShutdownType", ShutdownType.WindowsSMB);
			}

			public Insert = (onInserted: () => void): void => {
				if (this._canSave) {
					Ajax.Send({
						url: "./api/WinWRSApi/Add",
						data: this,
						success: (data: IComputer) => {
							this._parentViewModel.Add(new Computer(this._parentViewModel, data));
							this.Clear();
							onInserted();
						}
					});
				}
			}

			public Edit = (): void => {
				this._parentViewModel.Edit(this);
			}

			public Update = (onUpdated: () => void): void => {
				$.observable(this).setProperty("_actionExecuting", true);
				Ajax.Send({
					url: "./api/WinWRSApi/Update",
					data: this,
					success: () => {
						$.observable(this).setProperty("Password", "");
						onUpdated();
					},
					complete: () => {
						$.observable(this).setProperty("_actionExecuting", false);
					}
				});
			}

			public Remove = (): void => {
				$.observable(this).setProperty("_actionExecuting", true);
				Ajax.Send({
					url: "./api/WinWRSApi/Remove",
					data: this,
					success: (deleted: boolean) => {
						if (deleted)
							this._parentViewModel.Remove(this);
					},
					complete: () => {
						$.observable(this).setProperty("_actionExecuting", false);
					}
				});
			}

			public Wake = (): void => {
				$.observable(this).setProperty("_actionExecuting", true);
				Ajax.Send({
					url: "./api/WinWRSApi/Wake",
					data: this,
					complete: () => {
						$.observable(this).setProperty("_actionExecuting", false);
					}
				});
			}

			public Reboot = (): void => {
				$.observable(this).setProperty("_actionExecuting", true);
				Ajax.Send({
					url: "./api/WinWRSApi/Reboot",
					data: this,
					success: (error: string) => {
						if (error)
							alert("Reboot failed, error code: " + error);
					},
					complete: () => {
						$.observable(this).setProperty("_actionExecuting", false);
					}
				});
			}

			public Shutdown = (): void => {
				$.observable(this).setProperty("_actionExecuting", true);
				Ajax.Send({
					url: "./api/WinWRSApi/Shutdown",
					data: this,
					success: (error: string) => {
						if (error)
							alert("Shutdown failed, error code: " + error);
					},
					complete: () => {
						$.observable(this).setProperty("_actionExecuting", false);
					}
				});
			}

			public FillByName = (): void => {
				let computer = new Computer(this._parentViewModel, { Name: this.Name })

				Ajax.Send({
					url: "./api/WinWRSApi/FillByName",
					data: computer,
					success: (data: IComputer) => {
						$.observable(this).setProperty("MAC", data.MAC);
						$.observable(this).setProperty("IP", data.IP);
					}
				});
			}

			public FillByMac = (): void => {
				let computer = new Computer(this._parentViewModel, { MAC: this.MAC })

				Ajax.Send({
					url: "./api/WinWRSApi/FillByMac",
					data: computer,
					success: (data: IComputer) => {
						$.observable(this).setProperty("Name", data.Name);
						$.observable(this).setProperty("IP", data.IP);
					}
				});
			}

			public FillByIp = (): void => {
				let computer = new Computer(this._parentViewModel, { IP: this.IP })

				Ajax.Send({
					url: "./api/WinWRSApi/FillByIp",
					data: computer,
					success: (data: IComputer) => {
						$.observable(this).setProperty("Name", data.Name);
						$.observable(this).setProperty("MAC", data.MAC);
					}
				});
			}

			public get _shutdownTypeOptions(): Array<SelectOption> {
				let result = Object.keys(ErikvO.WinWRS.ShutdownType)
					.filter(key => $.isNumeric(key))
					.map(key => new SelectOption(parseInt(key), ErikvO.WinWRS.ShutdownType[key]));
				return result;
			}
		}
	}
}