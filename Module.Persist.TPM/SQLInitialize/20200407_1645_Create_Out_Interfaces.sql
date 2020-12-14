DELETE FROM FileSendInterfaceSetting
DELETE FROM CSVExtractInterfaceSetting
DELETE FROM FileBuffer WHERE FileName = N'IN_PROMO_UPLIFT_DMD_STG_0125.dat'
DELETE FROM Interface WHERE [Name] = N'INCREMENTAL_TO_APOLLO'
DELETE FROM LoopHandler WHERE [Name] = 'Module.Host.TPM.Handlers.Interface.Outcoming.OutputIncrementalProcessHandler'
OR [Name] = 'ProcessingHost.Handlers.Interface.Outcoming.FileSendHandler'

INSERT [Interface] ([Id], [Name], [Direction], [Description]) 
VALUES (N'4C373105-CD78-EA11-A608-7470FD19293C', N'INCREMENTAL_TO_APOLLO', N'OUTBOUND', N'Output Incremental To Apollo')
GO

INSERT [CSVExtractInterfaceSetting] ([Id], [InterfaceId], [Delimiter], [UseQuoting], [QuoteChar], [FileNameMask], [ExtractHandler]) 
VALUES (N'4D373105-CD78-EA11-A608-7470FD19293C'
	,N'4C373105-CD78-EA11-A608-7470FD19293C'
	,N','
	,0
	,N''
	,N'IN_PROMO_UPLIFT_DMD_STG_0125.dat'
	,N'Module.Host.TPM.Handlers.Interface.Outcoming.OutputIncrementalProcessHandler')
GO

INSERT [FileSendInterfaceSetting] ([Id], [InterfaceId], [TargetPath], [TargetFileMask], [SendHandler])  
VALUES (N'4E373105-CD78-EA11-A608-7470FD19293C'
	,N'4C373105-CD78-EA11-A608-7470FD19293C'
	,N''
	,N'IN_PROMO_UPLIFT_DMD_STG_0125.dat'
	,N'ProcessingHost.Handlers.Interface.Outcoming.FileSendHandler')
GO

INSERT [LoopHandler] ([Id], [Description], [Name], [ExecutionPeriod], [ExecutionMode], [CreateDate], [LastExecutionDate], [NextExecutionDate], [ConfigurationName], [Status], [RunGroup], [UserId], [RoleId]) 
VALUES (N'4D10125B-0785-4EC4-8802-524C1550CD1E'
	,N'Outcoming incremental processing'
	,N'Module.Host.TPM.Handlers.Interface.Outcoming.OutputIncrementalProcessHandler'
	,0
	,N'MANUAL'
	,GETDATE()
	,NULL
	,NULL
	,N'PROCESSING'
	,N'WAITING'
	,NULL
	,NULL
	,NULL)
GO

DECLARE @nextTwoHours DATETIMEOFFSET(7) = DATEADD(HOUR, 2, GETDATE());
INSERT [LoopHandler] ([Id], [Description], [Name], [ExecutionPeriod], [ExecutionMode], [CreateDate], [LastExecutionDate], [NextExecutionDate], [ConfigurationName], [Status], [RunGroup], [UserId], [RoleId]) 
VALUES (N'81DB97ED-B9A8-421E-9690-2DD98E33BF36'
	,N'Outcoming files sending'
	,N'ProcessingHost.Handlers.Interface.Outcoming.FileSendHandler'
	,300000
	,N'PERIOD'
	,GETDATE()
	,NULL
	,DATETIMEOFFSETFROMPARTS ( 
		YEAR(@nextTwoHours),
		MONTH(@nextTwoHours),
		DAY(@nextTwoHours),
		DATEPART(HOUR,@nextTwoHours),
		DATEPART(MINUTE,@nextTwoHours),
		0,0,3,0,7)
	,N'PROCESSING'
	,N'WAITING'
	,NULL
	,NULL
	,NULL)
GO
