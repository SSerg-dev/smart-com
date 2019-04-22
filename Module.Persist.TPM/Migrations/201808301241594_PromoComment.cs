namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "Comment", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "Comment");
        }
    }
}
