namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Priority_Filter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "Priority", c => c.Int());
            AddColumn("dbo.Promo", "ClientFilter", c => c.String());
            AddColumn("dbo.Promo", "ProductFilter", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "ProductFilter");
            DropColumn("dbo.Promo", "ClientFilter");
            DropColumn("dbo.Promo", "Priority");
        }
    }
}
