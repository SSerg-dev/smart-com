INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'PROMO_CANCELLED_NOTIFICATION_TEMPLATE_FILE'
           ,N'String'
           ,N'PromoCancelledTemplate.txt'
           ,N'File Cancelled Notifications template')
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
		   (N'CANCELLED_PROMO_NOTIFICATION'
           ,N'Notification of the cancelling promo'
           ,N'Promoes have been cancelled'
           ,N'#HTML_SCAFFOLD#'
           ,0
           ,'andrey.filyushkin@smartcom.software'
           ,NULL
           ,NULL
           ,0
           ,NULL)
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
		   ,N'Mailing Cancelled promo notifications'
           ,'Module.Host.TPM.Handlers.CancelledPromoNotificationHandler'
           ,24 * 60 * 60 * 1000 
           ,'SCHEDULE'
           ,'2019-06-12 22:30:00.3321785 +03:00'
           ,NULL
           ,NULL
           ,'PROCESSING'
           ,NULL
           ,NULL
           ,NULL
           ,NULL)
GO