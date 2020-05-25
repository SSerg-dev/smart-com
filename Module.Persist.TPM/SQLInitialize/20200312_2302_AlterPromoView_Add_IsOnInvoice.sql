ALTER VIEW[dbo].[PromoView] AS
SELECT  pr.Id, pr.Name, pr.IsOnInvoice, mmc.Name AS MarsMechanicName, mmt.Name AS MarsMechanicTypeName, pr.MarsMechanicDiscount, cl.SystemName AS ColorSystemName, ps.Color AS PromoStatusColor,
    ps.SystemName AS PromoStatusSystemName, ps.Name AS PromoStatusName, pr.CreatorId, pr.ClientTreeId, pr.BaseClientTreeIds, pr.StartDate, DATEADD(SECOND, 86399, pr.EndDate) AS EndDate, pr.DispatchesStart,
        pr.CalendarPriority, pr.Number, bt.Name AS BrandTechName, ev.Name AS EventName, pr.InOut, pt.SystemName as TypeName, pt.Glyph as TypeGlyph, pr.IsGrowthAcceleration
FROM    dbo.Promo AS pr LEFT OUTER JOIN
dbo.PromoStatus AS ps ON pr.PromoStatusId = ps.Id LEFT OUTER JOIN
dbo.PromoTypes AS pt ON pr.PromoTypesId = pt.Id LEFT OUTER JOIN
dbo.Color AS cl ON pr.ColorId = cl.Id LEFT OUTER JOIN
dbo.Mechanic AS mmc ON pr.MarsMechanicId = mmc.Id LEFT OUTER JOIN
dbo.MechanicType AS mmt ON pr.MarsMechanicTypeId = mmt.Id LEFT OUTER JOIN
dbo.Event AS ev ON pr.EventId = ev.Id LEFT OUTER JOIN
dbo.BrandTech AS bt ON pr.BrandTechId = bt.Id
WHERE pr.[Disabled] = 0;
GO