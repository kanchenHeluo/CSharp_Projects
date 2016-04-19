/// <summary>Script file for SearchBox.</summary>
/// <author>joschemd</author>
/* 
Description: Sets up unified styling on <input type="search" markup and attaches 
	events for search, clear and accessibility. Can be used for auto suggest/ auto complete or 
	standard search boxes.
Dependencies: jQuery, GlobalStyles, GlobalSymbols 
Usage: Include script, Can call SearchBoxObject.Init() to update injected markup after intial page load


HTML Markup attributes:

data-clear-title 'The alt title for the clear


*/
var SearchBoxObject = function (args) {
	/* Call the constructor when created */
	this._ctor(args);
}
SearchBoxObject.Init = function () {
	var instance = new SearchBoxObject();
};
// Most common keys to ignore so that we don't fire off search
SearchBoxObject.IgnoreKeys = {
	9: true,
	13: true,
	16: true,
	17: true,
	18: true,
	19: true,
	20: true,
	27: true,
	33: true,
	34: true,
	35: true,
	36: true,
	37: true,
	38: true,
	39: true,
	40: true,
	45: true,
	91: true,
	92: true,
};
SearchBoxObject.Pattern = "<span tabindex='0' class='symbol remove hidden'></span><span tabindex='0' class='symbol search'></span>";
SearchBoxObject.prototype = {
	//#region Constructor
	_ctor: function (args) {
		this.Bind();
	},
	//#endregion
	//#region Events
	Bind: function (args) {
		var $s = this;
		$(document).ready(function () {
			$("input[type='search']").not('.SearchBox')
				.each(function (i, originalInput) {
					var $inputSearch = $(originalInput).clone(true);
					var resultContainer = $("<span class='SearchBoxContainer'></span>");
					resultContainer.append($inputSearch);
					var container = $(SearchBoxObject.Pattern);
					var clearIco = container.siblings('.remove');
					var searchIco = container.siblings('.search');
					$inputSearch
						.addClass('SearchBox')
						.attr("title", $inputSearch.attr("placeholder"))
						.off("keydown keypress keyup");
					$inputSearch
						.on("keyup.search", { type: 'search', input: $inputSearch[0] }, $s.OnEvent)
						.on("change.search", $s.OnChange);
					searchIco.on("keydown.search keypress.search click.search", { type: 'search', input: $inputSearch[0] }, $s.OnEvent);
					searchIco.attr("title", $inputSearch.attr("placeholder"));
					clearIco.on("keydown.search keypress.search click.search", { type: 'clear', input: $inputSearch[0] }, $s.OnEvent);
					if ($inputSearch.attr('data-clear-title') != null) {
						clearIco.attr("title", $inputSearch.attr('data-clear-title'));
					}
					else {
						clearIco.attr("title", "close " + $inputSearch.attr("placeholder"));
					}
					$inputSearch.after(container);
					$(originalInput).replaceWith(resultContainer);
					
				});
		});
	},
	OnEvent: function (evnt) {
		var data = evnt.data;
		if (!data || !data.type || !data.input) {
			return false;
		}
		var closeButton = $(data.input).next(".symbol.remove");
		if (evnt.type === 'click' ||
			evnt.keyCode === 13 ||
			evnt.keyCode == 32 &&
			typeof (this.searchCallback) === 'function') {
			evnt.preventDefault();
			if (data.type === 'search') {
				evnt.stopImmediatePropagation();
				// apply change event
				if (data.input.value.trim() !== '') {
					$(data.input).trigger("change", data);
				}
			}
			else if (data.type === 'clear') {
				evnt.stopImmediatePropagation();
				data.input.value = '';
				closeButton.addClass('hidden');
				$(data.input).trigger("change", data);
				data.input.focus();
			}
		}
		else if (evnt.type === 'keyup') {
			closeButton.removeClass('hidden');
			var eventValue = data.input.value.trim();
			if (eventValue === '' && !closeButton.hasClass('hidden')) {
				closeButton.addClass('hidden');
				$(data.input).trigger("change", data);
			}

			if (eventValue !== '' && !SearchBoxObject.IgnoreKeys[evnt.keyCode]) {
				var searchType = $(data.input).attr('data-search-type');
				if (searchType != null && searchType.toLowerCase() === "autocomplete") {
					$(data.input).trigger("change", data);
				}
			}
		}
	},
	OnChange: function (evnt) {
		var data = evnt.data || arguments[1];
		if (!data || !data.type || !data.input) {
			evnt.stopImmediatePropagation();
		}
	}

	//#endregion
}

$(document).ready(function () {
	if (!window.ux_searchBox) {
		window.ux_searchBox = new SearchBoxObject();
	}
	else {
		//rebind
		window.ux_searchBox.Bind();
	}
});