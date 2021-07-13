namespace Module.Persist.TPM.Migrations
{
	using Core.Settings;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class Plu_Views : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");

            Sql($@"CREATE VIEW [{defaultSchema}].[AssortmentMatrix2Plu]
                   WITH SCHEMABINDING
                   AS
                        SELECT am.Id as Id, p.PluCode
                            FROM
                                {defaultSchema}.AssortmentMatrix am
                                INNER JOIN  {defaultSchema}.Plu p ON p.ClientTreeId = am.ClientTreeId
                                    AND p.ProductId = am.ProductId 
                    GO
                    CREATE UNIQUE CLUSTERED INDEX [AssortmentMatrix2Plu_IDX] ON [{defaultSchema}].[AssortmentMatrix2Plu]
                    (
	                    [Id] ASC
                    )
                    GO");

            Sql($@"CREATE VIEW [{defaultSchema}].[PromoProduct2Plu]
               WITH SCHEMABINDING
               AS 
                SELECT pp.Id, p1.PluCode
                    FROM
                        {defaultSchema}.Promo p
                        INNER JOIN {defaultSchema}.PromoProduct pp ON pp.PromoId = p.Id
                        INNER JOIN {defaultSchema}.Plu p1 ON p1.ClientTreeId = p.ClientTreeKeyId
                            AND p1.ProductId = pp.ProductId
                GO
                CREATE UNIQUE CLUSTERED INDEX[PromoProduct2Plu_IDX] ON [{defaultSchema}].[PromoProduct2Plu]
                (
                    [Id] ASC
                );
                GO");
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql($"DROP VIEW [{defaultSchema}].[AssortmentMatrix2Plu]");
            Sql($"DROP VIEW [{defaultSchema}].[PromoProduct2Plu]");
        }
    }
}
