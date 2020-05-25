namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_PreviousDayIncremental : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PreviousDayIncremental", new[] { "PromoId" });
            DropIndex("dbo.PreviousDayIncremental", new[] { "ProductId" });
            AlterColumn("dbo.PreviousDayIncremental", "PromoId", c => c.Guid());
            AlterColumn("dbo.PreviousDayIncremental", "ProductId", c => c.Guid());
            AlterColumn("dbo.PreviousDayIncremental", "IncrementalQty", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PreviousDayIncremental", "IncrementalQty", c => c.Double(nullable: false));
            AlterColumn("dbo.PreviousDayIncremental", "ProductId", c => c.Guid(nullable: false));
            AlterColumn("dbo.PreviousDayIncremental", "PromoId", c => c.Guid(nullable: false));
            CreateIndex("dbo.PreviousDayIncremental", "ProductId");
            CreateIndex("dbo.PreviousDayIncremental", "PromoId");
        }
    }
}
