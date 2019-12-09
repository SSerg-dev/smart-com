 
UPDATE [dbo].[Promo]
   SET  
      PromoTypesId = (select Id from PromoTypes where Name = 'Regular Promo')
 WHERE InOut = 0
GO

UPDATE [dbo].[Promo]
   SET  
      PromoTypesId = (select Id from PromoTypes where Name = 'InOut Promo')
 WHERE InOut = 1
GO
