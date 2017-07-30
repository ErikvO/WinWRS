"use strict";
module ErikvO {
	export module WinWRS {
		export class SelectOption {
			public Id: number;
			public Value: string;

			constructor(id: number, value: string) {
				this.Id = id;
				this.Value = value;
			}
		}
	}
}