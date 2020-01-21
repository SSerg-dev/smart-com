UPDATE [dbo].[PromoProductTree]
   SET [ProductTreeObjectId] = (SELECT [ObjectId] FROM [dbo].[ProductTree] WHERE [Name]='Mini pouch' AND [EndDate] IS NULL AND [Type] = 'Subrange')
 WHERE ProductTreeObjectId = (SELECT [ObjectId] FROM [dbo].[ProductTree] WHERE [Name]='Mini pouch' AND [EndDate] IS NOT NULL AND [Type] = 'Technology')
GO

UPDATE [Promo]
SET [ProductHierarchy] = 'Sheba > Pouch > Mini pouch' 
WHERE Id IN (SELECT [PromoId]
  FROM [dbo].[PromoProductTree]
  WHERE ProductTreeObjectId = (SELECT [ObjectId] FROM [dbo].[ProductTree] WHERE [Name]='Mini pouch' AND [EndDate] IS NULL AND [Type] = 'Subrange'))
GO