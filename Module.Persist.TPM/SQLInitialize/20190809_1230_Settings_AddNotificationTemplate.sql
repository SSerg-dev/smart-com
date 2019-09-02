DELETE [dbo].[Setting] WHERE [Name] = 'PROMO_ON_REJECT_NOTIFICATION_TEMPLATE_FILE'

GO

INSERT INTO [dbo].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'PROMO_ON_REJECT_NOTIFICATION_TEMPLATE_FILE'
           ,'String'
           ,'PromoOnRejectTemplate.txt'
           ,'On Reject notifications template')
GO


