namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseLine_RemoveDisabledUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BaseLine", "Unique_BaseLine");
            CreateIndex("dbo.BaseLine", new[] { "DeletedDate", "ProductId", "ClientTreeId", "StartDate" }, unique: true, name: "Unique_BaseLine");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BaseLine", "Unique_BaseLine");
            CreateIndex("dbo.BaseLine", new[] { "Disabled", "DeletedDate", "ProductId", "ClientTreeId", "StartDate" }, unique: true, name: "Unique_BaseLine");
        }
    }
}
