DECLARE @handlerName VARCHAR(255) = 'Module.Host.TPM.Handlers.PriceListMergeHandler';
DECLARE @nextDay DATETIMEOFFSET(7) = DATEADD(DAY, 1, SYSDATETIME());

DELETE [dbo].[LoopHandler]  WHERE [Name] = @handlerName;

INSERT INTO [dbo].[LoopHandler] ([Id] ,[Description] ,[Name] ,[ExecutionPeriod] ,[ExecutionMode] ,[CreateDate] ,[LastExecutionDate] ,[NextExecutionDate] ,[ConfigurationName] ,[Status] ,[RunGroup] ,[UserId] ,[RoleId])
    VALUES (NEWID() 
	,N'Datalake Price List -> TPM Price List' 
	,@handlerName 
	,86400000 
	,'SCHEDULE' 
	,GETDATE() 
	,NULL
	,DATETIMEOFFSETFROMPARTS ( 
		YEAR(@nextDay), 
		MONTH(@nextDay),
		DAY(@nextDay), 
		8, 0, 0, 0, 3, 0, 7 ) 
	,'PROCESSING' 
	,NULL ,NULL ,NULL ,NULL);
