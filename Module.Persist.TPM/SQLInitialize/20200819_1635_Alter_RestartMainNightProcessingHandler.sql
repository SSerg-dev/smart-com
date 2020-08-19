DECLARE @nextDay DATETIMEOFFSET(7) = DATEADD(DAY, 1, SYSDATETIME());

UPDATE [dbo].[LoopHandler]
   SET NextExecutionDate = DATETIMEOFFSETFROMPARTS ( 
				YEAR(@nextDay), 
				MONTH(@nextDay),
				DAY(@nextDay), 
				23, 30, 0, 0, 3, 0, 7 ),
		ExecutionPeriod = '86400000'
  where name = 'Module.Host.TPM.Handlers.RestartMainNightProcessingHandler' and NextExecutionDate IS NOT NULL
GO