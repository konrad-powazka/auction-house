import BusyIndicator from './BusyIndicator';

export default class BusyIndicatingHttpInterceptor implements ng.IHttpInterceptor {
	private numberOfRequestsInProgress = 0;
	private leaveHttpBusyStateFn: () => void;

	static $inject = ['busyIndicator', '$q'];

	constructor(private busyIndicator: BusyIndicator, private $q: ng.IQService) {
	}

	request = <T>(config: ng.IRequestConfig): ng.IRequestConfig | ng.IPromise<ng.IRequestConfig> => {
		if (this.numberOfRequestsInProgress === 0) {
			this.leaveHttpBusyStateFn = this.busyIndicator.enterBusyState();
		}

		this.numberOfRequestsInProgress++;
		return config;
	};

	response = <T>(response: ng.IHttpPromiseCallbackArg<T>): ng.IPromise<ng.IHttpPromiseCallbackArg<T>> | ng.
		IHttpPromiseCallbackArg<T> => {
			this.decrementNumberOfRequestsInProgress();
			return response;
	};

	responseError = (rejection: any): any => {
		this.decrementNumberOfRequestsInProgress();
		return this.$q.reject(rejection);
	};

	private decrementNumberOfRequestsInProgress(): void {
		this.numberOfRequestsInProgress--;

		if (this.numberOfRequestsInProgress === 0) {
			this.leaveHttpBusyStateFn();
		}
	}
}