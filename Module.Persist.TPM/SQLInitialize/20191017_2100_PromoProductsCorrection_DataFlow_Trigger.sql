	CREATE TRIGGER PromoProductsCorrection_ChangesIncident_Insert_Update_Trigger
	ON [dbo].[PromoProductsCorrection]
	AFTER INSERT, UPDATE
	AS 
		INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled])
		SELECT 'PromoProductsCorrection', INSERTED.Id, GETDATE(), NULL, NULL, 0 FROM INSERTED