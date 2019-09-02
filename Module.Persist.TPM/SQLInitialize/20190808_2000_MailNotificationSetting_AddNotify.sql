DELETE [dbo].[MailNotificationSetting] WHERE [Name] = 'PROMO_APPROVED_NOTIFICATION'

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
           ,'PROMO_APPROVED_NOTIFICATION'
           ,'Notification of promoes that have been approved'
           ,'Promoes that have been approved'
           ,'#HTML_SCAFFOLD#'
           ,0
           ,'valeriy.maslennikov@smartcom.software'
           ,NULL
           ,NULL
           ,0
           ,NULL)
GO


