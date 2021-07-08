namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPluRole : DbMigration
    {
        public override void Up()
        {
            Sql(@"

			DECLARE @ItemId UNIQUEIDENTIFIER
			INSERT INTO Jupiter.AccessPoint(Disabled, Resource, Action) VALUES(0, 'PromoProducts', 'DownloadTemplatePluXLSX')

			SELECT  @ItemId = Id FROM[Jupiter].[AccessPoint] where Resource = 'PromoProducts' AND Action = 'DownloadTemplatePluXLSX'
			INSERT INTO Jupiter.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			INSERT INTO Jupiter.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO Jupiter.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandFinance'

			INSERT INTO Jupiter.AccessPoint(Disabled, Resource, Action) VALUES(0, 'PromoProducts', 'FullImportPluXLSX')

			SELECT  @ItemId = Id FROM[Jupiter].[AccessPoint] where Resource = 'PromoProducts' AND Action = 'FullImportPluXLSX'
			INSERT INTO Jupiter.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			INSERT INTO Jupiter.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO Jupiter.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandFinance'
			");
        }

        public override void Down()
        {
        }
    }
}
