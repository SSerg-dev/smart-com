 
UPDATE [dbo].[PromoTypes]
   SET  
      [SystemName] = 'Regular'
 WHERE Name = 'Regular Promo'
GO


UPDATE [dbo].[PromoTypes]
   SET  
      [SystemName] = 'InOut'
 WHERE Name = 'InOut Promo'
GO


UPDATE [dbo].[PromoTypes]
   SET  
      [SystemName] = 'Loyalty'
 WHERE Name = 'Loyalty Promo'
GO


UPDATE [dbo].[PromoTypes]
   SET  
      [SystemName] = 'Dynamic'
 WHERE Name = 'Dynamic Promo'
GO


