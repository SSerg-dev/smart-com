INSERT INTO [Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'CHECK_NEW_PRODUCT_STATUS_LIST'
           ,N'String'
           ,N'OnApproval,Approved,Planned,Started'
           ,N'Promotional statuses for which it is necessary to check the compliance of a new product with a filter')
GO

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
           (N'Mailing list of new products suitable for current promotions'
           ,'Module.Host.TPM.Handlers.NewProductsNotificationHandler'
           ,24 * 60 * 60 * 1000 -- 24 часа
           ,'SCHEDULE'
           ,'2019-02-13 05:30:00.3321785 +03:00'
           ,'2019-02-13 05:30:00.3321785 +03:00'
           ,'2019-02-13 05:30:00.3321785 +03:00'
           ,'PROCESSING'
           ,NULL
           ,NULL
           ,NULL
           ,NULL)