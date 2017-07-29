$(document).ajaxError(function (event: JQueryEventObject, xMLHttpRequest: JQueryXHR, ajaxOptions: JQueryAjaxSettings, exception: any) {
	alert("An error occured\r\n" + xMLHttpRequest.responseText);
	console.log(xMLHttpRequest);
});

module ErikvO {
	export module WinWRS {
		export module Ajax {
			export function Send(parameters: JQueryAjaxSettings): void {
				if (!parameters.hasOwnProperty("type"))
					parameters.type = "POST";
				if (!parameters.hasOwnProperty("contentType"))
					parameters.contentType = "application/json; charset=utf-8";
				if (!parameters.hasOwnProperty("dataType"))
					parameters.dataType = "json";
				if (parameters.hasOwnProperty("data") && parameters.data === null)
					delete parameters.data;

				parameters.global = parameters.hasOwnProperty("global") && parameters.global === false ? parameters.global : true;

				if (typeof (parameters.data) === "object" && (Object.getOwnPropertyNames(parameters.data).length === 0))
					delete parameters.data;
				else
					parameters.data = JSON.stringify(parameters.data, ClientSidePropertiesFilter);

				$.ajax(parameters);
			}

			function ClientSidePropertiesFilter(key: string, value: any): any {
				if (typeof (key) === "string" && (key.slice(0, 1) === "_"))
					return undefined;

				if (value != null && typeof (value) === "object" && !Array.isArray(value)) {
					let result = {};
					for (let prop in value) {
						if ((typeof (prop) !== "string" || prop.slice(0, 1) !== "_") && typeof value[prop] !== 'function') {
							result[prop] = value[prop];
						}
					}
					return result;
				}

				return value;
			}
		}
	}
}