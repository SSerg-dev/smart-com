UPDATE Promo SET ActualPromoLSVSI = ActualPromoLSVByCompensation
WHERE Disabled = 0 AND ActualPromoLSVSI IS NULL
GO

UPDATE Promo SET ActualPromoLSVSO = ActualPromoLSV
WHERE Disabled = 0 AND ActualPromoLSVSO IS NULL
GO

UPDATE Promo SET ActualPromoLSV = ActualPromoLSVSI
WHERE Disabled = 0 AND IsOnInvoice = 1 AND ActualPromoLSVSI IS NOT NULL
GO