ALTER VIEW [ClientTreeSharesView] AS
WITH CTE(Id, ObjectId, Name, parentId, IsBaseClient, LevelS, BOI, EndDate, StartDate, ResultNameStr, depth, DemandCode) 
AS (SELECT Id, ObjectId, Name, parentId, IsBaseClient, 0 AS LevelS, ObjectId AS BOI, EndDate, StartDate, Name AS ResultNameStr, depth, DemandCode
FROM dbo.ClientTree
WHERE (EndDate IS NULL) OR (EndDate > GETDATE())
UNION ALL
SELECT t1.Id, t2.ObjectId, t2.Name, t2.parentId, t2.IsBaseClient, t1.LevelS + 1 AS Expr1, t1.BOI, t2.EndDate, t2.StartDate, t2.Name + ' > ' + t1.ResultNameStr AS Expr2, t2.depth, t1.DemandCode
FROM dbo.ClientTree AS t2 INNER JOIN
    CTE AS t1 ON t1.parentId = t2.ObjectId AND (t2.EndDate IS NULL OR t2.EndDate > GETDATE()) AND t2.StartDate < GETDATE()
WHERE (t2.ObjectId <> t2.parentId))
SELECT Id, BOI, ResultNameStr, DemandCode
FROM CTE
WHERE ((CAST(BOI AS varchar(40)) + '_' + CAST(LevelS AS varchar)) IN
    (SELECT CAST(BOI AS varchar(40)) + '_' + CAST(MAX(LevelS) AS varchar) AS Expr1
    FROM CTE
    GROUP BY BOI))