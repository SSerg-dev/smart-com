DELETE [dbo].[LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.NightProcessingMainHandler'
GO

DECLARE @nextDay DATETIMEOFFSET(7) = DATEADD(DAY, 1, SYSDATETIME());
INSERT INTO [dbo].[LoopHandler]
           ([Id]
           ,[Description]
           ,[Name]
           ,[ExecutionPeriod]
           ,[ExecutionMode]
           ,[CreateDate]
           ,[LastExecutionDate]
           ,[NextExecutionDate]
           ,[ConfigurationName]
           ,[Status]
           ,[RunGroup]
           ,[UserId]
           ,[RoleId])
     VALUES
           (NEWID()
           ,'Main night processing'
           ,'Module.Host.TPM.Handlers.NightProcessingMainHandler'
           ,86400000
           ,'SCHEDULE'
           ,SYSDATETIME()
           ,NULL
           ,DATETIMEOFFSETFROMPARTS ( 
				YEAR(@nextDay), 
				MONTH(@nextDay),
				DAY(@nextDay), 
				0, 20, 0, 0, 3, 0, 7 )
           ,'PROCESSING'
           ,'WAITING'
           ,NULL
           ,NULL
           ,NULL)
GO