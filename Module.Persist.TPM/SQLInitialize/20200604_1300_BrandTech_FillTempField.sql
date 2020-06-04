UPDATE [dbo].[BrandTech]
   SET 
      [TempSub_code] = '-02'
 WHERE Name = 'Whiskas Pouch Premium' AND Disabled = 0
GO
UPDATE [dbo].[BrandTech]
   SET 
      [TempSub_code] = '-01'
 WHERE Name = 'Whiskas Pouch' AND Disabled = 0
GO
