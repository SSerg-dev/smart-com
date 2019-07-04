UPDATE [dbo].[Promo]
SET [PromoStatusId] = (SELECT [Id] FROM [dbo].[PromoStatus] WHERE [SystemName] = 'Deleted')
WHERE ([Disabled] = 1)