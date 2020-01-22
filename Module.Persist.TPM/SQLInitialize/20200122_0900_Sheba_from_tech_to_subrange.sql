DECLARE @oldObjectId NVARCHAR(255) = (SELECT ObjectId FROM ProductTree WHERE [Name] = 'Mini pouch' AND [Type] = 'Technology' AND [EndDate] IS NULL)

UPDATE ProductTree 
SET EndDate = GETDATE() 
WHERE Name = 'Mini pouch' AND Type = 'Technology' AND EndDate IS NULL

UPDATE [dbo].[PromoProductTree]
SET [ProductTreeObjectId] = (SELECT [ObjectId] FROM [dbo].[ProductTree] WHERE [Name]='Mini pouch' AND [EndDate] IS NULL AND [Type] = 'Subrange')
WHERE ProductTreeObjectId = @oldObjectId
GO
