﻿ALTER VIEW [PromoGridView] AS SELECT  
pr.Id, pr.Name, pr.Number, pr.Disabled, pr.Mechanic, pr.CreatorId, pr.MechanicIA, pr.ClientTreeId, pr.ClientHierarchy, pr.MarsMechanicDiscount, pr.IsDemandFinanceApproved, pr.IsDemandPlanningApproved, 
pr.IsCMManagerApproved, pr.PlanInstoreMechanicDiscount, pr.EndDate, pr.StartDate, pr.DispatchesEnd, pr.DispatchesStart, pr.MarsEndDate, pr.MarsStartDate, pr.MarsDispatchesEnd, pr.MarsDispatchesStart, 
bnd.Name AS BrandName, bt.Name AS BrandTechName, ev.Name AS PromoEventName, ps.Name AS PromoStatusName, ps.Color AS PromoStatusColor, mmc.Name AS MarsMechanicName, 
mmt.Name AS MarsMechanicTypeName, pim.Name AS PlanInstoreMechanicName, ps.SystemName AS PromoStatusSystemName, pimt.Name AS PlanInstoreMechanicTypeName, pr.PlanPromoTIShopper, 
pr.PlanPromoTIMarketing, pr.PlanPromoXSites, pr.PlanPromoCatalogue, pr.PlanPromoPOSMInClient, pr.ActualPromoUpliftPercent, pr.ActualPromoTIShopper, pr.ActualPromoTIMarketing, pr.ActualPromoXSites, 
pr.ActualPromoCatalogue, pr.ActualPromoPOSMInClient, pr.PlanPromoUpliftPercent, pr.PlanPromoROIPercent, pr.ActualPromoNetIncrementalNSV, pr.ActualPromoIncrementalNSV, pr.ActualPromoROIPercent, pr.ProductHierarchy, 
pr.PlanPromoNetIncrementalNSV, pr.PlanPromoIncrementalNSV, pr.InOut
FROM            
dbo.Promo AS pr LEFT OUTER JOIN
dbo.Event AS ev ON pr.EventId = ev.Id LEFT OUTER JOIN
dbo.Brand AS bnd ON pr.BrandId = bnd.Id LEFT OUTER JOIN
dbo.BrandTech AS bt ON pr.BrandTechId = bt.Id LEFT OUTER JOIN
dbo.PromoStatus AS ps ON pr.PromoStatusId = ps.Id LEFT OUTER JOIN
dbo.Mechanic AS mmc ON pr.MarsMechanicId = mmc.Id LEFT OUTER JOIN
dbo.Mechanic AS pim ON pr.PlanInstoreMechanicId = pim.Id LEFT OUTER JOIN
dbo.MechanicType AS mmt ON pr.MarsMechanicTypeId = mmt.Id LEFT OUTER JOIN
dbo.MechanicType AS pimt ON pr.PlanInstoreMechanicTypeId = pimt.Id