namespace Module.Persist.TPM.Migrations
{
	using Core.Settings;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Patch_To_PLU : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");

            Sql($@"
            		DECLARE @ItemId UNIQUEIDENTIFIER
			        INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'PLUDictionaries', 'Patch')
			        SELECT  @ItemId = Id FROM {defaultSchema}.[AccessPoint] where Resource = 'PLUDictionaries' AND Action = 'Patch'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'
                ");
        }
        
        public override void Down()
        {
        }
    }
}
