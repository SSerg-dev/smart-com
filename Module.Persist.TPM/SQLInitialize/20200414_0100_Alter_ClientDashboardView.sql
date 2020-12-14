﻿ALTER VIEW [ClientDashboardView] AS
WITH Statuses
AS
	(SELECT SystemName, Id FROM dbo.PromoStatus),
	PromoParams
AS 
	(SELECT pr.ClientTreeId AS ClientTreeId, MIN(pr.ClientHierarchy) AS ClientHierarchy, YEAR(pr.StartDate) AS [Year], pr.BrandTechId AS BrandTechId, pr.PromoStatusId AS Status,

			SUM(CASE WHEN pr.EndDate <= GETDATE() THEN pr.PromoDuration ELSE
			(CASE WHEN pr.StartDate < GETDATE() AND pr.EndDate > GETDATE() THEN DATEDIFF(day, pr.StartDate, GETDATE()) ELSE
			0 END) END) AS promoDays,

			/* YTD Closed */
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoLSV ELSE 0 END) AS ActualPromoLSV,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoTIShopper ELSE 0 END) AS ActualPromoTIShopper,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoTIMarketing ELSE 0 END) AS ActualPromoTIMarketing,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoXSites ELSE 0 END) AS ActualPromoXSites,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoCatalogue ELSE 0 END) AS ActualPromoCatalogue,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoPOSMInClient ELSE 0 END) AS ActualPromoPOSMInClient,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoCostProduction ELSE 0 END) AS ActualPromoCostProduction,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoBranding ELSE 0 END) AS ActualPromoBranding,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoBTL ELSE 0 END) AS ActualPromoBTL,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoNetIncrementalEarnings ELSE 0 END) AS ActualPromoIncrementalEarnings,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoCost ELSE 0 END) AS ActualPromoCost,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoNetIncrementalNSV ELSE 0 END) AS ActualPromoIncrementalNSV,
			SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoNSV ELSE 0 END) AS ActualPromoNSV,
			/* YEE Finished */
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoLSV ELSE 0 END) AS PlanPromoLSVFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoNetIncrementalEarnings ELSE 0 END) AS PlanPromoIncrementalEarningsFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoCost ELSE 0 END) AS PlanPromoCostFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoNetIncrementalNSV ELSE 0 END) AS PlanPromoIncrementalNSVFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoTIShopper ELSE 0 END) AS PlanPromoTIShopperFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoTIMarketing ELSE 0 END) AS PlanPromoTIMarketingFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoXSites ELSE 0 END) AS PlanPromoXSitesFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoCatalogue ELSE 0 END) AS PlanPromoCatalogueFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoPOSMInClient ELSE 0 END) AS PlanPromoPOSMInClientFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoBTL ELSE 0 END) AS PlanPromoBTLFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoBranding ELSE 0 END) AS PlanPromoBrandingFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoCostProduction ELSE 0 END) AS PlanPromoCostProductionFinished,
			SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoNSV ELSE 0 END) AS PlanPromoNSVFinished,
			/* YEE */
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoLSV ELSE 0 END) AS PlanPromoLSV,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoNetIncrementalEarnings ELSE 0 END) AS PlanPromoIncrementalEarnings,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoCost ELSE 0 END) AS PlanPromoCost,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoNetIncrementalNSV ELSE 0 END) AS PlanPromoIncrementalNSV,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoTIShopper ELSE 0 END) AS PlanPromoTIShopper,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoTIMarketing ELSE 0 END) AS PlanPromoTIMarketing,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoXSites ELSE 0 END) AS PlanPromoXSites,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoCatalogue ELSE 0 END) AS PlanPromoCatalogue,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoPOSMInClient ELSE 0 END) AS PlanPromoPOSMInClient,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoBTL ELSE 0 END) AS PlanPromoBTL,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoBranding ELSE 0 END) AS PlanPromoBranding,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoCostProduction ELSE 0 END) AS PlanPromoCostProduction,
			SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoNSV ELSE 0 END) AS PlanPromoNSV
		FROM dbo.Promo AS pr 
		LEFT JOIN Statuses AS CS ON CS.Id = pr.PromoStatusId
		WHERE pr.Disabled = 0 AND pr.PromoStatusId IN (SELECT Id FROM PromoStatus WHERE SystemName IN ('Started', 'Approved', 'Closed', 'Finished', 'Started', 'Planned'))
		GROUP BY pr.ClientTreeId, pr.BrandTechId, YEAR(pr.StartDate), pr.PromoStatusId
		),
	YEEF
AS
	(SELECT DISTINCT MIN(YEE_LSV) AS YEE_LSV, MIN(DMR_PLAN_LSV) AS PlanLSV, MIN(YTD_LSV) AS YTD_LSV, G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR FROM YEAR_END_ESTIMATE_FDM
	GROUP BY G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR)

SELECT
	NEWID() AS Id, MIN(CD.Id) AS HistoryId, pp.ClientTreeId AS ObjectId, MIN(pp.ClientHierarchy) AS ClientHierarchy, pp.BrandTechId AS BrandTechId, pp.Year AS Year,
	(SELECT [Name] FROM BrandTech WHERE BrandTech.Id = pp.BrandTechId) AS BrandTechName, 
	(SELECT MIN([LogoFileName]) FROM ProductTree WHERE ProductTree.BrandId = (SELECT BrandId FROM BrandTech WHERE BrandTech.Id = pp.BrandTechId)) AS LogoFileName,
	ROUND(SUM(pp.promoDays)/7, 0) AS PromoWeeks,
	CASE WHEN SUM(YEE.YTD_LSV) != 0 AND SUM(YEE.YTD_LSV) IS NOT NULL
		AND SUM(pp.ActualPromoLSV) != 0 AND SUM(pp.ActualPromoLSV) IS NOT NULL 
		THEN SUM(pp.ActualPromoLSV)/SUM(YEE.YTD_LSV) ELSE 0 END AS VodYTD,
	CASE WHEN SUM(YEE.YEE_LSV) != 0 AND SUM(YEE.YTD_LSV) IS NOT NULL 
		AND SUM(YEE.YEE_LSV) != 0 AND SUM(YEE.YEE_LSV) IS NOT NULL
		THEN (SUM(YEE.YEE_LSV))/SUM(YEE.YEE_LSV) ELSE 0 END AS VodYEE,
	CASE WHEN (SUM(pp.ActualPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoLSV) END AS ActualPromoLSV,
	CASE WHEN (SUM(pp.PlanPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.PlanPromoLSV) END AS PlanPromoLSV,
	CASE WHEN (SUM(pp.ActualPromoCost) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoCost) END AS ActualPromoCost,
	CASE WHEN (SUM(pp.ActualPromoIncrementalEarnings) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoIncrementalEarnings) END AS ActualPromoIncrementalEarnings,
	CASE WHEN (SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost) END AS TotalPromoCost,
	CASE WHEN (SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings) IS NULL) THEN 0 ELSE
		SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings) END AS TotalPromoIncrementalEarnings,
	/* Shopper TI */
	CASE WHEN (MAX(CD.ShopperTiPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.ShopperTiPlanPercent) END AS ShopperTiPlanPercent,
	CASE WHEN (MAX(CD.ShopperTiPlanPercent) IS NULL OR SUM(CD.PlanLSV) IS NULL)
		THEN 0 ELSE (MAX(CD.ShopperTiPlanPercent) * SUM(CD.PlanLSV) / 100) END AS ShopperTiPlan,
	CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished) END AS ShopperTiYTD,
	(CASE WHEN (SUM(YEE.YTD_LSV) != 0 AND SUM(YEE.YTD_LSV) IS NOT NULL 
			AND (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished)) != 0 
			AND (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished)) IS NOT NULL) THEN
		((SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished))/SUM(YEE.YTD_LSV)) * 100
		ELSE 0 END) AS ShopperTiYTDPercent,
	CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) END AS ShopperTiYEE,
	(CASE WHEN (SUM(YEE.YEE_LSV) != 0 AND (SUM(YEE.YEE_LSV) IS NOT NULL)
	 AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) != 0 AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NOT NULL) THEN 
		((SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper))/(SUM(YEE.YEE_LSV))) * 100
		ELSE 0 END) AS ShopperTiYEEPercent,
	/* Marketing TI */
	CASE WHEN (MAX(CD.MarketingTiPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.MarketingTiPlanPercent) END AS MarketingTiPlanPercent,
	CASE WHEN (MAX(CD.MarketingTiPlanPercent) IS NULL OR SUM(CD.PlanLSV) IS NULL)
		THEN 0 ELSE (MAX(CD.MarketingTiPlanPercent) * SUM(CD.PlanLSV) / 100) END AS MarketingTiPlan,
	CASE WHEN (SUM(pp.ActualPromoTIMarketing) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) END AS MarketingTiYTD,
	CASE WHEN (SUM(pp.ActualPromoXSites) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoXSites) END AS XSitesYTD,
	CASE WHEN (SUM(pp.ActualPromoCatalogue) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoCatalogue) END AS CatalogueYTD,
	CASE WHEN (SUM(pp.ActualPromoPOSMInClient) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoPOSMInClient) END AS POSMInClientYTD,
	(CASE WHEN (SUM(YEE.YTD_LSV) != 0 AND SUM(YEE.YTD_LSV) IS NOT NULL) AND SUM(pp.ActualPromoTIMarketing) != 0 AND SUM(pp.ActualPromoTIMarketing) IS NOT NULL THEN
		(SUM(pp.ActualPromoTIMarketing)/SUM(YEE.YTD_LSV)) * 100
		ELSE 0 END) AS MarketingTiYTDPercent,
	CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END AS MarketingTiYEE,
	CASE WHEN (SUM(pp.ActualPromoXSites) + SUM(pp.PlanPromoXSites) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoXSites) + SUM(pp.PlanPromoXSites) END AS XSitesYEE,
	CASE WHEN (SUM(pp.ActualPromoCatalogue) + SUM(pp.PlanPromoCatalogue) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoCatalogue) + SUM(pp.PlanPromoCatalogue) END AS CatalogueYEE,
	CASE WHEN (SUM(pp.ActualPromoPOSMInClient) + SUM(pp.PlanPromoPOSMInClient) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoPOSMInClient) + SUM(pp.PlanPromoPOSMInClient) END AS POSMInClientTiYEE,
	(CASE WHEN (SUM(YEE.YEE_LSV) != 0 AND (SUM(YEE.YEE_LSV) IS NOT NULL))
		AND SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) != 0 AND (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NOT NULL) THEN 
		((SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing))/(SUM(YEE.YEE_LSV))) * 100
		ELSE 0 END) AS MarketingTiYEEPercent,
	/* Production */
	CASE WHEN (SUM(CD.PlanLSV) IS NULL OR SUM(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN MAX(CD.ProductionPlan)/SUM(CD.PlanLSV) IS NULL
		THEN 0 ELSE (MAX(CD.ProductionPlan)/SUM(CD.PlanLSV)) * 100 END) END AS ProductionPlanPercent,
	CASE WHEN (MAX(CD.ProductionPlan) IS NULL) THEN 0 ELSE MAX(CD.ProductionPlan) END AS ProductionPlan,
	CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished) IS NULL) THEN 0 
		ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished) END AS ProductionYTD,
	(CASE WHEN (SUM(YEE.YTD_LSV) != 0 AND SUM(YEE.YTD_LSV) IS NOT NULL
		AND (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished)) != 0 AND (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished)) IS NOT NULL) THEN
		((SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished))/SUM(YEE.YTD_LSV) * 100)
		ELSE 0 END) AS ProductionYTDPercent,
	CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) END AS ProductionYEE,
	(CASE WHEN (SUM(YEE.YEE_LSV) != 0 AND (SUM(YEE.YEE_LSV) IS NOT NULL)
		AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) != 0 AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NOT NULL) THEN 
		((SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction))/(SUM(YEE.YEE_LSV))) * 100
		ELSE 0 END) AS ProductionYEEPercent,
	/* Branding */
	CASE WHEN (SUM(CD.PlanLSV) IS NULL OR SUM(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (MAX(CD.BrandingPlan)/SUM(CD.PlanLSV)) IS NULL 
		THEN 0 ELSE (MAX(CD.BrandingPlan)/SUM(CD.PlanLSV) * 100) END) END AS BrandingPlanPercent,
	CASE WHEN (MAX(CD.BrandingPlan) IS NULL) THEN 0 ELSE MAX(CD.BrandingPlan) END AS BrandingPlan,
	CASE WHEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) IS NULL) THEN 0 ELSE (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) END AS BrandingYTD,
	(CASE WHEN (SUM(YEE.YTD_LSV) != 0 AND SUM(YEE.YTD_LSV) IS NOT NULL 
		AND (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) != 0 AND (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) IS NOT NULL) THEN
		((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished))/SUM(YEE.YTD_LSV)) * 100
		ELSE 0 END) AS BrandingYTDPercent,
	CASE WHEN (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) END AS BrandingYEE,
	(CASE WHEN (SUM(YEE.YEE_LSV) != 0 AND (SUM(YEE.YEE_LSV) IS NOT NULL)
		AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) != 0 AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NOT NULL) THEN 
		((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding))/(SUM(YEE.YEE_LSV))) * 100
		ELSE 0 END) AS BrandingYEEPercent,
	/* BTL */
	CASE WHEN (SUM(CD.PlanLSV) IS NULL OR SUM(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (MAX(CD.BTLPlan)/SUM(CD.PlanLSV)) IS NULL 
		THEN 0 ELSE (MAX(CD.BTLPlan)/SUM(CD.PlanLSV) * 100) END) END AS BTLPlanPercent,
	CASE WHEN (MAX(CD.BTLPlan) IS NULL) THEN 0 ELSE MAX(CD.BTLPlan) END AS BTLPlan,
	CASE WHEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) IS NULL) THEN 0 ELSE (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) END AS BTLYTD,
	(CASE WHEN (SUM(YEE.YTD_LSV) != 0 AND SUM(YEE.YTD_LSV) IS NOT NULL 
		AND (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) != 0
		AND (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) IS NOT NULL) THEN
		((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished))/SUM(YEE.YTD_LSV)) * 100
		ELSE 0 END) AS BTLYTDPercent,
	CASE WHEN (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) END AS BTLYEE,
	(CASE WHEN (SUM(YEE.YEE_LSV) != 0 AND (SUM(YEE.YEE_LSV) IS NOT NULL)
		AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) != 0 AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NOT NULL) THEN 
		((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL))/SUM(YEE.YEE_LSV) * 100)
		ELSE 0 END) AS BTLYEEPercent,
	/* ROI */
	CASE WHEN (MAX(CD.ROIPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.ROIPlanPercent) END AS ROIPlanPercent,
	CASE WHEN  ((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)) IS NULL) 
		OR ((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)) = 0) OR
		((((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished))/
		((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)))+1)*100) IS NULL) THEN 0 ELSE
		(((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished))/
		((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)))+1)*100)
		END AS ROIYTDPercent,
	CASE WHEN 
	((SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)) IS NULL) OR ((SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)) = 0) 
		OR ((((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
		/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)))+1)*100) IS NULL THEN 0 ELSE
	(((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
		/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost))+1)*100) END
		AS ROIYEEPercent,
	/* LSV */
	CASE WHEN (SUM(CD.PlanLSV) IS NULL) THEN 0 ELSE SUM(CD.PlanLSV) END AS LSVPlan,
	CASE WHEN (SUM(YEE.YTD_LSV) IS NULL) THEN 0 ELSE SUM(YEE.YTD_LSV) END AS LSVYTD,
	CASE WHEN (SUM(YEE.YEE_LSV) IS NULL) THEN 0 ELSE SUM(YEE.YEE_LSV) END AS LSVYEE,
	/* Incremental NSV */
	CASE WHEN (MAX(CD.IncrementalNSVPlan) IS NULL) THEN 0 ELSE MAX(CD.IncrementalNSVPlan) END AS IncrementalNSVPlan,
	CASE WHEN ((SUM(pp.ActualPromoIncrementalNSV) + SUM(pp.PlanPromoIncrementalNSVFinished)) IS NULL) THEN 0
		ELSE (SUM(pp.ActualPromoIncrementalNSV) + SUM(pp.PlanPromoIncrementalNSVFinished)) END AS IncrementalNSVYTD,
	CASE WHEN (SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) END AS IncrementalNSVYEE,
	/* Promo NSV */
	CASE WHEN (MAX(CD.PromoNSVPlan) IS NULL) THEN 0 ELSE MAX(CD.PromoNSVPlan) END AS PromoNSVPlan,
	CASE WHEN ((SUM(pp.ActualPromoNSV) + SUM(pp.PlanPromoNSVFinished)) IS NULL) THEN 0 ELSE (SUM(pp.ActualPromoNSV) + SUM(pp.PlanPromoNSVFinished)) END AS PromoNSVYTD,
	CASE WHEN (SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) END AS PromoNSVYEE

FROM PromoParams AS pp
LEFT JOIN dbo.ClientDashboard AS CD ON CD.ClientTreeId = pp.ClientTreeId AND CD.BrandTechId = pp.BrandTechId AND CD.Year = pp.Year
LEFT JOIN YEEF AS YEE ON YEE.YEAR = pp.Year
	AND YEE.BRAND_SEG_TECH_CODE = (Select BrandTech_code FROM BrandTech WHERE Id = pp.BrandTechId)
	AND YEE.G_HIERARCHY_ID LIKE '%' + (Select GHierarchyCode FROM ClientTree WHERE ObjectId = pp.ClientTreeId AND EndDate IS NULL)
		GROUP BY pp.ClientTreeId, pp.BrandTechId, pp.Year
GO