DELETE [Setting] WHERE [Name] = 'PROMO_ON_APPROVAL_NOTIFICATION_TEMPLATE_FILE'

GO

INSERT INTO [Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'PROMO_ON_APPROVAL_NOTIFICATION_TEMPLATE_FILE'
           ,'String'
           ,'PromoOnApprovalTemplate.txt'
           ,'On Approval notifications template')
GO