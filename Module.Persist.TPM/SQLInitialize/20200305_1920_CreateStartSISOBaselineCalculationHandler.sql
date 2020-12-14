DELETE [LoopHandler] WHERE [Name] = 'Module.Host.TPM.Handlers.StartSISOBaselineCalculationHandler'
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
           ,'Start baseline calculation'
           ,'Module.Host.TPM.Handlers.StartSISOBaselineCalculationHandler'
           ,86400000000
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