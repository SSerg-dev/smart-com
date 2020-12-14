CREATE TRIGGER ProductTree_ChangesIncident_Update_Trigger
ON [ProductTree]
INSTEAD OF UPDATE
AS BEGIN
	IF (
		(SELECT Filter FROM INSERTED) <> (SELECT Filter FROM [ProductTree] WHERE Id = (SELECT Id FROM INSERTED))
	)
	BEGIN
		INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled]) 
		VALUES ('ProductTree', (SELECT Id FROM INSERTED), GETDATE(), NULL, NULL, 0)
	END
END
