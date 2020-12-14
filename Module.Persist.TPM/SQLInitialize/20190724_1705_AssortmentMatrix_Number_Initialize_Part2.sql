CREATE TRIGGER AssortmentMatrix_ChangesIncident_Insert_Update_Trigger
ON [AssortmentMatrix]
AFTER INSERT, UPDATE
AS INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled]) 
VALUES ('AssortmentMatrix', (SELECT Id FROM INSERTED), GETDATE(), NULL, NULL, 0)