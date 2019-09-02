namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLinks_IncrementalPromo : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.IncrementalPromo", "Unique_IncrementalPromo");
            AlterColumn("dbo.IncrementalPromo", "PromoId", c => c.Guid(nullable: false));
            AlterColumn("dbo.IncrementalPromo", "ProductId", c => c.Guid(nullable: false));
            CreateIndex("dbo.IncrementalPromo", new[] { "DeletedDate", "PromoId", "ProductId" }, unique: true, name: "Unique_IncrementalPromo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.IncrementalPromo", "Unique_IncrementalPromo");
            AlterColumn("dbo.IncrementalPromo", "ProductId", c => c.Guid());
            AlterColumn("dbo.IncrementalPromo", "PromoId", c => c.Guid());
            CreateIndex("dbo.IncrementalPromo", new[] { "DeletedDate", "PromoId", "ProductId" }, unique: true, name: "Unique_IncrementalPromo");
        }
    }
}
