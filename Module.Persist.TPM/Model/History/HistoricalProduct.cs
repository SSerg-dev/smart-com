using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(Product))]
    public class HistoricalProduct : BaseHistoricalEntity<System.Guid>
    {
        public string ZREP { get; set; }
        public string EAN_Case { get; set; }
        public string EAN_PC { get; set; }
        public string ProductEN { get; set; }
		public string Brand { get; set; }
		public string Brand_code { get; set; }
		public string Technology { get; set; }
		public string Tech_code { get; set; }
		public string BrandTech { get; set; }
		public string BrandTech_code { get; set; }
		public string Segmen_code { get; set; }
		public string BrandsegTech_code { get; set; }
        public string Brandsegtech{ get; set; }
        public string BrandsegTechsub_code { get; set; }
        public string BrandsegTechsub { get; set; }
        public string SubBrand_code { get; set; }
        public string SubBrand { get; set; }
        public string BrandFlagAbbr { get; set; }
        public string BrandFlag { get; set; }
        public string SubmarkFlag { get; set; }
        public string IngredientVariety { get; set; }
        public string ProductCategory { get; set; }
        public string ProductType { get; set; }
        public string MarketSegment { get; set; }
        public string SupplySegment { get; set; }
        public string FunctionalVariety { get; set; }
        public string Size { get; set; }
        public string BrandEssence { get; set; }
        public string PackType { get; set; }
        public string GroupSize { get; set; }
        public string TradedUnitFormat { get; set; }
        public string ConsumerPackFormat { get; set; }

        public int? UOM_PC2Case { get; set; }
		public int? Division { get; set; }
	}
}