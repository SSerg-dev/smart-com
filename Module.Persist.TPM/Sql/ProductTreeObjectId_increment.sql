CREATE TRIGGER ProductTreeObjectId_increment ON ProductTree AFTER INSERT AS
BEGIN
	UPDATE ProductTree 
	SET ObjectId = (SELECT ISNULL((SELECT MAX(ObjectId) FROM ProductTree), 1000000) + 1) 
	FROM Inserted 
	WHERE ProductTree.Id = Inserted.Id AND Inserted.ObjectId = 0;
END