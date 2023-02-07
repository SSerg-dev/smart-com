namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Views;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedDate_Triggers : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Category"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Brand"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "BrandTech"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "ClientTreeBrandTech"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Product"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Mechanic"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "MechanicType"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "ProductTree"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Technology"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoTypes"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "BTLPromo"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "BTL"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Event"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "EventClientTree"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "EventType"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Color"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "CurrentDayIncremental"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoApprovedIncident"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoCancelledIncident"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoOnApprovalIncident"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoOnRejectIncident"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoProductTree"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoStatus"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoStatusChange"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "RejectReason"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoSupportPromo"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "BudgetSubItem"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "BudgetItem"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Budget"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "NonPromoEquipment"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "BudgetSubItemClientTree"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoUpliftFailIncident"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "RollingScenario"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PriceList"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "RollingVolume"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "CompetitorPromo"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Competitor"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "CompetitorBrandTech"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Plu"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "RATIShopper"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "TradeInvestment"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "CoefficientSI2SO"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Segment"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "TechHighLevel"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Format"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Subrange"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "AgeGroup"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Variety"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Region"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "CommercialNet"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "CommercialSubnet"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Distributor"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "StoreType"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "Sale"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "NodeType"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoDemand"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "RetailType"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "ExportQuery"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoDemandChangeIncident"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "ServiceInfo"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PostPromoEffect"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "COGS"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PlanCOGSTn"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "BlockedPromo"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "ChangesIncident"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "ActualCOGS"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "ActualCOGSTn"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "ActualTradeInvestment"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "ClientDashboard"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "NonPromoSupportDMP"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "PromoSupportDMP"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "RPASetting"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "RPA"));
            Sql(ViewMigrations.UpdateTriggerModifiedDate(defaultSchema, "MetricsLiveHistories"));
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }

        public override void Down()
        {
        }

        private string SqlString = @"
        
            ALTER TRIGGER [DefaultSchemaSetting].[NoneNego_Update_Trigger]
            ON [DefaultSchemaSetting].[NoneNego]
            AFTER INSERT, UPDATE AS
              UPDATE [DefaultSchemaSetting].[NoneNego]
              SET ModifiedDate = CURRENT_TIMESTAMP,[ToDate] = '9999-12-28T23:59:59.999+00:00'
              WHERE Id IN (SELECT DISTINCT Id FROM Inserted)
            GO

            ALTER TRIGGER [DefaultSchemaSetting].[AssortmentMatrix_ChangesIncident_Insert_Update_Trigger]
	            ON [DefaultSchemaSetting].[AssortmentMatrix]
	            AFTER INSERT, UPDATE
	            AS
		            INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled])
		            SELECT 'AssortmentMatrix', INSERTED.Id, GETDATE(), NULL, NULL, 0 FROM INSERTED
                    UPDATE [DefaultSchemaSetting].[AssortmentMatrix]
                    SET ModifiedDate = CURRENT_TIMESTAMP
                    WHERE Id IN (SELECT DISTINCT Id FROM Inserted)
            GO

            ALTER TRIGGER [DefaultSchemaSetting].[ClientTreeDemandCode_Update_Trigger]
            ON [DefaultSchemaSetting].[ClientTree]
            AFTER INSERT, UPDATE AS
              UPDATE [DefaultSchemaSetting].ClientTree
              SET DemandCode = NULL
              WHERE Id IN (SELECT DISTINCT Id FROM Inserted) and DemandCode = ''
              UPDATE [DefaultSchemaSetting].[ClientTree]
              SET ModifiedDate = CURRENT_TIMESTAMP
              WHERE Id IN (SELECT DISTINCT Id FROM Inserted)
        ";
    }
}
