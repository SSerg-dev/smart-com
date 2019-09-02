BEGIN TRANSACTION

GO
DISABLE TRIGGER AssortmentMatrix_ChangesIncident_Insert_Update_Trigger ON AssortmentMatrix
GO
DISABLE TRIGGER AssortmentMatrix_increment_number ON AssortmentMatrix

UPDATE AssortmentMatrix 
SET
	[StartDate] = DATETIMEOFFSETFROMPARTS (YEAR([StartDate]), MONTH([StartDate]), DAY([StartDate]), 0, 0, 0, 0, 3, 0, 7),
	[EndDate] = DATETIMEOFFSETFROMPARTS (YEAR([EndDate]), MONTH([EndDate]), DAY([EndDate]), 0, 0, 0, 0, 3, 0, 7)
WHERE
	[StartDate] IS NOT NULL AND [EndDate] IS NOT NULL;

GO
ENABLE TRIGGER AssortmentMatrix_ChangesIncident_Insert_Update_Trigger ON AssortmentMatrix
GO
ENABLE TRIGGER AssortmentMatrix_increment_number ON AssortmentMatrix

GO
DISABLE TRIGGER BaseLine_ChangesIncident_Insert_Update_Trigger ON BaseLine


UPDATE BaseLine
SET
	[StartDate] = DATETIMEOFFSETFROMPARTS (YEAR([StartDate]), MONTH([StartDate]), DAY([StartDate]), 0, 0, 0, 0, 3, 0, 7)
WHERE
	[StartDate] IS NOT NULL;

GO
ENABLE TRIGGER BaseLine_ChangesIncident_Insert_Update_Trigger ON BaseLine


UPDATE COGS
SET
	[StartDate] = DATETIMEOFFSETFROMPARTS (YEAR([StartDate]), MONTH([StartDate]), DAY([StartDate]), 0, 0, 0, 0, 3, 0, 7),
	[EndDate] = DATETIMEOFFSETFROMPARTS (YEAR([EndDate]), MONTH([EndDate]), DAY([EndDate]), 0, 0, 0, 0, 3, 0, 7)
WHERE
	[StartDate] IS NOT NULL AND [EndDate] IS NOT NULL;

DROP TRIGGER nonenego_UpdateInsertTrigger;
GO

CREATE TRIGGER nonenego_UpdateInsertTrigger ON NoneNego
AFTER INSERT, UPDATE
AS
	UPDATE [dbo].[NoneNego]
	SET [ToDate] = '9999-12-28T23:59:59.999+00:00'
	WHERE Id IN (SELECT Id FROM inserted) AND ToDate IS NULL
GO

UPDATE NoneNego
SET
	[FromDate] = DATETIMEOFFSETFROMPARTS (YEAR([FromDate]), MONTH([FromDate]), DAY([FromDate]), 0, 0, 0, 0, 3, 0, 7),
	[ToDate] = DATETIMEOFFSETFROMPARTS (YEAR([ToDate]), MONTH([ToDate]), DAY([ToDate]), 0, 0, 0, 0, 3, 0, 7),
	[CreateDate] = DATETIMEOFFSETFROMPARTS (YEAR([CreateDate]), MONTH([CreateDate]), DAY([CreateDate]), 0, 0, 0, 0, 3, 0, 7)
WHERE
	[FromDate] IS NOT NULL AND
	[ToDate] IS NOT NULL AND YEAR([ToDate]) < 9999 AND
	[CreateDate] IS NOT NULL;


UPDATE Promo 
SET
	[StartDate] = DATETIMEOFFSETFROMPARTS (YEAR([StartDate]), MONTH([StartDate]), DAY([StartDate]), 0, 0, 0, 0, 3, 0, 7),
	[EndDate] = DATETIMEOFFSETFROMPARTS (YEAR([EndDate]), MONTH([EndDate]), DAY([EndDate]), 0, 0, 0, 0, 3, 0, 7),
	[DispatchesStart] = DATETIMEOFFSETFROMPARTS (YEAR([DispatchesStart]), MONTH([DispatchesStart]), DAY([DispatchesStart]), 0, 0, 0, 0, 3, 0, 7),
	[DispatchesEnd] = DATETIMEOFFSETFROMPARTS (YEAR([DispatchesEnd]), MONTH([DispatchesEnd]), DAY([DispatchesEnd]), 0, 0, 0, 0, 3, 0, 7)
WHERE
	[StartDate] IS NOT NULL AND [EndDate] IS NOT NULL AND [DispatchesStart] IS NOT NULL AND [DispatchesEnd] IS NOT NULL;

UPDATE PromoDemandChangeIncident
SET
	[OldStartDate] = DATETIMEOFFSETFROMPARTS (YEAR([OldStartDate]), MONTH([OldStartDate]), DAY([OldStartDate]), 0, 0, 0, 0, 3, 0, 7),
	[NewStartDate] = DATETIMEOFFSETFROMPARTS (YEAR([NewStartDate]), MONTH([NewStartDate]), DAY([NewStartDate]), 0, 0, 0, 0, 3, 0, 7),
	[OldEndDate] = DATETIMEOFFSETFROMPARTS (YEAR([OldEndDate]), MONTH([OldEndDate]), DAY([OldEndDate]), 0, 0, 0, 0, 3, 0, 7),
	[NewEndDate] = DATETIMEOFFSETFROMPARTS (YEAR([NewEndDate]), MONTH([NewEndDate]), DAY([NewEndDate]), 0, 0, 0, 0, 3, 0, 7),
	[OldDispatchesStart] = DATETIMEOFFSETFROMPARTS (YEAR([OldDispatchesStart]), MONTH([OldDispatchesStart]), DAY([OldDispatchesStart]), 0, 0, 0, 0, 3, 0, 7),
	[NewDispatchesStart] = DATETIMEOFFSETFROMPARTS (YEAR([NewDispatchesStart]), MONTH([NewDispatchesStart]), DAY([NewDispatchesStart]), 0, 0, 0, 0, 3, 0, 7),
	[OldDispatchesEnd] = DATETIMEOFFSETFROMPARTS (YEAR([OldDispatchesEnd]), MONTH([OldDispatchesEnd]), DAY([OldDispatchesEnd]), 0, 0, 0, 0, 3, 0, 7),
	[NewDispatchesEnd] = DATETIMEOFFSETFROMPARTS (YEAR([NewDispatchesEnd]), MONTH([NewDispatchesEnd]), DAY([NewDispatchesEnd]), 0, 0, 0, 0, 3, 0, 7)
WHERE
	[OldStartDate] IS NOT NULL AND
	[NewStartDate] IS NOT NULL AND
	[OldEndDate] IS NOT NULL AND
	[NewEndDate] IS NOT NULL AND
	[OldDispatchesStart] IS NOT NULL AND
	[NewDispatchesStart] IS NOT NULL AND
	[OldDispatchesEnd] IS NOT NULL AND
	[NewDispatchesEnd] IS NOT NULL;


UPDATE PromoSupport
SET
	[StartDate] = DATETIMEOFFSETFROMPARTS (YEAR([StartDate]), MONTH([StartDate]), DAY([StartDate]), 0, 0, 0, 0, 3, 0, 7),
	[EndDate] = DATETIMEOFFSETFROMPARTS (YEAR([EndDate]), MONTH([EndDate]), DAY([EndDate]), 0, 0, 0, 0, 3, 0, 7)
WHERE
	[StartDate] IS NOT NULL AND [EndDate] IS NOT NULL;


UPDATE TradeInvestment 
SET
	[StartDate] = DATETIMEOFFSETFROMPARTS (YEAR([StartDate]), MONTH([StartDate]), DAY([StartDate]), 0, 0, 0, 0, 3, 0, 7),
	[EndDate] = DATETIMEOFFSETFROMPARTS (YEAR([EndDate]), MONTH([EndDate]), DAY([EndDate]), 0, 0, 0, 0, 3, 0, 7)
WHERE
	[StartDate] IS NOT NULL AND [EndDate] IS NOT NULL;


Drop View PromoView
GO

CREATE VIEW PromoView AS SELECT 
pr.[Id],
pr.[Name],
mmc.[Name] as MarsMechanicName,
mmt.[Name] as MarsMechanicTypeName,
pr.[MarsMechanicDiscount],
cl.[SystemName] as ColorSystemName,
ps.[Color] as PromoStatusColor,
ps.[SystemName] as PromoStatusSystemName,
pr.[CreatorId],
pr.[ClientTreeId],
pr.[BaseClientTreeIds],
pr.[StartDate],
DATEADD(SECOND, 86399, pr.[EndDate]) AS EndDate,
pr.[DispatchesStart],
pr.[CalendarPriority],
pr.[Number],
bt.[Name] as BrandTechName,
ev.[Name] as EventName,
pr.[InOut]
FROM [Promo] pr 
LEFT JOIN PromoStatus ps ON pr.PromoStatusId = ps.Id 
LEFT JOIN Color cl ON pr.ColorId = cl.Id 
LEFT JOIN Mechanic mmc ON pr.MarsMechanicId = mmc.Id 
LEFT JOIN MechanicType mmt ON pr.MarsMechanicTypeId = mmt.Id 
LEFT JOIN [Event] ev ON pr.EventId = ev.Id 
LEFT JOIN BrandTech bt ON pr.BrandTechId = bt.Id;
GO

--Всё или ничего!
IF (@@error <> 0) BEGIN
	print('ROLLBACK :(')
    ROLLBACK
END
ELSE BEGIN
	print('COMMIT OK')
	COMMIT
END