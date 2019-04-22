USE [TPM_Dev]
GO

INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_06'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'Sheba > Pouch > Pleasure'))
INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_06'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'SHEBA > Pouch > Appetito'))
INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_06'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'SHEBA > Pouch > Naturelle'))

INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_07'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'Sheba > Pouch > Pleasure'))
INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_07'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'SHEBA > Pouch > Appetito'))
INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_07'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'SHEBA > Pouch > Naturelle'))

INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_08'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'Sheba > Pouch > Pleasure'))
INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_08'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'SHEBA > Pouch > Appetito'))
INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_08'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'SHEBA > Pouch > Naturelle'))

INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_09'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'SHEBA > Pouch > Appetito'))

INSERT INTO [dbo].[PromoProductTree]([Id],[Disabled],[DeletedDate],[PromoId],[ProductTreeObjectId])
VALUES(NEWID(),0,NULL,(SELECT Id FROM Promo WHERE Name = 'TestUpliftPromo_10'),(SELECT TOP 1 ObjectId FROM ProductTree WHERE FullPathName = 'SHEBA > Pouch > Naturelle'))

GO


