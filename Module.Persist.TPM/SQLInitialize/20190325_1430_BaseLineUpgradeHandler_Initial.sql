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
           (N'Пересчет параметров при изменении BaseLine'
           ,'Module.Host.TPM.Handlers.BaseLineUpgradeHandler'
           ,60 * 60 * 1000 -- 1 час
           ,'SCHEDULE'
           ,'2019-03-25 05:30:00.3321785 +03:00'
           ,'2019-03-25 05:30:00.3321785 +03:00'
           ,'2019-03-25 05:30:00.3321785 +03:00'
           ,'PROCESSING'
           ,NULL
           ,NULL
           ,NULL
           ,NULL)