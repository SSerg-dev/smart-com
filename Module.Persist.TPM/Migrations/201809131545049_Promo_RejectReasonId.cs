namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_RejectReasonId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "RejectReasonId", c => c.Guid());
            CreateIndex("dbo.Promo", "RejectReasonId");
            AddForeignKey("dbo.Promo", "RejectReasonId", "dbo.RejectReason", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promo", "RejectReasonId", "dbo.RejectReason");
            DropIndex("dbo.Promo", new[] { "RejectReasonId" });
            DropColumn("dbo.Promo", "RejectReasonId");
        }
    }
}
