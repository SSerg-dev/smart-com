using System;
using Core.Data;

namespace Module.Persist.TPM.Model.DTO {
    public class PromoView : IEntity<Guid> {
        public Guid Id { get; set; }

        public string BrandTechName { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string EventName { get; set; }

        public string MarsMechanicName { get; set; }
        public double? MarsMechanicDiscount { get; set; }
        public string MarsMechanicTypeName { get; set; }

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
    }
}
