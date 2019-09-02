ALTER VIEW [dbo].[PromoView] AS SELECT 
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
LEFT JOIN BrandTech bt ON pr.BrandTechId = bt.Id
WHERE pr.[Disabled] = 0;
GO