using Core.Settings;

namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RPASettingAddTLC : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql(
            "\n INSERT [Jupiter].[RPASetting] ([Id], [Json], [Name]) VALUES (N'E4348943-29CE-49AE-82FA-09E7CDEA25E1', N'{ \"name\": \"TLC Draft\", \"type\":\"TLC_Draft\",\"parametrs\":[], \"roles\": [\"CMManager\",\"Administrator\",\"FunctionalExpert\",\"SupportAdministrator\"], \"templateColumns\": [{\"Order\":0,\"Field\":\"PromoType\",\"Header\":\"PromoType\",\"Quoting\":false" +
            "}, {\"Order\": 1,\"Field\": \"Client\",\"Header\": \"Client\",\"Quoting\": false" +
            "}, {\"Order\": 2,\"Field\": \"Clienthierarchycode\",\"Header\": \"Clienthierarchycode\",\"Quoting\": false" +
            "}, {\"Order\": 3,\"Field\": \"BrandTech\",\"Header\": \"BrandTech\",\"Quoting\": false" +
            "}, {\"Order\": 4,\"Field\": \"Subrange\",\"Header\": \"Subrange\",\"Quoting\": false" +
            "}, {\"Order\": 5,\"Field\": \"Mechanic\",\"Header\": \"Mechanic\",\"Quoting\": false" +
            "}, {\"Order\": 6,\"Field\": \"MechanicType\",\"Header\": \"MechanicType\",\"Quoting\": false" +
            "}, {\"Order\": 7,\"Field\": \"MechanicComment\",\"Header\": \"MechanicComment\",\"Quoting\": false" +
            "}, {\"Order\": 8,\"Field\": \"Discount\",\"Header\": \"Discount\",\"Quoting\": false" +
            "}, {\"Order\": 9,\"Field\": \"PromoStartDate\",\"Header\": \"PromoStartDate\",\"Quoting\": false" +
            "}, {\"Order\": 10,\"Field\": \"PromoEndDate\",\"Header\": \"PromoEndDate\",\"Quoting\": false" +
            "}, {\"Order\": 11,\"Field\": \"BudgetYear\",\"Header\": \"BudgetYear\",\"Quoting\": false" +
            "}, {\"Order\": 12,\"Field\": \"Email\",\"Header\": \"Email\",\"Quoting\": false" +
            "}]}', N'TLC Draft Handler')" +
            "\nGO\n" +
            "INSERT [Jupiter].[RPASetting] ([Id], [Json], [Name]) VALUES (N'309E558D-56C1-4EE2-B5CF-3CB59EB451A1', N'{ \"name\": \"TLC Closed\", \"type\":\"TLC_Closed\",\"parametrs\":[], \"roles\": [\"CMManager\",\"Administrator\",\"FunctionalExpert\",\"SupportAdministrator\"], \"templateColumns\": [{\"Order\":0,\"Field\":\"PromoType\",\"Header\":\"PromoType\",\"Quoting\":false" +
            "}, {\"Order\": 1,\"Field\": \"Client\",\"Header\": \"Client\",\"Quoting\": false" +
            "}, {\"Order\": 2,\"Field\": \"Clienthierarchycode\",\"Header\": \"Clienthierarchycode\",\"Quoting\": false" +
            "}, {\"Order\": 3,\"Field\": \"BrandTech\",\"Header\": \"BrandTech\",\"Quoting\": false" +
            "}, {\"Order\": 4,\"Field\": \"Subrange\",\"Header\": \"Subrange\",\"Quoting\": false" +
            "}, {\"Order\": 5,\"Field\": \"Mechanic\",\"Header\": \"Mechanic\",\"Quoting\": false" +
            "}, {\"Order\": 6,\"Field\": \"MechanicType\",\"Header\": \"MechanicType\",\"Quoting\": false" +
            "}, {\"Order\": 7,\"Field\": \"MechanicComment\",\"Header\": \"MechanicComment\",\"Quoting\": false" +
            "}, {\"Order\": 8,\"Field\": \"Discount\",\"Header\": \"Discount\",\"Quoting\": false" +
            "}, {\"Order\": 9,\"Field\": \"PromoStartDate\",\"Header\": \"PromoStartDate\",\"Quoting\": false" +
            "}, {\"Order\": 10,\"Field\": \"PromoEndDate\",\"Header\": \"PromoEndDate\",\"Quoting\": false" +
            "}, {\"Order\": 11,\"Field\": \"BudgetYear\",\"Header\": \"BudgetYear\",\"Quoting\": false" +
            "}, {\"Order\": 12,\"Field\": \"PromoDuration\",\"Header\": \"PromoDuration\",\"Quoting\": false" +
            "}, {\"Order\": 13,\"Field\": \"PlanPromoBaselineLSV\",\"Header\": \"PlanPromoBaselineLSV\",\"Quoting\": false" +
            "}, {\"Order\": 14,\"Field\": \"PlanPromoIncrementalLSV\",\"Header\": \"PlanPromoIncrementalLSV\",\"Quoting\": false" +
            "}, {\"Order\": 15,\"Field\": \"PlanPromoLSV\",\"Header\": \"PlanPromoLSV\",\"Quoting\": false" +
            "}, {\"Order\": 16,\"Field\": \"PlanPromoUplift\",\"Header\": \"PlanPromoUplift\",\"Quoting\": false" +
            "}, {\"Order\": 17,\"Field\": \"PlanPromoTIShopper\",\"Header\": \"PlanPromoTIShopper\",\"Quoting\": false" +
            "}, {\"Order\": 18,\"Field\": \"PlanPromoTIMarketing\",\"Header\": \"PlanPromoTIMarketing\",\"Quoting\": false" +
            "}, {\"Order\": 19,\"Field\": \"PlanPromoXSites\",\"Header\": \"PlanPromoXSites\",\"Quoting\": false" +
            "}, {\"Order\": 20,\"Field\": \"PlanPromoCatalogue\",\"Header\": \"PlanPromoCatalogue\",\"Quoting\": false" +
            "}, {\"Order\": 21,\"Field\": \"PlanPromoPOSMInClient\",\"Header\": \"PlanPromoPOSMInClient\",\"Quoting\": false" +
            "}, {\"Order\": 22,\"Field\": \"PlanPromoBranding\",\"Header\": \"PlanPromoBranding\",\"Quoting\": false" +
            "}, {\"Order\": 23,\"Field\": \"PlanPromoBTL\",\"Header\": \"PlanPromoBTL\",\"Quoting\": false" +
            "}, {\"Order\": 24,\"Field\": \"PlanPromoCostProduction\",\"Header\": \"PlanPromoCostProduction\",\"Quoting\": false" +
            "}, {\"Order\": 25,\"Field\": \"PlanPromoCostProdXSites\",\"Header\": \"PlanPromoCostProdXSites\",\"Quoting\": false" +
            "}, {\"Order\": 26,\"Field\": \"PlanPromoCostProdCatalogue\",\"Header\": \"PlanPromoCostProdCatalogue\",\"Quoting\": false" +
            "}, {\"Order\": 27,\"Field\": \"PlanPromoCostProdPOSMInClient\",\"Header\": \"PlanPromoCostProdPOSMInClient\",\"Quoting\": false" +
            "}, {\"Order\": 28,\"Field\": \"PlanPromoCost\",\"Header\": \"PlanPromoCost\",\"Quoting\": false" +
            "}, {\"Order\": 29,\"Field\": \"TIBase\",\"Header\": \"TIBase\",\"Quoting\": false" +
            "}, {\"Order\": 30,\"Field\": \"PlanPromoIncrementalBaseTI\",\"Header\": \"PlanPromoIncrementalBaseTI\",\"Quoting\": false" +
            "}, {\"Order\": 31,\"Field\": \"PlanPromoNetIncrementalBaseTI\",\"Header\": \"PlanPromoNetIncrementalBaseTI\",\"Quoting\": false" +
            "}, {\"Order\": 32,\"Field\": \"COGS\",\"Header\": \"COGS\",\"Quoting\": false" +
            "}, {\"Order\": 33,\"Field\": \"COGSTn\",\"Header\": \"COGSTn\",\"Quoting\": false" +
            "}, {\"Order\": 34,\"Field\": \"PlanPromoIncrementalCOGSLSV\",\"Header\": \"PlanPromoIncrementalCOGSLSV\",\"Quoting\": false" +
            "}, {\"Order\": 35,\"Field\": \"PlanPromoNetIncrementalCOGSLSV\",\"Header\": \"PlanPromoNetIncrementalCOGSLSV\",\"Quoting\": false" +
            "}, {\"Order\": 36,\"Field\": \"PlanPromoIncrementalCOGStn\",\"Header\": \"PlanPromoIncrementalCOGStn\",\"Quoting\": false" +
            "}, {\"Order\": 37,\"Field\": \"PlanPromoNetIncrementalCOGStn\",\"Header\": \"PlanPromoNetIncrementalCOGStn\",\"Quoting\": false" +
            "}, {\"Order\": 38,\"Field\": \"PlanPromoIncrementalEarningsLSV\",\"Header\": \"PlanPromoIncrementalEarningsLSV\",\"Quoting\": false" +
            "}, {\"Order\": 39,\"Field\": \"PlanPromoNetIncrementalEarningsLSV\",\"Header\": \"PlanPromoNetIncrementalEarningsLSV\",\"Quoting\": false" +
            "}, {\"Order\": 40,\"Field\": \"ActualPromoIncrementalEarningsLSV\",\"Header\": \"ActualPromoIncrementalEarningsLSV\",\"Quoting\": false" +
            "}, {\"Order\": 41,\"Field\": \"ActualPromoNetIncrementalEarningsLSV\",\"Header\": \"ActualPromoNetIncrementalEarningsLSV\",\"Quoting\": false" +
            "}, {\"Order\": 42,\"Field\": \"PlanPromoROILSV\",\"Header\": \"PlanPromoROILSV\",\"Quoting\": false" +
            "}, {\"Order\": 43,\"Field\": \"PlanPromoNetROILSV\",\"Header\": \"PlanPromoNetROILSV\",\"Quoting\": false" +
            "}, {\"Order\": 44,\"Field\": \"ActualPromoROILSV\",\"Header\": \"ActualPromoROILSV\",\"Quoting\": false" +
            "}, {\"Order\": 45,\"Field\": \"ActualPromoNetROILSV\",\"Header\": \"ActualPromoNetROILSV\",\"Quoting\": false" +
            "}, {\"Order\": 46,\"Field\": \"PlanPromoTotalCost\",\"Header\": \"PlanPromoTotalCost\",\"Quoting\": false" +
            "}, {\"Order\": 47,\"Field\": \"PlanPostPromoEffectLSV\",\"Header\": \"PlanPostPromoEffectLSV\",\"Quoting\": false" +
            "}, {\"Order\": 48,\"Field\": \"PlanPromoNetIncrementalLSV\",\"Header\": \"PlanPromoNetIncrementalLSV\",\"Quoting\": false" +
            "}, {\"Order\": 49,\"Field\": \"PlanPromoNetLSV\",\"Header\": \"PlanPromoNetLSV\",\"Quoting\": false" +
            "}, {\"Order\": 50,\"Field\": \"PlanPromoBaseTI\",\"Header\": \"PlanPromoBaseTI\",\"Quoting\": false" +
            "}, {\"Order\": 51,\"Field\": \"PlanPromoNetBaseTI\",\"Header\": \"PlanPromoNetBaseTI\",\"Quoting\": false" +
            "}, {\"Order\": 52,\"Field\": \"PlanPromoNSV\",\"Header\": \"PlanPromoNSV\",\"Quoting\": false" +
            "}, {\"Order\": 53,\"Field\": \"PlanPromoNetNSV\",\"Header\": \"PlanPromoNetNSV\",\"Quoting\": false" +
            "}, {\"Order\": 54,\"Field\": \"PlanPromoIncrementalNSV\",\"Header\": \"PlanPromoIncrementalNSV\",\"Quoting\": false" +
            "}, {\"Order\": 55,\"Field\": \"PlanPromoNetIncrementalNSV\",\"Header\": \"PlanPromoNetIncrementalNSV\",\"Quoting\": false" +
            "}, {\"Order\": 56,\"Field\": \"PlanPromoIncrementalMAC\",\"Header\": \"PlanPromoIncrementalMAC\",\"Quoting\": false" +
            "}, {\"Order\": 57,\"Field\": \"PlanPromoIncrementalMACLSV\",\"Header\": \"PlanPromoIncrementalMACLSV\",\"Quoting\": false" +
            "}, {\"Order\": 58,\"Field\": \"PlanPromoNetIncrementalMAC\",\"Header\": \"PlanPromoNetIncrementalMAC\",\"Quoting\": false" +
            "}, {\"Order\": 59,\"Field\": \"PlanPromoNetIncrementalMACLSV\",\"Header\": \"PlanPromoNetIncrementalMACLSV\",\"Quoting\": false" +
            "}, {\"Order\": 60,\"Field\": \"PlanPromoIncrementalEarnings\",\"Header\": \"PlanPromoIncrementalEarnings\",\"Quoting\": false" +
            "}, {\"Order\": 61,\"Field\": \"PlanPromoNetIncrementalEarnings\",\"Header\": \"PlanPromoNetIncrementalEarnings\",\"Quoting\": false" +
            "}, {\"Order\": 62,\"Field\": \"PlanPromoROI\",\"Header\": \"PlanPromoROI\",\"Quoting\": false" +
            "}, {\"Order\": 63,\"Field\": \"PlanPromoNetROI\",\"Header\": \"PlanPromoNetROI\",\"Quoting\": false" +
            "}, {\"Order\": 64,\"Field\": \"PlanPromoNetUplift\",\"Header\": \"PlanPromoNetUplift\",\"Quoting\": false" +
            "}, {\"Order\": 65,\"Field\": \"PlanAddTIShopperApproved\",\"Header\": \"PlanAddTIShopperApproved\",\"Quoting\": false" +
            "}, {\"Order\": 66,\"Field\": \"PlanAddTIShopperCalculated\",\"Header\": \"PlanAddTIShopperCalculated\",\"Quoting\": false" +
            "}, {\"Order\": 67,\"Field\": \"PlanAddTIMarketingApproved\",\"Header\": \"PlanAddTIMarketingApproved\",\"Quoting\": false" +
            "}, {\"Order\": 68,\"Field\": \"ActualInStoreMechanicName\",\"Header\": \"ActualInStoreMechanicName\",\"Quoting\": false" +
            "}, {\"Order\": 69,\"Field\": \"ActualInStoreMechanicTypeName\",\"Header\": \"ActualInStoreMechanicTypeName\",\"Quoting\": false" +
            "}, {\"Order\": 70,\"Field\": \"ActualInStoreMechanicDiscount\",\"Header\": \"ActualInStoreMechanicDiscount\",\"Quoting\": false" +
            "}, {\"Order\": 71,\"Field\": \"ActualInstoreShelfPrice\",\"Header\": \"ActualInstoreShelfPrice\",\"Quoting\": false" +
            "}, {\"Order\": 72,\"Field\": \"Invoicenumber\",\"Header\": \"Invoicenumber\",\"Quoting\": false" +
            "}, {\"Order\": 73,\"Field\": \"ActualPromoBaselineLSV\",\"Header\": \"ActualPromoBaselineLSV\",\"Quoting\": false" +
            "}, {\"Order\": 74,\"Field\": \"ActualPromoIncrementalLSV\",\"Header\": \"ActualPromoIncrementalLSV\",\"Quoting\": false" +
            "}, {\"Order\": 75,\"Field\": \"ActualPromoLSVByCompensation\",\"Header\": \"ActualPromoLSVByCompensation\",\"Quoting\": false" +
            "}, {\"Order\": 76,\"Field\": \"ActualPromoLSV\",\"Header\": \"ActualPromoLSV\",\"Quoting\": false" +
            "}, {\"Order\": 77,\"Field\": \"ActualPromoUplift\",\"Header\": \"ActualPromoUplift\",\"Quoting\": false" +
            "}, {\"Order\": 78,\"Field\": \"ActualPromoNetUpliftPercent\",\"Header\": \"ActualPromoNetUpliftPercent\",\"Quoting\": false" +
            "}, {\"Order\": 79,\"Field\": \"ActualPromoTIShopper\",\"Header\": \"ActualPromoTIShopper\",\"Quoting\": false" +
            "}, {\"Order\": 80,\"Field\": \"ActualPromoTIMarketing\",\"Header\": \"ActualPromoTIMarketing\",\"Quoting\": false" +
            "}]}', N'TLC Closed Handler')" +
            "\nGO\n");
        }
        
        public override void Down()
        {
        }
    }
}
