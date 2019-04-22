namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoStatsUniq : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PromoStatus", new[] { "Name" });
            CreateIndex("dbo.PromoStatus", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PromoStatus", "Unique_Name");
            CreateIndex("dbo.PromoStatus", "Name", unique: true);
        }
    }
}
