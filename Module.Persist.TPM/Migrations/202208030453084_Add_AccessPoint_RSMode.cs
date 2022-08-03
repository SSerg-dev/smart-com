namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AccessPoint_RSMode : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
        }

        private string SqlString = @"
            UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AccessPoint' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AccessPointRoles' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AccessPointRoles' AND Action = 'GetAccessPointRoles'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AccessPointRoles' AND Action = 'GetAccessPointRole'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AccessPointRoles' AND Action = 'GetAccessPoint'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AccessPointRoles' AND Action = 'GetRole'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AccessPoints' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AccessPoints' AND Action = 'GetAccessPoints'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AccessPoints' AND Action = 'GetAccessPoint'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualCOGSs' AND Action = 'GetActualCOGSs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualCOGSs' AND Action = 'GetActualCOGS'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualCOGSs' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualCOGSs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualCOGSTns' AND Action = 'GetActualCOGSTn'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualCOGSTns' AND Action = 'GetActualCOGSTns'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualCOGSTns' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualCOGSTns' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualCOGSTns' AND Action = 'DownloadTemplateXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualLSVs' AND Action = 'GetActualLSVs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualLSVs' AND Action = 'GetActualLSV'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualLSVs' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualLSVs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Actuals' AND Action = 'GetActual'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Actuals' AND Action = 'GetActuals'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Actuals' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualTradeInvestments' AND Action = 'GetActualTradeInvestments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualTradeInvestments' AND Action = 'GetActualTradeInvestment'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualTradeInvestments' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ActualTradeInvestments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AdUserDTOs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AdUsers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AdUsers' AND Action = 'GetAdUsers'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AgeGroups' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AgeGroups' AND Action = 'GetAgeGroup'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AgeGroups' AND Action = 'GetAgeGroups'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AgeGroups' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AssortmentMatrices' AND Action = 'GetAssortmentMatrices'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AssortmentMatrices' AND Action = 'GetAssortmentMatrix'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AssortmentMatrices' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'AssortmentMatrices' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BaseClients' AND Action = 'GetBaseClients'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BaseClients' AND Action = 'GetBaseClient'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BaseClientTreeViews' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BaseClientTreeViews' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BaseClientTreeViews' AND Action = 'GetBaseClientTreeViews'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BaseLines' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BaseLines' AND Action = 'GetBaseLine'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BaseLines' AND Action = 'GetBaseLines'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BaseLines' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BaseLines' AND Action = 'FullImportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Brands' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Brands' AND Action = 'GetBrand'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Brands' AND Action = 'GetBrands'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Brands' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BrandTeches' AND Action = 'GetBrandTechById'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BrandTeches' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BrandTeches' AND Action = 'GetBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BrandTeches' AND Action = 'GetBrandTeches'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BrandTeches' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLPromoes' AND Action = 'GetPromoesWithBTL'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLPromoes' AND Action = 'GetBTLPromoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLPromoes' AND Action = 'GetBTLPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLPromoes' AND Action = 'BTLPromoPost'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLPromoes' AND Action = 'Patch'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLPromoes' AND Action = 'Patch'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLPromoes' AND Action = 'Put'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLPromoes' AND Action = 'Post'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLPromoes' AND Action = 'Delete'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLPromoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLs' AND Action = 'GetBTLs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLs' AND Action = 'GetBTL'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLs' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BTLs' AND Action = 'GetEventBTL'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetItems' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetItems' AND Action = 'GetBudgetItem'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetItems' AND Action = 'GetBudgetItems'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetItems' AND Action = 'GetBudget'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetItems' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Budgets' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Budgets' AND Action = 'GetBudget'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Budgets' AND Action = 'GetBudgets'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Budgets' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetSubItemClientTrees' AND Action = 'GetBudgetSubItemClientTrees'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetSubItemClientTrees' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetSubItems' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetSubItems' AND Action = 'GetBudgetSubItem'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetSubItems' AND Action = 'GetBudgetSubItems'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'BudgetSubItems' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Categories' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Categories' AND Action = 'GetCategory'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Categories' AND Action = 'GetCategories'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Categories' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientDashboardViews' AND Action = 'GetClientDashboardViews'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientDashboardViews' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientDashboardViews' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientDashboardViews' AND Action = 'GetAllYEEF'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Clients' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Clients' AND Action = 'GetClient'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Clients' AND Action = 'GetClients'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Clients' AND Action = 'GetRegion'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Clients' AND Action = 'GetCommercialSubnet'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Clients' AND Action = 'GetDistributor'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Clients' AND Action = 'GetStoreType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Clients' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTreeBrandTeches' AND Action = 'GetClientTreeBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTreeBrandTeches' AND Action = 'GetClientTreeBrandTeches'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTreeBrandTeches' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTreeBrandTeches' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTrees' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTrees' AND Action = 'GetClientTree'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTrees' AND Action = 'GetClientTrees'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTrees' AND Action = 'GetHierarchyDetail'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTrees' AND Action = 'GetClientTreeByObjectId'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTreeSharesViews' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTreeSharesViews' AND Action = 'GetClientTreeSharesViews'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ClientTreeSharesViews' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CoefficientSI2SOs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CoefficientSI2SOs' AND Action = 'GetCoefficientSI2SOs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CoefficientSI2SOs' AND Action = 'GetCoefficientSI2SO'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CoefficientSI2SOs' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'COGSs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'COGSs' AND Action = 'GetCOGSs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'COGSs' AND Action = 'GetCOGS'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'COGSs' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Colors' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Colors' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Colors' AND Action = 'GetColor'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Colors' AND Action = 'GetColors'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Colors' AND Action = 'GetSuitable'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CommercialNets' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CommercialNets' AND Action = 'GetCommercialNet'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CommercialNets' AND Action = 'GetCommercialNets'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CommercialNets' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CommercialSubnets' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CommercialSubnets' AND Action = 'GetCommercialSubnet'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CommercialSubnets' AND Action = 'GetCommercialSubnets'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CommercialSubnets' AND Action = 'GetCommercialNet'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CommercialSubnets' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CompetitorBrandTechs' AND Action = 'DownloadTemplateXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CompetitorBrandTechs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CompetitorBrandTechs' AND Action = 'GetCompetitorBrandTechs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CompetitorBrandTechs' AND Action = 'GetCompetitorBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CompetitorBrandTechs' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CompetitorPromoes' AND Action = 'DownloadTemplateXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CompetitorPromoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CompetitorPromoes' AND Action = 'GetCompetitorPromoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CompetitorPromoes' AND Action = 'GetCompetitorPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CompetitorPromoes' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Competitors' AND Action = 'DownloadTemplateXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Competitors' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Competitors' AND Action = 'GetCompetitors'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Competitors' AND Action = 'GetCompetitor'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Competitors' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Constraints' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Constraints' AND Action = 'GetConstraints'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Constraints' AND Action = 'GetConstraint'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Constraints' AND Action = 'GetUserRole'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CSVExtractInterfaceSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CSVExtractInterfaceSettings' AND Action = 'GetCSVExtractInterfaceSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CSVExtractInterfaceSettings' AND Action = 'GetCSVExtractInterfaceSetting'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CSVExtractInterfaceSettings' AND Action = 'GetInterface'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CSVProcessInterfaceSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CSVProcessInterfaceSettings' AND Action = 'GetCSVProcessInterfaceSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CSVProcessInterfaceSettings' AND Action = 'GetCSVProcessInterfaceSetting'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'CSVProcessInterfaceSettings' AND Action = 'GetInterface'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedAccessPoints' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedAccessPoints' AND Action = 'GetDeletedAccessPoints'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedAccessPoints' AND Action = 'GetDeletedAccessPoint'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedActualCOGSs' AND Action = 'GetDeletedActualCOGSs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedActualCOGSs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedActualCOGSTns' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedActualCOGSTns' AND Action = 'GetDeletedActualCOGSTns'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedActualCOGSTns' AND Action = 'GetDeletedActualCOGSTn'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedActuals' AND Action = 'GetDeletedActual'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedActuals' AND Action = 'GetDeletedActuals'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedActualTradeInvestments' AND Action = 'GetDeletedActualTradeInvestments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedActualTradeInvestments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedAgeGroups' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedAgeGroups' AND Action = 'GetDeletedAgeGroup'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedAgeGroups' AND Action = 'GetDeletedAgeGroups'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedAssortmentMatrices' AND Action = 'GetDeletedAssortmentMatrix'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedAssortmentMatrices' AND Action = 'GetDeletedAssortmentMatrices'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedAssortmentMatrices' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBaseLine' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBaseLines' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBaseLines' AND Action = 'GetDeletedBaseLine'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBaseLines' AND Action = 'GetDeletedBaseLines'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBrands' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBrands' AND Action = 'GetDeletedBrand'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBrands' AND Action = 'GetDeletedBrands'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBrandTeches' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBrandTeches' AND Action = 'GetDeletedBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBrandTeches' AND Action = 'GetDeletedBrandTeches'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBTLs' AND Action = 'GetDeletedBTLs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBTLs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBudgetItems' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBudgetItems' AND Action = 'GetDeletedBudgetItem'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBudgetItems' AND Action = 'GetDeletedBudgetItems'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBudgetItems' AND Action = 'GetBudget'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBudgets' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBudgets' AND Action = 'GetDeletedBudget'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBudgets' AND Action = 'GetDeletedBudgets'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBudgetSubItems' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBudgetSubItems' AND Action = 'GetDeletedBudgetSubItem'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedBudgetSubItems' AND Action = 'GetDeletedBudgetSubItems'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCategories' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCategories' AND Action = 'GetDeletedCategory'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCategories' AND Action = 'GetDeletedCategories'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedClients' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedClients' AND Action = 'GetDeletedClient'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedClients' AND Action = 'GetDeletedClients'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedClients' AND Action = 'GetRegion'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedClients' AND Action = 'GetCommercialSubnet'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedClients' AND Action = 'GetDistributor'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedClients' AND Action = 'GetStoreType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCoefficientSI2SOs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCoefficientSI2SOs' AND Action = 'GetDeletedCoefficientSI2SOs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCOGSs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCOGSs' AND Action = 'GetDeletedCOGSs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCOGSs' AND Action = 'GetDeletedCOGS'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedColors' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedColors' AND Action = 'GetDeletedColors'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedColors' AND Action = 'GetDeletedColor'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCommercialNets' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCommercialNets' AND Action = 'GetDeletedCommercialNet'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCommercialNets' AND Action = 'GetDeletedCommercialNets'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCommercialSubnets' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCommercialSubnets' AND Action = 'GetDeletedCommercialSubnet'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCommercialSubnets' AND Action = 'GetDeletedCommercialSubnets'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCommercialSubnets' AND Action = 'GetCommercialNet'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCompetitorBrandTechs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCompetitorBrandTechs' AND Action = 'GetDeletedCompetitorBrandTechs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCompetitorBrandTechs' AND Action = 'GetDeletedCompetitorBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCompetitorPromoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCompetitorPromoes' AND Action = 'GetDeletedCompetitorPromoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCompetitorPromoes' AND Action = 'GetDeletedCompetitorPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCompetitors' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCompetitors' AND Action = 'GetDeletedCompetitors'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedCompetitors' AND Action = 'GetDeletedCompetitor'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedConstraints' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedDemands' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedDemands' AND Action = 'GetDeletedDemands'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedDemands' AND Action = 'GetDeletedDemand'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedDistributors' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedDistributors' AND Action = 'GetDeletedDistributor'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedDistributors' AND Action = 'GetDeletedDistributors'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedEvents' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedEvents' AND Action = 'GetDeletedEvent'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedEvents' AND Action = 'GetDeletedEvents'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedFormats' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedFormats' AND Action = 'GetDeletedFormat'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedFormats' AND Action = 'GetDeletedFormats'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedIncrementalPromoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedMailNotificationSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedMailNotificationSettings' AND Action = 'GetDeletedMailNotificationSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedMailNotificationSettings' AND Action = 'GetDeletedMailNotificationSetting'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedMechanics' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedMechanics' AND Action = 'GetDeletedMechanic'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedMechanics' AND Action = 'GetDeletedMechanics'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedMechanicTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedMechanicTypes' AND Action = 'GetDeletedMechanicType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedMechanicTypes' AND Action = 'GetDeletedMechanicTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNodeTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNodeTypes' AND Action = 'GetDeletedNodeType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNodeTypes' AND Action = 'GetDeletedNodeTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNoneNegoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNoneNegoes' AND Action = 'GetDeletedNoneNego'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNoneNegoes' AND Action = 'GetDeletedNoneNegoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNonPromoEquipment' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNonPromoEquipments' AND Action = 'GetDeletedNonPromoEquipments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNonPromoEquipments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNonPromoSupports' AND Action = 'GetDeletedNonPromoSupports'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedNonPromoSupports' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPlanCOGSTns' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPlanCOGSTns' AND Action = 'GetDeletedPlanCOGSTns'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPlanCOGSTns' AND Action = 'GetDeletedPlanCOGSTn'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPostPromoEffects' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPostPromoEffects' AND Action = 'GetDeletedPostPromoEffect'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPostPromoEffects' AND Action = 'GetDeletedPostPromoEffects'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetDeletedProduct'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetDeletedProducts'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetCategory'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetBrand'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetSegment'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetTechnology'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetTechHighLevel'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetProgram'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetFormat'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetSubrange'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetAgeGroup'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedProducts' AND Action = 'GetVariety'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPrograms' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPrograms' AND Action = 'GetDeletedProgram'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPrograms' AND Action = 'GetDeletedPrograms'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoDemands' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoDemands' AND Action = 'GetDeletedPromoDemand'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoDemands' AND Action = 'GetDeletedPromoDemands'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoDemands' AND Action = 'GetBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoDemands' AND Action = 'GetMechanic'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoes' AND Action = 'GetDeletedPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoes' AND Action = 'GetDeletedPromoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoes' AND Action = 'GetClient'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoes' AND Action = 'GetBrand'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoes' AND Action = 'GetBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoes' AND Action = 'GetProduct'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoes' AND Action = 'GetPromoStatus'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoes' AND Action = 'GetMechanic'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoGridViews' AND Action = 'GetDeletedPromoGridViews'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoGridViews' AND Action = 'GetDeletedPromoGridView'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoProducts' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoProducts' AND Action = 'GetDeletedPromoProduct'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoProducts' AND Action = 'GetDeletedPromoProducts'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoProductsCorrections' AND Action = 'GetDeletedPromoProductsCorrections'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoProductsCorrections' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoSaleses' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoSaleses' AND Action = 'GetDeletedPromoSaleses'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoSaleses' AND Action = 'GetDeletedPromoSales'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoStatuss' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoStatuss' AND Action = 'GetDeletedPromoStatus'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoStatuss' AND Action = 'GetDeletedPromoStatuss'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoSupports' AND Action = 'GetDeletedPromoSupports'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoSupports' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoTypes' AND Action = 'GetDeletedPromoTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedPromoTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRATIShoppers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRATIShoppers' AND Action = 'GetDeletedRATIShoppers'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRATIShoppers' AND Action = 'GetDeletedRATIShopper'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRegions' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRegions' AND Action = 'GetDeletedRegion'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRegions' AND Action = 'GetDeletedRegions'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRejectReasons' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRejectReasons' AND Action = 'GetDeletedRejectReason'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRejectReasons' AND Action = 'GetDeletedRejectReasons'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRetailTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRetailTypes' AND Action = 'GetDeletedRetailType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRetailTypes' AND Action = 'GetDeletedRetailTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedRoles' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSales' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSales' AND Action = 'GetDeletedSale'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSales' AND Action = 'GetDeletedSales'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSales' AND Action = 'GetPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSales' AND Action = 'GetBudget'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSales' AND Action = 'GetBudgetItem'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSegments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSegments' AND Action = 'GetDeletedSegment'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSegments' AND Action = 'GetDeletedSegments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedStoreTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedStoreTypes' AND Action = 'GetDeletedStoreType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedStoreTypes' AND Action = 'GetDeletedStoreTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSubranges' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSubranges' AND Action = 'GetDeletedSubrange'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedSubranges' AND Action = 'GetDeletedSubranges'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedTechHighLevels' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedTechHighLevels' AND Action = 'GetDeletedTechHighLevel'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedTechHighLevels' AND Action = 'GetDeletedTechHighLevels'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedTechnologies' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedTechnologies' AND Action = 'GetDeletedTechnology'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedTechnologies' AND Action = 'GetDeletedTechnologies'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedTradeInvestments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedTradeInvestments' AND Action = 'GetDeletedTradeInvestments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedTradeInvestments' AND Action = 'GetDeletedTradeInvestment'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedUserRoles' AND Action = 'GetDeletedUserRoles'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedUserRoles' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedUsers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedUsers' AND Action = 'GetDeletedUsers'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedUsers' AND Action = 'GetDeletedUser'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedVarieties' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedVarieties' AND Action = 'GetDeletedVariety'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'DeletedVarieties' AND Action = 'GetDeletedVarieties'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Demands' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Demands' AND Action = 'GetDemand'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Demands' AND Action = 'GetDemands'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Demands' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Distributors' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Distributors' AND Action = 'GetDistributor'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Distributors' AND Action = 'GetDistributors'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Distributors' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'EventClientTrees' AND Action = 'GetEventClientTree'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'EventClientTrees' AND Action = 'GetEventClientTrees'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'EventClientTrees' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Events' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Events' AND Action = 'GetEvent'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Events' AND Action = 'GetEvents'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Events' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'EventTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'EventTypes' AND Action = 'GetEventTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'DataLakeSyncResultSuccessDownload'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'DataLakeSyncResultWarningDownload'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'DataLakeSyncResultErrorDownload'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'DownloadHandlerLogFile'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'ExportDownload'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'ImportDownload'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'InterfaceDownload'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'ImportResultSuccessDownload'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'ImportResultWarningDownload'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'ImportResultErrorDownload'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'DownloadTemplate'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'DownloadBufferFile'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'File' AND Action = 'DownloadManual'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileBuffers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileBuffers' AND Action = 'GetFileBuffers'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileBuffers' AND Action = 'GetFileBuffer'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileBuffers' AND Action = 'GetInterface'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileBuffers' AND Action = 'ReadLogFile'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileCollectInterfaceSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileCollectInterfaceSettings' AND Action = 'GetFileCollectInterfaceSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileCollectInterfaceSettings' AND Action = 'GetFileCollectInterfaceSetting'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileCollectInterfaceSettings' AND Action = 'GetInterface'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileSendInterfaceSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileSendInterfaceSettings' AND Action = 'GetFileSendInterfaceSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileSendInterfaceSettings' AND Action = 'GetFileSendInterfaceSetting'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FileSendInterfaceSettings' AND Action = 'GetInterface'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Formats' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Formats' AND Action = 'GetFormat'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Formats' AND Action = 'GetFormats'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Formats' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'FullImportXLSX' AND Action = 'Actuals'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalAccessPoints' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalAccessPoints' AND Action = 'GetHistoricalAccessPoints'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalActualCOGSs' AND Action = 'GetHistoricalActualCOGSs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalActualCOGSs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalActualCOGSTns' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalActualCOGSTns' AND Action = 'GetHistoricalActualCOGSTns'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalActuals' AND Action = 'GetHistoricalActuals'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalActualTradeInvestments' AND Action = 'GetHistoricalActualTradeInvestments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalActualTradeInvestments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalAgeGroups' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalAgeGroups' AND Action = 'GetHistoricalAgeGroups'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalAssortmentMatrices' AND Action = 'GetHistoricalAssortmentMatrices'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalAssortmentMatrices' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBaseLines' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBaseLines' AND Action = 'GetHistoricalBaseLines'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBrands' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBrands' AND Action = 'GetHistoricalBrands'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBrandTeches' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBrandTeches' AND Action = 'GetHistoricalBrandTeches'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBTLs' AND Action = 'GetHistoricalBTLs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBTLs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBudgetItems' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBudgetItems' AND Action = 'GetHistoricalBudgetItems'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBudgets' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBudgets' AND Action = 'GetHistoricalBudgets'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBudgetSubItems' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalBudgetSubItems' AND Action = 'GetHistoricalBudgetSubItems'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCategories' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCategories' AND Action = 'GetHistoricalCategories'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalClientDashboards' AND Action = 'GetHistoricalClientDashboards'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalClientDashboards' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalClients' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalClients' AND Action = 'GetHistoricalClients'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalClientTreeBrandTeches' AND Action = 'GetHistoricalClientTreeBrandTeches'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalClientTreeBrandTeches' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCoefficientSI2SOs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCoefficientSI2SOs' AND Action = 'GetHistoricalCoefficientSI2SOs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCOGSs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCOGSs' AND Action = 'GetHistoricalCOGSs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalColors' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalColors' AND Action = 'GetHistoricalColors'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCommercialNets' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCommercialNets' AND Action = 'GetHistoricalCommercialNets'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCommercialSubnets' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCommercialSubnets' AND Action = 'GetHistoricalCommercialSubnets'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCompetitorBrandTechs' AND Action = 'GetHistoricalCompetitorBrandTechs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCompetitorBrandTechs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCompetitorPromoes' AND Action = 'GetHistoricalCompetitorPromoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCompetitorPromoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCompetitors' AND Action = 'GetHistoricalCompetitors'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCompetitors' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalConstraints' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalConstraints' AND Action = 'GetHistoricalConstraints'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCostProductions' AND Action = 'GetHistoricalCostProductions'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCostProductions' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCSVExtractInterfaceSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCSVExtractInterfaceSettings' AND Action = 'GetHistoricalCSVExtractInterfaceSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCSVProcessInterfaceSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalCSVProcessInterfaceSettings' AND Action = 'GetHistoricalCSVProcessInterfaceSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalDemands' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalDemands' AND Action = 'GetHistoricalDemands'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalDistributors' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalDistributors' AND Action = 'GetHistoricalDistributors'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalEvents' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalEvents' AND Action = 'GetHistoricalEvents'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalFileBuffers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalFileCollectInterfaceSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalFileCollectInterfaceSettings' AND Action = 'GetHistoricalFileCollectInterfaceSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalFileSendInterfaceSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalFileSendInterfaceSettings' AND Action = 'GetHistoricalFileSendInterfaceSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalFormats' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalFormats' AND Action = 'GetHistoricalFormats'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalIncrementalPromoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalInterfaces' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalInterfaces' AND Action = 'GetHistoricalInterfaces'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalMailNotificationSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalMailNotificationSettings' AND Action = 'GetHistoricalMailNotificationSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalMechanics' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalMechanics' AND Action = 'GetHistoricalMechanics'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalMechanicTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalMechanicTypes' AND Action = 'GetHistoricalMechanicTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalNodeTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalNodeTypes' AND Action = 'GetHistoricalNodeTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalNoneNegoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalNoneNegoes' AND Action = 'GetHistoricalNoneNegoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalNonPromoEquipments' AND Action = 'GetHistoricalNonPromoEquipments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalNonPromoEquipments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalNonPromoSupports' AND Action = 'GetHistoricalNonPromoSupports'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalNonPromoSupports' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPlanCOGSTns' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPlanCOGSTns' AND Action = 'GetHistoricalPlanCOGSTns'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPLUDictionaries' AND Action = 'GetHistoricalPLUDictionaries'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPLUDictionaries' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPostPromoEffects' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPostPromoEffects' AND Action = 'GetHistoricalPostPromoEffects'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalProducts' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalProducts' AND Action = 'GetHistoricalProducts'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPrograms' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPrograms' AND Action = 'GetHistoricalPrograms'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoDemands' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoDemands' AND Action = 'GetHistoricalPromoDemands'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoes' AND Action = 'GetHistoricalPromoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoGridViews' AND Action = 'GetHistoricalPromoGridView'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoProducts' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoProducts' AND Action = 'GetHistoricalPromoProducts'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoProductsCorrections' AND Action = 'GetHistoricalPromoProductsCorrections'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoProductsCorrections' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoSaleses' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoSaleses' AND Action = 'GetHistoricalPromoSaleses'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoStatuss' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoStatuss' AND Action = 'GetHistoricalPromoStatuss'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoSupports' AND Action = 'GetHistoricalPromoSupports'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoSupports' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalPromoTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRATIShoppers' AND Action = 'GetHistoricalRATIShoppers'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRATIShoppers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRecipients' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRecipients' AND Action = 'GetHistoricalRecipients'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRegions' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRegions' AND Action = 'GetHistoricalRegions'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRejectReasons' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRejectReasons' AND Action = 'GetHistoricalRejectReasons'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRetailTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRetailTypes' AND Action = 'GetHistoricalRetailTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRoles' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalRoles' AND Action = 'GetHistoricalRoles'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalSales' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalSales' AND Action = 'GetHistoricalSales'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalSegments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalSegments' AND Action = 'GetHistoricalSegments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalSettings' AND Action = 'GetHistoricalSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalStoreTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalStoreTypes' AND Action = 'GetHistoricalStoreTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalSubranges' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalSubranges' AND Action = 'GetHistoricalSubranges'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalTechHighLevels' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalTechHighLevels' AND Action = 'GetHistoricalTechHighLevels'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalTechnologies' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalTechnologies' AND Action = 'GetHistoricalTechnologies'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalTradeInvestments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalTradeInvestments' AND Action = 'GetHistoricalTradeInvestments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalUserRoles' AND Action = 'GetHistoricalUserRoles'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalUserRoles' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalUsers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalUsers' AND Action = 'GetHistoricalUsers'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalVarieties' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalVarieties' AND Action = 'GetHistoricalVarieties'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalXMLProcessInterfaceSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'HistoricalXMLProcessInterfaceSettings' AND Action = 'GetHistoricalXMLProcessInterfaceSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ImportPromoes' AND Action = 'Put'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ImportPromoes' AND Action = 'Post'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ImportPromoes' AND Action = 'Patch'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ImportPromoes' AND Action = 'Delete'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'IncrementalPromoes' AND Action = 'GetIncrementalPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'IncrementalPromoes' AND Action = 'GetIncrementalPromoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'IncrementalPromoes' AND Action = 'Put'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'IncrementalPromoes' AND Action = 'Patch'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'IncrementalPromoes' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'IncrementalPromoes' AND Action = 'FullImportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'IncrementalPromoes' AND Action = 'DownloadTemplateXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'IncrementalPromoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Interfaces' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Interfaces' AND Action = 'GetInterfaces'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Interfaces' AND Action = 'GetInterface'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'LoopHandlers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'LoopHandlers' AND Action = 'GetLoopHandlers'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'LoopHandlers' AND Action = 'GetLoopHandler'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'LoopHandlers' AND Action = 'Parameters'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'LoopHandlers' AND Action = 'Restart'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'LoopHandlers' AND Action = 'Start'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'LoopHandlers' AND Action = 'ReadLogFile'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'MailNotificationSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'MailNotificationSettings' AND Action = 'GetMailNotificationSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'MailNotificationSettings' AND Action = 'GetMailNotificationSetting'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Mechanics' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Mechanics' AND Action = 'GetMechanic'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Mechanics' AND Action = 'GetMechanics'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Mechanics' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'MechanicTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'MechanicTypes' AND Action = 'GetMechanicType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'MechanicTypes' AND Action = 'GetMechanicTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'MechanicTypes' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NodeTypes' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NodeTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NodeTypes' AND Action = 'GetNodeType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NodeTypes' AND Action = 'GetNodeTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NoneNegoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NoneNegoes' AND Action = 'GetNoneNego'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NoneNegoes' AND Action = 'GetNoneNegoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NoneNegoes' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NoneNegoes' AND Action = 'IsValidPeriod'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoEquipments' AND Action = 'GetNonPromoEquipments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoEquipments' AND Action = 'GetNonPromoEquipment'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoEquipments' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoEquipments' AND Action = 'DownloadTemplateXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoEquipments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoSupportBrandTeches' AND Action = 'GetNonPromoSupportBrandTeches'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoSupportBrandTeches' AND Action = 'GetNonPromoSupportBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoSupportBrandTeches' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoSupports' AND Action = 'GetNonPromoSupports'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoSupports' AND Action = 'GetNonPromoSupport'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoSupports' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoSupports' AND Action = 'DownloadFile'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'NonPromoSupports' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PlanCOGSTns' AND Action = 'GetPlanCOGSTn'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PlanCOGSTns' AND Action = 'GetPlanCOGSTns'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PlanCOGSTns' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PlanCOGSTns' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PlanIncrementalReports' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PlanIncrementalReports' AND Action = 'GetPlanIncrementalReports'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PlanIncrementalReports' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PlanPostPromoEffectReports' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PlanPostPromoEffectReports' AND Action = 'GetPlanPostPromoEffectReports'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PlanPostPromoEffectReports' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PLUDictionaries' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PLUDictionaries' AND Action = 'DownloadTemplateXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PLUDictionaries' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PLUDictionaries' AND Action = 'GetPLUDictionaries'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PostPromoEffects' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PostPromoEffects' AND Action = 'GetPostPromoEffect'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PostPromoEffects' AND Action = 'GetPostPromoEffects'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PostPromoEffects' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PreviousDayIncrementals' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PreviousDayIncrementals' AND Action = 'GetPreviousDayIncrementals'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PreviousDayIncrementals' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PriceLists' AND Action = 'GetPriceLists'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PriceLists' AND Action = 'GetPriceList'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PriceLists' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PriceLists' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetSelectedProducts'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetRegularSelectedProducts'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetProduct'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetProducts'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetCategory'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetBrand'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetSegment'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetTechnology'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetTechHighLevel'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetProgram'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetFormat'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetSubrange'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetAgeGroup'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'GetVariety'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Products' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ProductTrees' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ProductTrees' AND Action = 'GetProductTree'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ProductTrees' AND Action = 'GetProductTrees'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'ProductTrees' AND Action = 'GetHierarchyDetail'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Programs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Programs' AND Action = 'GetProgram'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Programs' AND Action = 'GetPrograms'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Programs' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoDemands' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoDemands' AND Action = 'GetPromoDemand'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoDemands' AND Action = 'GetPromoDemands'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoDemands' AND Action = 'GetBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoDemands' AND Action = 'GetMechanic'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoDemands' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'ExportPromoROIReportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetCanChangeStatePromoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'RecalculatePromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetProducts'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'CheckPromoCreator'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'ResetPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'ChangeResponsible'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetUserDashboardsCount'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'CheckIfLogHasErrors'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetPromoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'Put'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'Post'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'Patch'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'Delete'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetClient'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetBrand'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetBrandTech'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetProduct'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetPromoStatus'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'GetMechanic'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'FullImportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'DeclinePromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'CalculateMarketingTI'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'ChangeStatus'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'ReadPromoCalculatingLog'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'CheckPromoCalculatingStatus'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Promoes' AND Action = 'MassApprove'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoGridViews' AND Action = 'GetPromoGridViews'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoGridViews' AND Action = 'GetPromoGridView'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoGridViews' AND Action = 'Put'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoGridViews' AND Action = 'Post'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoGridViews' AND Action = 'Patch'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoGridViews' AND Action = 'Delete'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoGridViews' AND Action = 'DeclinePromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoGridViews' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoGridViews' AND Action = 'GetCanChangeStatePromoGridViews'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoGridViews' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProducts' AND Action = 'GetPromoProductByPromoAndProduct'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProducts' AND Action = 'SupportAdminExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProducts' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProducts' AND Action = 'GetPromoProduct'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProducts' AND Action = 'GetPromoProducts'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProducts' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProducts' AND Action = 'DownloadTemplatePluXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'GetPromoProductsCorrection'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'GetPromoProductsCorrections'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'Put'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'Patch'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'Post'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'Delete'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'FullImportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'DownloadTemplateXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsCorrections' AND Action = 'ExportCorrectionXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsViews' AND Action = 'GetPromoProductsView'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsViews' AND Action = 'GetPromoProductsViews'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsViews' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoProductsViews' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoROIReports' AND Action = 'GetPromoROIReports'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoROIReports' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoROIReports' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSaleses' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSaleses' AND Action = 'GetPromoSale'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSaleses' AND Action = 'GetPromoSaleses'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSaleses' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSaleses' AND Action = 'FullImportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoStatusChanges' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoStatusChanges' AND Action = 'PromoStatusChangesByPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoStatuss' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoStatuss' AND Action = 'GetPromoStatus'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoStatuss' AND Action = 'GetPromoStatuss'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoStatuss' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'ChangeListPSP'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'GetPromoSupportPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'GetPromoSupportPromoes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'Post'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'Delete'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'GetValuesForItems'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'PromoSuportPromoPost'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'Patch'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'GetLinkedSubItems'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupportPromoes' AND Action = 'ManageSubItems'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupports' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupports' AND Action = 'GetPromoSupportGroup'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupports' AND Action = 'DownloadFile'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupports' AND Action = 'GetUserTimestamp'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupports' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupports' AND Action = 'GetPromoSupport'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoSupports' AND Action = 'GetPromoSupports'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoTypes' AND Action = 'GetPromoType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoTypes' AND Action = 'GetPromoTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoViews' AND Action = 'GetPromoView'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoViews' AND Action = 'GetPromoViews'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoViews' AND Action = 'ExportSchedule'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'PromoViews' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RATIShoppers' AND Action = 'DownloadTemplateXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RATIShoppers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RATIShoppers' AND Action = 'GetRATIShoppers'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RATIShoppers' AND Action = 'GetRATIShopper'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RATIShoppers' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Recipients' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Recipients' AND Action = 'GetRecipients'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Recipients' AND Action = 'GetRecipient'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Recipients' AND Action = 'GetMailNotificationSetting'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Regions' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Regions' AND Action = 'GetRegion'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Regions' AND Action = 'GetRegions'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Regions' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RejectReasons' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RejectReasons' AND Action = 'GetRejectReason'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RejectReasons' AND Action = 'GetRejectReasons'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RejectReasons' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RetailTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RetailTypes' AND Action = 'GetRetailType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RetailTypes' AND Action = 'GetRetailTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RetailTypes' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Roles' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Roles' AND Action = 'GetRoles'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Roles' AND Action = 'GetRole'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RollingVolumes' AND Action = 'GetRollingVolumes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RollingVolumes' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'RollingVolumes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Sales' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Sales' AND Action = 'GetSale'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Sales' AND Action = 'GetSales'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Sales' AND Action = 'GetPromo'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Sales' AND Action = 'GetBudget'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Sales' AND Action = 'GetBudgetItem'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Sales' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'SchedulerClientTreeDTOs' AND Action = 'Post'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'SchedulerClientTreeDTOs' AND Action = 'GetSchedulerClientTreeDTOs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'SchedulerClientTreeDTOs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Security' AND Action = 'Get'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Segments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Segments' AND Action = 'GetSegment'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Segments' AND Action = 'GetSegments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Segments' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Settings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Settings' AND Action = 'GetSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Settings' AND Action = 'GetSetting'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Settings' AND Action = 'TakeSettingByName'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'SingleLoopHandlers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'SingleLoopHandlers' AND Action = 'GetSingleLoopHandlers'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'SingleLoopHandlers' AND Action = 'GetSingleLoopHandler'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'SingleLoopHandlers' AND Action = 'GetUser'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'StoreTypes' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'StoreTypes' AND Action = 'GetStoreType'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'StoreTypes' AND Action = 'GetStoreTypes'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'StoreTypes' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Subranges' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Subranges' AND Action = 'GetSubrange'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Subranges' AND Action = 'GetSubranges'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Subranges' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'TechHighLevels' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'TechHighLevels' AND Action = 'GetTechHighLevel'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'TechHighLevels' AND Action = 'GetTechHighLevels'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'TechHighLevels' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Technologies' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Technologies' AND Action = 'GetTechnology'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Technologies' AND Action = 'GetTechnologies'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Technologies' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'TradeInvestments' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'TradeInvestments' AND Action = 'GetTradeInvestments'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'TradeInvestments' AND Action = 'GetCOGS'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'TradeInvestments' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserDTOs' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserDTOs' AND Action = 'GetUserDTOs'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserDTOs' AND Action = 'GetUserDTO'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserLoopHandlers' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserLoopHandlers' AND Action = 'GetUserLoopHandlers'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserLoopHandlers' AND Action = 'GetUserLoopHandler'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserLoopHandlers' AND Action = 'GetUser'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserRoles' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserRoles' AND Action = 'GetUserRoles'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserRoles' AND Action = 'GetUserRole'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserRoles' AND Action = 'GetUser'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserRoles' AND Action = 'GetRole'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'UserRoles' AND Action = 'SetDefault'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Users' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Varieties' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Varieties' AND Action = 'GetVariety'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Varieties' AND Action = 'GetVarieties'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'Varieties' AND Action = 'ExportXLSX'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'XMLProcessInterfaceSettings' AND Action = 'GetFilteredData'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'XMLProcessInterfaceSettings' AND Action = 'GetXMLProcessInterfaceSettings'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'XMLProcessInterfaceSettings' AND Action = 'GetXMLProcessInterfaceSetting'
                UPDATE [DefaultSchemaSetting].[AccessPoint]
                SET TPMmode = 1
                WHERE Resource = 'XMLProcessInterfaceSettings' AND Action = 'GetInterface'
                GO
";
    }
}
