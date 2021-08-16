namespace Module.Persist.TPM.Migrations
{
	using Core.Settings;
	using System;
    using System.Data.Entity.Migrations;

    public partial class PLUDictionary : DbMigration
    {
        public override void Up()
        {
			var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");

			Sql($@"
                    CREATE VIEW [{defaultSchema}].[PLUDictionary]
                    AS
                    SELECT NEWID() as Id, ct.Name AS ClientTreeName , ct.Id AS ClientTreeId, ct.ObjectId, p.ProductEN, p.Id AS ProductId, p.EAN_PC, plu.PluCode
	                    FROM
		                    {defaultSchema}.ClientTree ct
		                    INNER JOIN {defaultSchema}.AssortmentMatrix am ON  am.ClientTreeId = ct.Id
			                    AND am.Disabled = 0
		                    INNER JOIN  {defaultSchema}.Product p ON p.Id = am.ProductId
		                    LEFT JOIN {defaultSchema}.Plu plu ON plu.ClientTreeId =ct.Id
			                    AND plu.ProductId = p.Id
	                    WHERE 
		                    ct.IsBaseClient = 1
                    GO"); ;

			Sql($@"
			DECLARE @ItemId UNIQUEIDENTIFIER
			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'PLUDictionaries', 'ExportXLSX')
			SELECT @ItemId = Id FROM [{defaultSchema}].[AccessPoint] where Resource = 'PLUDictionaries' AND Action = 'ExportXLSX'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'

			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'PLUDictionaries', 'FullImportXLSX')
			SELECT  @ItemId = Id FROM [{defaultSchema}].[AccessPoint] where Resource = 'PLUDictionaries' AND Action = 'FullImportXLSX'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'

			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'PLUDictionaries', 'DownloadTemplateXLSX')
			SELECT  @ItemId = Id FROM [{defaultSchema}].[AccessPoint] where Resource = 'PLUDictionaries' AND Action = 'DownloadTemplateXLSX'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'

			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'PLUDictionaries', 'GetFilteredData')
			SELECT  @ItemId = Id FROM [{defaultSchema}].[AccessPoint] where Resource = 'PLUDictionaries' AND Action = 'GetFilteredData'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'

			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'PLUDictionaries', 'GetPLUDictionaries')
			SELECT @ItemId = Id FROM [{defaultSchema}].[AccessPoint] where Resource = 'PLUDictionaries' AND Action = 'GetPLUDictionaries'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role 			
");
		} 
        
        public override void Down()
        {
			var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
			Sql($"DROP VIEW [{defaultSchema}].[PLUDictionary]");
        }
    }
}
