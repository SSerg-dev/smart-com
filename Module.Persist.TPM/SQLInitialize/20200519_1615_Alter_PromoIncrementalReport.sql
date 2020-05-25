ALTER VIEW [dbo].[PlanIncrementalReport]
AS
  SELECT NEWID()                                    AS Id,
         CONCAT(joined.ZREP, '_0125')               AS ZREP,
         joined.DemandCode                          AS DemandCode,
         p.InOut                                    AS InOut,
         FORMATMESSAGE('%s#%i', p.[Name], p.Number) AS PromoNameId,
         'RU_0125'                                  AS LocApollo,
         '7'                                        AS TypeApollo,
         'SHIP_LEWAND_CS'                           AS ModelApollo,
         CONVERT(DATETIMEOFFSET(7), IIF((SELECT COUNT(*) FROM Dates WHERE OriginalDate = joined.WeekStartDate AND MarsDay = 1) > 0, 
										joined.WeekStartDate, (SELECT TOP(1) OriginalDate FROM Dates WHERE OriginalDate < joined.WeekStartDate AND OriginalDate > DATEADD(DAY, -7, joined.WeekStartDate) AND MarsDay = 1))) 
													AS WeekStartDate,
		 IIF(joined.PlanProductIncrementalCaseQty / p.DispatchDuration * (joined.EndDay - (joined.StartDay - 1)) > 0,
			joined.PlanProductIncrementalCaseQty / p.DispatchDuration * (joined.EndDay - (joined.StartDay - 1)), 0)
													AS PlanProductIncrementalCaseQty,
		 joined.PlanProductBaselineCaseQty / p.DispatchDuration * (joined.EndDay - (joined.StartDay - 1))
													AS PlanProductBaselineCaseQty,
		 IIF(joined.PlanProductIncrementalLSV / p.DispatchDuration * (joined.EndDay - (joined.StartDay - 1)) > 0,
			joined.PlanProductIncrementalLSV / p.DispatchDuration * (joined.EndDay - (joined.StartDay - 1)), 0)
													AS PlanProductIncrementalLSV,
		 joined.PlanProductBaselineLSV / p.DispatchDuration * (joined.EndDay - (joined.StartDay - 1))
													AS PlanProductBaselineLSV,
		 IIF(joined.PlanProductIncrementalCaseQty / p.DispatchDuration * (joined.EndDay - (joined.StartDay - 1)) > 0,
			p.PlanPromoUpliftPercent, 0)			AS PlanUplift,
         p.DispatchesStart							AS DispatchesStart,
         p.DispatchesEnd							AS DispatchesEnd,
         p.IsGrowthAcceleration						AS IsGrowthAcceleration,
         ''											AS [Week],
         ps.[Name]									AS [Status]
  FROM  (SELECT pp.PromoId								AS PromoId,
                pp.ZREP									AS ZREP,
                pp.PlanProductIncrementalCaseQty		AS PlanProductIncrementalCaseQty,
                dc.DemandCode							AS DemandCode,
                Min(dt.OriginalDate)					AS WeekStartDate,
                Min(dt.MarsDay)							AS StartDay,
                Max(dt.MarsDay)							AS EndDay,
                pp.PlanProductBaselineCaseQty			AS PlanProductBaselineCaseQty,
                pp.PlanProductIncrementalLSV			AS PlanProductIncrementalLSV,
                pp.PlanProductBaselineLSV				AS PlanProductBaselineLSV
         FROM   PromoProduct pp
                LEFT JOIN Promo p
                       ON pp.PromoId = p.Id
                LEFT JOIN DemandCodeView dc
                       ON p.ClientTreeId = dc.ObjectId
                INNER JOIN Dates dt
                        ON dt.OriginalDate >= CAST(p.DispatchesStart AS DATE)
                           AND dt.OriginalDate <= CAST(p.DispatchesEnd AS DATE)
         WHERE  pp.[Disabled] = 0
                AND p.[Disabled] = 0
				AND p.[IsApolloExport] = 1
         GROUP  BY pp.PromoId,
                   pp.ZREP,
                   pp.PlanProductIncrementalCaseQty,
                   pp.PlanProductBaselineCaseQty,
                   pp.PlanProductIncrementalLSV,
                   pp.PlanProductBaselineLSV,
                   dc.DemandCode,
                   dt.MarsYear,
                   dt.MarsPeriod,
                   dt.MarsWeek) AS joined
        LEFT JOIN Promo p
               ON PromoId = p.Id
        LEFT JOIN PromoStatus ps
               ON p.PromoStatusId = ps.Id
GO