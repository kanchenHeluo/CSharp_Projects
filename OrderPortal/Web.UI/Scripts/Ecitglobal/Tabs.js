/// <summary>Script file for SideNav.</summary>
/// <author>joschemd</author>
// Dependencies: jQuery 1.9+

// Notes:
// Only one instance is needed to run all tab controls on a page,
// Following the code structure template and css, a developer can create
// a tab control from the server side or dynamically on the client side
// The dev will be responsible to ensure that the tab header events propagate up 
//	to the body 
//
// This exposes a tabchange event that originates from the tab container
// A developer can subscribe to the event to listen for changes
// example: $("#myTab.UxTab").on("tabchange", function(jQueryEventObject, tabId){ });

var UxTabObject = function (args) {
	/* Call the constructor when created */
	this._ctor(args);
}

UxTabObject.prototype = {
	Version: function () { return "1.5.0"; },
	//#region Constructor
	_ctor: function (args) {
		// this will be called when the object is created
		var $s = this;
		if (args) {
			//copy properties from args
			for (var property in args) {
				if (property[0] !== '_' && this.hasOwnProperty(property)) { this[property] = args[property]; }
			}
		}

		$("body").ready(function () {
			// bind when the body is ready
			$s.Bind();
		});
	},
	//#endregion
	//#region Events
	Bind: function () {
		var $s = this;
		/* Use this to initially bind events to elements that exist on load, ex onclick etc */
		/* 
			we place a single event delegate on the page to handle all tab controls 
			this allows us to add tab controls at any time and keep a single instance of logic to run.
			We also only want to look for events from items with the .tabItem class, these are the headers for the control
		*/
		var body = $("body");
		if (body.data("UxTabInitialized") == null) {
			body.data("UxTabInitialized", true);
			body.on("click.UxTab keydown.UxTab keypress.UxTab touchstart.UxTab", ".tabItem", function () { $s.Action.apply($s, arguments); });
		}
	},
	Action: function (evnt) {
		var keys =
			{
				8: true,	// backspace 
				13: true,	// enter
				32: true	// spacebar
			};
		if (evnt == null || (evnt.type !== 'click' &&
			evnt.type !== 'touchstart' &&
			!keys[evnt.which])) {
			return;
		}
		var $s = this;

		var target = $(evnt.target);
		if (!target.hasClass('active')) {
			var parentContainer = target.parents('.UxTab');
			if (parentContainer.length == 0) {
				return;
			}
			evnt.stopPropagation();
			evnt.preventDefault();
			var newTargetId = target.attr('data-id');
			if (newTargetId != null && newTargetId.length > 0) {
				$s.SetActive(newTargetId, parentContainer);
			}
		}
	},
	SetActive: function (dataId, uxTabContainer) {
		// dataId can either be id of the tab or numeric index base 0
		// uxTabContainer is the root node of the tab container (the element with the class .UxTab)
		var $s = this;
		if (uxTabContainer == null || (uxTabContainer.length != null && uxTabContainer.length === 0)) { return; }
		//ensure jquery object
		uxTabContainer = $(uxTabContainer);
		$s.ClearActive(uxTabContainer);

		if (!isNaN(dataId)) {
			var id = $(uxTabContainer.find('>.header>.tabItem')[dataId]).attr('data-id');
			// Trigger tab change event with arguments the tab id, and the corresponding container
			uxTabContainer.trigger('tabchange', [id]);
			$(uxTabContainer.find('>.header>.tabItem')[dataId]).addClass('active');
			$(uxTabContainer.find('>.tabContainer')[dataId]).addClass('active');
		}
		else {
			// Trigger tab change event with arguments the tab id, and the corresponding container
			uxTabContainer.trigger('tabchange', [dataId]);
			uxTabContainer.find('>.header>.tabItem[data-id="' + dataId + '"], >.tabContainer[data-id="' + dataId + '"]').addClass('active');
		}

	},
	ClearActive: function (uxTabContainer) {
		$(uxTabContainer)
			.find('>.header>.tabItem.active, >.tabContainer.active').removeClass('active');
	}

	//#endregion
}

/* Create a manager */
var UxTab = new UxTabObject();