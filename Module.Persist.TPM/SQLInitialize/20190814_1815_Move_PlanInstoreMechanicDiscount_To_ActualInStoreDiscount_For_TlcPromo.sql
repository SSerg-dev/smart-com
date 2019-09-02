UPDATE [dbo].[Promo]
   SET ActualInStoreDiscount = PlanInstoreMechanicDiscount,
        PlanInstoreMechanicDiscount = NULL
where PlanInstoreMechanicDiscount IS NOT NULL
	AND LoadFromTLC = 1
GO
