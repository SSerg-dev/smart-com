namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_TreeId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "ClientTreeId", c => c.Guid());
            AddColumn("dbo.Promo", "ProductTreeId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "ProductTreeId");
            DropColumn("dbo.Promo", "ClientTreeId");
        }
    }
}
