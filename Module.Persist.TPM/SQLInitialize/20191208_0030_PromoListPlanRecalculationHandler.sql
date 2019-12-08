DELETE [dbo].[LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.PromoListPlanRecalculationHandler'
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
           ,'Recalculating all plan parameters for the promo from list'
           ,'Module.Host.TPM.Handlers.PromoListPlanRecalculationHandler'
           ,864000000000
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