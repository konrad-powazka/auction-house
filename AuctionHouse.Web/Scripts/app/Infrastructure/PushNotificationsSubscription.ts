export class PushNotificationsSubscription {
    private wasSubscriptionActivated = false;

    constructor(public activatedPromise: angular.IPromise<void>, private cancelCallback: () => void) {
        activatedPromise.then(() => this.wasSubscriptionActivated = true);
    }

    cancel(): void {
        if (!this.wasSubscriptionActivated) {
            throw new Error('Cannot cancel a subscription before its activation.');
        }

        this.cancelCallback();
    }
}