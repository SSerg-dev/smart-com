namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_BaseClientTreeId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "BaseClientTreeId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "BaseClientTreeId");
        }
    }
}
