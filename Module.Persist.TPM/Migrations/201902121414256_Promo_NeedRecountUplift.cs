namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_NeedRecountUplift : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "NeedRecountUplift", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "NeedRecountUplift");
        }
    }
}
