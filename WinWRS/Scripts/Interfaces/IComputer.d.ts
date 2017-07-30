/// <reference path="../Enums.ts"/>
declare module ErikvO {
	export module WinWRS {
		interface IComputer {
			Id?: number;
			Name?: string;
			MAC?: string;
			IP?: string;
			UserName?: string;
			Password?: string;
			ShutdownType?: ShutdownType;
		}
	}
}