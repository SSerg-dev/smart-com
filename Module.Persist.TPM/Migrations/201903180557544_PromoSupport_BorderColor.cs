namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupport_BorderColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupport", "BorderColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoSupport", "BorderColor");
        }
    }
}
