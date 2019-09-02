DELETE [dbo].[MailNotificationSetting] WHERE [Name] = 'WEEK_BEFORE_DISPATCH_PROMO_NOTIFICATION'

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
           ,'WEEK_BEFORE_DISPATCH_PROMO_NOTIFICATION'
           ,'Notification of promoes that have a week before dispatch start'
           ,'Promoes that have a week left before dispatch start date'
           ,'#HTML_SCAFFOLD#'
           ,0
           ,'valeriy.maslennikov@smartcom.software'
           ,NULL
           ,NULL
           ,0
           ,NULL)
GO