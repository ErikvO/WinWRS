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

			constructor(parentViewModel: ViewModel, data: IComputer) {
				this._parentViewModel = parentViewModel;
				this.Id = data.Id || 0;
				this.Name = data.Name || "";
				this.MAC = data.MAC || "";
				this.IP = data.IP || "";
				this.UserName = data.UserName || "";
				this.Password = data.Password || "";
			}

			public Update = () => {
				$.observable(this).setProperty("_actionExecuting", true);
				Ajax.Send({
					url: "./api/WinWRSApi/Update",
					data: this,
					complete: () => {
						$.observable(this).setProperty("_actionExecuting", false);
					}
				});
			}

			public Remove = () => {
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

			public Wake = () => {
				$.observable(this).setProperty("_actionExecuting", true);
				Ajax.Send({
					url: "./api/WinWRSApi/Wake",
					data: this,
					complete: () => {
						$.observable(this).setProperty("_actionExecuting", false);
					}
				});
			}

			public Reboot = () => {
				$.observable(this).setProperty("_actionExecuting", true);
				Ajax.Send({
					url: "./api/WinWRSApi/Reboot",
					data: this,
					success: (errorCode: number) => {
						if (errorCode != 0)
							alert("Reboot failed, error code: " + errorCode.toString());
					},
					complete: () => {
						$.observable(this).setProperty("_actionExecuting", false);
					}
				});
			}

			public Shutdown = () => {
				$.observable(this).setProperty("_actionExecuting", true);
				Ajax.Send({
					url: "./api/WinWRSApi/Shutdown",
					data: this,
					success: (errorCode: number) => {
						if (errorCode != 0)
							alert("Shutdown failed, error code: " + errorCode.toString());
					},
					complete: () => {
						$.observable(this).setProperty("_actionExecuting", false);
					}
				});
			}
		}
	}
}