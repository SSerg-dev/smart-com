DECLARE @handlerName VARCHAR(255) = 'Module.Host.TPM.Handlers.UpdatePreviousDayIncrementalHandler';

DELETE [dbo].[LoopHandler]  WHERE [Name] = @handlerName;

INSERT INTO [dbo].[LoopHandler] (
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
	N'CurrentDayIncremental -> PreviousDayIncremental. Update CurrentDayIncremental',
	@handlerName,
	86400000,
	'SCHEDULE',
	SYSDATETIME(),
	NULL,
	NULL,
	'PROCESSING',
	'WAITING',
	NULL,
	NULL,
	NULL
);
