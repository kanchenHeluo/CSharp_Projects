/// <summary>Script file for ModalDialog.</summary>
/// <author>joschemd</author>


// Dependency on GlobalSymbols

var ModalDialogObject = function (args) {
    //#region Instance Properties
    var container = $(ModalDialogObject.ContainerTemplate);
	this.open = false;
	this.source = null;
	this.container = args ? args.container || container : container;
	this.title = null;
	this.keepOpen = false;
	this.onShow = null;
	this.onClose = null;
	this.dialogType = "default"; // choices are default / warning

	//#endregion

	/* Call the constructor when created */
	if (args) { this._ctor(args); }
}

//#region Static/Base object Functions
$.extend(ModalDialogObject, new function () {
	this.ContainerTemplate =
		"<div class='ModalDialog'><header><h1></h1><span class='symbol remove' tabindex='0' data-item='dialogClose'>" +
		"</span></header><section class='content'></section></div>";
	this.Create = function (args) {
		return new ModalDialogObject(args);
	};
});
//#endregion

ModalDialogObject.prototype = {
	//#region Constructor
	_ctor: function (args) {
		// this will be called when the object is created
		if (args) {
			//copy properties from args
			for (var property in args) {
				if (property[0] != '_' && this.hasOwnProperty(property)) { this[property] = args[property]; }
			}
		}
		this.Bind();
	},
	//#endregion
	//#region Events
	Bind: function () {
		var $s = this;
		/* Use this to initially bind events to elements that exist on load, ex onclick etc */
		$($s.source).data("UxDialog", $s);
		$($s.container).on("keypress keydown click", function () { $s.Action.apply($s, arguments); });
		$(window).on("resize", function () { $s.Resize.apply($s, arguments); });

		if ($s.open === true) {
			$s.Show();
		}
	},
	Action: function (evnt) {
		var $s = this;
		var container = $($s.container);
		var target = $(evnt.target).closest("[id]").not(":disabled");
		if (evnt.keyCode === 27 /*Escape*/) {
			$s.Close();
		}
		if (evnt == null || evnt.type === 'click' || evnt.keyCode === 13 /*Enter*/ || evnt.keyCode === 32/*Space*/) {
			// handle chrome events
			if ($(evnt.target).attr("data-item") === "dialogClose") {
				$s.Close();
			}
		}
		else if (evnt.keyCode && evnt.keyCode == 9 /*TAB*/) {
			var focusableItems = $s._getFocusable();
			if (evnt.target === focusableItems.last()[0] && !evnt.shiftKey) {
				evnt.preventDefault();
				focusableItems.first().focus();
			}
			else if (evnt.target === focusableItems.first()[0] && evnt.shiftKey) {
				evnt.preventDefault();
				focusableItems.last().focus();
			}
		}
	},
	Show: function () {
		var $s = this;
		var body = $('body');
		var container = $($s.container);
		if ($s.keepOpen === true)
		{
			container.find(">header>[data-item=dialogClose]").remove();
		}

		var curtain = body.children('div.ModalDialogCurtain');
		if (curtain.length === 0) {
			body.append('<div class="ModalDialogCurtain"></div>');
			curtain = body.children('div.ModalDialogCurtain');
		}
		curtain.show();
		container.show();
		if ($s.title != null) {
			var header = container.find('>header');
			var title = header.find(">h1");
			title.text($s.title);
			if ($s.dialogType === "warning") {
				if (header.find(".symbol._warning").length === 0) {
					title.before($("<span class='symbol _warning'></span>"));
				}
			}
		}
		if (container.parent().length === 0) {
			var content = container.find(">section");
			if (content.children().length === 0) {
				var source = $($s.source).detach();
				source.removeClass('hidden hide');
				content.append(source);
			}
			body.append(container);
		}
		$s.Resize();
		if ($s.onShow instanceof Function) {
			$s.onShow.apply();
		}
		$s._getFocusable().not("[data-item]").focus();
	},
	Close: function () {
		var $s = this;
		var container = $($s.container);
		var stayOpen = $s.keepOpen;
		if ($s.onClose instanceof Function) {
			stayOpen = $s.onClose.apply() === false;
		}
		if (stayOpen !== true) {
			container.hide();
			$('body>div.ModalDialogCurtain').hide();
		}
	},
	Resize: function (evnt) {
		var $s = this;
		var container = $($s.container).is('.ModalDialog') ? $($s.container) : $($s.container).children('.ModalDialog');
		if (container.is(":visible")) {
		    container
				.css("position", "fixed")
				.css('display', 'table')
				.css("top", '50%')
				.css("margin-top", -container.outerHeight() / 2)
    			.css("z-index", $('body > div.ModalDialogCurtain').css("z-index") + 10);
			if (container.outerHeight() > $(window).height()) {
			    container
				.css("position", "absolute")
				.css("margin-top", 0)
				.css("top", 0);
			}
		}

	},
	_getFocusable: function () {
		var $s = this;
		var container = $($s.container);
		return container.find("input,select,textarea,button,object,[tabindex]");
	}
	//#endregion
}
