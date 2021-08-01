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

            Sql("DROP VIEW Jupiter.AssortmentMatrix2Plu");
            Sql("DROP VIEW Jupiter.PLUDictionary");
            Sql("DROP VIEW Jupiter.PromoProduct2Plu");
            Sql("TRUNCATE TABLE Jupiter.Plu");
            DropForeignKey("Jupiter.Plu", "ProductId", "Jupiter.Product");
            DropIndex("Jupiter.Plu", new[] { "ProductId" });
            DropPrimaryKey("Jupiter.Plu");
            AddColumn("Jupiter.Plu", "EAN_PC", c => c.String(nullable: false, maxLength: 255));
            AddPrimaryKey("Jupiter.Plu", new[] { "ClientTreeId", "EAN_PC" });
            DropColumn("Jupiter.Plu", "ProductId");

            Sql($@"CREATE VIEW jupiter.[AssortmentMatrix2Plu]
                   WITH SCHEMABINDING
                   AS
                        SELECT  am.Id , p.EAN_PC, plu.PluCode, am.ClientTreeId
	                        FROM
		                        Jupiter.AssortmentMatrix am
		                        INNER JOIN Jupiter.Product p ON p.Id=am.ProductId
		                        INNER JOIN Jupiter.Plu plu ON plu.ClientTreeId= am.ClientTreeId
			                        AND plu.EAN_PC = p.EAN_PC
                    GO
                    CREATE UNIQUE CLUSTERED INDEX [AssortmentMatrix2Plu_IDX] ON jupiter.[AssortmentMatrix2Plu]
                    (
	                     Id ASC
                    )
                    GO");

            Sql($@"
                    CREATE VIEW Jupiter.[PLUDictionary]
                    AS
					WITH cte(ClientTreeId, EAN_PC) AS
					(
						SELECT DISTINCT ct.Id AS ClientTreeId,   p.EAN_PC
	                    FROM
		                    Jupiter.ClientTree ct
		                    INNER JOIN Jupiter.AssortmentMatrix am ON  am.ClientTreeId = ct.Id
			                    AND am.Disabled = 0
		                    INNER JOIN  Jupiter.Product p ON p.Id = am.ProductId
		                    LEFT JOIN Jupiter.Plu plu ON plu.ClientTreeId =ct.Id
			                    AND plu.EAN_PC = p.EAN_PC
					)
                    SELECT NEWID() AS Id, ct.Name AS ClientTreeName , ct.Id AS ClientTreeId, ct.ObjectId,  cte.EAN_PC, plu.PluCode
	                    FROM
							cte 
		                    INNER JOIN Jupiter.ClientTree ct ON ct.id=cte.ClientTreeId
		                    LEFT JOIN Jupiter.Plu plu ON plu.ClientTreeId =ct.Id
			                    AND plu.EAN_PC = cte.EAN_PC
	                    WHERE 
		                    ct.IsBaseClient = 1
                    GO");

            Sql($@"CREATE VIEW Jupiter.[PromoProduct2Plu]
               WITH SCHEMABINDING
               AS 
                SELECT pp.Id, p1.PluCode
                    FROM
                        Jupiter.Promo p
                        INNER JOIN Jupiter.PromoProduct pp ON pp.PromoId = p.Id
                        INNER JOIN Jupiter.Plu p1 ON p1.ClientTreeId = p.ClientTreeKeyId
                            AND p1.EAN_PC = pp.EAN_PC
                GO
                CREATE UNIQUE CLUSTERED INDEX[PromoProduct2Plu_IDX] ON Jupiter.[PromoProduct2Plu]
                (
                    [Id] ASC
                );
                GO");
        }

        public override void Down()
        {
            AddColumn("Jupiter.Plu", "ProductId", c => c.Guid(nullable: false));
            DropPrimaryKey("Jupiter.Plu");
            DropColumn("Jupiter.Plu", "EAN_PC");
            AddPrimaryKey("Jupiter.Plu", new[] { "ClientTreeId", "ProductId" });
            CreateIndex("Jupiter.Plu", "ProductId");
            AddForeignKey("Jupiter.Plu", "ProductId", "Jupiter.Product", "Id");
        }
    }
}
