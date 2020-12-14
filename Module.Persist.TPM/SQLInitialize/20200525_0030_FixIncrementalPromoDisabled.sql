DISABLE TRIGGER [IncrementalPromo_ChangesIncident_Insert_Update_Trigger] ON [IncrementalPromo]

UPDATE IncrementalPromo SET
	Disabled = 1,
	DeletedDate = t.promoDeletedDate
FROM 
	(SELECT ipr.Id AS iprId,
			p.Disabled AS promoDisabled,
			p.DeletedDate AS promoDeletedDate,
			ipr.Disabled AS iprDisabled
	FROM IncrementalPromo AS ipr
	JOIN Promo AS p ON p.Id = ipr.PromoId) 
	AS t
WHERE Id = t.iprId AND promoDisabled = 1 AND iprDisabled <> 1;

ENABLE TRIGGER [IncrementalPromo_ChangesIncident_Insert_Update_Trigger] ON [IncrementalPromo]
