DECLARE @handlerName VARCHAR(255) = 'Module.Host.TPM.Handlers.UpdatePreviousDayIncrementalHandler';

DELETE [dbo].[LoopHandler]  WHERE [Name] = @handlerName;

SET @handlerName = 'Module.Host.TPM.Handlers.DayIncrementalQTYRecalculationHandler';

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
	N'Update dat incremental QTY',
	@handlerName,
	NULL,
	'SINGLE',
	SYSDATETIME(),
	NULL,
	NULL,
	'PROCESSING',
	'WAITING',
	NULL,
	NULL,
	NULL
);
