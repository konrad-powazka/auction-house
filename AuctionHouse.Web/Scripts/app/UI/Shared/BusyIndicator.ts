export default class BusyIndicator {
    private busyStatesCount = 0;

    get isBusy(): boolean {
        return this.busyStatesCount > 0 ||
            (!!this.parentBusyIndicator && (this.parentBusyIndicator as BusyIndicator).isBusy);
    }

    constructor(private parentBusyIndicator?: BusyIndicator) {
    }

    enterBusyState(): () => void {
        var wasBusyStateLeft = false;
        this.busyStatesCount++;

        const leaveBusyStateFn = () => {
            if (!wasBusyStateLeft) {
                wasBusyStateLeft = true;
                this.busyStatesCount--;
            }
        };

        return leaveBusyStateFn;
    }

    createNestedBusyIndicator(): BusyIndicator {
        return new BusyIndicator(this);
    }

    attachToPromise<T>(promise: ng.IPromise<T>): ng.IPromise<T> {
        var leaveBusyStateFn = this.enterBusyState();

        promise.finally(() => {
            leaveBusyStateFn();
        });

        return promise;
    }
}