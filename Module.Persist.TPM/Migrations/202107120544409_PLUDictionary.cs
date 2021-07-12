namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PLUDictionary : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                    CREATE VIEW [Jupiter].[PLUDictionary]
                    AS
                    SELECT ct.Name AS ClientTreeName , ct.Id AS ClientTreeId, ct.ObjectId, p.ProductEN, p.Id AS ProductId, p.EAN_PC, plu.PluCode
	                    FROM
		                    Jupiter.ClientTree ct
		                    INNER JOIN Jupiter.AssortmentMatrix am ON  am.ClientTreeId = ct.Id
			                    AND am.Disabled = 0
		                    INNER JOIN  Jupiter.Product p ON p.Id = am.ProductId
		                    LEFT JOIN Jupiter.Plu plu ON plu.ClientTreeId =ct.Id
			                    AND plu.ProductId = p.Id
	                    WHERE 
		                    ct.IsBaseClient = 1
                    GO");
                            }
        
        public override void Down()
        {
            DropTable("Jupiter.PLUDictionary");
        }
    }
}
