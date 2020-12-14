
DECLARE @handlerName VARCHAR(255) = 'Module.Host.TPM.Handlers.RestartMainNightProcessingHandler';

DELETE [LoopHandler]  WHERE [Name] = @handlerName;
 
 
DECLARE @nextDay DATETIMEOFFSET(7) = DATEADD(DAY, 1, SYSDATETIME());
INSERT INTO [LoopHandler] (
	[Id],
	[Description],
	[Name],
	[ExecutionPeriod],
	[ExecutionMode],
	[CreateDate],
	[LastExecutionDate],
	[NextExecutionDate],
	[ConfigurationName],
	[Status],
	[RunGroup],
	[UserId],
	[RoleId]
)
VALUES (
	NEWID(),
	N'Restart Main Night Processing Check Handler',
	@handlerName,
	172800000,
	'SCHEDULE',
	SYSDATETIME(),
	NULL,
	 DATETIMEOFFSETFROMPARTS ( 
				YEAR(@nextDay), 
				MONTH(@nextDay),
				DAY(@nextDay), 
				8, 20, 0, 0, 3, 0, 7 ),
	'PROCESSING',
	'WAITING',
	NULL,
	NULL,
	NULL
);



