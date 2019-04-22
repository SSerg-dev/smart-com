namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductChangeIncident_CREATE : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductChangeIncident",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ProductId = c.Guid(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductChangeIncident", "ProductId", "dbo.Product");
            DropIndex("dbo.ProductChangeIncident", new[] { "ProductId" });
            DropTable("dbo.ProductChangeIncident");
        }
    }
}
