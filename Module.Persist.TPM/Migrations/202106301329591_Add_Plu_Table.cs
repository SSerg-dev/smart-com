namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Plu_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Jupiter.Plu",
                c => new
                {
                    ClientTreeId = c.Int(nullable: false),
                    ProductId = c.Guid(nullable: false),
                    PluCode = c.String(maxLength: 20),
                })
                .PrimaryKey(t => new { t.ClientTreeId, t.ProductId })
                .ForeignKey("Jupiter.ClientTree", t => t.ClientTreeId)
                .ForeignKey("Jupiter.Product", t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Jupiter.Plu", "ProductId", "Jupiter.Product");
            DropForeignKey("Jupiter.Plu", "ClientTreeId", "Jupiter.ClientTree");
            DropTable("Jupiter.Plu");
        }
    }
}
