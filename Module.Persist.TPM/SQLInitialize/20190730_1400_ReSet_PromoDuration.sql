UPDATE 
	Promo
SET
	PromoDuration = DATEDIFF(DAY, StartDate, EndDate) + 1,
	DispatchDuration = DATEDIFF(DAY, DispatchesStart, DispatchesEnd) + 1
WHERE
	StartDate IS NOT NULL 
AND
	EndDate IS NOT NULL
AND
	DispatchesStart IS NOT NULL
AND
	DispatchesEnd IS NOT NULL;