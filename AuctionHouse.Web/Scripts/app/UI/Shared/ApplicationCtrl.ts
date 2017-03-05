import {SecurityUiService} from './SecurityUiService';
import BusyIndicator from './BusyIndicator';

export class ApplicationCtrl implements ng.IController {
	auctionsSearchQueryString: string;
    spinnerOptions: any;
	static $inject = ['securityUiService', 'busyIndicator', '$state'];

	constructor(public securityUiService: SecurityUiService, public busyIndicator: BusyIndicator, private $state: ng.ui.IStateService) {
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
	}

	checkIfAuctionsSearchQueryStringIsValid(): boolean {
		return _(this.auctionsSearchQueryString).isString() && !!this.auctionsSearchQueryString.trim();
	}

	searchAuctions(): void {
		this.$state.go('auctionsSearch', { queryString: this.auctionsSearchQueryString });
	}
}