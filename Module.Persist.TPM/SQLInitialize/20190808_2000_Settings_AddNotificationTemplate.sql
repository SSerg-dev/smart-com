DELETE [dbo].[Setting] WHERE [Name] = 'PROMO_APPROVED_NOTIFICATION_TEMPLATE_FILE'

GO

INSERT INTO [dbo].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'PROMO_APPROVED_NOTIFICATION_TEMPLATE_FILE'
           ,'String'
           ,'PromoApprovedTemplate.txt'
           ,'Approved notifications template')
GO


