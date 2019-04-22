INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'PROMO_DEMAND_CHANGE_NOTIFICATION_TEMPLATE_FILE'
           ,N'String'
           ,N'PromoDemandChangeTemplate.txt'
           ,N'Promo Demand Change notification template file')
GO

INSERT INTO [dbo].[LoopHandler]
           ([Description]
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
           (N'Mailing Promo Demand Change notifications'
           ,'Module.Host.TPM.Handlers.PromoDemandChangeNotificationHandler'
           ,24 * 60 * 60 * 1000 -- 24 часа
           ,'SCHEDULE'
           ,'2019-02-07 05:30:00.3321785 +03:00'
           ,'2019-02-07 05:30:00.3321785 +03:00'
           ,'2019-02-08 05:30:00.3321785 +03:00'
           ,'PROCESSING'
           ,NULL
           ,NULL
           ,NULL
           ,NULL)