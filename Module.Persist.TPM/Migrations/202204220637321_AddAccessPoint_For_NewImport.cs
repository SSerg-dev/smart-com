namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccessPoint_For_NewImport : DbMigration
    {

		public override void Up()
		{
			var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
			Sql($@"

			DECLARE @ItemId UNIQUEIDENTIFIER

			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'CompetitorPromoes', 'NewFullImportXLSX')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'CompetitorPromoes' AND Action = 'NewFullImportXLSX'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from {defaultSchema}.Role WHERE SystemName = 'Administrator'            
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SupportAdminist'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CMManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'FunctionalExpert'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandFinance'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CustomerMarketing'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SuperReader'
            ");
		}

		public override void Down()
		{

		}
	}
}

