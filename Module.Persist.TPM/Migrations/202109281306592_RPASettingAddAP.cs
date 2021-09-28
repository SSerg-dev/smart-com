namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RPASettingAddAP : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql($@"
            
            INSERT INTO { defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPASettings', 'GetRPASettings')
            SELECT @ItemId = Id FROM[{ defaultSchema}].[AccessPoint] where Resource = 'RPASettings' AND Action = 'GetRPASettings'
            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdminist'
            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'

            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'

            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CMManager'

            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'FunctionalExpert'

            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandFinance'

            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CustomerMarketing'

            INSERT INTO { defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPASettings', 'GetRPAs')
            SELECT @ItemId = Id FROM[{ defaultSchema}].[AccessPoint] where Resource = 'RPASettings' AND Action = 'GetRPAs'
            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdminist'
            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'

            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'

            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CMManager'

            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'FunctionalExpert'

            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandFinance'

            INSERT INTO { defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CustomerMarketing'
            ");
        }
        
        public override void Down()
        {
        }
    }
}
