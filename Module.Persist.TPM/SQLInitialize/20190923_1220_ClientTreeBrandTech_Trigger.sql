DROP TRIGGER IF EXISTS [dbo].[ClientTreeBrandTech_ChangesIncident_Insert_Update_Trigger]
GO

CREATE TRIGGER [dbo].[ClientTreeBrandTech_ChangesIncident_Insert_Update_Trigger]
ON [dbo].[ClientTreeBrandTech]
AFTER INSERT, UPDATE
AS 
	INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled])
	SELECT 'ClientTreeBrandTech', INSERTED.Id, GETDATE(), NULL, NULL, 0 FROM INSERTED

GO
