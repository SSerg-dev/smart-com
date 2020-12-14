DELETE [LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.RemoveDeletedDataHandler'

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
           ,N'Remove deleted items from DB'
           ,'Module.Host.TPM.Handlers.RemoveDeletedData'
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