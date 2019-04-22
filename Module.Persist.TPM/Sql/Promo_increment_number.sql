CREATE TRIGGER Promo_increment_number ON Promo AFTER INSERT AS
BEGIN
	UPDATE Promo SET Number = (SELECT ISNULL((SELECT MAX(Number) FROM Promo), 0) + 1) FROM Inserted WHERE Promo.Id = Inserted.Id;
END