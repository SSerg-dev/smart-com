DECLARE @nextDay DATETIMEOFFSET(7) = DATEADD(DAY, 1, SYSDATETIME());

UPDATE [dbo].[LoopHandler] SET [NextExecutionDate] = 
	DATETIMEOFFSETFROMPARTS ( 
		YEAR(@nextDay), 
		MONTH(@nextDay),
		DAY(@nextDay), 
		0, 10, 0, 0, 3, 0, 7 )
WHERE ([Name] = 'Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsCustomersCheckHandler'
	OR [Name] = 'Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsProductsCheckStarterHandler')
	AND [ExecutionMode] = 'SCHEDULE'
GO