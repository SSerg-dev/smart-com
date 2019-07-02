UPDATE Promo 
SET CreatorId=(SELECT Id FROM [User] WHERE Name = 'mars-ad\ragozolg') 
WHERE CreatorId = (SELECT Id FROM [User] WHERE Name = 'mars-ad\volovmik') 
AND PromoStatusId = (SELECT Id FROM PromoStatus WHERE SystemName = 'Draft')