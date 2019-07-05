UPDATE [dbo].[Promo]
SET [PromoStatusId] = (SELECT [Id] FROM [dbo].[PromoStatus] WHERE [SystemName] = 'Deleted' AND [Disabled] = 0)
WHERE ([Disabled] = 1)