ALTER VIEW [dbo].[ClientDashboardView] AS
WITH Statuses
AS
	(SELECT SystemName, Id FROM dbo.PromoStatus),
	PromoParams
AS 
	(SELECT pr.ClientTreeId AS ClientTreeId, MIN(ClientName) AS ClientName, MIN(pr.ClientHierarchy) AS ClientHierarchy, YEAR(pr.StartDate) AS [Year], pr.BrandTechId AS BrandTechId, pr.PromoStatusId AS Status,
			SUM(pr.PlanPromoTIShopper) AS TotalPlanPromoTIShopper,
			SUM(pr.PlanPromoTIMarketing) AS TotalPlanPromoTIMarketing,

			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.PromoDuration ELSE
			(CASE WHEN pr.StartDate < GETDATE() AND pr.EndDate > GETDATE() THEN DATEDIFF(day, pr.StartDate, GETDATE()) ELSE
			0 END) END) AS promoDays,

			/* YTD */
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoLSV ELSE 0 END) AS ActualPromoLSV,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoTIShopper ELSE 0 END) AS ActualPromoTIShopper,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoTIMarketing ELSE 0 END) AS ActualPromoTIMarketing,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoCostProduction ELSE 0 END) AS ActualPromoCostProduction,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoBranding ELSE 0 END) AS ActualPromoBranding,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoBTL ELSE 0 END) AS ActualPromoBTL,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoIncrementalEarnings ELSE 0 END) AS ActualPromoIncrementalEarnings,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoCost ELSE 0 END) AS ActualPromoCost,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.PlanPromoIncrementalNSV ELSE 0 END) AS PlanPromoIncrementalNSV,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoNSV ELSE 0 END) AS ActualPromoNSV,
			/* YEE */
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoLSV ELSE 0 END) AS PlanPromoLSV,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoIncrementalEarnings ELSE 0 END) AS PlanPromoIncrementalEarnings,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoCost ELSE 0 END) AS PlanPromoCost,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.ActualPromoIncrementalNSV ELSE 0 END) AS ActualPromoIncrementalNSV,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoTIShopper ELSE 0 END) AS PlanPromoTIShopper,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoTIMarketing ELSE 0 END) AS PlanPromoTIMarketing,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoBTL ELSE 0 END) AS PlanPromoBTL,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoBranding ELSE 0 END) AS PlanPromoBranding,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoCostProduction ELSE 0 END) AS PlanPromoCostProduction,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoNSV ELSE 0 END) AS PlanPromoNSV
		FROM dbo.Promo AS pr 
		LEFT JOIN Statuses AS CS ON CS.Id = pr.PromoStatusId
		WHERE pr.Disabled = 0 AND pr.PromoStatusId NOT IN (SELECT Id FROM PromoStatus WHERE SystemName IN ('Cancelled', 'Draft', 'Deleted'))
		GROUP BY pr.ClientTreeId, pr.BrandTechId, YEAR(pr.StartDate), pr.PromoStatusId
		),
	YEEF
AS
	(SELECT DISTINCT MIN(YEE_LSV) AS YEE_LSV, MIN(DMR_PLAN_LSV) AS PlanLSV, MIN(YTD_LSV) AS YTD_LSV, G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR FROM YEAR_END_ESTIMATE_FDM
	GROUP BY G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR)

SELECT
	NEWID() AS Id, pp.ClientTreeId AS ObjectId, MIN(pp.ClientName) AS ClientName, MIN(pp.ClientHierarchy) AS ClientHierarchy, pp.BrandTechId AS BrandTechId, pp.Year AS Year,
	(SELECT [Name] FROM BrandTech WHERE BrandTech.Id = pp.BrandTechId) AS BrandTechName, 
	(SELECT MIN([LogoFileName]) FROM ProductTree WHERE ProductTree.BrandId = (SELECT BrandId FROM BrandTech WHERE BrandTech.Id = pp.BrandTechId)) AS LogoFileName,
	ROUND(SUM(pp.promoDays)/7, 0) AS PromoWeeks,
	CASE WHEN SUM(YEE.YTD_LSV) != 0 AND SUM(YEE.YTD_LSV) IS NOT NULL
		AND SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL 
		THEN SUM(pp.ActualPromoLSV)/SUM(YEE.YTD_LSV) ELSE 0 END AS VodYTD,
	CASE WHEN SUM(YEE.YEE_LSV) != 0 AND SUM(YEE.YTD_LSV) IS NOT NULL 
		AND SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL
		THEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))/SUM(YEE.YEE_LSV) ELSE 0 END AS VodYEE,
	CASE WHEN (SUM(pp.ActualPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoLSV) END AS ActualPromoLSV,
	CASE WHEN (SUM(pp.PlanPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.PlanPromoLSV) END AS PlanPromoLSV,
	/* Shopper TI */
	CASE WHEN (SUM(CD.ShopperTiPlanPercent) IS NULL) THEN 0 ELSE SUM(CD.ShopperTiPlanPercent) END AS ShopperTiPlanPercent,
	CASE WHEN (SUM(pp.TotalPlanPromoTIShopper) IS NULL) THEN 0 ELSE SUM(pp.TotalPlanPromoTIShopper) END AS ShopperTiPlan,
	CASE WHEN (SUM(pp.ActualPromoTIShopper) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoTIShopper) END AS ShopperTiYTD,
	(CASE WHEN (SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL AND SUM(pp.ActualPromoTIShopper) != 0 AND SUM(pp.ActualPromoTIShopper) IS NOT NULL) THEN
		SUM(pp.ActualPromoTIShopper)/SUM(pp.ActualPromoLSV)
		ELSE 0 END) AS ShopperTiYTDPercent,
	CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) END AS ShopperTiYEE,
	(CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL)
	 AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) != 0 AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NOT NULL) THEN 
		(SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper))/(SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))
		ELSE 0 END) AS ShopperTiYEEPercent,
	/* Marketing TI */
	CASE WHEN (SUM(CD.MarketingTiPlanPercent) IS NULL) THEN 0 ELSE SUM(CD.MarketingTiPlanPercent) END AS MarketingTiPlanPercent,
	CASE WHEN (SUM(pp.TotalPlanPromoTIMarketing) IS NULL) THEN 0 ELSE SUM(pp.TotalPlanPromoTIMarketing) END AS MarketingTiPlan,
	CASE WHEN (SUM(pp.ActualPromoTIMarketing) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) END AS MarketingTiYTD,
	(CASE WHEN (SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL) AND SUM(pp.ActualPromoTIMarketing) != 0 AND SUM(pp.ActualPromoTIMarketing) IS NOT NULL THEN
		SUM(pp.ActualPromoTIMarketing)/SUM(pp.ActualPromoLSV)
		ELSE 0 END) AS MarketingTiYTDPercent,
	CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END AS MarketingTiYEE,
	(CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL))
		AND SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) != 0 AND (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NOT NULL) THEN 
		(SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing))/(SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))
		ELSE 0 END) AS MarketingTiYEEPercent,
	/* Production */
	CASE WHEN (SUM(YEE.PlanLSV) IS NULL OR SUM(YEE.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN SUM(CD.ProductionPlan)/SUM(YEE.PlanLSV) IS NULL
		THEN 0 ELSE SUM(CD.ProductionPlan)/SUM(YEE.PlanLSV) END) END AS ProductionPlanPercent,
	CASE WHEN (SUM(CD.ProductionPlan) IS NULL) THEN 0 ELSE SUM(CD.ProductionPlan) END AS ProductionPlan,
	CASE WHEN (SUM(pp.ActualPromoCostProduction) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoCostProduction) END AS ProductionYTD,
	(CASE WHEN (SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL AND SUM(pp.ActualPromoCostProduction) != 0 AND SUM(pp.ActualPromoCostProduction) IS NOT NULL) THEN
		SUM(pp.ActualPromoCostProduction)/SUM(pp.ActualPromoLSV)
		ELSE 0 END) AS ProductionYTDPercent,
	CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) END AS ProductionYEE,
	(CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL)
		AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) != 0 AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NOT NULL) THEN 
		(SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction))/(SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))
		ELSE 0 END) AS ProductionYEEPercent,
	/* Branding */
	CASE WHEN (SUM(YEE.PlanLSV) IS NULL OR SUM(YEE.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (SUM(CD.BrandingPlan)/SUM(YEE.PlanLSV)) IS NULL THEN 0 ELSE SUM(CD.BrandingPlan)/SUM(YEE.PlanLSV) END) END AS BrandingPlanPercent,
	CASE WHEN (SUM(CD.BrandingPlan) IS NULL) THEN 0 ELSE SUM(CD.BrandingPlan) END AS BrandingPlan,
	CASE WHEN (SUM(pp.ActualPromoBranding) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoBranding) END AS BrandingYTD,
	(CASE WHEN (SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL AND SUM(pp.ActualPromoBranding) != 0 AND SUM(pp.ActualPromoBranding) IS NOT NULL) THEN
		SUM(pp.ActualPromoBranding)/SUM(pp.ActualPromoLSV)
		ELSE 0 END) AS BrandingYTDPercent,
	CASE WHEN (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) END AS BrandingYEE,
	(CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL)
		AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) != 0 AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NOT NULL) THEN 
		(SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding))/(SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))
		ELSE 0 END) AS BrandingYEEPercent,
	/* BTL */
	CASE WHEN (SUM(YEE.PlanLSV) IS NULL OR SUM(YEE.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (SUM(CD.BTLPlan)/SUM(YEE.PlanLSV)) IS NULL THEN 0 ELSE SUM(CD.BTLPlan)/SUM(YEE.PlanLSV) END) END AS BTLPlanPercent,
	CASE WHEN (SUM(CD.BTLPlan) IS NULL) THEN 0 ELSE SUM(CD.BTLPlan) END AS BTLPlan,
	CASE WHEN (SUM(pp.ActualPromoBTL) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoBTL) END AS BTLYTD,
	(CASE WHEN (SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL AND SUM(pp.ActualPromoBTL) != 0 AND SUM(pp.ActualPromoBTL) IS NOT NULL) THEN
		SUM(pp.ActualPromoBTL)/SUM(pp.ActualPromoLSV)
		ELSE 0 END) AS BTLYTDPercent,
	CASE WHEN (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) END AS BTLYEE,
	(CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL)
		AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) != 0 AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NOT NULL) THEN 
		(SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL))/(SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))
		ELSE 0 END) AS BTLYEEPercent,
	/* ROI */
	CASE WHEN (SUM(CD.ROIPlanPercent) IS NULL) THEN 0 ELSE SUM(CD.ROIPlanPercent) END AS ROIPlanPercent,
	CASE WHEN ((SUM(pp.ActualPromoIncrementalEarnings)/(SUM(pp.ActualPromoCost)+1)*100) IS NULL) THEN 0 ELSE (SUM(pp.ActualPromoIncrementalEarnings)/(SUM(pp.ActualPromoCost)+1)*100)
		END AS ROIYTDPercent,
	CASE WHEN 
	(((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
		/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)+1)*100)) IS NULL THEN 0 ELSE
	((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
		/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)+1)*100) END
		AS ROIYEEPercent,
	/* LSV */
	CASE WHEN (SUM(YEE.PlanLSV) IS NULL) THEN 0 ELSE SUM(YEE.PlanLSV) END AS LSVPlan,
	CASE WHEN (SUM(YEE.YTD_LSV) IS NULL) THEN 0 ELSE SUM(YEE.YTD_LSV) END AS LSVYTD,
	CASE WHEN (SUM(YEE.YEE_LSV) IS NULL) THEN 0 ELSE SUM(YEE.YEE_LSV) END AS LSVYEE,
	/* Incremental NSV */
	CASE WHEN (SUM(CD.IncrementalNSVPlan) IS NULL) THEN 0 ELSE SUM(CD.IncrementalNSVPlan) END AS IncrementalNSVPlan,
	CASE WHEN (SUM(pp.ActualPromoIncrementalNSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoIncrementalNSV) END AS IncrementalNSVYTD,
	CASE WHEN (SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) END AS IncrementalNSVYEE,
	/* Promo NSV */
	CASE WHEN (SUM(CD.PromoNSVPlan) IS NULL) THEN 0 ELSE SUM(CD.PromoNSVPlan) END AS PromoNSVPlan,
	CASE WHEN (SUM(pp.ActualPromoNSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoNSV) END AS PromoNSVYTD,
	CASE WHEN (SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) END AS PromoNSVYEE

FROM PromoParams AS pp
LEFT JOIN dbo.ClientDashboard AS CD ON CD.ClientTreeId = pp.ClientTreeId AND CD.BrandTechId = pp.BrandTechId AND CD.Year = pp.Year
LEFT JOIN YEEF AS YEE ON YEE.YEAR = pp.Year
AND YEE.BRAND_SEG_TECH_CODE = (Select BrandTech_code FROM BrandTech WHERE Id = pp.BrandTechId)
AND '00' + YEE.G_HIERARCHY_ID LIKE (Select GHierarchyCode FROM ClientTree WHERE ObjectId = pp.ClientTreeId AND EndDate IS NULL)
		GROUP BY pp.ClientTreeId, pp.BrandTechId, pp.Year
GO