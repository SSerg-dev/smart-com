DISABLE TRIGGER [IncrementalPromo_ChangesIncident_Insert_Update_Trigger] ON [IncrementalPromo]

UPDATE IncrementalPromo SET
	CasePrice = t.FoundPrice
FROM 
	(SELECT ipr.Id AS iprId,
	(SELECT TOP(1) Price FROM PriceList AS pl 
	WHERE pl.StartDate <= p.DispatchesStart 
		AND pl.EndDate >= p.DispatchesStart 
		AND pl.ClientTreeId = p.ClientTreeKeyId 
		AND pl.ProductId = ipr.ProductId
	ORDER BY pl.StartDate DESC) AS FoundPrice
	FROM IncrementalPromo AS ipr
	JOIN Promo AS p ON p.Id = ipr.PromoId) 
	AS t
WHERE Id = t.iprId AND t.FoundPrice IS NOT NULL;

UPDATE IncrementalPromo SET 
	PlanPromoIncrementalLSV = PlanPromoIncrementalCases * CasePrice
WHERE PlanPromoIncrementalCases IS NOT NULL AND PlanPromoIncrementalCases <> 0 AND
	  CasePrice IS NOT NULL AND CasePrice <> 0;

ENABLE TRIGGER [IncrementalPromo_ChangesIncident_Insert_Update_Trigger] ON [IncrementalPromo]
