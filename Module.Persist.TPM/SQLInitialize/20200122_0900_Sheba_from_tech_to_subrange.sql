DECLARE @oldObjectId NVARCHAR(255) = (SELECT ObjectId FROM ProductTree WHERE [Name] = 'Mini pouch' AND [Type] = 'Technology' AND [EndDate] IS NULL)

UPDATE ProductTree 
SET EndDate = GETDATE() 
WHERE Name = 'Mini pouch' AND Type = 'Technology' AND EndDate IS NULL

IF (SELECT COUNT([ObjectId]) FROM [ProductTree] WHERE [Name]='Mini pouch' AND [EndDate] IS NULL AND [Type] = 'Subrange') > 0
	BEGIN
		UPDATE [PromoProductTree]
		SET [ProductTreeObjectId] = (SELECT [ObjectId] FROM [ProductTree] WHERE [Name]='Mini pouch' AND [EndDate] IS NULL AND [Type] = 'Subrange')
		WHERE ProductTreeObjectId = @oldObjectId
	END
ELSE
	BEGIN
		INSERT INTO [ProductTree]
		           ([depth]
		           ,[Name]
		           ,[StartDate]
		           ,[EndDate]
		           ,[Type]
		           ,[Abbreviation]
		           ,[Filter]
		           ,[parentId]
		           ,[FullPathName])
		     VALUES
		           (3
		           ,'Mini pouch'
		           ,GETDATE()
		           ,NULL
		           ,'Subrange'
		           ,NULL
		           ,'{
		    "and": [
		        {
		            "BrandFlag": {
		                "eq": "Sheba"
		            }
		        },
		        {
		            "Size": {
		                "eq": "50g"
		            }
		        }
		    ]
		}'
		           ,1000068
		           ,'Sheba > Pouch > Mini pouch')

		UPDATE [PromoProductTree]
		SET [ProductTreeObjectId] = (SELECT [ObjectId] FROM [ProductTree] WHERE [Name]='Mini pouch' AND [EndDate] IS NULL AND [Type] = 'Subrange')
		WHERE ProductTreeObjectId = @oldObjectId
	END
GO