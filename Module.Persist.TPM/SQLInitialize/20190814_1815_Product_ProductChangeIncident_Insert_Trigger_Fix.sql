ALTER TRIGGER [Product_ProductChangesIncident_Insert_Trigger]
ON [Product]
AFTER INSERT
AS INSERT INTO ProductChangeIncident ([ProductId], [CreateDate], [IsCreate], [IsDelete], [NotificationProcessDate], [RecalculationProcessDate]) 
VALUES ((SELECT Id FROM INSERTED), GETDATE(), 1, 0, NULL, NULL)
