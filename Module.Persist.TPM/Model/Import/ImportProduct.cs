using Core.Import;
using Module.Persist.TPM.Model.TPM;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.TPM
{
    public class ImportProduct : BaseImportEntity {

        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "ZREP")]
        public string ZREP { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "EAN Case")]
        public string EAN_Case { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "EAN PC")]
        public string EAN_PC { get; set; }

       
        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Product EN")]
        public string ProductEN { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
		[Display(Name = "Brand code")]
        public string Brand—ode { get; set; }
        public string Brand_code
        {
            get
            {
                return this.Brand—ode.TrimStart('0').PadLeft(3, '0');
            }
            set { }
        }

        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Technology")]
        public string Technology { get; set; }

        [ImportCSVColumn(ColumnNumber = 7)]
		[Display(Name = "Technology code")]
		public string Tech—ode { get; set; }
        public string Tech_code
        {
            get
            {
                return this.Tech—ode.TrimStart('0').PadLeft(3, '0');
            }
            set { }
        }

        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "Brand Tech")]
        public string BrandTech { get; set; }

        [ImportCSVColumn(ColumnNumber = 9)]
        [Display(Name = "Brand Tech code")]
        public string BrandTech_code { get; set; }

        [ImportCSVColumn(ColumnNumber = 10)]
		[Display(Name = "Segmen code")]
		public string Segmen—ode { get; set; }
        public string Segmen_code
        {
            get
            {
                return this.Segmen—ode.TrimStart('0').PadLeft(2, '0');
            }
            set { }
        }

        [ImportCSVColumn(ColumnNumber = 11)]
        [Display(Name = "Brand Seg Tech Code")]
        public string BrandsegTech_code { get; set; }

        [ImportCSVColumn(ColumnNumber = 12)]
        [Display(Name = "Brand Seg Tech")]
        public string Brandsegtech { get; set; }

        [ImportCSVColumn(ColumnNumber = 13)]
        [Display(Name = "Brand Seg Tech Sub Code")]
        public string BrandsegTechsub_code { get; set; }

        [ImportCSVColumn(ColumnNumber = 14)]
        [Display(Name = "Brand Seg Tech Sub")]
        public string BrandsegTechsub { get; set; }

        [ImportCSVColumn(ColumnNumber = 15)]
        [Display(Name = "Sub Code")]
        public string SubBrand_code { get; set; }

        [ImportCSVColumn(ColumnNumber = 16)]
        [Display(Name = "Sub")]
        public string SubBrand { get; set; }

        [ImportCSVColumn(ColumnNumber = 17)]
        [Display(Name = "Brand flag abbr")]
        public string BrandFlagAbbr { get; set; }

        [ImportCSVColumn(ColumnNumber = 18)]
        [Display(Name = "Brand flag")]
        public string BrandFlag { get; set; }

        [ImportCSVColumn(ColumnNumber = 19)]
        [Display(Name = "Submark flag")]
        public string SubmarkFlag { get; set; }

        [ImportCSVColumn(ColumnNumber = 20)]
        [Display(Name = "Ingredient variety")]
        public string IngredientVariety { get; set; }

        [ImportCSVColumn(ColumnNumber = 21)]
        [Display(Name = "Product category")]
        public string ProductCategory { get; set; }

        [ImportCSVColumn(ColumnNumber = 22)]
        [Display(Name = "Product type")]
        public string ProductType { get; set; }

        [ImportCSVColumn(ColumnNumber = 23)]
        [Display(Name = "Market segment")]
        public string MarketSegment { get; set; }

        [ImportCSVColumn(ColumnNumber = 24)]
        [Display(Name = "Supply segment")]
        public string SupplySegment { get; set; }

        [ImportCSVColumn(ColumnNumber = 25)]
        [Display(Name = "Functional variety")]
        public string FunctionalVariety { get; set; }

        [ImportCSVColumn(ColumnNumber = 26)]
        [Display(Name = "Size")]
        public string Size { get; set; }

        [ImportCSVColumn(ColumnNumber = 27)]
        [Display(Name = "Brand essence")]
        public string BrandEssence { get; set; }

        [ImportCSVColumn(ColumnNumber = 28)]
        [Display(Name = "Pack type")]
        public string PackType { get; set; }

        [ImportCSVColumn(ColumnNumber = 29)]
        [Display(Name = "Group size")]
        public string GroupSize { get; set; }

        [ImportCSVColumn(ColumnNumber = 30)]
        [Display(Name = "Traded unit format")]
        public string TradedUnitFormat { get; set; }

        [ImportCSVColumn(ColumnNumber = 31)]
        [Display(Name = "Consumer pack format")]
        public string ConsumerPackFormat { get; set; }

        [ImportCSVColumn(ColumnNumber = 32)]
        [Display(Name = "UOM_PC2Case")]
        public int? UOM_PC2Case { get; set; }

		[ImportCSVColumn(ColumnNumber = 33)]
		[Display(Name = "Division")]
		public int? Division { get; set; }
	}
}
