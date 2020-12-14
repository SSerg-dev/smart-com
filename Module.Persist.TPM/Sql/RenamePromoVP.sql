UPDATE [Promo]
	SET  
	  [Name] = CONCAT(SUBSTRING (Name, 0 ,CHARINDEX('VP ',Name)),'VP',' ',(SELECT Name FROM MechanicType WHERE Id = MarsMechanicTypeId))
	
	WHERE Name LIKE '%VP%|%' ESCAPE '|'   
GO


