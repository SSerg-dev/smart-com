 
UPDATE [Mechanic]
   SET  [PromoTypesId] = (select Id from PromoTypes where SystemName = 'Regular') 
GO

INSERT INTO [Mechanic]
           ([Id]
           ,[Disabled]
           ,[DeletedDate]
           ,[Name]
           ,[SystemName]
           ,[PromoTypesId])
     VALUES
           ( NEWID(), 0, null, 'TPR','TPR',(select Id from PromoTypes where SystemName = 'InOut') ),
		        ( NEWID(), 0, null, 'Other','Other',(select Id from PromoTypes where SystemName = 'InOut') ),
				     ( NEWID(), 0, null, 'VP','VP',(select Id from PromoTypes where SystemName = 'InOut') )
					 
GO
INSERT INTO [Mechanic]
           ([Id]
           ,[Disabled]
           ,[DeletedDate]
           ,[Name]
           ,[SystemName]
           ,[PromoTypesId])
     VALUES
           ( NEWID(), 0, null, 'Points ','Points',(select Id from PromoTypes where SystemName = 'Loyalty') ),
		        ( NEWID(), 0, null, 'Coupons','Coupons',(select Id from PromoTypes where SystemName = 'Loyalty') ),
				     ( NEWID(), 0, null, 'Programs ','Programs',(select Id from PromoTypes where SystemName = 'Loyalty') )
					 
GO
UPDATE [Promo]
   SET [MarsMechanicId] = (select id from Mechanic where  SystemName = (select SystemName from Mechanic where Id = MarsMechanicId) and PromoTypesId = (select Id from PromoTypes where SystemName = 'InOut'))
       
     
      
 WHERE InOut = 1
GO

 UPDATE [Promo]
   SET [PlanInstoreMechanicId] = (select id from Mechanic where  SystemName = (select SystemName from Mechanic where Id = PlanInstoreMechanicId) and PromoTypesId = (select Id from PromoTypes where SystemName = 'InOut'))
       
     
      
 WHERE InOut = 1
GO
 
 UPDATE [Promo]
   SET [ActualInStoreMechanicId] = (select id from Mechanic where  SystemName = (select SystemName from Mechanic where Id = ActualInStoreMechanicId) and PromoTypesId = (select Id from PromoTypes where SystemName = 'InOut'))
       
     
      
 WHERE InOut = 1
GO