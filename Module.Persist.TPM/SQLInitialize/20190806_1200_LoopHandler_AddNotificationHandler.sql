DELETE [LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.Notifications.WeekBeforeDispatchPromoNotificationHandler'

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
           ,'Sending notifications of promoes that have a week before dispath start'
           ,'Module.Host.TPM.Handlers.Notifications.WeekBeforeDispatchPromoNotificationHandler'
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