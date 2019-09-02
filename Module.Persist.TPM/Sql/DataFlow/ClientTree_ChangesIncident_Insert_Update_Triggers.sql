CREATE TRIGGER ClientTree_ChangesIncident_Update_Trigger
ON [dbo].[ClientTree]
INSTEAD OF UPDATE
AS BEGIN
	IF (
		(SELECT Share FROM INSERTED) <> (SELECT Share FROM [dbo].[ClientTree] WHERE Id = (SELECT Id FROM INSERTED))
	)
	BEGIN
		INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled]) 
		VALUES ('ClientTree', (SELECT Id FROM INSERTED), GETDATE(), NULL, NULL, 0)
	END
END
