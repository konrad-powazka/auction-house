﻿import {SecurityUiService} from './SecurityUiService';
import BusyIndicator from './BusyIndicator';

export class ApplicationCtrl implements ng.IController {
	auctionsSearchQueryString: string;
	busySpinnerIsVisible = false;
	spinnerOptions: any;
	static $inject = ['securityUiService', 'busyIndicator', '$state', '$timeout', '$scope'];

	constructor(public securityUiService: SecurityUiService,
		busyIndicator: BusyIndicator,
		private $state: ng.ui.IStateService,
		$timeout: ng.ITimeoutService,
		$scope: ng.IScope) {
		this.spinnerOptions = {
			lines: 13, // The number of lines to draw
			length: 28, // The length of each line
			width: 14, // The line thickness
			radius: 35, // The radius of the inner circle
			scale: 1, // Scales overall size of the spinner
			corners: 1, // Corner roundness (0..1)
			color: '#FFFFFF', // #rgb or #rrggbb or array of colors
			opacity: 0.25, // Opacity of the lines
			rotate: 0, // The rotation offset
			direction: 1, // 1: clockwise, -1: counterclockwise
			speed: 2.2, // Rounds per second
			trail: 60, // Afterglow percentage
			fps: 20, // Frames per second when using setTimeout() as a fallback for CSS
			zIndex: 2e9, // The z-index (defaults to 2000000000)
			className: 'spinner', // The CSS class to assign to the spinner
			top: '50%', // Top position relative to parent
			left: '50%', // Left position relative to parent
			shadow: false, // Whether to render a shadow
			hwaccel: false, // Whether to use hardware acceleration
			position: 'relative' // Element positioning
		};

		$scope.$watch(() => busyIndicator.isBusy,
		() => {
			if (busyIndicator.isBusy) {
				const showBusyIndicatorDelayInMilliseconds = 400;

				$timeout(showBusyIndicatorDelayInMilliseconds)
					.then(() => {
						if (busyIndicator.isBusy) {
							this.busySpinnerIsVisible = true;
						}
					});
			} else {
				this.busySpinnerIsVisible = false;
			}
		});
	}

	searchAuctions(): void {
		const queryString = this.auctionsSearchQueryString;
		this.auctionsSearchQueryString = '';
		this.$state.go('auctionsSearch', { queryString: queryString });
	}
}