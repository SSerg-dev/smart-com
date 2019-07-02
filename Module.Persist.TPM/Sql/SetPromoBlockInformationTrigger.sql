CREATE TRIGGER Promo_BlockInformation ON BlockedPromo AFTER INSERT AS
BEGIN
	UPDATE Promo SET BlockInformation = CONCAT(Inserted.HandlerId, '_', Inserted.CreateDate) FROM Inserted WHERE Promo.Id = Inserted.PromoId;
END