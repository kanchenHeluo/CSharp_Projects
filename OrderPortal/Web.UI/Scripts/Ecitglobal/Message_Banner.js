
//----------------------------------------------------------------------------------------
//------------------------Script for toast  notifications---------------------------------
//----------------------------------------------------------------------------------------
var ToastNotification = function (a) {
	this.Title = "";
	this.Message = "";
	this.Color = "";
	this.TitleText = "";
	this.CloseTitle = "Close";
	this.parentContainerSelector = "body";
	this._ctor(a);
}
/* static properties*/
ToastNotification.COLORS = {
	GREEN: "green",
	BLUE: "blue",
	YELLOW: "yellow",
	RED: "red"
};
ToastNotification.ICONCSS = {
	GREEN: "i_ToastSuccessCheckmark",
	BLUE: "i_ToastInfo",
	YELLOW: "i_ToastWarning",
	RED: "i_ToastError"
};
ToastNotification.CSS = {
	STANDARDTEXT: "stdText",
	BANNERHEADING: "toastTitle",
};
/* Prototype*/
ToastNotification.prototype = {
	_ctor: function (a) {
		if (a) {
			this.LoadArgument(a);
		}
		else {
			var textBox = $('#ToastNotificationTB')
			if (textBox.length > 0) {
				this.Message = textBox.val();
				this.Color = textBox.attr("class");
				if (this.Color != null) { this.Color = this.Color.toLowerCase(); }
				this.TitleText = textBox.attr("title");
				this.CloseTitle = textBox.attr("closeTitle");
			}
		}
		if (this.Message.length > 0 || this.Title.length > 0) {
			this.Show();
		}
	},
	Show: function (a) {
		var $s = this;
		if (a) {
			this.LoadArgument(a);
		}
		var messageColor = $s.Color.toLowerCase();
		var container = $(document.createElement("div"));
		container.addClass("toast");

		var iconDiv = $(document.createElement("div"));

		// Code for displaying Close button
		var closeButton = $(document.createElement("div"));
		closeButton.addClass("toastCloseButton");
		closeButton.attr({
			"role": "button",
			"title": this.CloseTitle,
			"tabindex": "0"
		});
		closeButton.html("x");

		if (messageColor == ToastNotification.COLORS.GREEN) {
			iconDiv.addClass(ToastNotification.ICONCSS.GREEN);
			/*adding title attribute according to StyleGuide*/
			iconDiv.attr('title', $s.TitleText);
		}
		else if (messageColor == ToastNotification.COLORS.YELLOW) {
			iconDiv.addClass(ToastNotification.ICONCSS.YELLOW);
			/*adding title attribute according to StyleGuide*/
			iconDiv.attr('title', $s.TitleText);
		}
		else if (messageColor == ToastNotification.COLORS.RED) {
			iconDiv.addClass(ToastNotification.ICONCSS.RED);
			/*adding title attribute according to StyleGuide*/
			iconDiv.attr('title', $s.TitleText);
		}
		else if (messageColor == ToastNotification.COLORS.BLUE) {
			iconDiv.addClass(ToastNotification.ICONCSS.BLUE);
			/*adding title attribute according to StyleGuide*/
			iconDiv.attr('title', this.TitleText);
		}

		var textContainer = $(document.createElement("div"));
		textContainer.addClass("textContainer");

		var messageSpan = $(document.createElement("span"));
		if (messageColor != ToastNotification.COLORS.YELLOW) {
			messageSpan.addClass('white');
		}
		messageSpan.addClass(ToastNotification.CSS.STANDARDTEXT);
		messageSpan.html(this.Message);

		var messageDiv = $(document.createElement("div"));
		messageDiv.addClass("message");
		messageDiv.addClass(this.Color);
		messageDiv.html(textContainer);

		textContainer.html(messageSpan);

		/*Check to see if title has been requested, if so, create and add, if not, give styling to message instead*/
		if (this.Title != "") {
			var titleSpan = $(document.createElement("span"));
			titleSpan.html(this.Title);
			if (messageColor != ToastNotification.COLORS.YELLOW) {
				titleSpan.addClass('white');
			}
			titleSpan.addClass(ToastNotification.CSS.BANNERHEADING);
			titleSpan.prependTo(textContainer);
		}
		else {
			messageSpan.removeClass(ToastNotification.CSS.STANDARDTEXT);
			//messageSpan.addClass('toastTitle');
			if (messageColor != ToastNotification.COLORS.YELLOW) {
				messageSpan.addClass('white');
			}
			messageSpan.addClass(ToastNotification.CSS.BANNERHEADING);
		}

		iconDiv.prependTo(messageDiv);

		if (messageColor === ToastNotification.COLORS.YELLOW || messageColor === ToastNotification.COLORS.RED) {
			closeButton.appendTo(messageDiv);
		}

		textContainer.find("a").each(function (i, e) {
			// remove any font-sizes on inserted alinks and set them to inherit to enforce a match the surrounding text styling
			$(e).css('font-size', 'inherit');
		});

		messageDiv.appendTo(container);

		$(this.parentContainerSelector).append(container);

		//Timer for auto hide and mouse over
		var autoTimer = null;

		//If the toast is blue or green, we have it auto hide unless there is a mouseover
		if (messageColor === ToastNotification.COLORS.BLUE || messageColor === ToastNotification.COLORS.GREEN) {
			//Handles mouseover to keep green and blue notifications showing while mouse is over it

			container.on("mouseover", function () {
				if (autoTimer) {
					//Showing close button on Mouseover
					closeButton.appendTo(messageDiv);
					$(".toastCloseButton").focus();
					$(".toastCloseButton").on("click", function () {
						$(".toast").fadeOut('slow', function () {
							$(".toast").remove();
						});
					});
					$('.toastCloseButton').on("keypress", function (e) {
						if (e.which == 13 || e.which == 32) {
							e.preventDefault();
							$(".toast").fadeOut('slow', function () {
								$(".toast").remove();
							});
						}
					});
					clearTimeout(autoTimer);
					autoTimer = null;
				}
			});
			container.on("mouseleave", function () {
				closeButton.remove();
				autoTimer = setTimeout(function () {
					container.fadeOut('slow');
					container.remove();
				}, 2000);
			});

			autoTimer = setTimeout(function () {
				container.slideDown("slow");
				autoTimer = setTimeout(function () {
					container.fadeOut('slow');
					container.remove();
				}, 7000);
			}, 200);
		}
			//Not blue or green, so it shows permanently with a close button for user to click on
		else {
			container.delay(200).slideDown('slow', function () {
				$(".toastCloseButton").on("click", function () {
					$(".toast").fadeOut('slow', function () {
						$(".toast").remove();
					});
				});
				$('.toastCloseButton').on("keypress", function (e) {
					if (e.which == 13 || e.which == 32) {
						e.preventDefault();
						$(".toast").fadeOut('slow', function () {
							$(".toast").remove();
						});
					}
				});
			});
		}
	},
	LoadArgument: function (a) {
		if (a != null) {
			if (a.Title != null) { this.Title = a.Title; }
			if (a.Message != null) { this.Message = a.Message; }
			if (a.Color != null) { this.Color = a.Color.toLowerCase(); }
		}
	}
}

$().ready(function () {
	if (!window.toastMessage) {
		window.toastMessage = new ToastNotification();
	}
});

//#region Mini Toast
var MiniToastObject = function (args) {
	this.template = "<div class='MiniToast'>{0}</div>";
	this.appendTo = args.appendto;
	this.message = "";

	this._ctor(args);
};
MiniToastObject.prototype = {
	_ctor: function (args) {
		var $s = this;
		if (!args) { return; }

		//copy properties from args
		for (var property in args) {
			if (property[0] != '_' && this.hasOwnProperty(property)) { this[property] = args[property]; }
		}

		var destContainer = $(this.appendTo);
		destContainer.remove('.MiniToast');
		var toast = $(String.format(this.template, $s.message))
			.fadeIn(350, function () {
				setTimeout(function () {
					toast.fadeOut(1000);
				}, 1500);
			});
		destContainer.append(toast);
	}
};
//#endregion