namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Plu_Views : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE VIEW [Jupiter].[AssortmentMatrix2Plu]
                   WITH SCHEMABINDING
                   AS
                        SELECT am.Id as Id, p.PluCode
                            FROM
                                Jupiter.AssortmentMatrix am
                                INNER JOIN  Jupiter.Plu p ON p.ClientTreeId = am.ClientTreeId
                                    AND p.ProductId = am.ProductId 
                    GO
                    CREATE UNIQUE CLUSTERED INDEX [AssortmentMatrix2Plu_IDX] ON [Jupiter].[AssortmentMatrix2Plu]
                    (
	                    [Id] ASC
                    )
                    GO");

            Sql(@"CREATE VIEW [Jupiter].[PromoProduct2Plu]
               WITH SCHEMABINDING
               AS 
                SELECT pp.Id, p1.PluCode
                    FROM
                        Jupiter.Promo p
                        INNER JOIN Jupiter.PromoProduct pp ON pp.PromoId = p.Id
                        INNER JOIN Jupiter.Plu p1 ON p1.ClientTreeId = p.ClientTreeKeyId
                            AND p1.ProductId = pp.ProductId
                GO
                CREATE UNIQUE CLUSTERED INDEX[PromoProduct2Plu_IDX] ON[Jupiter].[PromoProduct2Plu]
                (
                    [Id] ASC
                );
                GO");
            
        }
        
        public override void Down()
        {
            Sql("DROP VIEW [Jupiter].[AssortmentMatrix2Plu]");
            Sql("DROP VIEW [Jupiter].[PromoProduct2Plu]");
        }
    }
}
