using Core.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportRpaTLCclosed : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Promo Type")]
        public int PromoType { get; set; }
        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Client")]
        public int Client { get; set; }
        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Client hierarchy code")]
        public int ClientHierarchyCode { get; set; }
        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "BrandTech")]
        public int BrandTech { get; set; }
        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Subrange")]
        public int Subrange { get; set; }
        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Mechanic")]
        public int Mechanic { get; set; }
        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Mechanic Type")]
        public int MechanicType { get; set; }
        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "Mechanic comment")]
        public int MechanicComment { get; set; }
        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "Discount %")]
        public int Discount { get; set; }
        [ImportCSVColumn(ColumnNumber = 9)]
        [Display(Name = "Promo Start Date")]
        public int PromoStartDate { get; set; }
        [ImportCSVColumn(ColumnNumber = 10)]
        [Display(Name = "Promo End Date")]
        public int PromoEndDate { get; set; }
        [ImportCSVColumn(ColumnNumber = 11)]
        [Display(Name = "Budget Year")]
        public int BudgetYear { get; set; }
        [ImportCSVColumn(ColumnNumber = 12)]
        [Display(Name = "Promo duration")]
        public int PromoDuration { get; set; }
        [ImportCSVColumn(ColumnNumber = 13)]
        [Display(Name = "Plan Promo Baseline LSV")]
        public int PlanPromoBaselineLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 14)]
        [Display(Name = "Plan Promo Incremental LSV")]
        public int PlanPromoIncrementalLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 15)]
        [Display(Name = "Plan Promo LSV")]
        public int PlanPromoLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 16)]
        [Display(Name = "Plan Promo Uplift %")]
        public int PlanPromoUpliftPercent { get; set; }
        [ImportCSVColumn(ColumnNumber = 17)]
        [Display(Name = "Plan Promo TI Shopper")]
        public int PlanPromoTIShopper { get; set; }
        [ImportCSVColumn(ColumnNumber = 18)]
        [Display(Name = "Plan Promo TI Marketing")]
        public int PlanPromoTIMarketing { get; set; }
        [ImportCSVColumn(ColumnNumber = 19)]
        [Display(Name = "Plan Promo X-Sites")]
        public int PlanPromoXSites { get; set; }
        [ImportCSVColumn(ColumnNumber = 20)]
        [Display(Name = "Plan Promo Catalogue")]
        public int PlanPromoCatalogue { get; set; }
        [ImportCSVColumn(ColumnNumber = 21)]
        [Display(Name = "Plan Promo POSM In Client")]
        public int PlanPromoPOSMInClient { get; set; }
        [ImportCSVColumn(ColumnNumber = 22)]
        [Display(Name = "Plan Promo Branding")]
        public int PlanPromoBranding { get; set; }
        [ImportCSVColumn(ColumnNumber = 23)]
        [Display(Name = "Plan Promo BTL")]
        public int PlanPromoBTL { get; set; }
        [ImportCSVColumn(ColumnNumber = 24)]
        [Display(Name = "Plan Promo Cost Production")]
        public int PlanPromoCostProduction { get; set; }
        [ImportCSVColumn(ColumnNumber = 25)]
        [Display(Name = "Plan PromoCostProdXSites")]
        public int PlanPromoCostProdXSites { get; set; }
        [ImportCSVColumn(ColumnNumber = 26)]
        [Display(Name = "Plan PromoCostProdCatalogue")]
        public int PlanPromoCostProdCatalogue { get; set; }
        [ImportCSVColumn(ColumnNumber = 27)]
        [Display(Name = "Plan PromoCostProdPOSMInClient")]
        public int PlanPromoCostProdPOSMInClient { get; set; }
        [ImportCSVColumn(ColumnNumber = 28)]
        [Display(Name = "Plan Promo Cost")]
        public int PlanPromoCost { get; set; }
        [ImportCSVColumn(ColumnNumber = 29)]
        [Display(Name = "TI Base")]
        public int TIBase { get; set; }
        [ImportCSVColumn(ColumnNumber = 30)]
        [Display(Name = "Plan Promo Incremental Base TI")]
        public int PlanPromoIncrementalBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 31)]
        [Display(Name = "Plan Promo Net Incremental Base TI")]
        public int PlanPromoNetIncrementalBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 32)]
        [Display(Name = "COGS")]
        public int COGS { get; set; }
        [ImportCSVColumn(ColumnNumber = 33)]
        [Display(Name = "COGS/Tn")]
        public int COGSTn { get; set; }
        [ImportCSVColumn(ColumnNumber = 34)]
        [Display(Name = "Plan Promo Incremental COGS LSV")]
        public int PlanPromoIncrementalCOGSLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 35)]
        [Display(Name = "Plan Promo Net Incremental COGS LSV")]
        public int PlanPromoNetIncrementalCOGSLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 36)]
        [Display(Name = "Plan Promo Incremental COGS/tn")]
        public int PlanPromoIncrementalCOGStn { get; set; }
        [ImportCSVColumn(ColumnNumber = 37)]
        [Display(Name = "Plan Promo Net Incremental COGS/tn")]
        public int PlanPromoNetIncrementalCOGStn { get; set; }
        [ImportCSVColumn(ColumnNumber = 38)]
        [Display(Name = "Plan Promo Incremental Earnings LSV")]
        public int PlanPromoIncrementalEarningsLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 39)]
        [Display(Name = "Plan Promo Net Incremental Earnings LSV")]
        public int PlanPromoNetIncrementalEarningsLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 40)]
        [Display(Name = "Actual Promo Incremental Earnings LSV")]
        public int ActualPromoIncrementalEarningsLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 41)]
        [Display(Name = "Actual Promo Net Incremental Earnings LSV")]
        public int ActualPromoNetIncrementalEarningsLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 42)]
        [Display(Name = "Plan Promo ROI % LSV")]
        public int PlanPromoROILSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 43)]
        [Display(Name = "Plan Promo Net ROI % LSV")]
        public int PlanPromoNetROILSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 44)]
        [Display(Name = "Actual Promo ROI % LSV")]
        public int ActualPromoROILSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 45)]
        [Display(Name = "Actual Promo Net ROI % LSV")]
        public int ActualPromoNetROILSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 46)]
        [Display(Name = "Plan Promo Total Cost")]
        public int PlanPromoTotalCost { get; set; }
        [ImportCSVColumn(ColumnNumber = 47)]
        [Display(Name = "Plan Post Promo Effect LSV")]
        public int PlanPostPromoEffectLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 48)]
        [Display(Name = "Plan Promo Net Incremental LSV")]
        public int PlanPromoNetIncrementalLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 49)]
        [Display(Name = "PlanPromo Net LSV")]
        public int PlanPromoNetLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 50)]
        [Display(Name = "Plan Promo Baseline Base TI")]
        public int PlanPromoBaselineBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 51)]
        [Display(Name = "Plan Promo Base TI")]
        public int PlanPromoBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 52)]
        [Display(Name = "Plan Promo Net Base TI")]
        public int PlanPromoNetBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 53)]
        [Display(Name = "Plan Promo NSV")]
        public int PlanPromoNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 54)]
        [Display(Name = "Plan Promo Net NSV")]
        public int PlanPromoNetNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 55)]
        [Display(Name = "Plan Promo Incremental NSV")]
        public int PlanPromoIncrementalNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 56)]
        [Display(Name = "Plan Promo Net Incremental NSV")]
        public int PlanPromoNetIncrementalNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 57)]
        [Display(Name = "Plan Promo Incremental MAC")]
        public int PlanPromoIncrementalMAC { get; set; }
        [ImportCSVColumn(ColumnNumber = 58)]
        [Display(Name = "Plan Promo Incremental MAC LSV")]
        public int PlanPromoIncrementalMACLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 59)]
        [Display(Name = "Plan Promo Net Incremental MAC")]
        public int PlanPromoNetIncrementalMAC { get; set; }
        [ImportCSVColumn(ColumnNumber = 60)]
        [Display(Name = "Plan Promo Net Incremental MAC LSV")]
        public int PlanPromoNetIncrementalMACLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 61)]
        [Display(Name = "Plan Promo Incremental Earnings")]
        public int PlanPromoIncrementalEarnings { get; set; }
        [ImportCSVColumn(ColumnNumber = 62)]
        [Display(Name = "Plan Promo Net Incremental Earnings")]
        public int PlanPromoNetIncrementalEarnings { get; set; }
        [ImportCSVColumn(ColumnNumber = 63)]
        [Display(Name = "Plan Promo ROI, %")]
        public int PlanPromoROI { get; set; }
        [ImportCSVColumn(ColumnNumber = 64)]
        [Display(Name = "Plan Promo Net ROI, %")]
        public int PlanPromoNetROI { get; set; }
        [ImportCSVColumn(ColumnNumber = 65)]
        [Display(Name = "Plan Promo Net Uplift %")]
        public int PlanPromoNetUplift { get; set; }
        [ImportCSVColumn(ColumnNumber = 66)]
        [Display(Name = "Plan Add TI Shopper Approved")]
        public int PlanAddTIShopperApproved { get; set; }
        [ImportCSVColumn(ColumnNumber = 67)]
        [Display(Name = "Plan Add TI Shopper Calculated")]
        public int PlanAddTIShopperCalculated { get; set; }
        [ImportCSVColumn(ColumnNumber = 68)]
        [Display(Name = "Plan Add TI Marketing Approved")]
        public int PlanAddTIMarketingApproved { get; set; }
        [ImportCSVColumn(ColumnNumber = 69)]
        [Display(Name = "Actual InStore Mechanic Name")]
        public int ActualInStoreMechanicName { get; set; }
        [ImportCSVColumn(ColumnNumber = 70)]
        [Display(Name = "Actual InStore Mechanic Type Name")]
        public int ActualInStoreMechanicTypeName { get; set; }
        [ImportCSVColumn(ColumnNumber = 71)]
        [Display(Name = "Actual InStore Mechanic Discount")]
        public int ActualInStoreMechanicDiscount { get; set; }
        [ImportCSVColumn(ColumnNumber = 72)]
        [Display(Name = "Actual Instore Shelf Price")]
        public int ActualInstoreShelfPrice { get; set; }
        [ImportCSVColumn(ColumnNumber = 73)]
        [Display(Name = "Invoice number")]
        public int Invoicenumber { get; set; }
        [ImportCSVColumn(ColumnNumber = 74)]
        [Display(Name = "Actual Promo Baseline LSV")]
        public int ActualPromoBaselineLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 75)]
        [Display(Name = "Actual Promo Incremental LSV")]
        public int ActualPromoIncrementalLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 76)]
        [Display(Name = "Actual PromoLSV By Compensation")]
        public int ActualPromoLSVByCompensation { get; set; }
        [ImportCSVColumn(ColumnNumber = 77)]
        [Display(Name = "Actual Promo LSV")]
        public int ActualPromoLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 78)]
        [Display(Name = "Actual Promo Uplift %")]
        public int ActualPromoUplift { get; set; }
        [ImportCSVColumn(ColumnNumber = 79)]
        [Display(Name = "Actual Promo Net Uplift Percent")]
        public int ActualPromoNetUpliftPercent { get; set; }
        [ImportCSVColumn(ColumnNumber = 80)]
        [Display(Name = "Actual Promo TI Shopper")]
        public int ActualPromoTIShopper { get; set; }
        [ImportCSVColumn(ColumnNumber = 81)]
        [Display(Name = "Actual Promo TI Marketing")]
        public int ActualPromoTIMarketing { get; set; }
        [ImportCSVColumn(ColumnNumber = 82)]
        [Display(Name = "Actual Promo Prod XSites")]
        public int ActualPromoProdXSites { get; set; }
        [ImportCSVColumn(ColumnNumber = 83)]
        [Display(Name = "Actual Promo Prod Catalogue")]
        public int ActualPromoProdCatalogue { get; set; }
        [ImportCSVColumn(ColumnNumber = 84)]
        [Display(Name = "Actual Promo Prod POSMInClient")]
        public int ActualPromoProdPOSMInClient { get; set; }
        [ImportCSVColumn(ColumnNumber = 85)]
        [Display(Name = "Actual Promo Branding")]
        public int ActualPromoBranding { get; set; }
        [ImportCSVColumn(ColumnNumber = 86)]
        [Display(Name = "Actual Promo BTL")]
        public int ActualPromoBTL { get; set; }
        [ImportCSVColumn(ColumnNumber = 87)]
        [Display(Name = "Actual Promo Cost Production")]
        public int ActualPromoCostProduction { get; set; }
        [ImportCSVColumn(ColumnNumber = 88)]
        [Display(Name = "Actual Promo CostProdXSites")]
        public int ActualPromoCostProdXSites { get; set; }
        [ImportCSVColumn(ColumnNumber = 89)]
        [Display(Name = "Actual Promo Cost ProdCatalogue")]
        public int ActualPromoCostProdCatalogue { get; set; }
        [ImportCSVColumn(ColumnNumber = 90)]
        [Display(Name = "Actual Promo Cost ProdPOSMInClient")]
        public int ActualPromoCostProdPOSMInClient { get; set; }
        [ImportCSVColumn(ColumnNumber = 91)]
        [Display(Name = "Actual Promo Cost")]
        public int ActualPromoCost { get; set; }
        [ImportCSVColumn(ColumnNumber = 92)]
        [Display(Name = "Actual Promo Incremental BaseTI")]
        public int ActualPromoIncrementalBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 93)]
        [Display(Name = "Actual Promo Net Incremental BaseTI")]
        public int ActualPromoNetIncrementalBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 94)]
        [Display(Name = "Actual Promo Incremental COGS LSV")]
        public int ActualPromoIncrementalCOGSLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 95)]
        [Display(Name = "Actual Promo Net Incremental COGS LSV")]
        public int ActualPromoNetIncrementalCOGSLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 96)]
        [Display(Name = "Actual Promo Incremental COGS/tn")]
        public int ActualPromoIncrementalCOGStn { get; set; }
        [ImportCSVColumn(ColumnNumber = 97)]
        [Display(Name = "Actual Promo Net Incremental COGS/tn")]
        public int ActualPromoNetIncrementalCOGStn { get; set; }
        [ImportCSVColumn(ColumnNumber = 98)]
        [Display(Name = "Actual Promo Total Cost")]
        public int ActualPromoTotalCost { get; set; }
        [ImportCSVColumn(ColumnNumber = 99)]
        [Display(Name = "Actual Post Promo Effect LSV")]
        public int ActualPostPromoEffectLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 100)]
        [Display(Name = "Actual Promo Net Incremental LSV")]
        public int ActualPromoNetIncrementalLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 101)]
        [Display(Name = "Actual Promo Net LSV")]
        public int ActualPromoNetLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 102)]
        [Display(Name = "Actual Promo Incremental NSV")]
        public int ActualPromoIncrementalNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 103)]
        [Display(Name = "Actual Promo Net Incremental NSV")]
        public int ActualPromoNetIncrementalNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 104)]
        [Display(Name = "Actual Promo Baseline Base TI")]
        public int ActualPromoBaselineBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 105)]
        [Display(Name = "Actual Promo Base TI")]
        public int ActualPromoBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 106)]
        [Display(Name = "Actual Promo Net Base TI")]
        public int ActualPromoNetBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 107)]
        [Display(Name = "Actual Promo NSV")]
        public int ActualPromoNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 108)]
        [Display(Name = "Actual Promo Net NSV")]
        public int ActualPromoNetNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 109)]
        [Display(Name = "Actual Promo Incremental MAC")]
        public int ActualPromoIncrementalMAC { get; set; }
        [ImportCSVColumn(ColumnNumber = 110)]
        [Display(Name = "Actual Promo Incremental MAC LSV")]
        public int ActualPromoIncrementalMACLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 111)]
        [Display(Name = "Actual Promo Net Incremental MAC")]
        public int ActualPromoNetIncrementalMAC { get; set; }
        [ImportCSVColumn(ColumnNumber = 112)]
        [Display(Name = "Actual Promo Net Incremental MAC LSV")]
        public int ActualPromoNetIncrementalMACLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 113)]
        [Display(Name = "Actual Promo Incremental Earnings")]
        public int ActualPromoIncrementalEarnings { get; set; }
        [ImportCSVColumn(ColumnNumber = 114)]
        [Display(Name = "Actual Promo Net Incremental Earnings")]
        public int ActualPromoNetIncrementalEarnings { get; set; }
        [ImportCSVColumn(ColumnNumber = 115)]
        [Display(Name = "Actual Promo ROI, %")]
        public int ActualPromoROI { get; set; }
        [ImportCSVColumn(ColumnNumber = 116)]
        [Display(Name = "Actual Promo Net ROI%")]
        public int ActualPromoNetROI { get; set; }
        [ImportCSVColumn(ColumnNumber = 117)]
        [Display(Name = "Actual Add TI Shopper")]
        public int ActualAddTIShopper { get; set; }
        [ImportCSVColumn(ColumnNumber = 118)]
        [Display(Name = "Actual Add TI Marketing")]
        public int ActualAddTIMarketing { get; set; }
        [ImportCSVColumn(ColumnNumber = 119)]
        [Display(Name = "Sum In Invoice")]
        public int SumInInvoice { get; set; }

    }
}
