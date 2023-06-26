using System;
using Core.Data;
using Module.Persist.TPM.Model.Interfaces;

namespace Module.Persist.TPM.Model.DTO {
    public class PromoView : IEntity<Guid> {
        public Guid Id { get; set; }
        public bool Disabled { get; set; }

        public TPMmode TPMmode { get; set; }
        public string BrandTechName { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string EventName { get; set; }

        public string MarsMechanicName { get; set; }
        public double? MarsMechanicDiscount { get; set; }
        public string MarsMechanicTypeName { get; set; }
        public string MechanicComment { get; set; }

        public string CompetitorName { get; set; }
        public string CompetitorBrandTechName { get; set; }

        public string ColorSystemName { get; set; }
        public string PromoStatusColor { get; set; }
        public string PromoStatusSystemName { get; set; }
        public string PromoStatusName { get; set; }

        public Guid? CreatorId { get; set; }

        public int? ClientTreeId { get; set; }
        public string BaseClientTreeIds { get; set; }

        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public DateTimeOffset? DispatchesStart { get; set; }

        public int? CalendarPriority { get; set; }

        public bool? InOut { get; set; }
        public bool IsGrowthAcceleration { get; set; }
        public bool IsInExchange { get; set; }
        public Guid? MasterPromoId { get; set; }
        public string TypeName { get; set; }
        public string TypeGlyph { get; set; }
        public bool IsOnInvoice { get; set; }
        public double DeviationCoefficient { get; set; }

        public double Price { get; set; }
        public double Discount { get; set; }

        public string Subranges { get; set; }
        public bool IsOnHold { get; set; }
        public int? BudgetYear { get; set; }
    }

    public class PromoRSView : IEntity<Guid> 
    {
        public Guid Id { get; set; }
        public bool Disabled { get; set; }

        public TPMmode TPMmode { get; set; }
        public string BrandTechName { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string EventName { get; set; }

        public string MarsMechanicName { get; set; }
        public double? MarsMechanicDiscount { get; set; }
        public string MarsMechanicTypeName { get; set; }
        public string MechanicComment { get; set; }

        public string CompetitorName { get; set; }
        public string CompetitorBrandTechName { get; set; }

        public string ColorSystemName { get; set; }
        public string PromoStatusColor { get; set; }
        public string PromoStatusSystemName { get; set; }
        public string PromoStatusName { get; set; }

        public Guid? CreatorId { get; set; }

        public int? ClientTreeId { get; set; }
        public string BaseClientTreeIds { get; set; }

        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public DateTimeOffset? DispatchesStart { get; set; }

        public int? CalendarPriority { get; set; }

        public bool? InOut { get; set; }
        public bool IsGrowthAcceleration { get; set; }
        public bool IsInExchange { get; set; }
        public Guid? MasterPromoId { get; set; }
        public string TypeName { get; set; }
        public string TypeGlyph { get; set; }
        public bool IsOnInvoice { get; set; }
        public double DeviationCoefficient { get; set; }

        public double Price { get; set; }
        public double Discount { get; set; }

        public string Subranges { get; set; }
        public bool IsOnHold { get; set; }
        public int? BudgetYear { get; set; }
    }
}
