namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoBaseClientString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "BaseClientTreeIds", c => c.String(maxLength: 400));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "BaseClientTreeIds");
        }
    }
}
