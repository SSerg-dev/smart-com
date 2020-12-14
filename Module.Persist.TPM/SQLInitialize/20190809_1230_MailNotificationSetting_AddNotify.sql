DELETE [MailNotificationSetting] WHERE [Name] = 'Notification of promoes that have been rejected'

GO

INSERT INTO [MailNotificationSetting]
           ([Id]
           ,[Name]
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
           (NEWID()
           ,'PROMO_ON_REJECT_NOTIFICATION'
           ,'Notification of promoes that have been rejected'
           ,'Promoes that have been rejected'
           ,'#HTML_SCAFFOLD#'
           ,0
           ,'valeriy.maslennikov@smartcom.software'
           ,NULL
           ,NULL
           ,0
           ,NULL)
GO


