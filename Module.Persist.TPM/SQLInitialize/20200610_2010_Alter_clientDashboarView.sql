CREATE OR ALTER VIEW [dbo].[ClientDashboardView] AS
WITH
	Years
AS
	(SELECT Year(StartDate) AS [Year]
	FROM Promo (NOLOCK)
	GROUP BY Year(StartDate)),
	
	YEEF
AS
	-- TODO: remove distinct
	(SELECT DISTINCT MIN(YEE_LSV) AS YEE_LSV, MIN(DMR_PLAN_LSV) AS PlanLSV, MIN(YTD_LSV) AS YTD_LSV, G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR 
		FROM YEAR_END_ESTIMATE_FDM  (NOLOCK)
		GROUP BY G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR),
	
	CrossJoinBrandTechClientTree
AS
	(SELECT 
		BrandTech.Id AS BTId, BrandTech.[BrandsegTechsub] AS BTName, [BrandsegTechsub_code] AS BTCode, BrandId AS BrandId,
		CT.[Name] AS CTName, CT.ObjectId AS ObjectId, CT.Id AS CTId, CT.[FullPathName] AS ClientHierarchy,
		Y.[Year],
		YEE.YEE_LSV AS YEE_LSV, YEE.PlanLSV AS PlanLSV, YEE.YTD_LSV AS YTD_LSV
	FROM BrandTech (NOLOCK)
	CROSS JOIN ClientTree AS CT (NOLOCK)
	CROSS JOIN Years AS Y (NOLOCK)
	LEFT JOIN YEEF AS YEE (NOLOCK) ON YEE.YEAR = Y.[Year]
		AND YEE.BRAND_SEG_TECH_CODE = [BrandsegTechsub_code]
		AND YEE.G_HIERARCHY_ID LIKE dbo.ClientDashboardGetParentGHierarchyCode(CT.ObjectId)
	WHERE 
		CT.EndDate IS NULL AND CT.IsBaseClient = 1
		AND BrandTech.[Disabled] = 0),

	PromoParams
AS 
	(SELECT pr.ClientTreeId AS ClientTreeId, MIN(pr.ClientTreeKeyId) AS ClientTreeKeyId, MIN(pr.ClientHierarchy) AS ClientHierarchy, YEAR(pr.StartDate) AS [Year], pr.BrandTechId AS BrandTechId, pr.PromoStatusId AS Status,

		SUM(CASE WHEN pr.EndDate <= GETDATE() THEN pr.PromoDuration ELSE
		(CASE WHEN pr.StartDate < GETDATE() AND pr.EndDate > GETDATE() THEN DATEDIFF(day, pr.StartDate, GETDATE()) ELSE
		0 END) END) AS promoDays,

		/* YTD Closed */
		SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoLSV ELSE 0 END) AS ActualPromoLSV,
		SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoTIShopper ELSE 0 END) AS ActualPromoTIShopper,
		SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoTIMarketing ELSE 0 END) AS ActualPromoTIMarketing,
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
		SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoBTL ELSE 0 END) AS PlanPromoBTL,
		SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoBranding ELSE 0 END) AS PlanPromoBranding,
		SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoCostProduction ELSE 0 END) AS PlanPromoCostProduction,
		SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoNSV ELSE 0 END) AS PlanPromoNSV
	FROM dbo.Promo AS pr (NOLOCK)
	LEFT JOIN dbo.PromoStatus AS CS (NOLOCK) ON CS.Id = pr.PromoStatusId
	WHERE pr.Disabled = 0 AND CS.SystemName IN ('Started', 'Approved', 'Closed', 'Finished', 'Planned')
	GROUP BY pr.ClientTreeId, pr.BrandTechId, YEAR(pr.StartDate), pr.PromoStatusId),
	
	GroupedPP
AS
	(SELECT pp.ClientTreeId AS ClientTreeId, MIN(pp.ClientTreeKeyId) AS ClientTreeKeyId, MIN(pp.ClientHierarchy) AS ClientHierarchy, pp.YEAR AS [Year], pp.BrandTechId AS BrandTechId,
		SUM( promoDays) as promoDays,

		/* YTD Closed */
		SUM( ActualPromoLSV)AS ActualPromoLSV ,
		SUM( ActualPromoTIShopper)as ActualPromoTIShopper ,
		SUM( ActualPromoTIMarketing)as ActualPromoTIMarketing,
		SUM( ActualPromoCostProduction)as ActualPromoCostProduction,
		SUM( ActualPromoBranding)as ActualPromoBranding,
		SUM( ActualPromoBTL)as ActualPromoBTL,
		SUM( ActualPromoIncrementalEarnings)as ActualPromoIncrementalEarnings,
		SUM( ActualPromoCost)as ActualPromoCost,
		SUM( ActualPromoIncrementalNSV)as ActualPromoIncrementalNSV,
		SUM( ActualPromoNSV)as ActualPromoNSV,
		/* YEE Finished */
		SUM( PlanPromoLSVFinished)as PlanPromoLSVFinished,
		SUM( PlanPromoIncrementalEarningsFinished)as PlanPromoIncrementalEarningsFinished,
		SUM( PlanPromoCostFinished)as PlanPromoCostFinished,
		SUM( PlanPromoIncrementalNSVFinished)as PlanPromoIncrementalNSVFinished,
		SUM( PlanPromoTIShopperFinished)as PlanPromoTIShopperFinished,
		SUM( PlanPromoTIMarketingFinished)as PlanPromoTIMarketingFinished,
		SUM( PlanPromoBTLFinished)as PlanPromoBTLFinished,
		SUM( PlanPromoBrandingFinished)as PlanPromoBrandingFinished,
		SUM( PlanPromoCostProductionFinished)as PlanPromoCostProductionFinished,
		SUM( PlanPromoNSVFinished)as PlanPromoNSVFinished,
		/* YEE */
		SUM( PlanPromoLSV)as PlanPromoLSV,
		SUM( PlanPromoIncrementalEarnings)as PlanPromoIncrementalEarnings,
		SUM( PlanPromoCost)as PlanPromoCost,
		SUM( PlanPromoIncrementalNSV)as PlanPromoIncrementalNSV,
		SUM( PlanPromoTIShopper)as PlanPromoTIShopper,
		SUM( PlanPromoTIMarketing)as PlanPromoTIMarketing,
		SUM( PlanPromoBTL)as PlanPromoBTL,
		SUM( PlanPromoBranding)as PlanPromoBranding,
		SUM( PlanPromoCostProduction)as PlanPromoCostProduction,
		SUM( PlanPromoNSV) as PlanPromoNSV
	FROM PromoParams AS pp
	GROUP BY pp.ClientTreeId, pp.BrandTechId, pp.YEAR),

	NonPromo
AS
	(SELECT nps.ClientTreeId AS ClientTreeKeyId,
			YEAR(nps.StartDate) AS [Year],
			SUM(CASE WHEN nps.ActualCostTE IS NOT NULL THEN nps.ActualCostTE ELSE 0 END) AS ActualCostTE,
			SUM(CASE WHEN (nps.EndDate < CAST(GETDATE() AS DATE)) AND (nps.ActualCostTE IS NULL OR nps.ActualCostTE = 0) 
					THEN nps.PlanCostTE ELSE 0 END) AS PlanCostTEYTD,
			SUM(CASE WHEN nps.ActualCostTE IS NULL OR nps.ActualCostTE = 0 THEN nps.PlanCostTE ELSE 0 END) AS PlanCostTEYEE
		FROM NonPromoSupport AS nps (NOLOCK)
		GROUP BY nps.ClientTreeId, YEAR(nps.StartDate))

SELECT
	NEWID() AS Id, MIN(CD.Id) AS HistoryId, MAX(BTCT.ObjectId) AS ObjectId, MIN(BTCT.ClientHierarchy) AS ClientHierarchy, MAX(BTCT.BTId) AS BrandTechId, MAX(BTCT.[Year]) AS Year,
	-- TODO: replace with join
	MIN(BTCT.BTName) AS BrandsegTechsubName, 
	-- TODO: replace with join
	(SELECT MIN([LogoFileName]) FROM ProductTree WHERE ProductTree.BrandId = MIN(BTCT.BrandId)) AS LogoFileName,
	CASE WHEN (SUM(pp.promoDays) IS NULL) THEN 0
		ELSE ROUND(SUM(pp.promoDays)/7, 0) END AS PromoWeeks,
	CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0)
			AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished)) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished)) IS NOT NULL 
		THEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished))/(dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) ELSE 0 END AS VodYTD,
	CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV)) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV)) IS NOT NULL 
			AND dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
		THEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))/dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) ELSE 0 END AS VodYEE,
	CASE WHEN (SUM(pp.ActualPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoLSV) END AS ActualPromoLSV,
	CASE WHEN (SUM(pp.PlanPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.PlanPromoLSV) END AS PlanPromoLSV,
	CASE WHEN (SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished) END AS ActualPromoCost,
	CASE WHEN (SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished) END AS ActualPromoIncrementalEarnings,
	CASE WHEN (SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost) END AS TotalPromoCost,
	CASE WHEN (SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings) IS NULL) THEN 0 ELSE
		SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings) END AS TotalPromoIncrementalEarnings,
	/* Shopper TI */
		-- From ClientDashboard
	CASE WHEN (MAX(CD.ShopperTiPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.ShopperTiPlanPercent) END AS ShopperTiPlanPercent,
		-- Plan% * PlanLSV / 100
	CASE WHEN (MAX(CD.ShopperTiPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
		THEN 0 ELSE (MAX(CD.ShopperTiPlanPercent) * MAX(CD.PlanLSV) / 100) END AS ShopperTiPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)
	CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished) END AS ShopperTiYTD,
		-- YTD / YTD with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0 
			AND (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished)) != 0 
			AND (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished)) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished))/dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
		ELSE 0 END) AS ShopperTiYTDPercent,
		-- Sum of Actual (closed) + Sum of Plan (approved & planned & started & finished)
	CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) END AS ShopperTiYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
			AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) != 0 AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper))/(dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
		ELSE 0 END) AS ShopperTiYEEPercent,
	/* Marketing TI */ 
		-- From ClientDashboard
	CASE WHEN (MAX(CD.MarketingTiPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.MarketingTiPlanPercent) END AS MarketingTiPlanPercent,
		-- Plan% * PlanLSV / 100
	CASE WHEN (MAX(CD.MarketingTiPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
		THEN 0 ELSE (MAX(CD.MarketingTiPlanPercent) * MAX(CD.PlanLSV) / 100) END AS MarketingTiPlan,
		-- Promo TI cost YTD + Non promo TI cost YTD
	(CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) IS NULL) 
			THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) END
		+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) IS NULL) 
			THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) END) AS MarketingTiYTD,
		-- (Promo TI cost YTD + Non promo TI cost YTD) / YTD with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0)
		THEN (CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) IS NULL) 
				THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) END
			+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) IS NULL) 
				THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) END) / dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) * 100
		ELSE 0 END) AS MarketingTiYTDPercent,
		-- Promo TI cost YEE + Non promo TI cost YEE
	(CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END
		+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NULL) 
			THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) END) AS MarketingTiYEE,
		-- (Promo TI cost YEE + Non promo TI cost YEE) / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0)
		THEN (CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) 
				THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END
			+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NULL) 
				THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) END) / dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) * 100
		ELSE 0 END) AS MarketingTiYEEPercent,
	/* Promo TI Cost*/
		-- From ClientDashboard
	CASE WHEN (MAX(CD.PromoTiCostPlanPercent) IS NULL) 
		THEN 0 ELSE MAX(CD.PromoTiCostPlanPercent) END AS PromoTiCostPlanPercent,
		-- Plan% * PlanLSV / 100
	CASE WHEN (MAX(CD.PromoTiCostPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
		THEN 0 ELSE (MAX(CD.PromoTiCostPlanPercent) * MAX(CD.PlanLSV) / 100) END AS PromoTiCostPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)
	CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) END AS PromoTiCostYTD,
		-- YTD / YTD with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0 
			AND (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished)) != 0 
			AND (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished)) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished))/dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
		ELSE 0 END) AS PromoTiCostYTDPercent,
		-- Sum of Actual (closed) + Sum of Plan (approved & planned & started & finished)
	CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END AS PromoTiCostYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
			AND SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) != 0 
			AND SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing))/(dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
		ELSE 0 END) AS PromoTiCostYEEPercent,
	/* Non Promo TI Cost */
		-- From ClientDashboard
	CASE WHEN (MAX(CD.NonPromoTiCostPlanPercent) IS NULL) 
		THEN 0 ELSE MAX(CD.NonPromoTiCostPlanPercent) END AS NonPromoTiCostPlanPercent,
		-- Plan% * PlanLSV / 100
	CASE WHEN (MAX(CD.NonPromoTiCostPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
		THEN 0 ELSE (MAX(CD.NonPromoTiCostPlanPercent) * MAX(CD.PlanLSV) / 100) END AS NonPromoTiCostPlan,
		-- Sum of Actual + Sum of Plan (end date < now & Actual = 0/null) (rules are observed in NonPromo select)
	CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) IS NULL) 
		THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) END AS NonPromoTiCostYTD,
		-- YTD / YTD with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0  
			AND (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD)) != 0 
			AND (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD)) IS NOT NULL) 
		THEN ((MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD))/dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
		ELSE 0 END) AS NonPromoTiCostYTDPercent,
		-- Sum of Actual + Sum of Plan (Actual = 0/null) (rules are observed in NonPromo select)
	CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NULL) 
		THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) END AS NonPromoTiCostYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
			AND MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) != 0 
			AND MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NOT NULL) 
		THEN ((MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE))/(dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
		ELSE 0 END) AS NonPromoTiCostYEEPercent,
	/* Production */
		-- Plan / PlanLSV
	CASE WHEN (MAX(CD.PlanLSV) IS NULL OR MAX(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN MAX(CD.ProductionPlan)/MAX(CD.PlanLSV) IS NULL
		THEN 0 ELSE (MAX(CD.ProductionPlan)/MAX(CD.PlanLSV)) * 100 END) END AS ProductionPlanPercent,
		-- From ClientDashboard
	CASE WHEN (MAX(CD.ProductionPlan) IS NULL) THEN 0 ELSE MAX(CD.ProductionPlan) END AS ProductionPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)		
	CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished) IS NULL) THEN 0 
		ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished) END AS ProductionYTD,
		-- YTD / YTD with Share * 100
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0
				AND (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished)) != 0 
				AND (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished)) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished))/dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) * 100)
		ELSE 0 END) AS ProductionYTDPercent,
		-- YTD + Sum of Plan (approved & planned & started)
	CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) END AS ProductionYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
			AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) != 0 
			AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction))/(dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
		ELSE 0 END) AS ProductionYEEPercent,
	/* Branding */
		-- Plan / PlanLSV
	CASE WHEN (MAX(CD.PlanLSV) IS NULL OR MAX(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (MAX(CD.BrandingPlan)/MAX(CD.PlanLSV)) IS NULL 
		THEN 0 ELSE (MAX(CD.BrandingPlan)/MAX(CD.PlanLSV) * 100) END) END AS BrandingPlanPercent,
		-- From ClientDashboard
	CASE WHEN (MAX(CD.BrandingPlan) IS NULL) THEN 0 ELSE MAX(CD.BrandingPlan) END AS BrandingPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)	
	CASE WHEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) IS NULL) 
		THEN 0 ELSE (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) END AS BrandingYTD,
		-- YTD / YTD with Share * 100
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0
			AND (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) != 0 
			AND (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished))/dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
		ELSE 0 END) AS BrandingYTDPercent,
		-- YTD + Sum of Plan (approved & planned & started)
	CASE WHEN (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) END AS BrandingYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
			AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) != 0 AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding))/(dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
		ELSE 0 END) AS BrandingYEEPercent,
	/* BTL */
		-- Plan / PlanLSV
	CASE WHEN (MAX(CD.PlanLSV) IS NULL OR MAX(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (MAX(CD.BTLPlan)/MAX(CD.PlanLSV)) IS NULL 
		THEN 0 ELSE (MAX(CD.BTLPlan)/MAX(CD.PlanLSV) * 100) END) END AS BTLPlanPercent,
		-- From ClientDashboard
	CASE WHEN (MAX(CD.BTLPlan) IS NULL) THEN 0 ELSE MAX(CD.BTLPlan) END AS BTLPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)	
	CASE WHEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) IS NULL) 
		THEN 0 ELSE (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) END AS BTLYTD,
		-- YTD / YTD with Share * 100
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0 
			AND (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) != 0
			AND (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished))/dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
		ELSE 0 END) AS BTLYTDPercent,
		-- YTD + Sum of Plan (approved & planned & started)
	CASE WHEN (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) END AS BTLYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
			AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) != 0 
			AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL))/dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) * 100)
		ELSE 0 END) AS BTLYEEPercent,
	/* ROI */
		-- From ClientDashboard
	CASE WHEN (MAX(CD.ROIPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.ROIPlanPercent) END AS ROIPlanPercent,
		-- ([Sum of Actual Net Incremental Earnings (closed) + Sum of Plan Net Incremental Earnings (finished)] 
		--	/ [Sum of Actual Promo Cost (closed) + Sum of Plan Promo Cost (finished)] + 1) * 100
	CASE WHEN ((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)) IS NULL) 
			OR ((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)) = 0) 
			OR ((((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished))
				/((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)))+1)*100) IS NULL) 
		THEN 0 ELSE
			(((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished))
			/((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)))+1)*100)
		END AS ROIYTDPercent,
		-- ([Sum of Actual Net Incremental Earnings (closed) + Sum of Plan Net Incremental Earnings (approved, planned, started è finished)] 
		--	/ [Sum of Actual Promo Cost (closed) + Sum of Plan Promo Cost (approved, planned, started è finished)] +1 ) * 100
	CASE WHEN ((SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)) IS NULL) 
			OR ((SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)) = 0) 
			OR ((((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
				/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)))+1)*100) IS NULL 
		THEN 0 ELSE
		(((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
			/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost))+1)*100) 
		END AS ROIYEEPercent,
	/* LSV */
		-- From ClientDashboard
	CASE WHEN (MAX(CD.PlanLSV) IS NULL) THEN 0 ELSE MAX(CD.PlanLSV) END AS LSVPlan,
		-- YTD with Share
	dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) AS LSVYTD,
		-- YEE with Share
	dbo.ClientDashboardGetYTDwithShare(MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) AS LSVYEE,
	/* Incremental NSV */
		-- From ClientDashboard
	CASE WHEN (MAX(CD.IncrementalNSVPlan) IS NULL) THEN 0 ELSE MAX(CD.IncrementalNSVPlan) END AS IncrementalNSVPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)	
	CASE WHEN ((SUM(pp.ActualPromoIncrementalNSV) + SUM(pp.PlanPromoIncrementalNSVFinished)) IS NULL) THEN 0
		ELSE (SUM(pp.ActualPromoIncrementalNSV) + SUM(pp.PlanPromoIncrementalNSVFinished)) END AS IncrementalNSVYTD,
		-- YTD + Sum of Plan (approved & planned & started)
	CASE WHEN (SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) END AS IncrementalNSVYEE,
	/* Promo NSV */
		-- From ClientDashboard
	CASE WHEN (MAX(CD.PromoNSVPlan) IS NULL) THEN 0 ELSE MAX(CD.PromoNSVPlan) END AS PromoNSVPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)	
	CASE WHEN ((SUM(pp.ActualPromoNSV) + SUM(pp.PlanPromoNSVFinished)) IS NULL) 
		THEN 0 ELSE (SUM(pp.ActualPromoNSV) + SUM(pp.PlanPromoNSVFinished)) END AS PromoNSVYTD,
		-- YTD + Sum of Plan (approved & planned & started)
	CASE WHEN (SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) END AS PromoNSVYEE		
FROM CrossJoinBrandTechClientTree AS BTCT (NOLOCK)
LEFT JOIN PromoParams AS pp (NOLOCK) ON BTCT.BTId = pp.BrandTechId AND BTCT.ObjectId = pp.ClientTreeId AND BTCT.YEAR = pp.YEAR
LEFT JOIN dbo.ClientDashboard AS CD (NOLOCK) ON CD.ClientTreeId = BTCT.ObjectId AND CD.BrandTechId = BTCT.BTId AND CD.Year = BTCT.[Year]
LEFT JOIN NonPromo AS np (NOLOCK) ON BTCT.CTId = np.ClientTreeKeyId AND BTCT.[Year] = np.Year
LEFT JOIN ClientTreeBrandTech AS SHARES (NOLOCK) ON BTCT.CTId = SHARES.ClientTreeId AND 
	SHARES.[BrandTechId] = BTCT.BTId AND SHARES.[Disabled] = 0
GROUP BY BTCT.ObjectId, BTCT.BTId, BTCT.[Year]
GO


