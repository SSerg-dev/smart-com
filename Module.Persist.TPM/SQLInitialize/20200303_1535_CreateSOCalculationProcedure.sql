CREATE OR ALTER PROCEDURE [dbo].[SO_Calculation] AS
   BEGIN
		UPDATE BaseLine SET 
			SellOutBaselineQTY = SellInBaselineQTY * cValue
		FROM
			(SELECT b.Id AS baseLineId, c.CoefficientValue AS cValue
			FROM [dbo].[BaseLine] AS b
			JOIN Product AS p ON p.Id = b.ProductId 
			JOIN BrandTech AS bt ON bt.BrandTech_code = p.BrandsegTech_code 
			JOIN ClientTree AS ct ON ct.DemandCode = b.DemandCode 
			JOIN CoefficientSI2SO AS c ON c.BrandTechId = bt.Id		
			WHERE b.Disabled = 0 
				AND c.Disabled = 0
				AND p.Disabled = 0
				AND bt.Disabled = 0
				AND (b.NeedProcessing = 1 OR c.NeedProcessing = 1)
				AND c.DemandCode = ct.DemandCode
				AND ct.EndDate IS NULL) AS t
		WHERE Id = baseLineId;

		--чтобы не создавать лишние инциденты на Baseline
		DISABLE TRIGGER [dbo].[BaseLine_ChangesIncident_Insert_Update_Trigger] ON [dbo].[BaseLine]

		UPDATE BaseLine SET NeedProcessing = 0 WHERE NeedProcessing = 1;
		UPDATE CoefficientSI2SO SET NeedProcessing = 0 WHERE NeedProcessing = 1;

		ENABLE TRIGGER [dbo].[BaseLine_ChangesIncident_Insert_Update_Trigger] ON [dbo].[BaseLine]
   END