UPDATE [Promo]
   SET  
       [Name] = CONCAT(SUBSTRING (Name, 0 ,CHARINDEX('VP ',Name)),'TPR',' ',MarsMechanicDiscount,'%')
      ,[MarsMechanicId] = (SELECT Id FROM Mechanic  WHERE Name = 'TPR' AND PromoTypesId = [Promo].[PromoTypesId]) 
      ,[Mechanic] = CONCAT('TPR ', MarsMechanicDiscount,'%')  
   WHERE Mechanic LIKE('VP%')  AND Mechanic NOT LIKE('%+%')


