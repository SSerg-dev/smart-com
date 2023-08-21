using Core.Import;
using Module.Persist.TPM.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportRpaTLCclosed : BaseImportEntity
    {
        private DateTimeOffset startDate;
        private DateTimeOffset endDate;
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Promo Type")]
        public string PromoType { get; set; }
        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Client")]
        public string Client { get; set; }
        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Client hierarchy code")]
        public int ClientHierarchyCode { get; set; }
        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "BrandTech")]
        public string BrandTech { get; set; }
        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Subrange")]
        public string Subrange { get; set; }
        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Mechanic")]
        public string Mechanic { get; set; }
        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Mechanic Type")]
        public string MechanicType { get; set; }
        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "Mechanic comment")]
        public string MechanicComment { get; set; }
        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "Discount %")]
        public int Discount { get; set; }
        [ImportCSVColumn(ColumnNumber = 9)]
        [Display(Name = "Promo Start Date")]
        public DateTimeOffset PromoStartDate
        {
            get { return startDate; }
            set { startDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }
        [ImportCSVColumn(ColumnNumber = 10)]
        [Display(Name = "Promo End Date")]
        public DateTimeOffset PromoEndDate
        {
            get { return endDate; }
            set { endDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }
        [ImportCSVColumn(ColumnNumber = 11)]
        [Display(Name = "Budget Year")]
        public int BudgetYear { get; set; }
        [ImportCSVColumn(ColumnNumber = 12)]
        [Display(Name = "Promo duration")]
        public int PromoDuration { get; set; }
        [ImportCSVColumn(ColumnNumber = 13)]
        [Display(Name = "Plan Promo Baseline LSV")]
        public double PlanPromoBaselineLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 14)]
        [Display(Name = "Plan Promo Incremental LSV")]
        public double PlanPromoIncrementalLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 15)]
        [Display(Name = "Plan Promo LSV")]
        public double PlanPromoLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 16)]
        [Display(Name = "Plan Promo Uplift %")]
        public double? PlanPromoUpliftPercent { get; set; }
        [ImportCSVColumn(ColumnNumber = 17)]
        [Display(Name = "Plan Promo TI Shopper")]
        public double PlanPromoTIShopper { get; set; }
        [ImportCSVColumn(ColumnNumber = 18)]
        [Display(Name = "Plan Promo TI Marketing")]
        public double? PlanPromoTIMarketing { get; set; }
        [ImportCSVColumn(ColumnNumber = 19)]
        [Display(Name = "Plan Promo X-Sites")]
        public double? PlanPromoXSites { get; set; }
        [ImportCSVColumn(ColumnNumber = 20)]
        [Display(Name = "Plan Promo Catalogue")]
        public double? PlanPromoCatalogue { get; set; }
        [ImportCSVColumn(ColumnNumber = 21)]
        [Display(Name = "Plan Promo POSM In Client")]
        public double? PlanPromoPOSMInClient { get; set; }
        [ImportCSVColumn(ColumnNumber = 22)]
        [Display(Name = "Plan Promo Branding")]
        public double PlanPromoBranding { get; set; }
        [ImportCSVColumn(ColumnNumber = 23)]
        [Display(Name = "Plan Promo BTL")]
        public double PlanPromoBTL { get; set; }
        [ImportCSVColumn(ColumnNumber = 24)]
        [Display(Name = "Plan Promo Cost Production")]
        public double? PlanPromoCostProduction { get; set; }
        [ImportCSVColumn(ColumnNumber = 25)]
        [Display(Name = "Plan PromoCostProdXSites")]
        public double? PlanPromoCostProdXSites { get; set; }
        [ImportCSVColumn(ColumnNumber = 26)]
        [Display(Name = "Plan PromoCostProdCatalogue")]
        public double? PlanPromoCostProdCatalogue { get; set; }
        [ImportCSVColumn(ColumnNumber = 27)]
        [Display(Name = "Plan PromoCostProdPOSMInClient")]
        public double? PlanPromoCostProdPOSMInClient { get; set; }
        [ImportCSVColumn(ColumnNumber = 28)]
        [Display(Name = "Plan Promo Cost")]
        public double PlanPromoCost { get; set; }
        [ImportCSVColumn(ColumnNumber = 29)]
        [Display(Name = "TI Base")]
        public double TIBase { get; set; }
        [ImportCSVColumn(ColumnNumber = 30)]
        [Display(Name = "Plan Promo Incremental Base TI")]
        public double PlanPromoIncrementalBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 31)]
        [Display(Name = "Plan Promo Net Incremental Base TI")]
        public double PlanPromoNetIncrementalBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 32)]
        [Display(Name = "COGS")]
        public double COGS { get; set; }
        [ImportCSVColumn(ColumnNumber = 33)]
        [Display(Name = "COGS/Tn")]
        public double COGSTn { get; set; }
        [ImportCSVColumn(ColumnNumber = 34)]
        [Display(Name = "Plan Promo Incremental COGS LSV")]
        public double PlanPromoIncrementalCOGSLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 35)]
        [Display(Name = "Plan Promo Net Incremental COGS LSV")]
        public double PlanPromoNetIncrementalCOGSLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 36)]
        [Display(Name = "Plan Promo Incremental COGS/tn")]
        public double PlanPromoIncrementalCOGStn { get; set; }
        [ImportCSVColumn(ColumnNumber = 37)]
        [Display(Name = "Plan Promo Net Incremental COGS/tn")]
        public double PlanPromoNetIncrementalCOGStn { get; set; }
        [ImportCSVColumn(ColumnNumber = 38)]
        [Display(Name = "Plan Promo Incremental Earnings LSV")]
        public double PlanPromoIncrementalEarningsLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 39)]
        [Display(Name = "Plan Promo Net Incremental Earnings LSV")]
        public double PlanPromoNetIncrementalEarningsLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 40)]
        [Display(Name = "Actual Promo Incremental Earnings LSV")]
        public double ActualPromoIncrementalEarningsLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 41)]
        [Display(Name = "Actual Promo Net Incremental Earnings LSV")]
        public double ActualPromoNetIncrementalEarningsLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 42)]
        [Display(Name = "Plan Promo ROI % LSV")]
        public double PlanPromoROILSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 43)]
        [Display(Name = "Plan Promo Net ROI % LSV")]
        public double PlanPromoNetROILSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 44)]
        [Display(Name = "Actual Promo ROI % LSV")]
        public double ActualPromoROILSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 45)]
        [Display(Name = "Actual Promo Net ROI % LSV")]
        public double ActualPromoNetROILSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 46)]
        [Display(Name = "Plan Promo Total Cost")]
        public double PlanPromoTotalCost { get; set; }
        [ImportCSVColumn(ColumnNumber = 47)]
        [Display(Name = "Plan Post Promo Effect LSV")]
        public double PlanPostPromoEffectLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 48)]
        [Display(Name = "Plan Promo Net Incremental LSV")]
        public double PlanPromoNetIncrementalLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 49)]
        [Display(Name = "PlanPromo Net LSV")]
        public double PlanPromoNetLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 50)]
        [Display(Name = "Plan Promo Baseline Base TI")]
        public double PlanPromoBaselineBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 51)]
        [Display(Name = "Plan Promo Base TI")]
        public double PlanPromoBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 52)]
        [Display(Name = "Plan Promo Net Base TI")]
        public double PlanPromoNetBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 53)]
        [Display(Name = "Plan Promo NSV")]
        public double PlanPromoNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 54)]
        [Display(Name = "Plan Promo Net NSV")]
        public double PlanPromoNetNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 55)]
        [Display(Name = "Plan Promo Incremental NSV")]
        public double PlanPromoIncrementalNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 56)]
        [Display(Name = "Plan Promo Net Incremental NSV")]
        public double PlanPromoNetIncrementalNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 57)]
        [Display(Name = "Plan Promo Incremental MAC")]
        public double PlanPromoIncrementalMAC { get; set; }
        [ImportCSVColumn(ColumnNumber = 58)]
        [Display(Name = "Plan Promo Incremental MAC LSV")]
        public double PlanPromoIncrementalMACLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 59)]
        [Display(Name = "Plan Promo Net Incremental MAC")]
        public double PlanPromoNetIncrementalMAC { get; set; }
        [ImportCSVColumn(ColumnNumber = 60)]
        [Display(Name = "Plan Promo Net Incremental MAC LSV")]
        public double PlanPromoNetIncrementalMACLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 61)]
        [Display(Name = "Plan Promo Incremental Earnings")]
        public double PlanPromoIncrementalEarnings { get; set; }
        [ImportCSVColumn(ColumnNumber = 62)]
        [Display(Name = "Plan Promo Net Incremental Earnings")]
        public double PlanPromoNetIncrementalEarnings { get; set; }
        [ImportCSVColumn(ColumnNumber = 63)]
        [Display(Name = "Plan Promo ROI, %")]
        public double PlanPromoROI { get; set; }
        [ImportCSVColumn(ColumnNumber = 64)]
        [Display(Name = "Plan Promo Net ROI, %")]
        public double PlanPromoNetROI { get; set; }
        [ImportCSVColumn(ColumnNumber = 65)]
        [Display(Name = "Plan Promo Net Uplift %")]
        public double PlanPromoNetUplift { get; set; }
        [ImportCSVColumn(ColumnNumber = 66)]
        [Display(Name = "Plan Add TI Shopper Approved")]
        public double PlanAddTIShopperApproved { get; set; }
        [ImportCSVColumn(ColumnNumber = 67)]
        [Display(Name = "Plan Add TI Shopper Calculated")]
        public double PlanAddTIShopperCalculated { get; set; }
        [ImportCSVColumn(ColumnNumber = 68)]
        [Display(Name = "Plan Add TI Marketing Approved")]
        public double? PlanAddTIMarketingApproved { get; set; }
        [ImportCSVColumn(ColumnNumber = 69)]
        [Display(Name = "Actual InStore Mechanic Name")]
        public string ActualInStoreMechanicName { get; set; }
        [ImportCSVColumn(ColumnNumber = 70)]
        [Display(Name = "Actual InStore Mechanic Type Name")]
        public string ActualInStoreMechanicTypeName { get; set; }
        [ImportCSVColumn(ColumnNumber = 71)]
        [Display(Name = "Actual InStore Mechanic Discount")]
        public double ActualInStoreMechanicDiscount { get; set; }
        [ImportCSVColumn(ColumnNumber = 72)]
        [Display(Name = "Actual Instore Shelf Price")]
        public double ActualInstoreShelfPrice { get; set; }
        [ImportCSVColumn(ColumnNumber = 73)]
        [Display(Name = "Invoice number")]
        public string Invoicenumber { get; set; }
        [ImportCSVColumn(ColumnNumber = 74)]
        [Display(Name = "Actual Promo Baseline LSV")]
        public double ActualPromoBaselineLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 75)]
        [Display(Name = "Actual Promo Incremental LSV")]
        public double ActualPromoIncrementalLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 76)]
        [Display(Name = "Actual PromoLSV By Compensation")]
        public double ActualPromoLSVByCompensation { get; set; }
        [ImportCSVColumn(ColumnNumber = 77)]
        [Display(Name = "Actual Promo LSV")]
        public double ActualPromoLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 78)]
        [Display(Name = "Actual Promo Uplift %")]
        public double? ActualPromoUplift { get; set; }
        [ImportCSVColumn(ColumnNumber = 79)]
        [Display(Name = "Actual Promo Net Uplift Percent")]
        public double? ActualPromoNetUpliftPercent { get; set; }
        [ImportCSVColumn(ColumnNumber = 80)]
        [Display(Name = "Actual Promo TI Shopper")]
        public double ActualPromoTIShopper { get; set; }
        [ImportCSVColumn(ColumnNumber = 81)]
        [Display(Name = "Actual Promo TI Marketing")]
        public double? ActualPromoTIMarketing { get; set; }
        [ImportCSVColumn(ColumnNumber = 82)]
        [Display(Name = "Actual Promo Prod XSites")]
        public double? ActualPromoProdXSites { get; set; }
        [ImportCSVColumn(ColumnNumber = 83)]
        [Display(Name = "Actual Promo Prod Catalogue")]
        public double? ActualPromoProdCatalogue { get; set; }
        [ImportCSVColumn(ColumnNumber = 84)]
        [Display(Name = "Actual Promo Prod POSMInClient")]
        public double? ActualPromoProdPOSMInClient { get; set; }
        [ImportCSVColumn(ColumnNumber = 85)]
        [Display(Name = "Actual Promo Branding")]
        public double ActualPromoBranding { get; set; }
        [ImportCSVColumn(ColumnNumber = 86)]
        [Display(Name = "Actual Promo BTL")]
        public double ActualPromoBTL { get; set; }
        [ImportCSVColumn(ColumnNumber = 87)]
        [Display(Name = "Actual Promo Cost Production")]
        public double? ActualPromoCostProduction { get; set; }
        [ImportCSVColumn(ColumnNumber = 88)]
        [Display(Name = "Actual Promo CostProdXSites")]
        public double? ActualPromoCostProdXSites { get; set; }
        [ImportCSVColumn(ColumnNumber = 89)]
        [Display(Name = "Actual Promo Cost ProdCatalogue")]
        public double? ActualPromoCostProdCatalogue { get; set; }
        [ImportCSVColumn(ColumnNumber = 90)]
        [Display(Name = "Actual Promo Cost ProdPOSMInClient")]
        public double? ActualPromoCostProdPOSMInClient { get; set; }
        [ImportCSVColumn(ColumnNumber = 91)]
        [Display(Name = "Actual Promo Cost")]
        public double ActualPromoCost { get; set; }
        [ImportCSVColumn(ColumnNumber = 92)]
        [Display(Name = "Actual Promo Incremental BaseTI")]
        public double ActualPromoIncrementalBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 93)]
        [Display(Name = "Actual Promo Net Incremental BaseTI")]
        public double ActualPromoNetIncrementalBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 94)]
        [Display(Name = "Actual Promo Incremental COGS LSV")]
        public double ActualPromoIncrementalCOGSLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 95)]
        [Display(Name = "Actual Promo Net Incremental COGS LSV")]
        public double ActualPromoNetIncrementalCOGSLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 96)]
        [Display(Name = "Actual Promo Incremental COGS/tn")]
        public double ActualPromoIncrementalCOGStn { get; set; }
        [ImportCSVColumn(ColumnNumber = 97)]
        [Display(Name = "Actual Promo Net Incremental COGS/tn")]
        public double ActualPromoNetIncrementalCOGStn { get; set; }
        [ImportCSVColumn(ColumnNumber = 98)]
        [Display(Name = "Actual Promo Total Cost")]
        public double ActualPromoTotalCost { get; set; }
        [ImportCSVColumn(ColumnNumber = 99)]
        [Display(Name = "Actual Post Promo Effect LSV")]
        public double ActualPostPromoEffectLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 100)]
        [Display(Name = "Actual Promo Net Incremental LSV")]
        public double ActualPromoNetIncrementalLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 101)]
        [Display(Name = "Actual Promo Net LSV")]
        public double ActualPromoNetLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 102)]
        [Display(Name = "Actual Promo Incremental NSV")]
        public double ActualPromoIncrementalNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 103)]
        [Display(Name = "Actual Promo Net Incremental NSV")]
        public double ActualPromoNetIncrementalNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 104)]
        [Display(Name = "Actual Promo Baseline Base TI")]
        public double ActualPromoBaselineBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 105)]
        [Display(Name = "Actual Promo Base TI")]
        public double ActualPromoBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 106)]
        [Display(Name = "Actual Promo Net Base TI")]
        public double ActualPromoNetBaseTI { get; set; }
        [ImportCSVColumn(ColumnNumber = 107)]
        [Display(Name = "Actual Promo NSV")]
        public double ActualPromoNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 108)]
        [Display(Name = "Actual Promo Net NSV")]
        public double ActualPromoNetNSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 109)]
        [Display(Name = "Actual Promo Incremental MAC")]
        public double ActualPromoIncrementalMAC { get; set; }
        [ImportCSVColumn(ColumnNumber = 110)]
        [Display(Name = "Actual Promo Incremental MAC LSV")]
        public double ActualPromoIncrementalMACLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 111)]
        [Display(Name = "Actual Promo Net Incremental MAC")]
        public double ActualPromoNetIncrementalMAC { get; set; }
        [ImportCSVColumn(ColumnNumber = 112)]
        [Display(Name = "Actual Promo Net Incremental MAC LSV")]
        public double ActualPromoNetIncrementalMACLSV { get; set; }
        [ImportCSVColumn(ColumnNumber = 113)]
        [Display(Name = "Actual Promo Incremental Earnings")]
        public double ActualPromoIncrementalEarnings { get; set; }
        [ImportCSVColumn(ColumnNumber = 114)]
        [Display(Name = "Actual Promo Net Incremental Earnings")]
        public double ActualPromoNetIncrementalEarnings { get; set; }
        [ImportCSVColumn(ColumnNumber = 115)]
        [Display(Name = "Actual Promo ROI, %")]
        public double ActualPromoROI { get; set; }
        [ImportCSVColumn(ColumnNumber = 116)]
        [Display(Name = "Actual Promo Net ROI%")]
        public double ActualPromoNetROI { get; set; }
        [ImportCSVColumn(ColumnNumber = 117)]
        [Display(Name = "Actual Add TI Shopper")]
        public double ActualAddTIShopper { get; set; }
        [ImportCSVColumn(ColumnNumber = 118)]
        [Display(Name = "Actual Add TI Marketing")]
        public double ActualAddTIMarketing { get; set; }
        [ImportCSVColumn(ColumnNumber = 119)]
        [Display(Name = "Sum In Invoice")]
        public double SumInInvoice { get; set; }

    }
}
