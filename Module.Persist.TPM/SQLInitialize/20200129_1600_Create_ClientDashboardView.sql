CREATE VIEW [dbo].[ClientDashboardView] AS
WITH ClosedStatus
AS
	(SELECT SystemName, Id FROM dbo.PromoStatus WHERE SystemName = 'Closed'),
	PromoParams
AS 
	(SELECT Count(pr.Id) AS CId, pr.ClientTreeId AS ClientTreeId, MIN(pr.ClientHierarchy) AS ClientHierarchy, YEAR(pr.StartDate) AS [Year], pr.BrandTechId AS BrandTechId, pr.PromoStatusId AS Status,
			SUM(pr.PlanPromoTIShopper) AS TotalPlanPromoTIShopper,
			SUM(pr.PlanPromoTIMarketing) AS TotalPlanPromoTIMarketing,
			SUM(pr.PlanPromoCostProduction) AS TotalPlanPromoCostProduction,
			SUM(pr.PlanPromoBranding) AS TotalPlanPromoBranding,
			SUM(pr.PlanPromoBTL) AS TotalPlanPromoBTL,
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
		LEFT JOIN ClosedStatus AS CS ON CS.Id = pr.PromoStatusId
		WHERE pr.Disabled = 0 AND pr.PromoStatusId NOT IN (SELECT Id FROM PromoStatus WHERE SystemName IN ('Cancelled', 'Draft', 'Deleted'))
		GROUP BY pr.ClientTreeId, pr.BrandTechId, YEAR(pr.StartDate), pr.PromoStatusId
		),
	YEEF
AS
	(SELECT DISTINCT MIN(YEE_LSV) AS YEE_LSV, MIN(DMR_PLAN_LSV) AS PlanLSV, MIN(YTD_LSV) AS YTD_LSV, G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR FROM YEAR_END_ESTIMATE_FDM
	GROUP BY G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR)

SELECT
	pp.ClientTreeId AS ObjectId, min(pp.ClientHierarchy) AS ClientHierarchy, pp.BrandTechId AS BrandTechId, pp.Year AS Year,
	(SELECT [Name] FROM BrandTech WHERE BrandTech.Id = pp.BrandTechId) AS BrandTechName,
	/* Shopper TI */
	SUM(CD.ShopperTiPlanPercent) AS ShopperTiPlanPercent,
	SUM(pp.TotalPlanPromoTIShopper) AS ShopperTiPlan,
	SUM(pp.ActualPromoTIShopper) AS ShopperTiYTD,
	(CASE WHEN (SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL) THEN
		SUM(pp.ActualPromoTIShopper)/SUM(pp.ActualPromoLSV)
		ELSE 0 END) AS ShopperTiYTDPercent,
	SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) AS ShopperTiYEE,
	(CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL)) THEN 
		(SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper))/(SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))
		ELSE 0 END) AS ShopperTiYEEPercent,
	/* Marketing TI */
	SUM(CD.MarketingTiPlanPercent) AS MarketingTiPlanPercent,
	SUM(pp.TotalPlanPromoTIMarketing) AS MarketingTiPlan,
	SUM(pp.ActualPromoTIMarketing) AS MarketingTiYTD,
	(CASE WHEN (SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL) THEN
		SUM(pp.ActualPromoTIMarketing)/SUM(pp.ActualPromoLSV)
		ELSE 0 END) AS MarketingTiYTDPercent,
	SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) AS MarketingTiYEE,
	(CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL)) THEN 
		(SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing))/(SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))
		ELSE 0 END) AS MarketingTiYEEPercent,
	/* Production */
	SUM(CD.ProductionPlan/YEE.PlanLSV) AS ProductionPlanPercent,
	SUM(CD.ProductionPlan) AS ProductionPlan,
	SUM(pp.ActualPromoCostProduction) AS ProductionYTD,
	(CASE WHEN (SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL) THEN
		SUM(pp.ActualPromoCostProduction)/SUM(pp.ActualPromoLSV)
		ELSE 0 END) AS ProductionYTDPercent,
	SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) AS ProductionYEE,
	(CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL)) THEN 
		(SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction))/(SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))
		ELSE 0 END) AS ProductionYEEPercent,
	/* Branding */
	SUM(CD.BrandingPlan/YEE.PlanLSV) AS BrandingPlanPercent,
	SUM(CD.BrandingPlan) AS BrandingPlan,
	SUM(pp.ActualPromoBranding) AS BrandingYTD,
	(CASE WHEN (SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL) THEN
		SUM(pp.ActualPromoBranding)/SUM(pp.ActualPromoLSV)
		ELSE 0 END) AS BrandingYTDPercent,
	SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) AS BrandingYEE,
	(CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL)) THEN 
		(SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding))/(SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))
		ELSE 0 END) AS BrandingYEEPercent,
	/* BTL */
	SUM(CD.BTLPlan/YEE.PlanLSV) AS BTLPlanPercent,
	SUM(CD.BTLPlan) AS BTLPlan,
	SUM(pp.ActualPromoBTL) AS BTLYTD,
	(CASE WHEN (SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL) THEN
		SUM(pp.ActualPromoBTL)/SUM(pp.ActualPromoLSV)
		ELSE 0 END) AS BTLYTDPercent,
	SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) AS BTLYEE,
	(CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV) IS NOT NULL)) THEN 
		(SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL))/(SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))
		ELSE 0 END) AS BTLYEEPercent,
	/* ROI */
	SUM(CD.ROIPlanPercent) AS ROIPlanPercent,
	(SUM(pp.ActualPromoIncrementalEarnings)/(SUM(pp.ActualPromoCost)+1)*100)
		AS ROIYTDPercent,
	((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
		/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)+1)*100)
		AS ROIYEEPercent,
	/* LSV */
	SUM(YEE.PlanLSV) AS LSVPlan,
	SUM(YEE.YTD_LSV) AS LSVYTD,
	SUM(YEE.YEE_LSV) AS LSVYEE,
	/* Incremental NSV */
	SUM(CD.IncrementalNSVPlan) AS IncrementalNSVPlan,
	SUM(pp.ActualPromoIncrementalNSV) AS IncrementalNSVYTD,
	SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) AS IncrementalNSVYEE,
	/* Promo NSV */
	SUM(CD.PromoNSVPlan) AS PromoNSVPlan,
	SUM(pp.ActualPromoNSV) AS PromoNSVYTD,
	SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) AS PromoNSVYEE

FROM PromoParams AS pp
LEFT JOIN dbo.ClientDashboard AS CD ON CD.ClientTreeId = pp.ClientTreeId AND CD.BrandTechId = pp.BrandTechId AND CD.Year = pp.Year
LEFT JOIN YEEF AS YEE ON YEE.YEAR = pp.Year
AND YEE.BRAND_SEG_TECH_CODE = (Select BrandTech_code FROM BrandTech WHERE Id = pp.BrandTechId)
AND '00' + YEE.G_HIERARCHY_ID LIKE (Select GHierarchyCode FROM ClientTree WHERE ObjectId = pp.ClientTreeId AND EndDate IS NULL)
		GROUP BY pp.ClientTreeId, pp.BrandTechId, pp.Year

GO