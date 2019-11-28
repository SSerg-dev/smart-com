namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_IsGrowthAcceleration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "IsGrowthAcceleration", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "IsGrowthAcceleration");
        }
    }
}
