UPDATE [Promo]
SET [PromoStatusId] = (SELECT [Id] FROM [PromoStatus] WHERE [SystemName] = 'Deleted' AND [Disabled] = 0)
WHERE ([Disabled] = 1)