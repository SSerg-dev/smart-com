 
DECLARE @handlerName VARCHAR(255) = 'Module.Host.TPM.Handlers.RollingVolumeQTYRecalculationHandler'; 

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
	N'Rolling promo collecting',
	@handlerName,
	0,
	'MANUAL',
	SYSDATETIME(),
	NULL,
	NULL,
	'PROCESSING',
	'WAITING',
	NULL,
	NULL,
	NULL
);

GO


