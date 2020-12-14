ALTER VIEW [ClientDashboardView] AS
WITH 
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
		SUM(CASE WHEN CS.SystemName = 'Closed' THEN PSP.FactCalculation ELSE 0 END) AS ActualPromoTICost,
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
		SUM(CASE WHEN CS.SystemName = 'Finished' THEN PSP.PlanCalculation ELSE 0 END) AS PlanPromoTICostFinished,
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
		SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoNSV ELSE 0 END) AS PlanPromoNSV,
		SUM(CASE WHEN CS.SystemName != 'Closed' THEN PSP.PlanCalculation ELSE 0 END) AS PlanPromoTICost
	FROM dbo.Promo AS pr 
	LEFT JOIN dbo.PromoStatus AS CS ON CS.Id = pr.PromoStatusId
	LEFT JOIN ( SELECT PromoId AS PromoId, SUM(FactCalculation) AS FactCalculation, SUM(PlanCalculation) AS PlanCalculation
			FROM PromoSupportPromo
			GROUP BY PromoId
		) AS PSP ON pr.Id = PSP.PromoId
	WHERE pr.Disabled = 0 AND CS.SystemName IN ('Started', 'Approved', 'Closed', 'Finished', 'Planned')
	GROUP BY pr.ClientTreeId, pr.BrandTechId, YEAR(pr.StartDate), pr.PromoStatusId),
	
	YEEF
AS
	-- TODO: remove distinct
	(SELECT DISTINCT MIN(YEE_LSV) AS YEE_LSV, MIN(DMR_PLAN_LSV) AS PlanLSV, MIN(YTD_LSV) AS YTD_LSV, G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR FROM YEAR_END_ESTIMATE_FDM
		GROUP BY G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR),
	
	NonPromo
AS
	(SELECT CT.ObjectId,
			YEAR(nps.StartDate) AS [Year],
			SUM(CASE WHEN nps.ActualCostTE IS NOT NULL THEN nps.ActualCostTE ELSE 0 END) AS ActualCostTE,
			SUM(CASE WHEN (nps.EndDate < CAST(GETDATE() AS DATE)) AND (nps.ActualCostTE IS NULL OR nps.ActualCostTE = 0) 
					THEN nps.PlanCostTE ELSE 0 END) AS PlanCostTEYTD,
			SUM(CASE WHEN nps.ActualCostTE IS NULL OR nps.ActualCostTE = 0 THEN nps.PlanCostTE ELSE 0 END) AS PlanCostTEYEE
		FROM NonPromoSupport AS nps
		LEFT JOIN ClientTree AS CT ON CT.Id = nps.ClientTreeId
		GROUP BY CT.ObjectId, YEAR(nps.StartDate))

SELECT
	NEWID() AS Id, MIN(CD.Id) AS HistoryId, pp.ClientTreeId AS ObjectId, MIN(pp.ClientHierarchy) AS ClientHierarchy, pp.BrandTechId AS BrandTechId, pp.Year AS Year,
	-- TODO: replace with join
	(SELECT [Name] FROM BrandTech WHERE BrandTech.Id = pp.BrandTechId) AS BrandTechName, 
	-- TODO: replace with join
	(SELECT MIN([LogoFileName]) FROM ProductTree WHERE ProductTree.BrandId = (SELECT BrandId FROM BrandTech WHERE BrandTech.Id = pp.BrandTechId)) AS LogoFileName,
	ROUND(SUM(pp.promoDays)/7, 0) AS PromoWeeks,
	CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) != 0)
			AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished)) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished)) IS NOT NULL 
		THEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished))/(dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share))) ELSE 0 END AS VodYTD,
	CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV)) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV)) IS NOT NULL 
			AND dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) != 0
		THEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))/dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) ELSE 0 END AS VodYEE,
	CASE WHEN (SUM(pp.ActualPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoLSV) END AS ActualPromoLSV,
	CASE WHEN (SUM(pp.PlanPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.PlanPromoLSV) END AS PlanPromoLSV,
	CASE WHEN (SUM(pp.ActualPromoCost) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoCost) END AS ActualPromoCost,
	CASE WHEN (SUM(pp.ActualPromoIncrementalEarnings) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoIncrementalEarnings) END AS ActualPromoIncrementalEarnings,
	CASE WHEN (SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost) END AS TotalPromoCost,
	CASE WHEN (SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings) IS NULL) THEN 0 ELSE
		SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings) END AS TotalPromoIncrementalEarnings,
	/* Shopper TI */
		-- From ClientDashboard
	CASE WHEN (MAX(CD.ShopperTiPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.ShopperTiPlanPercent) END AS ShopperTiPlanPercent,
		-- Plan% * PlanLSV / 100
	CASE WHEN (MAX(CD.ShopperTiPlanPercent) IS NULL OR SUM(CD.PlanLSV) IS NULL)
		THEN 0 ELSE (MAX(CD.ShopperTiPlanPercent) * SUM(CD.PlanLSV) / 100) END AS ShopperTiPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)
	CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished) END AS ShopperTiYTD,
		-- YTD / YTD with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) != 0 
			AND (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished)) != 0 
			AND (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished)) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished))/dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share))) * 100
		ELSE 0 END) AS ShopperTiYTDPercent,
		-- Sum of Actual (closed) + Sum of Plan (approved & planned & started & finished)
	CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) END AS ShopperTiYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) != 0
			AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) != 0 AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper))/(dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)))) * 100
		ELSE 0 END) AS ShopperTiYEEPercent,
	/* Marketing TI */ 
		-- From ClientDashboard
	CASE WHEN (MAX(CD.MarketingTiPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.MarketingTiPlanPercent) END AS MarketingTiPlanPercent,
		-- Plan% * PlanLSV / 100
	CASE WHEN (MAX(CD.MarketingTiPlanPercent) IS NULL OR SUM(CD.PlanLSV) IS NULL)
		THEN 0 ELSE (MAX(CD.MarketingTiPlanPercent) * SUM(CD.PlanLSV) / 100) END AS MarketingTiPlan,
		-- Promo TI cost YTD + Non promo TI cost YTD
	(CASE WHEN (SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICostFinished) IS NULL) 
			THEN 0 ELSE SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICostFinished) END
		+ CASE WHEN (SUM(np.ActualCostTE) + SUM(np.PlanCostTEYTD) IS NULL) 
			THEN 0 ELSE SUM(np.ActualCostTE) + SUM(np.PlanCostTEYTD) END) AS MarketingTiYTD,
		-- (Promo TI cost YTD + Non promo TI cost YTD) / YTD with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) != 0)
		THEN (CASE WHEN (SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICostFinished) IS NULL) 
				THEN 0 ELSE SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICostFinished) END
			+ CASE WHEN (SUM(np.ActualCostTE) + SUM(np.PlanCostTEYTD) IS NULL) 
				THEN 0 ELSE SUM(np.ActualCostTE) + SUM(np.PlanCostTEYTD) END) / dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) 
		ELSE 0 END) AS MarketingTiYTDPercent,
		-- Promo TI cost YEE + Non promo TI cost YEE
	(CASE WHEN (SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICost) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICost) END
		+ CASE WHEN (SUM(np.ActualCostTE) + SUM(np.PlanCostTEYEE) IS NULL) 
			THEN 0 ELSE SUM(np.ActualCostTE) + SUM(np.PlanCostTEYEE) END) AS MarketingTiYEE,
		-- (Promo TI cost YEE + Non promo TI cost YEE) / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) != 0)
		THEN (CASE WHEN (SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICost) IS NULL) 
				THEN 0 ELSE SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICost) END
			+ CASE WHEN (SUM(np.ActualCostTE) + SUM(np.PlanCostTEYEE) IS NULL) 
				THEN 0 ELSE SUM(np.ActualCostTE) + SUM(np.PlanCostTEYEE) END) / dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) 
		ELSE 0 END) AS MarketingTiYEEPercent,
	/* Promo TI Cost*/
		-- From ClientDashboard
	CASE WHEN (MAX(CD.PromoTiCostPlanPercent) IS NULL) 
		THEN 0 ELSE MAX(CD.PromoTiCostPlanPercent) END AS PromoTiCostPlanPercent,
		-- Plan% * PlanLSV / 100
	CASE WHEN (MAX(CD.PromoTiCostPlanPercent) IS NULL OR SUM(CD.PlanLSV) IS NULL)
		THEN 0 ELSE (MAX(CD.PromoTiCostPlanPercent) * SUM(CD.PlanLSV) / 100) END AS PromoTiCostPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)
	CASE WHEN (SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICostFinished) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICostFinished) END AS PromoTiCostYTD,
		-- YTD / YTD with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) != 0 
			AND (SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICostFinished)) != 0 
			AND (SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICostFinished)) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICostFinished))/dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share))) * 100
		ELSE 0 END) AS PromoTiCostYTDPercent,
		-- Sum of Actual (closed) + Sum of Plan (approved & planned & started & finished)
	CASE WHEN (SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICost) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICost) END AS PromoTiCostYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) != 0
			AND SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICost) != 0 
			AND SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICost) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoTICost) + SUM(pp.PlanPromoTICost))/(dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)))) * 100
		ELSE 0 END) AS PromoTiCostYEEPercent,
	/* Non Promo TI Cost */
		-- From ClientDashboard
	CASE WHEN (MAX(CD.NonPromoTiCostPlanPercent) IS NULL) 
		THEN 0 ELSE MAX(CD.NonPromoTiCostPlanPercent) END AS NonPromoTiCostPlanPercent,
		-- Plan% * PlanLSV / 100
	CASE WHEN (MAX(CD.NonPromoTiCostPlanPercent) IS NULL OR SUM(CD.PlanLSV) IS NULL)
		THEN 0 ELSE (MAX(CD.NonPromoTiCostPlanPercent) * SUM(CD.PlanLSV) / 100) END AS NonPromoTiCostPlan,
		-- Sum of Actual + Sum of Plan (end date < now & Actual = 0/null) (rules are observed in NonPromo select)
	CASE WHEN (SUM(np.ActualCostTE) + SUM(np.PlanCostTEYTD) IS NULL) 
		THEN 0 ELSE SUM(np.ActualCostTE) + SUM(np.PlanCostTEYTD) END AS NonPromoTiCostYTD,
		-- YTD / YTD with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) != 0  
			AND (SUM(np.ActualCostTE) + SUM(np.PlanCostTEYTD)) != 0 
			AND (SUM(np.ActualCostTE) + SUM(np.PlanCostTEYTD)) IS NOT NULL) 
		THEN ((SUM(np.ActualCostTE) + SUM(np.PlanCostTEYTD))/dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share))) * 100
		ELSE 0 END) AS NonPromoTiCostYTDPercent,
		-- Sum of Actual + Sum of Plan (Actual = 0/null) (rules are observed in NonPromo select)
	CASE WHEN (SUM(np.ActualCostTE) + SUM(np.PlanCostTEYEE) IS NULL) 
		THEN 0 ELSE SUM(np.ActualCostTE) + SUM(np.PlanCostTEYEE) END AS NonPromoTiCostYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) != 0
			AND SUM(np.ActualCostTE) + SUM(np.PlanCostTEYEE) != 0 
			AND SUM(np.ActualCostTE) + SUM(np.PlanCostTEYEE) IS NOT NULL) 
		THEN ((SUM(np.ActualCostTE) + SUM(np.PlanCostTEYEE))/(dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)))) * 100
		ELSE 0 END) AS NonPromoTiCostYEEPercent,
	/* Production */
		-- Plan / PlanLSV
	CASE WHEN (SUM(CD.PlanLSV) IS NULL OR SUM(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN MAX(CD.ProductionPlan)/SUM(CD.PlanLSV) IS NULL
		THEN 0 ELSE (MAX(CD.ProductionPlan)/SUM(CD.PlanLSV)) * 100 END) END AS ProductionPlanPercent,
		-- From ClientDashboard
	CASE WHEN (MAX(CD.ProductionPlan) IS NULL) THEN 0 ELSE MAX(CD.ProductionPlan) END AS ProductionPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)		
	CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished) IS NULL) THEN 0 
		ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished) END AS ProductionYTD,
		-- YTD / YTD with Share * 100
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) != 0
				AND (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished)) != 0 
				AND (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished)) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished))/dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) * 100)
		ELSE 0 END) AS ProductionYTDPercent,
		-- YTD + Sum of Plan (approved & planned & started)
	CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) END AS ProductionYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) != 0
			AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) != 0 
			AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction))/(dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)))) * 100
		ELSE 0 END) AS ProductionYEEPercent,
	/* Branding */
		-- Plan / PlanLSV
	CASE WHEN (SUM(CD.PlanLSV) IS NULL OR SUM(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (MAX(CD.BrandingPlan)/SUM(CD.PlanLSV)) IS NULL 
		THEN 0 ELSE (MAX(CD.BrandingPlan)/SUM(CD.PlanLSV) * 100) END) END AS BrandingPlanPercent,
		-- From ClientDashboard
	CASE WHEN (MAX(CD.BrandingPlan) IS NULL) THEN 0 ELSE MAX(CD.BrandingPlan) END AS BrandingPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)	
	CASE WHEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) IS NULL) 
		THEN 0 ELSE (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) END AS BrandingYTD,
		-- YTD / YTD with Share * 100
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) != 0
			AND (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) != 0 
			AND (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished))/dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share))) * 100
		ELSE 0 END) AS BrandingYTDPercent,
		-- YTD + Sum of Plan (approved & planned & started)
	CASE WHEN (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) END AS BrandingYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) != 0
			AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) != 0 AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding))/(dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)))) * 100
		ELSE 0 END) AS BrandingYEEPercent,
	/* BTL */
		-- Plan / PlanLSV
	CASE WHEN (SUM(CD.PlanLSV) IS NULL OR SUM(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (MAX(CD.BTLPlan)/SUM(CD.PlanLSV)) IS NULL 
		THEN 0 ELSE (MAX(CD.BTLPlan)/SUM(CD.PlanLSV) * 100) END) END AS BTLPlanPercent,
		-- From ClientDashboard
	CASE WHEN (MAX(CD.BTLPlan) IS NULL) THEN 0 ELSE MAX(CD.BTLPlan) END AS BTLPlan,
		-- Sum of Actual (closed) + Sum of Plan (finished)	
	CASE WHEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) IS NULL) 
		THEN 0 ELSE (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) END AS BTLYTD,
		-- YTD / YTD with Share * 100
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) != 0 
			AND (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) != 0
			AND (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished))/dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share))) * 100
		ELSE 0 END) AS BTLYTDPercent,
		-- YTD + Sum of Plan (approved & planned & started)
	CASE WHEN (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NULL) 
		THEN 0 ELSE SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) END AS BTLYEE,
		-- YEE / YEE with Share
	(CASE WHEN (dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) != 0
			AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) != 0 
			AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NOT NULL) 
		THEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL))/dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) * 100)
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
		-- ([Sum of Actual Net Incremental Earnings (closed) + Sum of Plan Net Incremental Earnings (approved, planned, started и finished)] 
		--	/ [Sum of Actual Promo Cost (closed) + Sum of Plan Promo Cost (approved, planned, started и finished)] +1 ) * 100
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
	CASE WHEN (SUM(CD.PlanLSV) IS NULL) THEN 0 ELSE SUM(CD.PlanLSV) END AS LSVPlan,
		-- YTD with Share
	dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YTD_LSV), SUM(SHARES.Share)) AS LSVYTD,
		-- YEE with Share
	dbo.ClientDashboardGetYTDwithShare(SUM(YEE.YEE_LSV), SUM(SHARES.Share)) AS LSVYEE,
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
FROM PromoParams AS pp
LEFT JOIN dbo.ClientDashboard AS CD ON CD.ClientTreeId = pp.ClientTreeId AND CD.BrandTechId = pp.BrandTechId AND CD.Year = pp.Year
LEFT JOIN NonPromo AS np ON pp.ClientTreeId = np.ObjectId AND pp.Year = np.Year
LEFT JOIN YEEF AS YEE ON YEE.YEAR = pp.Year -- TODO: replace with join
	AND YEE.BRAND_SEG_TECH_CODE = (Select BrandTech_code FROM BrandTech WHERE Id = pp.BrandTechId)
	AND YEE.G_HIERARCHY_ID LIKE dbo.ClientDashboardGetParentGHierarchyCode(pp.ClientTreeId)
LEFT JOIN ClientTreeBrandTech AS SHARES ON pp.ClientTreeKeyId = SHARES.ClientTreeId AND 
	SHARES.[BrandTechId] = pp.BrandTechId AND SHARES.[Disabled] = 0
	GROUP BY pp.ClientTreeId, pp.BrandTechId, pp.Year
GO