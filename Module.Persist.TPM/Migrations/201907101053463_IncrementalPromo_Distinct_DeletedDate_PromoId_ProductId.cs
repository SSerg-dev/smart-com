namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncrementalPromo_Distinct_DeletedDate_PromoId_ProductId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.IncrementalPromo", new[] { "PromoId" });
            DropIndex("dbo.IncrementalPromo", new[] { "ProductId" });
            CreateIndex("dbo.IncrementalPromo", new[] { "DeletedDate", "PromoId", "ProductId" }, unique: true, name: "Unique_IncrementalPromo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.IncrementalPromo", "Unique_IncrementalPromo");
            CreateIndex("dbo.IncrementalPromo", "ProductId");
            CreateIndex("dbo.IncrementalPromo", "PromoId");
        }
    }
}
