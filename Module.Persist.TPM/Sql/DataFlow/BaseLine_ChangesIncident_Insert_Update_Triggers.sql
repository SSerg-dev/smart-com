CREATE TRIGGER BaseLine_ChangesIncident_Insert_Update_Trigger
ON [dbo].[BaseLine]
AFTER INSERT, UPDATE
AS INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled]) 
VALUES ('BaseLine', (SELECT Id FROM INSERTED), GETDATE(), NULL, NULL, 0)
