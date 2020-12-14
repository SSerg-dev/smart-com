DELETE [LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsProductsCheckHandler'
GO

INSERT INTO [LoopHandler]
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
           ,'Check materials for synchronization with products'
           ,'Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsProductsCheckHandler'
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