namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Gam_Role_AP : DbMigration
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
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AccessPoint' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AccessPointRoles' and [Action]='GetAccessPoint' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AccessPointRoles' and [Action]='GetAccessPointRole' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AccessPointRoles' and [Action]='GetAccessPointRoles' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AccessPointRoles' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AccessPointRoles' and [Action]='GetRole' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AccessPoints' and [Action]='GetAccessPoint' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AccessPoints' and [Action]='GetAccessPoints' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AccessPoints' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSs' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSs' and [Action]='GetActualCOGS' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSs' and [Action]='GetActualCOGSs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='FullImportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTn' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetActualCOGSTns' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualLSVs' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualLSVs' and [Action]='GetActualLSV' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualLSVs' and [Action]='GetActualLSVs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualLSVs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Actuals' and [Action]='ExportXLSX' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualTradeInvestments' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualTradeInvestments' and [Action]='GetActualTradeInvestment' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualTradeInvestments' and [Action]='GetActualTradeInvestments' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualTradeInvestments' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AdUserDTOs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AdUsers' and [Action]='GetAdUsers' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AdUsers' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AgeGroups' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AgeGroups' and [Action]='GetAgeGroup' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AgeGroups' and [Action]='GetAgeGroups' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AgeGroups' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AssortmentMatrices' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AssortmentMatrices' and [Action]='GetAssortmentMatrices' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AssortmentMatrices' and [Action]='GetAssortmentMatrix' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='AssortmentMatrices' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseClients' and [Action]='GetBaseClient' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseClients' and [Action]='GetBaseClients' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseClientTreeViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseClientTreeViews' and [Action]='GetBaseClientTreeViews' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseClientTreeViews' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseLines' and [Action]='ExportDemandPriceListXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseLines' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseLines' and [Action]='GetBaseLine' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseLines' and [Action]='GetBaseLines' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseLines' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Brands' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Brands' and [Action]='GetBrand' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Brands' and [Action]='GetBrands' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Brands' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BrandTeches' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BrandTeches' and [Action]='GetBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BrandTeches' and [Action]='GetBrandTechById' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BrandTeches' and [Action]='GetBrandTeches' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BrandTeches' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BTLPromoes' and [Action]='GetBTLPromo' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BTLPromoes' and [Action]='GetBTLPromoes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BTLPromoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BTLPromoes' and [Action]='GetPromoesWithBTL' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BTLs' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BTLs' and [Action]='GetBTL' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BTLs' and [Action]='GetBTLs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BTLs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BudgetItems' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BudgetItems' and [Action]='GetBudget' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BudgetItems' and [Action]='GetBudgetItem' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BudgetItems' and [Action]='GetBudgetItems' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BudgetItems' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Budgets' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Budgets' and [Action]='GetBudget' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Budgets' and [Action]='GetBudgets' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Budgets' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BudgetSubItemClientTrees' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BudgetSubItems' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BudgetSubItems' and [Action]='GetBudgetSubItem' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BudgetSubItems' and [Action]='GetBudgetSubItems' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BudgetSubItems' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Categories' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Categories' and [Action]='GetCategories' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Categories' and [Action]='GetCategory' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Categories' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardViews' and [Action]='GetAllYEEF' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardViews' and [Action]='GetClientDashboardViews' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardViews' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Clients' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Clients' and [Action]='GetClient' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Clients' and [Action]='GetClients' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Clients' and [Action]='GetCommercialSubnet' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Clients' and [Action]='GetDistributor' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Clients' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Clients' and [Action]='GetRegion' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Clients' and [Action]='GetStoreType' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTreeBrandTeches' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTreeBrandTeches' and [Action]='GetClientTreeBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTreeBrandTeches' and [Action]='GetClientTreeBrandTeches' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTreeBrandTeches' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='DownloadLogoFile' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='GetClientTree' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='GetClientTreeByObjectId' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='GetClientTrees' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='GetHierarchyDetail' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTreeSharesViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTreeSharesViews' and [Action]='GetClientTreeSharesViews' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTreeSharesViews' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CoefficientSI2SOs' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CoefficientSI2SOs' and [Action]='GetCoefficientSI2SO' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CoefficientSI2SOs' and [Action]='GetCoefficientSI2SOs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CoefficientSI2SOs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='COGSs' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='COGSs' and [Action]='GetCOGS' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='COGSs' and [Action]='GetCOGSs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='COGSs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Colors' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Colors' and [Action]='GetColor' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Colors' and [Action]='GetColors' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Colors' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Colors' and [Action]='GetSuitable' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CommercialNets' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CommercialNets' and [Action]='GetCommercialNet' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CommercialNets' and [Action]='GetCommercialNets' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CommercialNets' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CommercialSubnets' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CommercialSubnets' and [Action]='GetCommercialNet' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CommercialSubnets' and [Action]='GetCommercialSubnet' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CommercialSubnets' and [Action]='GetCommercialSubnets' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CommercialSubnets' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTechs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromo' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromoes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='NewFullImportXLSX' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitor' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitors' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Constraints' and [Action]='GetConstraint' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Constraints' and [Action]='GetConstraints' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Constraints' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Constraints' and [Action]='GetUserRole' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CSVExtractInterfaceSettings' and [Action]='GetCSVExtractInterfaceSetting' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CSVExtractInterfaceSettings' and [Action]='GetCSVExtractInterfaceSettings' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CSVExtractInterfaceSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CSVExtractInterfaceSettings' and [Action]='GetInterface' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CSVProcessInterfaceSettings' and [Action]='GetCSVProcessInterfaceSetting' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CSVProcessInterfaceSettings' and [Action]='GetCSVProcessInterfaceSettings' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CSVProcessInterfaceSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CSVProcessInterfaceSettings' and [Action]='GetInterface' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedAccessPoints' and [Action]='GetDeletedAccessPoint' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedAccessPoints' and [Action]='GetDeletedAccessPoints' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedAccessPoints' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSs' and [Action]='GetDeletedActualCOGSs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTn' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetDeletedActualCOGSTns' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualCOGSTns' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualTradeInvestments' and [Action]='GetDeletedActualTradeInvestments' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedActualTradeInvestments' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedAgeGroups' and [Action]='GetDeletedAgeGroup' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedAgeGroups' and [Action]='GetDeletedAgeGroups' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedAgeGroups' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedAssortmentMatrices' and [Action]='GetDeletedAssortmentMatrices' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedAssortmentMatrices' and [Action]='GetDeletedAssortmentMatrix' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedAssortmentMatrices' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBaseLine' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBaseLines' and [Action]='GetDeletedBaseLine' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBaseLines' and [Action]='GetDeletedBaseLines' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBaseLines' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBrands' and [Action]='GetDeletedBrand' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBrands' and [Action]='GetDeletedBrands' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBrands' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBrandTeches' and [Action]='GetDeletedBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBrandTeches' and [Action]='GetDeletedBrandTeches' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBrandTeches' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBTLs' and [Action]='GetDeletedBTLs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBTLs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBudgetItems' and [Action]='GetBudget' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBudgetItems' and [Action]='GetDeletedBudgetItem' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBudgetItems' and [Action]='GetDeletedBudgetItems' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBudgetItems' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBudgets' and [Action]='GetDeletedBudget' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBudgets' and [Action]='GetDeletedBudgets' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBudgets' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBudgetSubItems' and [Action]='GetDeletedBudgetSubItem' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBudgetSubItems' and [Action]='GetDeletedBudgetSubItems' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedBudgetSubItems' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCategories' and [Action]='GetDeletedCategories' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCategories' and [Action]='GetDeletedCategory' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCategories' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedClients' and [Action]='GetCommercialSubnet' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedClients' and [Action]='GetDeletedClient' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedClients' and [Action]='GetDeletedClients' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedClients' and [Action]='GetDistributor' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedClients' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedClients' and [Action]='GetRegion' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedClients' and [Action]='GetStoreType' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCoefficientSI2SOs' and [Action]='GetDeletedCoefficientSI2SOs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCoefficientSI2SOs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCOGSs' and [Action]='GetDeletedCOGS' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCOGSs' and [Action]='GetDeletedCOGSs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCOGSs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedColors' and [Action]='GetDeletedColor' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedColors' and [Action]='GetDeletedColors' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedColors' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCommercialNets' and [Action]='GetDeletedCommercialNet' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCommercialNets' and [Action]='GetDeletedCommercialNets' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCommercialNets' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCommercialSubnets' and [Action]='GetCommercialNet' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCommercialSubnets' and [Action]='GetDeletedCommercialSubnet' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCommercialSubnets' and [Action]='GetDeletedCommercialSubnets' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCommercialSubnets' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTechs' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromo' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromoes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitor' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitors' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedConstraints' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedDemands' and [Action]='GetDeletedDemand' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedDemands' and [Action]='GetDeletedDemands' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedDemands' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedDistributors' and [Action]='GetDeletedDistributor' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedDistributors' and [Action]='GetDeletedDistributors' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedDistributors' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedEvents' and [Action]='GetDeletedEvent' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedEvents' and [Action]='GetDeletedEvents' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedEvents' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedFormats' and [Action]='GetDeletedFormat' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedFormats' and [Action]='GetDeletedFormats' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedFormats' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedIncrementalPromoes' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedMailNotificationSettings' and [Action]='GetDeletedMailNotificationSetting' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedMailNotificationSettings' and [Action]='GetDeletedMailNotificationSettings' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedMailNotificationSettings' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedMechanics' and [Action]='GetDeletedMechanic' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedMechanics' and [Action]='GetDeletedMechanics' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedMechanics' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedMechanicTypes' and [Action]='GetDeletedMechanicType' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedMechanicTypes' and [Action]='GetDeletedMechanicTypes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedMechanicTypes' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNodeTypes' and [Action]='GetDeletedNodeType' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNodeTypes' and [Action]='GetDeletedNodeTypes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNodeTypes' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNoneNegoes' and [Action]='GetDeletedNoneNego' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNoneNegoes' and [Action]='GetDeletedNoneNegoes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNoneNegoes' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNonPromoEquipment' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNonPromoEquipments' and [Action]='GetDeletedNonPromoEquipments' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNonPromoEquipments' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNonPromoSupports' and [Action]='GetDeletedNonPromoSupports' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedNonPromoSupports' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTn' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetDeletedPlanCOGSTns' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPlanCOGSTns' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPostPromoEffects' and [Action]='GetDeletedPostPromoEffect' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPostPromoEffects' and [Action]='GetDeletedPostPromoEffects' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPostPromoEffects' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetAgeGroup' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetBrand' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetCategory' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetDeletedProduct' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetDeletedProducts' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetFormat' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetProgram' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetSegment' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetSubrange' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetTechHighLevel' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetTechnology' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedProducts' and [Action]='GetVariety' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPrograms' and [Action]='GetDeletedProgram' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPrograms' and [Action]='GetDeletedPrograms' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPrograms' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoDemands' and [Action]='GetBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoDemands' and [Action]='GetDeletedPromoDemand' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoDemands' and [Action]='GetDeletedPromoDemands' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoDemands' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoDemands' and [Action]='GetMechanic' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoes' and [Action]='GetBrand' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoes' and [Action]='GetBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoes' and [Action]='GetClient' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoes' and [Action]='GetDeletedPromo' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoes' and [Action]='GetDeletedPromoes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoes' and [Action]='GetMechanic' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoes' and [Action]='GetProduct' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoes' and [Action]='GetPromoStatus' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoGridViews' and [Action]='GetDeletedPromoGridView' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoGridViews' and [Action]='GetDeletedPromoGridViews' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProducts' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductsCorrections' and [Action]='GetDeletedPromoProductsCorrections' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoProductsCorrections' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoSaleses' and [Action]='GetDeletedPromoSales' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoSaleses' and [Action]='GetDeletedPromoSaleses' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoSaleses' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoStatuss' and [Action]='GetDeletedPromoStatus' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoStatuss' and [Action]='GetDeletedPromoStatuss' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoStatuss' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoSupports' and [Action]='GetDeletedPromoSupports' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoSupports' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoTypes' and [Action]='GetDeletedPromoTypes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedPromoTypes' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRATIShoppers' and [Action]='GetDeletedRATIShopper' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRATIShoppers' and [Action]='GetDeletedRATIShoppers' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRATIShoppers' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRegions' and [Action]='GetDeletedRegion' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRegions' and [Action]='GetDeletedRegions' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRegions' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRejectReasons' and [Action]='GetDeletedRejectReason' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRejectReasons' and [Action]='GetDeletedRejectReasons' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRejectReasons' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRetailTypes' and [Action]='GetDeletedRetailType' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRetailTypes' and [Action]='GetDeletedRetailTypes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRetailTypes' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRoles' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSales' and [Action]='GetBudget' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSales' and [Action]='GetBudgetItem' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSales' and [Action]='GetDeletedSale' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSales' and [Action]='GetDeletedSales' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSales' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSales' and [Action]='GetPromo' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSegments' and [Action]='GetDeletedSegment' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSegments' and [Action]='GetDeletedSegments' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSegments' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedStoreTypes' and [Action]='GetDeletedStoreType' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedStoreTypes' and [Action]='GetDeletedStoreTypes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedStoreTypes' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSubranges' and [Action]='GetDeletedSubrange' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSubranges' and [Action]='GetDeletedSubranges' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedSubranges' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedTechHighLevels' and [Action]='GetDeletedTechHighLevel' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedTechHighLevels' and [Action]='GetDeletedTechHighLevels' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedTechHighLevels' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedTechnologies' and [Action]='GetDeletedTechnologies' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedTechnologies' and [Action]='GetDeletedTechnology' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedTechnologies' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedTradeInvestments' and [Action]='GetDeletedTradeInvestment' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedTradeInvestments' and [Action]='GetDeletedTradeInvestments' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedTradeInvestments' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedUserRoles' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedUsers' and [Action]='GetDeletedUser' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedUsers' and [Action]='GetDeletedUsers' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedUsers' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedVarieties' and [Action]='GetDeletedVarieties' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedVarieties' and [Action]='GetDeletedVariety' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedVarieties' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Demands' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Demands' and [Action]='GetDemand' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Demands' and [Action]='GetDemands' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Demands' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Distributors' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Distributors' and [Action]='GetDistributor' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Distributors' and [Action]='GetDistributors' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Distributors' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='EventClientTrees' and [Action]='GetEventClientTree' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='EventClientTrees' and [Action]='GetEventClientTrees' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='EventClientTrees' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Events' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Events' and [Action]='GetEvent' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Events' and [Action]='GetEvents' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Events' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='DataLakeSyncResultErrorDownload' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='DataLakeSyncResultSuccessDownload' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='DataLakeSyncResultWarningDownload' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='DownloadBufferFile' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='DownloadHandlerLogFile' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='DownloadManual' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='DownloadTemplate' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='ExportDownload' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='ImportDownload' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='ImportResultErrorDownload' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='ImportResultSuccessDownload' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='ImportResultWarningDownload' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='File' and [Action]='InterfaceDownload' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileBuffers' and [Action]='GetFileBuffer' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileBuffers' and [Action]='GetFileBuffers' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileBuffers' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileBuffers' and [Action]='GetInterface' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileBuffers' and [Action]='ManualProcess' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileBuffers' and [Action]='ManualSend' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileBuffers' and [Action]='ReadLogFile' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileCollectInterfaceSettings' and [Action]='GetFileCollectInterfaceSetting' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileCollectInterfaceSettings' and [Action]='GetFileCollectInterfaceSettings' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileCollectInterfaceSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileCollectInterfaceSettings' and [Action]='GetInterface' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileSendInterfaceSettings' and [Action]='GetFileSendInterfaceSetting' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileSendInterfaceSettings' and [Action]='GetFileSendInterfaceSettings' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileSendInterfaceSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='FileSendInterfaceSettings' and [Action]='GetInterface' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Formats' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Formats' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Formats' and [Action]='GetFormat' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Formats' and [Action]='GetFormats' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalAccessPoints' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalAccessPoints' and [Action]='GetHistoricalAccessPoints' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSs' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSs' and [Action]='GetHistoricalActualCOGSs' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualCOGSTns' and [Action]='GetHistoricalActualCOGSTns' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualTradeInvestments' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalActualTradeInvestments' and [Action]='GetHistoricalActualTradeInvestments' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalAgeGroups' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalAgeGroups' and [Action]='GetHistoricalAgeGroups' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalAssortmentMatrices' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalAssortmentMatrices' and [Action]='GetHistoricalAssortmentMatrices' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBaseLines' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBaseLines' and [Action]='GetHistoricalBaseLines' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBrands' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBrands' and [Action]='GetHistoricalBrands' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBrandTeches' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBrandTeches' and [Action]='GetHistoricalBrandTeches' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBTLs' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBTLs' and [Action]='GetHistoricalBTLs' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBudgetItems' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBudgetItems' and [Action]='GetHistoricalBudgetItems' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBudgets' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBudgets' and [Action]='GetHistoricalBudgets' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBudgetSubItems' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalBudgetSubItems' and [Action]='GetHistoricalBudgetSubItems' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCategories' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCategories' and [Action]='GetHistoricalCategories' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalClientDashboards' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalClientDashboards' and [Action]='GetHistoricalClientDashboards' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalClients' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalClients' and [Action]='GetHistoricalClients' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalClientTreeBrandTeches' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalClientTreeBrandTeches' and [Action]='GetHistoricalClientTreeBrandTeches' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCoefficientSI2SOs' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCoefficientSI2SOs' and [Action]='GetHistoricalCoefficientSI2SOs' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCOGSs' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCOGSs' and [Action]='GetHistoricalCOGSs' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalColors' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalColors' and [Action]='GetHistoricalColors' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCommercialNets' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCommercialNets' and [Action]='GetHistoricalCommercialNets' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCommercialSubnets' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCommercialSubnets' and [Action]='GetHistoricalCommercialSubnets' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetHistoricalCompetitorBrandTechs' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetHistoricalCompetitorPromoes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetHistoricalCompetitors' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalConstraints' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalConstraints' and [Action]='GetHistoricalConstraints' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCostProductions' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCostProductions' and [Action]='GetHistoricalCostProductions' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCSVExtractInterfaceSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCSVExtractInterfaceSettings' and [Action]='GetHistoricalCSVExtractInterfaceSettings' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCSVProcessInterfaceSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCSVProcessInterfaceSettings' and [Action]='GetHistoricalCSVProcessInterfaceSettings' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalDemands' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalDemands' and [Action]='GetHistoricalDemands' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalDistributors' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalDistributors' and [Action]='GetHistoricalDistributors' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalEvents' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalEvents' and [Action]='GetHistoricalEvents' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalFileBuffers' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalFileCollectInterfaceSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalFileCollectInterfaceSettings' and [Action]='GetHistoricalFileCollectInterfaceSettings' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalFileSendInterfaceSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalFileSendInterfaceSettings' and [Action]='GetHistoricalFileSendInterfaceSettings' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalFormats' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalFormats' and [Action]='GetHistoricalFormats' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalIncrementalPromoes' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalInterfaces' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalInterfaces' and [Action]='GetHistoricalInterfaces' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalMailNotificationSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalMailNotificationSettings' and [Action]='GetHistoricalMailNotificationSettings' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalMechanics' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalMechanics' and [Action]='GetHistoricalMechanics' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalMechanicTypes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalMechanicTypes' and [Action]='GetHistoricalMechanicTypes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalNodeTypes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalNodeTypes' and [Action]='GetHistoricalNodeTypes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalNoneNegoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalNoneNegoes' and [Action]='GetHistoricalNoneNegoes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalNonPromoEquipments' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalNonPromoEquipments' and [Action]='GetHistoricalNonPromoEquipments' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalNonPromoSupports' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalNonPromoSupports' and [Action]='GetHistoricalNonPromoSupports' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPlanCOGSTns' and [Action]='GetHistoricalPlanCOGSTns' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPostPromoEffects' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPostPromoEffects' and [Action]='GetHistoricalPostPromoEffects' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalProducts' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalProducts' and [Action]='GetHistoricalProducts' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPrograms' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPrograms' and [Action]='GetHistoricalPrograms' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoDemands' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoDemands' and [Action]='GetHistoricalPromoDemands' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoes' and [Action]='GetHistoricalPromoes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProducts' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductsCorrections' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoProductsCorrections' and [Action]='GetHistoricalPromoProductsCorrections' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoSaleses' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoSaleses' and [Action]='GetHistoricalPromoSaleses' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoStatuss' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoStatuss' and [Action]='GetHistoricalPromoStatuss' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoSupports' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoSupports' and [Action]='GetHistoricalPromoSupports' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalPromoTypes' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRATIShoppers' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRATIShoppers' and [Action]='GetHistoricalRATIShoppers' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRecipients' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRecipients' and [Action]='GetHistoricalRecipients' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRegions' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRegions' and [Action]='GetHistoricalRegions' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRejectReasons' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRejectReasons' and [Action]='GetHistoricalRejectReasons' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRetailTypes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRetailTypes' and [Action]='GetHistoricalRetailTypes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRoles' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalRoles' and [Action]='GetHistoricalRoles' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalSales' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalSales' and [Action]='GetHistoricalSales' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalSegments' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalSegments' and [Action]='GetHistoricalSegments' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalSettings' and [Action]='GetHistoricalSettings' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalStoreTypes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalStoreTypes' and [Action]='GetHistoricalStoreTypes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalSubranges' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalSubranges' and [Action]='GetHistoricalSubranges' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalTechHighLevels' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalTechHighLevels' and [Action]='GetHistoricalTechHighLevels' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalTechnologies' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalTechnologies' and [Action]='GetHistoricalTechnologies' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalTradeInvestments' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalTradeInvestments' and [Action]='GetHistoricalTradeInvestments' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalUserRoles' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalUsers' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalUsers' and [Action]='GetHistoricalUsers' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalVarieties' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalVarieties' and [Action]='GetHistoricalVarieties' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalXMLProcessInterfaceSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalXMLProcessInterfaceSettings' and [Action]='GetHistoricalXMLProcessInterfaceSettings' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='IncrementalPromoes' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='IncrementalPromoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='IncrementalPromoes' and [Action]='GetIncrementalPromo' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='IncrementalPromoes' and [Action]='GetIncrementalPromoes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Interfaces' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Interfaces' and [Action]='GetInterface' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Interfaces' and [Action]='GetInterfaces' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Interfaces' and [Action]='ManualCollect' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Interfaces' and [Action]='ManualExtract' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='LoopHandlers' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='LoopHandlers' and [Action]='GetLoopHandler' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='LoopHandlers' and [Action]='GetLoopHandlers' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='LoopHandlers' and [Action]='GetUser' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='LoopHandlers' and [Action]='Parameters' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='LoopHandlers' and [Action]='ReadLogFile' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='LoopHandlers' and [Action]='Start' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='MailNotificationSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='MailNotificationSettings' and [Action]='GetMailNotificationSetting' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='MailNotificationSettings' and [Action]='GetMailNotificationSettings' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Mechanics' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Mechanics' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Mechanics' and [Action]='GetMechanic' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Mechanics' and [Action]='GetMechanics' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='MechanicTypes' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='MechanicTypes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='MechanicTypes' and [Action]='GetMechanicType' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='MechanicTypes' and [Action]='GetMechanicTypes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NodeTypes' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NodeTypes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NodeTypes' and [Action]='GetNodeType' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NodeTypes' and [Action]='GetNodeTypes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NoneNegoes' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NoneNegoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NoneNegoes' and [Action]='GetNoneNego' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NoneNegoes' and [Action]='GetNoneNegoes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NoneNegoes' and [Action]='IsValidPeriod' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoEquipments' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoEquipments' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoEquipments' and [Action]='GetNonPromoEquipment' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoEquipments' and [Action]='GetNonPromoEquipments' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoSupportBrandTeches' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoSupportBrandTeches' and [Action]='GetNonPromoSupportBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoSupportBrandTeches' and [Action]='GetNonPromoSupportBrandTeches' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoSupports' and [Action]='DownloadFile' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoSupports' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoSupports' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoSupports' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoSupports' and [Action]='GetNonPromoSupport' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='NonPromoSupports' and [Action]='GetNonPromoSupports' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='FullImportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTn' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanCOGSTns' and [Action]='GetPlanCOGSTns' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanIncrementalReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanIncrementalReports' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanIncrementalReports' and [Action]='GetPlanIncrementalReports' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanPostPromoEffectReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanPostPromoEffectReports' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PlanPostPromoEffectReports' and [Action]='GetPlanPostPromoEffectReports' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PLUDictionaries' and [Action]='GetPLUDictionaries' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PostPromoEffects' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PostPromoEffects' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PostPromoEffects' and [Action]='GetPostPromoEffect' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PostPromoEffects' and [Action]='GetPostPromoEffects' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PreviousDayIncrementals' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PreviousDayIncrementals' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PreviousDayIncrementals' and [Action]='GetPreviousDayIncrementals' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PriceLists' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PriceLists' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PriceLists' and [Action]='GetPriceList' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PriceLists' and [Action]='GetPriceLists' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetAgeGroup' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetBrand' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetCategory' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetFormat' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetProduct' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetProducts' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetProgram' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetRegularSelectedProducts' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetSegment' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetSelectedProducts' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetSubrange' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetTechHighLevel' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetTechnology' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Products' and [Action]='GetVariety' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ProductTrees' and [Action]='DownloadLogoFile' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetHierarchyDetail' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetProductTree' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetProductTrees' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Programs' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Programs' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Programs' and [Action]='GetProgram' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Programs' and [Action]='GetPrograms' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoDemands' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoDemands' and [Action]='GetBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoDemands' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoDemands' and [Action]='GetMechanic' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoDemands' and [Action]='GetPromoDemand' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoDemands' and [Action]='GetPromoDemands' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='CheckIfLogHasErrors' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='CheckPromoCalculatingStatus' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='CheckPromoCreator' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='ExportPromoROIReportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetBrand' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetBrandTech' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetClient' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetMechanic' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetProduct' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetProducts' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetPromo' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetPromoes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetPromoStatus' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='ReadPromoCalculatingLog' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoGridViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoGridViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoGridViews' and [Action]='GetPromoGridView' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoGridViews' and [Action]='GetPromoGridViews' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProducts' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProducts' and [Action]='GetPromoProducts' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsCorrections' and [Action]='ExportCorrectionXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsCorrections' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsCorrections' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsCorrections' and [Action]='GetPromoProductsCorrection' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsCorrections' and [Action]='GetPromoProductsCorrections' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsViews' and [Action]='GetPromoProductsView' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsViews' and [Action]='GetPromoProductsViews' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsViews' and [Action]='Patch' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoProductsViews' and [Action]='Put' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoROIReports' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoROIReports' and [Action]='GetPromoROIReports' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSaleses' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSaleses' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSaleses' and [Action]='GetPromoSaleses' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoStatusChanges' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoStatusChanges' and [Action]='PromoStatusChangesByPromo' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoStatuss' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoStatuss' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoStatuss' and [Action]='GetPromoStatus' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoStatuss' and [Action]='GetPromoStatuss' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupportPromoes' and [Action]='ChangeListPSP' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupportPromoes' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupportPromoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupportPromoes' and [Action]='GetLinkedSubItems' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupportPromoes' and [Action]='GetPromoSupportPromo' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupportPromoes' and [Action]='GetPromoSupportPromoes' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupportPromoes' and [Action]='GetValuesForItems' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupportPromoes' and [Action]='PromoSuportPromoPost' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupports' and [Action]='DownloadFile' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupports' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupports' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupports' and [Action]='GetPromoSupport' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupports' and [Action]='GetPromoSupportGroup' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupports' and [Action]='GetPromoSupports' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupports' and [Action]='GetUserTimestamp' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoSupports' and [Action]='SetUserTimestamp' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoTypes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoTypes' and [Action]='GetPromoType' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoTypes' and [Action]='GetPromoTypes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoViews' and [Action]='ExportSchedule' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoViews' and [Action]='GetPromoView' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoViews' and [Action]='GetPromoViews' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RATIShoppers' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RATIShoppers' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RATIShoppers' and [Action]='GetRATIShopper' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RATIShoppers' and [Action]='GetRATIShoppers' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Recipients' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Recipients' and [Action]='GetMailNotificationSetting' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Recipients' and [Action]='GetRecipient' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Recipients' and [Action]='GetRecipients' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Regions' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Regions' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Regions' and [Action]='GetRegion' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Regions' and [Action]='GetRegions' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RejectReasons' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RejectReasons' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RejectReasons' and [Action]='GetRejectReason' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RejectReasons' and [Action]='GetRejectReasons' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RetailTypes' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RetailTypes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RetailTypes' and [Action]='GetRetailType' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RetailTypes' and [Action]='GetRetailTypes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Roles' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Roles' and [Action]='GetRole' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Roles' and [Action]='GetRoles' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RollingVolumes' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='RollingVolumes' and [Action]='GetRollingVolumes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Sales' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Sales' and [Action]='GetBudget' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Sales' and [Action]='GetBudgetItem' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Sales' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Sales' and [Action]='GetPromo' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Sales' and [Action]='GetSale' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Sales' and [Action]='GetSales' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='SchedulerClientTreeDTOs' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='SchedulerClientTreeDTOs' and [Action]='GetSchedulerClientTreeDTOs' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Security' and [Action]='Get' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Security' and [Action]='SaveGridSettings' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Segments' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Segments' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Segments' and [Action]='GetSegment' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Segments' and [Action]='GetSegments' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Settings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Settings' and [Action]='GetSetting' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Settings' and [Action]='GetSettings' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='SingleLoopHandlers' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='SingleLoopHandlers' and [Action]='GetSingleLoopHandler' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='SingleLoopHandlers' and [Action]='GetSingleLoopHandlers' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='SingleLoopHandlers' and [Action]='GetUser' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='StoreTypes' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='StoreTypes' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='StoreTypes' and [Action]='GetStoreType' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='StoreTypes' and [Action]='GetStoreTypes' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Subranges' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Subranges' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Subranges' and [Action]='GetSubrange' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Subranges' and [Action]='GetSubranges' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='TechHighLevels' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='TechHighLevels' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='TechHighLevels' and [Action]='GetTechHighLevel' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='TechHighLevels' and [Action]='GetTechHighLevels' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Technologies' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Technologies' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Technologies' and [Action]='GetTechnologies' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Technologies' and [Action]='GetTechnology' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='TradeInvestments' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='TradeInvestments' and [Action]='GetCOGS' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='TradeInvestments' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='TradeInvestments' and [Action]='GetTradeInvestments' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserDTOs' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserDTOs' and [Action]='GetUserDTO' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserDTOs' and [Action]='GetUserDTOs' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserLoopHandlers' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserLoopHandlers' and [Action]='GetUser' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserLoopHandlers' and [Action]='GetUserLoopHandler' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserLoopHandlers' and [Action]='GetUserLoopHandlers' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserRoles' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserRoles' and [Action]='GetRole' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserRoles' and [Action]='GetUser' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserRoles' and [Action]='GetUserRole' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserRoles' and [Action]='GetUserRoles' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Users' and [Action]='GetFilteredData' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Varieties' and [Action]='ExportXLSX' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Varieties' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Varieties' and [Action]='GetVarieties' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Varieties' and [Action]='GetVariety' and [Disabled] = 0))
   GO
       DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager' and [Disabled] = 0);
       INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
       (RoleId, AccessPointId) values
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='XMLProcessInterfaceSettings' and [Action]='GetFilteredData' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='XMLProcessInterfaceSettings' and [Action]='GetInterface' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='XMLProcessInterfaceSettings' and [Action]='GetXMLProcessInterfaceSetting' and [Disabled] = 0)),
       (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='XMLProcessInterfaceSettings' and [Action]='GetXMLProcessInterfaceSettings' and [Disabled] = 0))
   GO

        INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
        (
            [Id]
            ,[RoleId]
            ,[AccessPointId]
        )
        VALUES
        (NEWID(), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[Role] WHERE SystemName = 'GAManager'), (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'MassApprove'))
    GO
            ";
    }
}
