DELETE [LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.SetCOGSPercentValuesHandler'
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
           ,'Calculate COGSPercent for all promo'
           ,'Module.Host.TPM.Handlers.SetCOGSPercentValuesHandler'
           ,0
		   ,N'MANUAL'
		   ,GETDATE()
		   ,NULL
		   ,NULL
		   ,N'PROCESSING'
		   ,N'WAITING'
		   ,NULL
		   ,NULL
		   ,NULL)
GO