namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoStatusChange_AddField_RejectReasonId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoStatusChange", "RejectReasonId", c => c.Guid());
            CreateIndex("dbo.PromoStatusChange", "RejectReasonId");
            AddForeignKey("dbo.PromoStatusChange", "RejectReasonId", "dbo.RejectReason", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoStatusChange", "RejectReasonId", "dbo.RejectReason");
            DropIndex("dbo.PromoStatusChange", new[] { "RejectReasonId" });
            DropColumn("dbo.PromoStatusChange", "RejectReasonId");
        }
    }
}
