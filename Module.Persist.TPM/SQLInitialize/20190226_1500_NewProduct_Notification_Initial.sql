INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'PROMO_PRODUCT_CHANGE_NOTIFICATION_TEMPLATE_FILE'
           ,N'String'
           ,N'PromoProductChangeTemplate.txt'
           ,N'Promo Product Change Notifications template')
GO

INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'PRODUCT_CHANGE_PERIOD_DAYS'
           ,N'Int'
           ,N'56'
           ,N'If before the start of the promo there are fewer days than the specified number, you need to include it in the Product Change notification.')
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
           (N'PROMO_PRODUCT_CREATE_NOTIFICATION'
           ,N'Notification of the addition of a new product suitable for existing promo'
           ,N'New products relevant for the following promotions have been created'
           ,N'#HTML_SCAFFOLD#'
           ,0
           ,'denis.moskvitin@smartcom.software'
           ,NULL
           ,NULL
           ,0
           ,NULL)
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
           (N'PROMO_PRODUCT_DELETE_NOTIFICATION'
           ,N'Notification of the removal of the product involved in the promo'
           ,N'Products used in the following promotions have been deleted'
           ,N'#HTML_SCAFFOLD#'
           ,0
           ,'denis.moskvitin@smartcom.software'
           ,NULL
           ,NULL
           ,0
           ,NULL)
GO