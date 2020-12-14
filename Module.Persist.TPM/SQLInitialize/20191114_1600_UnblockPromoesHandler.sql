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
		   ,N'Unblock promoes'
           ,'Module.Host.TPM.Handlers.UnblockPromoesHandler'
           ,15 * 60 * 1000 
           ,'SCHEDULE'
           ,'2019-07-15 09:00:00.3321785 +03:00'
           ,NULL
           ,NULL
           ,'PROCESSING'
           ,NULL
           ,NULL
           ,NULL
           ,NULL)
GO

INSERT INTO [Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'UNBLOCK_PROMO_HOURS'
           ,N'Int'
           ,N'6'
           ,N'Hours for promoes unblocking.')
GO