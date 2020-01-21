DELETE [dbo].[LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsProductsCheckHandler'
GO
DELETE [dbo].[LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.MarsProductsCheckHandler'
GO
DELETE [dbo].[LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsProductsCheckStarterHandler'
GO
DELETE [dbo].[LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.MarsProductsCheckStarterHandler'
GO

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
           ,'Starter of Mars products sync proccess.'
           ,'Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsProductsCheckStarterHandler'
           ,86400000
           ,'SCHEDULE'
           ,SYSDATETIME()
           ,NULL
           ,NULL
           ,'PROCESSING'
           ,'WAITING'
           ,NULL
           ,NULL
           ,NULL)
GO