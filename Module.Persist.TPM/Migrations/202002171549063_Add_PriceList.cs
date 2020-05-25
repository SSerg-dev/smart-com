namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PriceList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PriceList",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        GHierarchyCode = c.String(maxLength: 255),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ProductId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .Index(t => new { t.DeletedDate, t.GHierarchyCode, t.StartDate, t.EndDate, t.ProductId }, unique: true, name: "Unique_ProductList");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceList", "ProductId", "dbo.Product");
            DropIndex("dbo.PriceList", "Unique_ProductList");
            DropTable("dbo.PriceList");
        }
    }
}
