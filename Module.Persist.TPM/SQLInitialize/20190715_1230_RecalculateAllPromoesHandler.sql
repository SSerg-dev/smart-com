﻿INSERT INTO [LoopHandler]
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
		   ,N'Promo recalculation in dataflow process'
           ,'Module.Host.TPM.Handlers.DataFlow.RecalculateAllPromoesHandler'
           ,24 * 60 * 60 * 1000 
           ,'SCHEDULE'
           ,'2019-07-15 09:00:00.3321785 +03:00'
           ,NULL
           ,NULL
           ,'PROCESSING'
           ,NULL
           ,NULL
           ,NULL
           ,NULL)
