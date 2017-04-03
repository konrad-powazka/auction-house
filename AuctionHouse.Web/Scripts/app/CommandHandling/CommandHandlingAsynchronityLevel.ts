export enum CommandHandlingAsynchronityLevel {
	QueueOnly,
	WaitUntilCommandIsProcessed,
	WaitUnitReadModelIsUpdated
}