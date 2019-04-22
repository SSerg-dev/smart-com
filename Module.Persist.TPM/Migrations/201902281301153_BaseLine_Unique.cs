namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseLine_Unique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BaseLine", new[] { "ProductId" });
            DropIndex("dbo.BaseLine", new[] { "ClientTreeId" });
            CreateIndex("dbo.BaseLine", new[] { "Disabled", "ProductId", "ClientTreeId", "StartDate" }, unique: true, name: "Unique_BaseLine");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BaseLine", "Unique_BaseLine");
            CreateIndex("dbo.BaseLine", "ClientTreeId");
            CreateIndex("dbo.BaseLine", "ProductId");
        }
    }
}
