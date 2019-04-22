namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoiPlan_RoiFact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "RoiPlan", c => c.Int());
            AddColumn("dbo.Promo", "RoiFact", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "RoiFact");
            DropColumn("dbo.Promo", "RoiPlan");
        }
    }
}
