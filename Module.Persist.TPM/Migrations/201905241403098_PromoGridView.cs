namespace Module.Persist.TPM.Migrations {
    using System;
    using System.Data.Entity.Migrations;

    public partial class PromoGridView : DbMigration {
        public override void Up() {
            Sql("CREATE VIEW [PromoGridView] " +
                "AS SELECT " +
                "pr.[Id], " +
                "pr.[Name]," +
                "pr.[Number]," +
                "pr.[Disabled], " +
                "pr.[Mechanic], " +
                "pr.[CreatorId], " +
                "pr.[MechanicIA], " +
                "pr.[ClientTreeId], " +
                "pr.[ClientHierarchy], " +
                "pr.[MarsMechanicDiscount], " +
                "pr.[IsDemandFinanceApproved], " +
                "pr.[IsDemandPlanningApproved], " +
                "pr.[IsCustomerMarketingApproved], " +
                "pr.[PlanInstoreMechanicDiscount], " +
                "pr.[EndDate], " +
                "pr.[StartDate], " +
                "pr.[DispatchesEnd], " +
                "pr.[DispatchesStart], " +
                "pr.[MarsEndDate], " +
                "pr.[MarsStartDate], " +
                "pr.[MarsDispatchesEnd], " +
                "pr.[MarsDispatchesStart], " +
                "bnd.[Name] as BrandName, " +
                "bt.[Name] as BrandTechName, " +
                "ev.[Name] as PromoEventName, " +
                "ps.[Name] as PromoStatusName, " +
                "ps.[Color] as PromoStatusColor, " +
                "mmc.[Name] as MarsMechanicName, " +
                "mmt.[Name] as MarsMechanicTypeName, " +
                "pim.[Name] as PlanInstoreMechanicName, " +
                "ps.[SystemName] as PromoStatusSystemName, " +
                "pimt.[Name] as PlanInstoreMechanicTypeName, " +
                "pr.[PlanPromoTIShopper], " +
                "pr.[PlanPromoTIMarketing], " +
                "pr.[PlanPromoXSites], " +
                "pr.[PlanPromoCatalogue], " +
                "pr.[PlanPromoPOSMInClient], " +
                "pr.[ActualPromoUpliftPercent], " +
                "pr.[ActualPromoTIShopper], " +
                "pr.[ActualPromoTIMarketing], " +
                "pr.[ActualPromoXSites], " +
                "pr.[ActualPromoCatalogue], " +
                "pr.[ActualPromoPOSMInClient], " +
                "pr.[PlanPromoUpliftPercent], " +
                "pr.[PlanPromoROIPercent], " +
                "pr.[ActualPromoNetIncrementalNSV], " +
                "pr.[ActualPromoIncrementalNSV], " +
                "pr.[ActualPromoROIPercent], " +
                "pr.[ProductHierarchy], " +
                "pr.[PlanPromoNetIncrementalNSV], " +
                "pr.[PlanPromoIncrementalNSV] " +
                "FROM [Promo] pr " +
                "LEFT JOIN [Event] ev ON pr.EventId = ev.Id " +
                "LEFT JOIN [Brand] bnd ON pr.BrandId = bnd.Id " +
                "LEFT JOIN [BrandTech] bt ON pr.BrandTechId = bt.Id " +
                "LEFT JOIN [PromoStatus] ps ON pr.PromoStatusId = ps.Id " +
                "LEFT JOIN [Mechanic] mmc ON pr.MarsMechanicId = mmc.Id " +
                "LEFT JOIN [Mechanic] pim ON pr.PlanInstoreMechanicId = pim.Id " +
                "LEFT JOIN [MechanicType] mmt ON pr.MarsMechanicTypeId = mmt.Id " +
                "LEFT JOIN [MechanicType] pimt ON pr.PlanInstoreMechanicTypeId = pimt.Id ");
        }

        public override void Down() {
            Sql("DROP VIEW [PromoGridView]");
        }
    }
}
