CREATE TRIGGER AssortmentMatrix_increment_number ON AssortmentMatrix AFTER INSERT AS
BEGIN
	UPDATE AssortmentMatrix SET Number = (SELECT ISNULL((SELECT MAX(Number) FROM AssortmentMatrix), 0) + 1) FROM Inserted WHERE AssortmentMatrix.Id = Inserted.Id;
END