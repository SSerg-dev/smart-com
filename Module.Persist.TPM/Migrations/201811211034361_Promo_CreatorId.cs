namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_CreatorId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "CreatorId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "CreatorId");
        }
    }
}
