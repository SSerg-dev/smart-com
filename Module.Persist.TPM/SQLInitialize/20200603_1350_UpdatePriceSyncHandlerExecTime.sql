DECLARE @today DATETIMEOFFSET(7) = SYSDATETIME();

UPDATE [dbo].[LoopHandler] SET [NextExecutionDate] = 
	DATETIMEOFFSETFROMPARTS ( 
		YEAR(@today), 
		MONTH(@today),
		DAY(@today), 
		22, 0, 0, 0, 3, 0, 7 ) 
WHERE [Name] = 'Module.Host.TPM.Handlers.PriceListMergeHandler' AND [ExecutionMode] = 'SCHEDULE';
