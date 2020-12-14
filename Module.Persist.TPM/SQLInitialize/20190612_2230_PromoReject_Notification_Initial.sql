INSERT INTO [Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'PROMO_REJECT_NOTIFICATION_TEMPLATE_FILE'
           ,N'String'
           ,N'PromoRejectTemplate.txt'
           ,N'File Reject Notifications template')
GO

INSERT INTO [MailNotificationSetting]
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
		   (N'REJECT_PROMO_NOTIFICATION'
           ,N'Notification of the rejecting promo'
           ,N'Promoes have been rejected'
           ,N'#HTML_SCAFFOLD#'
           ,0
           ,'andrey.filyushkin@smartcom.software'
           ,NULL
           ,NULL
           ,0
           ,NULL)
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
		   ,N'Mailing Reject promo notifications'
           ,'Module.Host.TPM.Handlers.RejectPromoNotificationHandler'
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