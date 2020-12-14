CREATE OR ALTER PROCEDURE [SI_Calculation] AS
   BEGIN
       UPDATE BaseLine SET 
			SellInBaselineQTY = t.SellInQty
		FROM
			(SELECT b.Id AS baseLineId, [CountAverage](b.StartDate, b.ProductId, b.DemandCode) AS SellInQty
			FROM [BaseLine] AS b
			WHERE b.NeedProcessing = 1 AND b.Disabled = 0 AND YEAR(b.StartDate) <> 9999) AS t
		WHERE Id = baseLineId
   END