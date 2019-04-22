UPDATE [PromoSupport] SET 
		PlanProdCostPer1Item = CASE WHEN PlanProdCost IS NOT NULL AND PlanProdCost <> 0 AND PlanQuantity IS NOT NULL AND PlanQuantity <> 0 THEN PlanProdCost / PlanQuantity ELSE 0 END,
		ActualProdCostPer1Item = CASE WHEN ActualProdCost IS NOT NULL AND ActualProdCost <> 0 AND ActualQuantity IS NOT NULL AND ActualQuantity <> 0 THEN ActualProdCost / ActualQuantity ELSE 0 END