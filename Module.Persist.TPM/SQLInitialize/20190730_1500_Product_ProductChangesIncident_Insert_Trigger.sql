CREATE TRIGGER [Product_ProductChangesIncident_Insert_Trigger]
ON [Product]
AFTER INSERT
AS INSERT INTO ProductChangesIncident ([ProductId], [CreateDate], [IsCreate], [IsDelete], [NotificationProcessDate], [RecalculationProcessDate]) 
VALUES ((SELECT Id FROM INSERTED), GETDATE(), 1, 0, NULL, NULL)
GO
