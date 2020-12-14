DELETE [LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.Notifications.ClientTreeNeedUpdateNotificationHandler'
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
           ,N'Adding new client tree notifications'
           ,'Module.Host.TPM.Handlers.Notifications.ClientTreeNeedUpdateNotificationHandler'
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


