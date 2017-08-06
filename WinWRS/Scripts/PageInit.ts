"use strict";
$.holdReady(true);
document.addEventListener("DOMContentLoaded", loadTemplates, false);

function loadTemplates() {
	(<any>window).JsViewsTotalScripts = $(".templateScript").length;
	(<any>window).JsViewsScriptsLoaded = 0;
	$(".templateScript").each(function () {
		if (!this.hasAttribute("data-src"))
			throw new Error("This templateScript is missing a data-src attribute: " + this.outerHTML);
		let element = $(this);
		let url = element.attr("data-src");
		$.ajax({
			url: url,
			dataType: "html",
			type: "GET",
			timeout: 5000,
			success: function (response) {
				$(response).insertAfter(element);
				$(element).remove();
				(<any>window).JsViewsScriptsLoaded++;
			}
		});
	});
	let intervalId = setInterval(function () {
		if ((<any>window).JsViewsTotalScripts == (<any>window).JsViewsScriptsLoaded) {
			clearInterval(intervalId);
			$.holdReady(false);
			delete (<any>window).JsViewsTotalScripts;
			delete (<any>window).JsViewsScriptsLoaded;
		}
	}, 1);
}

$(() => {
	(<any>window).ViewModelInstance = new ErikvO.WinWRS.ViewModel();
	let template = $.templates({
		helpers: { WinWRS: ErikvO.WinWRS },
		converters: {
			StringToInt: function (stringValue: string): number {
				if (stringValue === null || typeof (stringValue) === "undefined") {
					$(this.linkCtx.elem).val("0"); //Put 0 into the textbox.
					return 0;
				}

				if (!$.isNumeric(stringValue)) {
					let boundProperty: string = this.tagCtx.params.args[0];
					let originalValue: number = this.linkCtx.data[boundProperty];
					$(this.linkCtx.elem).val(originalValue.toString()); //Put the original value back into the textbox.
					return originalValue;
				}

				let newValue: number = parseInt(stringValue);
				$(this.linkCtx.elem).val(newValue.toString());
				return newValue;
			},
		},
		markup: "#PageTemplate"
	});
	template.link("#WinWRS", (<any>window).ViewModelInstance);
});	