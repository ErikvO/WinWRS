"use strict";
/// <reference path="TypeScriptDefinitions/jquery.d.ts"/>
/// <reference path="TypeScriptDefinitions/jsviews.d.ts"/>
module ErikvO {
	export module WinWRS {
		export class ViewModel {
			public Editing: boolean = false;
			public EditedComputer: Computer = new Computer(this, {});
			public Computers: Array<Computer> = new Array<Computer>();

			constructor() {
				this.Refresh();
			}

			public Refresh = (): void => {
				Ajax.Send({
					method: "GET",
					url: "./api/WinWRSApi",
					success: (data: Array<IComputer>) => {
						$.observable(this.Computers).refresh(data.map((iComputer) => new Computer(this, iComputer)));
						$.observable(this).setProperty("Editing", false);
					}
				});
			}

			public AddNew = (): void => {
				$.observable(this).setProperty("EditedComputer", new Computer(this, {}));
				$.observable(this).setProperty("Editing", true);
			}

			public Edit(computer: Computer): void {
				$.observable(this).setProperty("EditedComputer", computer);
				$.observable(this).setProperty("Editing", true);
			}

			public StopEdit = (): void => {
				$.observable(this).setProperty("Editing", false);
			}
			
			public Add(computer: Computer): void {
				$.observable(this.Computers).insert(computer);
			}

			public Remove(computer: Computer): void {
				$.observable(this.Computers).remove(this.Computers.indexOf(computer));
			}
		}
	}
}