namespace Module.Persist.TPM.Migrations
{
	using Core.Settings;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class PLUHistory : DbMigration
    {
        public override void Up()
        {
			var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");

			Sql($@"DELETE {defaultSchema}.Plu");

			AddColumn($"{defaultSchema}.Plu", "Id", c => c.Guid(nullable: false));
            Sql($@"
            		DECLARE @ItemId UNIQUEIDENTIFIER
			        INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'HistoricalPLUDictionaries', 'GetHistoricalPLUDictionaries')
			        SELECT  @ItemId = Id FROM {defaultSchema}.[AccessPoint] where Resource = 'HistoricalPLUDictionaries' AND Action = 'GetHistoricalPLUDictionaries'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'

			        INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'HistoricalPLUDictionaries', 'GetFilteredData')
			        SELECT  @ItemId = Id FROM {defaultSchema}.[AccessPoint] where Resource = 'HistoricalPLUDictionaries' AND Action = 'GetFilteredData'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			        INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'
                ");

			Sql($@"
                    ALTER VIEW {defaultSchema}.[PLUDictionary]
                    AS
					WITH cte(ClientTreeId, EAN_PC) AS
					(
						SELECT DISTINCT ct.Id AS ClientTreeId,   p.EAN_PC
	                    FROM
		                    {defaultSchema}.ClientTree ct
		                    INNER JOIN {defaultSchema}.AssortmentMatrix am ON  am.ClientTreeId = ct.Id
			                    AND am.Disabled = 0
		                    INNER JOIN  {defaultSchema}.Product p ON p.Id = am.ProductId
		                    LEFT JOIN {defaultSchema}.Plu plu ON plu.ClientTreeId =ct.Id
			                    AND plu.EAN_PC = p.EAN_PC
					)
                    SELECT ISNULL(plu.Id, NEWID()) AS Id, ct.Name AS ClientTreeName , ct.Id AS ClientTreeId, ct.ObjectId,  cte.EAN_PC, plu.PluCode
	                    FROM
							cte 
		                    INNER JOIN {defaultSchema}.ClientTree ct ON ct.id=cte.ClientTreeId
		                    LEFT JOIN {defaultSchema}.Plu plu ON plu.ClientTreeId =ct.Id
			                    AND plu.EAN_PC = cte.EAN_PC
	                    WHERE 
		                    ct.IsBaseClient = 1
                    GO");
		}
        
        public override void Down()
        {
			var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
			DropColumn($"{defaultSchema}.Plu", "Id");
        }
    }
}
