INSERT INTO [LoopHandler]
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
           (N'Обработка Promo по дате начала/окончания'
           ,'Module.Host.TPM.Handlers.PromoWorkflowHandler'
           ,24 * 60 * 60 * 1000 -- 24 часа
           ,'SCHEDULE'
           ,'2019-02-19 01:00:00.0000000 +03:00'
           ,'2019-02-19 01:00:00.0000000 +03:00'
           ,'2019-02-20 01:00:00.0000000 +03:00'
           ,'PROCESSING'
           ,NULL
           ,NULL
           ,NULL
           ,NULL)
