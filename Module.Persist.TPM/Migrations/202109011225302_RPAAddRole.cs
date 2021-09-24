namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RPAAddRole : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql($@"

			DECLARE @ItemId UNIQUEIDENTIFIER

			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'GetRPAs')
			SELECT  @ItemId = Id FROM [{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'GetRPAs'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'			
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CMManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'FunctionalExpert'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandFinance'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CustomerMarketing'

			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'GetRPA')
			SELECT  @ItemId = Id FROM [{defaultSchema}].[AccessPoint] where Resource = 'PromoProducts' AND Action = 'GetRPA'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CMManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'FunctionalExpert'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandFinance'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CustomerMarketing'

			
            INSERT INTO { defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'GetFilteredData')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'GetFilteredData'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CMManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'FunctionalExpert'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandFinance'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CustomerMarketing'


			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'Post')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'Post'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'            
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CMManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'FunctionalExpert'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandFinance'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CustomerMarketing'


            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'UploadFile')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'UploadFile'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'            
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdminist'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CMManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'FunctionalExpert'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandFinance'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CustomerMarketing'


            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'DownloadFile')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'DownloadFile'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'            
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdminist'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CMManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'FunctionalExpert'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandFinance'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'CustomerMarketing'

            ");
        }
        
        public override void Down()
        {
           
        }
    }
}
