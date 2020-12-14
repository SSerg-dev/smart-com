UPDATE [LoopHandler] 
SET 
	[ExecutionPeriod] = 0, 
	[ExecutionMode] = 'MANUAL', 
	[LastExecutionDate] = NULL, 
	[NextExecutionDate] = NULL, 
	[Status] = 'WAITING' 
WHERE 
	[Name] = 'Module.Host.TPM.Handlers.DayIncrementalQTYRecalculationHandler';