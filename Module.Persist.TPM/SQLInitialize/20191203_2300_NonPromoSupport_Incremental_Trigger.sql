
CREATE OR ALTER TRIGGER [NonPromoSupport_Increment_Number] ON [NonPromoSupport] AFTER INSERT AS BEGIN UPDATE NonPromoSupport SET Number = (SELECT ISNULL((SELECT MAX(Number) FROM NonPromoSupport), 0) + 1) FROM Inserted WHERE NonPromoSupport.Id = Inserted.Id; END
GO