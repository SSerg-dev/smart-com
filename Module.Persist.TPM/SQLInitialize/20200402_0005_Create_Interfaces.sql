DELETE FROM FileCollectInterfaceSetting
DELETE FROM CSVProcessInterfaceSetting
DELETE FROM Interface WHERE [Name] = N'BASELINE_APOLLO'
DELETE FROM LoopHandler WHERE [Name] = 'Module.Host.TPM.Handlers.Interface.Incoming.InputBaseLineProcessHandler'
OR [Name] = 'ProcessingHost.Handlers.Interface.Incoming.FileCollectHandler'
GO

INSERT [Interface] ([Id], [Name], [Direction], [Description]) 
VALUES (N'F35E7B12-7367-EA11-8BD5-08606E18DF3F', N'BASELINE_APOLLO', N'INBOUND', N'Input Baseline SI from Apollo')
GO

INSERT [CSVProcessInterfaceSetting] ([Id], [InterfaceId], [Delimiter], [UseQuoting], [QuoteChar], [ProcessHandler]) 
VALUES (N'F45E7B12-7367-EA11-8BD5-08606E18DF3F'
	,N'F35E7B12-7367-EA11-8BD5-08606E18DF3F'
	,N','
	,0
	,N''
	,N'Module.Host.TPM.Handlers.Interface.Incoming.InputBaseLineProcessHandler')
GO

INSERT [FileCollectInterfaceSetting] ([Id], [InterfaceId], [SourcePath], [SourceFileMask], [CollectHandler]) 
VALUES (N'F55E7B12-7367-EA11-8BD5-08606E18DF3F'
	,N'F35E7B12-7367-EA11-8BD5-08606E18DF3F'
	,N''
	,N'*.dat*'
	,N'ProcessingHost.Handlers.Interface.Incoming.FileCollectHandler')
GO

INSERT [LoopHandler] ([Id], [Description], [Name], [ExecutionPeriod], [ExecutionMode], [CreateDate], [LastExecutionDate], [NextExecutionDate], [ConfigurationName], [Status], [RunGroup], [UserId], [RoleId]) 
VALUES (N'DEFF0588-02B3-461E-AFDC-B716DDC1090F'
	,N'Incoming baseline processing'
	,N'Module.Host.TPM.Handlers.Interface.Incoming.InputBaseLineProcessHandler'
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
VALUES (N'A5F80A9E-8205-4464-A3D8-1DF3EE150BBC'
	,N'Incoming files collecting'
	,N'ProcessingHost.Handlers.Interface.Incoming.FileCollectHandler'
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
