CREATE TRIGGER ClientTreeObjectId_increment ON ClientTree AFTER INSERT AS
BEGIN
	UPDATE ClientTree 
	SET ObjectId = (SELECT ISNULL((SELECT MAX(ObjectId) FROM ClientTree), 5000000) + 1) 
	FROM Inserted 
	WHERE ClientTree.Id = Inserted.Id AND Inserted.ObjectId = 0;
END