export enum CommandHandlingErrorType {
    FailedToConnectToFeedbackHub,
    FailedToQueue,
    ProcessingTimeout,
    FailedToProcess,
	FailedToSubscribeToReadModelChangeNotification,
	ReadModelChangeNotificationTimeout
}