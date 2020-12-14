DROP TRIGGER IF EXISTS [ClientTreeBrandTech_ChangesIncident_Insert_Update_Trigger]
GO

CREATE TRIGGER [ClientTreeBrandTech_ChangesIncident_Insert_Update_Trigger]
ON [ClientTreeBrandTech]
AFTER INSERT, UPDATE
AS 
	INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled])
	SELECT 'ClientTreeBrandTech', INSERTED.Id, GETDATE(), NULL, NULL, 0 FROM INSERTED

GO
