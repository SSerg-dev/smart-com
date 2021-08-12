namespace Module.Persist.TPM.Migrations
{
	using Core.Settings;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePLUTable : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");

            Sql($"DROP VIEW {defaultSchema}.AssortmentMatrix2Plu");
            Sql($"DROP VIEW {defaultSchema}.PLUDictionary");
            Sql($"DROP VIEW {defaultSchema}.PromoProduct2Plu");
            Sql($"TRUNCATE TABLE {defaultSchema}.Plu");
            DropForeignKey($"{defaultSchema}.Plu", "ProductId", "Jupiter.Product");
            DropIndex($"{defaultSchema}.Plu", new[] { "ProductId" });
            DropPrimaryKey($"{defaultSchema}.Plu");
            AddColumn($"{defaultSchema}.Plu", "EAN_PC", c => c.String(nullable: false, maxLength: 255));
            AddPrimaryKey($"{defaultSchema}.Plu", new[] { "ClientTreeId", "EAN_PC" });
            DropColumn($"{defaultSchema}.Plu", "ProductId");

            Sql($@"CREATE VIEW {defaultSchema}.[AssortmentMatrix2Plu]
                   WITH SCHEMABINDING
                   AS
                        SELECT  am.Id , p.EAN_PC, plu.PluCode, am.ClientTreeId
	                        FROM
		                        {defaultSchema}.AssortmentMatrix am
		                        INNER JOIN {defaultSchema}.Product p ON p.Id=am.ProductId
		                        INNER JOIN {defaultSchema}.Plu plu ON plu.ClientTreeId= am.ClientTreeId
			                        AND plu.EAN_PC = p.EAN_PC
                    GO
                    CREATE UNIQUE CLUSTERED INDEX [AssortmentMatrix2Plu_IDX] ON {defaultSchema}.[AssortmentMatrix2Plu]
                    (
	                     Id ASC
                    )
                    GO");

            Sql($@"
                    CREATE VIEW {defaultSchema}.[PLUDictionary]
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
                    SELECT NEWID() AS Id, ct.Name AS ClientTreeName , ct.Id AS ClientTreeId, ct.ObjectId,  cte.EAN_PC, plu.PluCode
	                    FROM
							cte 
		                    INNER JOIN {defaultSchema}.ClientTree ct ON ct.id=cte.ClientTreeId
		                    LEFT JOIN {defaultSchema}.Plu plu ON plu.ClientTreeId =ct.Id
			                    AND plu.EAN_PC = cte.EAN_PC
	                    WHERE 
		                    ct.IsBaseClient = 1
                    GO");

            Sql($@"CREATE VIEW {defaultSchema}.[PromoProduct2Plu]
               WITH SCHEMABINDING
               AS 
                SELECT pp.Id, p1.PluCode
                    FROM
                        {defaultSchema}.Promo p
                        INNER JOIN {defaultSchema}.PromoProduct pp ON pp.PromoId = p.Id
                        INNER JOIN {defaultSchema}.Plu p1 ON p1.ClientTreeId = p.ClientTreeKeyId
                            AND p1.EAN_PC = pp.EAN_PC
                GO
                CREATE UNIQUE CLUSTERED INDEX[PromoProduct2Plu_IDX] ON {defaultSchema}.[PromoProduct2Plu]
                (
                    [Id] ASC
                );
                GO");
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Plu", "ProductId", c => c.Guid(nullable: false));
            DropPrimaryKey($"{defaultSchema}.Plu");
            DropColumn($"{defaultSchema}.Plu", "EAN_PC");
            AddPrimaryKey($"{defaultSchema}.Plu", new[] { "ClientTreeId", "ProductId" });
            CreateIndex($"{defaultSchema}.Plu", "ProductId");
            AddForeignKey($"{defaultSchema}.Plu", "ProductId", $"{defaultSchema}.Product", "Id");
        }
    }
}
