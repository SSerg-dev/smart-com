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
		   ,N'Auto reset promo'
           ,'Module.Host.TPM.Handlers.AutoResetPromoHandler'
           ,24 * 60 * 60 * 1000 
           ,'SCHEDULE'
           ,'2019-06-04 19:40:00.3321785 +03:00'
           ,NULL
           ,NULL
           ,'PROCESSING'
           ,NULL
           ,NULL
           ,NULL
           ,NULL)
