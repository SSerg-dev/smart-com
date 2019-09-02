DELETE [dbo].[Setting] WHERE [Name] = 'WEEK_BEFORE_DISPATCH_PROMO_NOTIFICATION_TEMPLATE'

GO

INSERT INTO [dbo].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'WEEK_BEFORE_DISPATCH_PROMO_NOTIFICATION_TEMPLATE'
           ,'String'
           ,'WeekBeforeDispatchPromoTemplate.txt'
           ,'Week before dispatch strart promoes notifications template')
GO