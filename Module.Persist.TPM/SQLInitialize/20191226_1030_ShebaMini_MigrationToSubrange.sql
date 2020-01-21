UPDATE ProductTree 
SET EndDate = GETDATE() 
WHERE Name = 'Mini pouch' AND Type = 'Technology' AND EndDate IS NULL
GO

INSERT INTO [dbo].[ProductTree]
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
GO