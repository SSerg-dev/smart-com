DELETE [dbo].[MailNotificationSetting] WHERE [Name] = 'PROMO_ON_APPROVAL_NOTIFICATION'

GO

INSERT INTO [dbo].[MailNotificationSetting]
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
           ,'PROMO_ON_APPROVAL_NOTIFICATION'
           ,'Notification of promoes that need approval'
           ,'Promoes that need approval'
           ,'#HTML_SCAFFOLD#'
           ,0
           ,'valeriy.maslennikov@smartcom.software'
           ,NULL
           ,NULL
           ,0
           ,NULL)
GO