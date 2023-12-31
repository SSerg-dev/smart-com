using Core.Data;
using Module.Persist.TPM.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class Product : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Index("Unique_ZREP", 2, IsUnique = true)]
        public bool Disabled { get; set; }

        [Index("Unique_ZREP", 3, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(255)]
        [Index("Unique_ZREP", 1, IsUnique = true)]
        public string ZREP { get; set; }

        [StringLength(255)]
        public string EAN_Case { get; set; }

        [StringLength(255)]
        public string EAN_PC { get; set; }

        [StringLength(255)]
        public string ProductEN { get; set; }

        [StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SpecialNotKeyProperty]
        public string Brand { get; set; }

        [StringLength(255)]
        public string Brand_code { get; set; }

        [StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SpecialNotKeyProperty]
        public string Technology { get; set; }

        [StringLength(255)]
        public string Tech_code { get; set; }

        [StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SpecialNotKeyProperty]
        public string BrandTech { get; set; }

        [StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SpecialNotKeyProperty]
        public string BrandTech_code { get; set; }

        [StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SpecialNotKeyProperty]
        public string Brandsegtech { get; set; }

        [StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SpecialNotKeyProperty]
        public string BrandsegTech_code { get; set; }

        [StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SpecialNotKeyProperty]
        public string BrandsegTechsub { get; set; }

        [StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SpecialNotKeyProperty]
        public string BrandsegTechsub_code { get; set; }

        [StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SpecialNotKeyProperty]
        public string SubBrand { get; set; }

        [StringLength(255)]
        public string SubBrand_code { get; set; }

        [StringLength(255)]
        public string BrandFlagAbbr { get; set; }

        [StringLength(255)]
        public string BrandFlag { get; set; }

        [StringLength(255)]
        public string SubmarkFlag { get; set; }

        [StringLength(255)]
        public string IngredientVariety { get; set; }

        [StringLength(255)]
        public string ProductCategory { get; set; }

        [StringLength(255)]
        public string ProductType { get; set; }

        [StringLength(255)]
        public string MarketSegment { get; set; }

        [StringLength(255)]
        public string Segmen_code { get; set; }

        [StringLength(255)]
        public string SupplySegment { get; set; }

        [StringLength(255)]
        public string FunctionalVariety { get; set; }

        [StringLength(255)]
        public string Size { get; set; }

        [StringLength(255)]
        public string BrandEssence { get; set; }

        [StringLength(255)]
        public string PackType { get; set; }

        [StringLength(255)]
        public string GroupSize { get; set; }

        [StringLength(255)]
        public string TradedUnitFormat { get; set; }

        [StringLength(255)]
        public string ConsumerPackFormat { get; set; }

        public int? UOM_PC2Case { get; set; }

        public int? Division { get; set; }

        public double? NetWeight { get; set; }

        [StringLength(255)]
        public string UOM { get; set; }

        public double? CaseVolume { get; set; }

        public double? PCVolume { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public ICollection<AssortmentMatrix> AssortmentMatrices { get; set; }
        public ICollection<BaseLine> BaseLines { get; set; }
        public ICollection<IncreaseBaseLine> IncreaseBaseLines { get; set; }
        public ICollection<IncrementalPromo> IncrementalPromoes { get; set; }
        public ICollection<PreviousDayIncremental> PreviousDayIncrementals { get; set; }
        public ICollection<PriceList> PriceLists { get; set; }
        public ICollection<ProductChangeIncident> ProductChangeIncidents { get; set; }
        public ICollection<PromoProduct> PromoProducts { get; set; }
        public ICollection<RollingVolume> RollingVolumes { get; set; }
    }
}
