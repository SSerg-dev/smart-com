using System;
using Core.Data;
using Module.Persist.TPM.Model.Interfaces;

namespace Module.Persist.TPM.Model.DTO
{
    public class PromoProductCorrectionView: IEntity<Guid>
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public string BrandTechName { get; set; }
        public string MarsMechanicName { get; set; }
        public string EventName { get; set; }
        public string PromoStatusSystemName { get; set; }
        public string MarsStartDate { get; set; }
        public string MarsEndDate { get; set; }
        public double? PlanProductBaselineLSV { get; set; }
        public double? PlanProductIncrementalLSV { get; set; }
        public double? PlanProductLSV { get; set; }
        public string ZREP { get; set; }
        public double? PlanProductUpliftPercentCorrected { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? ChangeDate { get; set; }
        public string UserName { get; set; }
        public TPMmode TPMmode { get; set; }
        public int? ObjectId { get; set; }
        public bool Disabled { get; set; }
        public Guid? PromoProductId { get; set; }

    }
}
