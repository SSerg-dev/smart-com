INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'PROMO_UPLIFT_FAIL_NOTIFICATION_TEMPLATE_FILE'
           ,N'String'
           ,N'PromoUpliftFailTemplate.txt'
           ,N'File Uplift Fail Notifications template')
GO

INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'UPLIFT_FAIL_PERIOD_DAYS'
           ,N'Int'
           ,N'56'
           ,N'If before the start of the promo there are fewer days than the specified number, you need to include it in the Uplift Fail notification.')
GO

INSERT INTO [dbo].[MailNotificationSetting]
           ([Name]
           ,[Description]
           ,[Subject]
           ,[Body]
           ,[IsDisabled]
           ,[To]
           ,[CC]
           ,[BCC]
           ,[Disabled]
           ,[DeletedDate])
     VALUES
           (N'PROMO_UPLIFT_FAIL_NOTIFICATION'
           ,N'Notification about the impossibility of selecting uplift for promo'
           ,N'Failed to calculate uplift for the following promoes '
           ,N'#HTML_SCAFFOLD#'
           ,0
           ,'denis.moskvitin@smartcom.software'
           ,NULL
           ,NULL
           ,0
           ,NULL)
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
           (N'Mailing list promo for which could not pick uplift'
           ,'Module.Host.TPM.Handlers.PromoUpliftFailNotificationHandler'
           ,24 * 60 * 60 * 1000 -- 24 часа
           ,'SCHEDULE'
           ,'2019-03-06 05:30:00.3321785 +03:00'
           ,'2019-03-06 05:30:00.3321785 +03:00'
           ,'2019-03-06 05:30:00.3321785 +03:00'
           ,'PROCESSING'
           ,NULL
           ,NULL
           ,NULL
           ,NULL)