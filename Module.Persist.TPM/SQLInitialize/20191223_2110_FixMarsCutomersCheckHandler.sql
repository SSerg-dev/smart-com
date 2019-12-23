DELETE [dbo].[LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.MarsCustomersCheckHandler'
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
           ,'Checking of the new Mars customers adding'
           ,'Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsCustomersCheckHandler'
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


