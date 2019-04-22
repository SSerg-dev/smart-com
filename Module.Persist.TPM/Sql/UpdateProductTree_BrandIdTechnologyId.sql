UPDATE [dbo].[ProductTree]
SET [BrandId] = CA.Id
FROM ProductTree AS PT CROSS APPLY (SELECT TOP 1 Id, Name FROM Brand B WHERE PT.Name = B.Name) AS CA
WHERE PT.Type = 'Brand'

UPDATE [dbo].[ProductTree]
SET [TechnologyId] = CA.Id
FROM ProductTree AS PT CROSS APPLY (SELECT TOP 1 Id, Name FROM Technology T WHERE PT.Name = T.Name) AS CA
WHERE PT.Type = 'Technology'