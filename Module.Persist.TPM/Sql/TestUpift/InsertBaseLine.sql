USE [TPM_Dev]
GO
 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-10 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,685 ,883 ,604855 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-001'))
 
 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-17 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,25 ,883 ,22075 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-001')) 
 
 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-24 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,4 ,883 ,3532 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-001'))  
 
  INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-10 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,684 ,455 ,311220 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-002'))
 
 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-17 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,96 ,455 ,43680 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-002')) 
 
 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-24 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,120 ,455 ,54600 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-002'))  

 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-10 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,98 ,136 ,13328 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-003'))
 
 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-17 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,56 ,136 ,7616 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-003')) 
 
 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-24 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,88 ,136 ,11968 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-003'))  

 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-10 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,956 ,229 ,218924 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-004'))
 
 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-17 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,158 ,229 ,36182 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-004')) 
 
 INSERT INTO [dbo].[BaseLine] ([Id] ,[Disabled] ,[DeletedDate] ,[StartDate] ,[ClientTreeId] ,[QTY] ,[Price] ,[BaselineLSV] ,[Type] ,[ProductId])      
 VALUES (NEWID() ,0 ,NULL ,'2019-02-24 00:00:00.0000000 +03:00' ,(SELECT Id FROM ClientTree WHERE DemandCode = '0145-563') ,33 ,229 ,7557 ,4 ,(SELECT Id FROM Product WHERE ZREP = '100-004'))  
 GO   