--INSERT INTO ClientTree ([Depth], [Type], [Name], [StartDate], [parentId] ,[FullPathName]) 
--	VALUES (2, 'Group Chain', 'Big Boxes', '2019-02-28 18:00:00.000', (SELECT ObjectId FROM ClientTree WHERE Name = 'X5 Retail Group' AND EndDate IS NULL), 'NA > X5 Retail Group > Big Boxes');

--UPDATE ClientTree SET ParentId = (SELECT ObjectId FROM ClientTree WHERE Name = 'Big Boxes' AND EndDate IS NULL) WHERE Name = 'Perekrestok' OR Name = 'Karusel';