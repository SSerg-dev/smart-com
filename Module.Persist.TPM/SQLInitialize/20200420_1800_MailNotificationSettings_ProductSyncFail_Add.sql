DELETE [dbo].[MailNotificationSetting] WHERE [Name] = 'PRODUCT_SYNC_FAIL_NOTIFICATION'

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
           ,'PRODUCT_SYNC_FAIL_NOTIFICATION'
           ,'Notification with ZREPs that failed to sync with Products'
           ,'ZREPs that failed to sync with products'
           ,'#HTML_SCAFFOLD#'
           ,0
           ,NULL
           ,NULL
           ,NULL
           ,0
           ,NULL)
GO


