
DELETE FROM [dbo].[PromoTypes]
GO

INSERT INTO [dbo].[PromoTypes]
           ([Disabled]
           ,[Name]
           ,[Glyph])
     VALUES
           ( 0,'Regular Promo','FBFA'),
		    ( 0,'InOut Promo','FAC2'),
			 ( 0,'Loyalty Promo','F46F'),
			  ( 0,'Dynamic Promo','F6E0')
GO




