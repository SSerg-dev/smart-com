UPDATE p SET p.IsOnInvoice = ct.IsOnInvoice
FROM Promo p
INNER JOIN ClientTree ct ON p.ClientTreeId = ct.ObjectId WHERE ct.IsBaseClient = 1;