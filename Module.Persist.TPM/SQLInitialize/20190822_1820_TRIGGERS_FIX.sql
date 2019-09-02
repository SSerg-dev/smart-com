BEGIN TRANSACTION

BEGIN TRY  
     DROP TRIGGER BaseLine_ChangesIncident_Insert_Update_Trigger;
END TRY  
BEGIN CATCH     
END CATCH

BEGIN TRY  
	 DROP TRIGGER AssortmentMatrix_ChangesIncident_Insert_Update_Trigger;
END TRY  
BEGIN CATCH     
END CATCH

BEGIN TRY  
	 DROP TRIGGER IncrementalPromo_ChangesIncident_Insert_Update_Trigger;
END TRY  
BEGIN CATCH     
END CATCH

GO
	CREATE TRIGGER [dbo].[BaseLine_ChangesIncident_Insert_Update_Trigger]
	ON [dbo].[BaseLine]
	AFTER INSERT, UPDATE
	AS 
		INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled])
		SELECT 'BaseLine', INSERTED.Id, GETDATE(), NULL, NULL, 0 FROM INSERTED

GO
	CREATE TRIGGER AssortmentMatrix_ChangesIncident_Insert_Update_Trigger
	ON [dbo].[AssortmentMatrix]
	AFTER INSERT, UPDATE
	AS
		INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled])
		SELECT 'AssortmentMatrix', INSERTED.Id, GETDATE(), NULL, NULL, 0 FROM INSERTED

GO
	CREATE TRIGGER IncrementalPromo_ChangesIncident_Insert_Update_Trigger
	ON [dbo].[IncrementalPromo]
	AFTER INSERT, UPDATE
	AS 
		INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled])
		SELECT 'IncrementalPromo', INSERTED.Id, GETDATE(), NULL, NULL, 0 FROM INSERTED

GO

IF (@@error <> 0) BEGIN
	print('ROLLBACK :(')
    ROLLBACK
END
ELSE BEGIN
	print('COMMIT :)')
	COMMIT
END